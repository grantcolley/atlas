using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Atlas.Data.Access.Data
{
    public class SupportData(ApplicationDbContext applicationDbContext, ILogger<SupportData> logger)
        : AuthorisationData<SupportData>(applicationDbContext, logger), ISupportData
    {
        private static JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        public async Task<IEnumerable<Log>> GetLogsAsync(LogArgs logArgs, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(logArgs, nameof(logArgs));

                List<string> level = [];

                if(!string.IsNullOrWhiteSpace(logArgs.Level))
                {
                    if (logArgs.Level == "Error")
                    {
                        level.Add("Error");
                    }
                    else if (logArgs.Level == "Warning")
                    {
                        level.AddRange(["Warning", "Error"]);
                    }
                }

                return await _applicationDbContext.Logs
                    .AsNoTracking()
                    .Where(l => l.TimeStamp >= logArgs.From && l.TimeStamp <= logArgs.To
                            && (level.Count == 0 || level.Contains(l.Level))
                            && (string.IsNullOrWhiteSpace(logArgs.Message) || l.Message.Contains(logArgs.Message))
                            && (string.IsNullOrWhiteSpace(logArgs.User) || l.User.Contains(logArgs.User))
                            && (string.IsNullOrWhiteSpace(logArgs.Context) || l.Context.Contains(logArgs.Context)))
                    .OrderBy(l => l.TimeStamp)
                    .ThenBy(l => l.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, JsonSerializer.Serialize(logArgs, jsonSerializerOptions));
            }
        }
    }
}
