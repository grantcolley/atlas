using Atlas.Core.Models;
using Atlas.Data.Access.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Atlas.Data.Access.Context
{
    public class ApplicationDbContext : DbContext
    {
        private string? _user;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public void SetUser(string user)
        {
            this._user = user;
        }

        public string? GetUser()
        {
            return _user;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            builder.Entity<Permission>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Permission>()
                .HasIndex(p => p.Code)
                .IsUnique();

            builder.Entity<Module>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<MenuItem>()
                .HasIndex(m => m.Name)
                .IsUnique();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var audits = OnBeforeSaveChanges();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            if (audits.Any())
            {
                OnAfterSaveChanges(audits);

                base.SaveChanges(acceptAllChangesOnSuccess);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var audits = OnBeforeSaveChanges();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (audits.Any())
            {
                OnAfterSaveChanges(audits);

                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        private List<Audit> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var audits = new List<Audit>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not ModelBase
                    || entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var now = DateTime.Now;

                if (entry.State.Equals(EntityState.Added))
                {
                    ((ModelBase)entry.Entity).CreatedDate = now;
                    ((ModelBase)entry.Entity).CreatedBy = _user ?? null;
                    ((ModelBase)entry.Entity).ModifiedDate = now;
                    ((ModelBase)entry.Entity).ModifiedBy = _user ?? null;
                }
                else if (entry.State.Equals(EntityState.Modified))
                {
                    ((ModelBase)entry.Entity).ModifiedDate = now;
                    ((ModelBase)entry.Entity).ModifiedBy = _user ?? null;
                }

                var audit = new Audit
                {
                    TableName = entry.Metadata.GetTableName(),
                    ClrType = entry.Metadata.ClrType.Name,
                    Action = entry.State == EntityState.Added ? "ADD" : entry.State == EntityState.Modified ? "UPDATE" : "DELETE",
                    User = _user,
                    DateTime = now
                };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        audit.TemporaryProperties.Add(property);
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        audit.EntityId = property.CurrentValue?.ToString();
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            audit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            audit.OldValuesDictionary[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                audit.OldValuesDictionary[propertyName] = property.OriginalValue;
                                audit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                audits.Add(audit);
            }

            foreach (var audit in audits.Where(e => !e.TemporaryProperties.Any()))
            {
                Audits.Add(SerializeValues(audit));
            }

            return audits.Where(e => e.TemporaryProperties.Any()).ToList();
        }

        private void OnAfterSaveChanges(List<Audit> audits)
        {
            foreach (var audit in audits)
            {
                foreach (var prop in audit.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        audit.EntityId = prop.CurrentValue?.ToString();
                    }

                    audit.NewValuesDictionary[prop.Metadata.Name] = prop.CurrentValue;
                }

                Audits.Add(SerializeValues(audit));
            }
        }

        private static Audit SerializeValues(Audit audit)
        {
            if (audit.OldValuesDictionary.Any())
            {
                audit.OldValues = JsonSerializer.Serialize(audit.OldValuesDictionary);
            }

            if (audit.NewValuesDictionary.Any())
            {
                audit.NewValues = JsonSerializer.Serialize(audit.NewValuesDictionary);
            }

            return audit;
        }
    }
}
