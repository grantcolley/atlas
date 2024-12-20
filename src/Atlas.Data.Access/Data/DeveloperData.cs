using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Atlas.Data.Context;
using Atlas.Seed.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class DeveloperData(ApplicationDbContext applicationDbContext, IConfiguration configuration, ILogService logService, ILogger<DeveloperData> logger)
        : AuthorisationData<DeveloperData>(applicationDbContext, logger), IDeveloperData
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<DatabaseStatus?> GetDatabaseStatusAsync(string? user, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(user, nameof(user));

                DatabaseStatus databaseStatus = new()
                {
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false)
                };

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
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false)
                };

                if (!databaseStatus.CanConnect)
                {
                    _logger.LogDebug($"Creating the Atlas database {_applicationDbContext.Database.GetConnectionString()}");

                    databaseStatus.CanConnect = await _applicationDbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

                    logService.Log(Core.Logging.Enums.LogLevel.Information, $"Atlas database created {_applicationDbContext.Database.GetConnectionString()}", user);
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
                    CanConnect = await _applicationDbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false)
                };

                if (databaseStatus.CanConnect)
                {
                    bool generateSeedData = bool.Parse(_configuration["SeedData:GenerateSeedData"] ?? "false");
                    bool generateDemoLogs = bool.Parse(_configuration["SeedData:GenerateDemoLogs"] ?? "false");

                    if (generateSeedData)
                    {
                        SeedData.Generate(_applicationDbContext);

                        logService.Log(Core.Logging.Enums.LogLevel.Information, $"Atlas database seeded {_applicationDbContext.Database.GetConnectionString()}", user);

                        if (generateDemoLogs)
                        {
                            for (int i = 0; i < 500; i++)
                            {
                                logService.Log(Core.Logging.Enums.LogLevel.Information, new AtlasException("myNumber is zero", new DivideByZeroException(), "myNumber=0"), "test@email.com");
                                logService.Log(Core.Logging.Enums.LogLevel.Warning, new AtlasException("myVariable is null", new NullReferenceException("myVariable"), "myVariable=null"), "system@email.com");
                                logService.Log(Core.Logging.Enums.LogLevel.Error, new AtlasException("Boom!", new StackOverflowException(), "what the...."), "user@email.com");
                            }

                            logService.Log(Core.Logging.Enums.LogLevel.Information, $"Atlas database demo logs created {_applicationDbContext.Database.GetConnectionString()}", user);
                        }
                        else
                        {
                            logService.Log(Core.Logging.Enums.LogLevel.Information, "SeedData:GenerateDemoLogs = false", user);
                        }
                    }
                    else
                    {
                        logService.Log(Core.Logging.Enums.LogLevel.Warning, "SeedData:GenerateSeedData = false", user);
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
