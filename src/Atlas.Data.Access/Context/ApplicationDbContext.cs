using Atlas.Core.Models;
using Atlas.Data.Access.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Atlas.Data.Access.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        private string? _user;

        public DbSet<Audit> Audits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Log> Logs { get; set; }

        public void SetUser(string user)
        {
            _user = user;
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

            builder.Entity<Page>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Log>()
                .HasIndex(l => l.User);

            builder.Entity<Log>()
                .HasIndex(l => l.TimeStamp);

            builder.Entity<Log>()
                .HasIndex(l => l.Message);

            builder.Entity<Log>()
                .HasIndex(l => l.Context);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            List<Audit> audits = OnBeforeSaveChanges();

            int result = base.SaveChanges(acceptAllChangesOnSuccess);

            if (audits.Count > 0)
            {
                OnAfterSaveChanges(audits);

                base.SaveChanges(acceptAllChangesOnSuccess);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            List<Audit> audits = OnBeforeSaveChanges();

            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (audits.Count > 0)
            {
                OnAfterSaveChanges(audits);

                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        private List<Audit> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            List<Audit> audits = new List<Audit>();

            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not ModelBase
                    || entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                DateTime now = DateTime.Now;

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

                Audit audit = new()
                {
                    TableName = entry.Metadata.GetTableName(),
                    ClrType = entry.Metadata.ClrType.Name,
                    Action = entry.State == EntityState.Added ? "ADD" : entry.State == EntityState.Modified ? "UPDATE" : "DELETE",
                    User = _user,
                    DateTime = now
                };

                foreach (PropertyEntry property in entry.Properties)
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

            foreach (Audit audit in audits.Where(e => e.TemporaryProperties.Count == 0))
            {
                Audits.Add(SerializeValues(audit));
            }

            return audits.Where(e => e.TemporaryProperties.Count > 0).ToList();
        }

        private void OnAfterSaveChanges(List<Audit> audits)
        {
            foreach (Audit audit in audits)
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
            if (audit.OldValuesDictionary.Count > 0)
            {
                audit.OldValues = JsonSerializer.Serialize(audit.OldValuesDictionary);
            }

            if (audit.NewValuesDictionary.Count > 0)
            {
                audit.NewValues = JsonSerializer.Serialize(audit.NewValuesDictionary);
            }

            return audit;
        }
    }
}
