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

        public async Task<IEnumerable<Log>?> GetLogsAsync(LogArgs logArgs, CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Logs
                    .AsNoTracking()
                    .OrderBy(l => l.TimeStamp)
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
