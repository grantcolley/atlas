using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.EF.Context;
using Atlas.Data.Access.Interfaces;
using Atlas.Logging.Interfaces;
using Atlas.Seed.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.EF.Data
{
    public class DeveloperData(ApplicationDbContext applicationDbContext, AtlasConfig atlasConfig, ILogService logService, ILogger<DeveloperData> logger)
        : AuthorisationData<DeveloperData>(applicationDbContext, logger), IDeveloperData
    {
        private readonly AtlasConfig _atlasConfig = atlasConfig;

        public async Task<DatabaseStatus?> GetDatabaseStatusAsync(string? user, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(user, nameof(user));

                DatabaseStatus databaseStatus = new()
                {
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false),
                    CanCreate = _atlasConfig.DatabaseCreate
                };

                if(databaseStatus.CanConnect)
                {
                    databaseStatus.CanSeedData = _atlasConfig.DatabaseSeedData;
                }

                return databaseStatus;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, user);
            }
        }

        public async Task<DatabaseStatus?> CreateDatabaseAsync(string? user, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(user, nameof(user));

                DatabaseStatus databaseStatus = new()
                {
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false),
                    CanCreate = _atlasConfig.DatabaseCreate
                };

                if (!databaseStatus.CanConnect)
                {
                    logService.Log(Logging.Enums.LogLevel.Information, $"Creating the Atlas database {_applicationDbContext.Database.GetConnectionString()}", user);

                    databaseStatus.CanConnect = await _applicationDbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

                    if (databaseStatus.CanConnect)
                    {
                        databaseStatus.CanSeedData = _atlasConfig.DatabaseSeedData;
                    }

                    logService.Log(Logging.Enums.LogLevel.Information, $"Atlas database created {_applicationDbContext.Database.GetConnectionString()}", user);
                }

                return databaseStatus;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, user);
            }
        }

        public async Task<DatabaseStatus?> SeedDatabaseAsync(string? user, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(user, nameof(user));

                DatabaseStatus databaseStatus = new()
                {
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false),
                    CanCreate = _atlasConfig.DatabaseCreate,
                    CanSeedData = _atlasConfig.DatabaseSeedData
                };

                if (databaseStatus.CanConnect)
                {
                    bool generateSeedLogs = _atlasConfig.DatabaseSeedLogs;

                    if (databaseStatus.CanSeedData)
                    {
                        SeedData.Generate(_applicationDbContext);

                        logService.Log(Logging.Enums.LogLevel.Information, $"Atlas database seeded {_applicationDbContext.Database.GetConnectionString()}", user);

                        if (generateSeedLogs)
                        {
                            for (int i = 0; i < 500; i++)
                            {
                                logService.Log(Logging.Enums.LogLevel.Information, new AtlasException("myNumber is zero", new DivideByZeroException(), "myNumber=0"), "test@email.com");
                                logService.Log(Logging.Enums.LogLevel.Warning, new AtlasException("myVariable is null", new NullReferenceException("myVariable"), "myVariable=null"), "system@email.com");
                                logService.Log(Logging.Enums.LogLevel.Error, new AtlasException("Boom!", new StackOverflowException(), "what the...."), "user@email.com");
                            }

                            logService.Log(Logging.Enums.LogLevel.Information, $"Atlas database seed logs created {_applicationDbContext.Database.GetConnectionString()}", user);
                        }
                    }
                }

                return databaseStatus;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, user);
            }
        }
    }
}
