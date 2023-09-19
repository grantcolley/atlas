using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class ComponentArgsData : AuthorisationData<ComponentArgsData>, IComponentArgsData
    {
        public ComponentArgsData(ApplicationDbContext applicationDbContext, ILogger<ComponentArgsData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<ComponentArgs?> GetComponentArgsAsync(string? componentCode, CancellationToken cancellationToken)
        {
            if(componentCode == null) throw new ArgumentNullException(nameof(componentCode));

            return await _applicationDbContext.ComponentArgs
                .AsNoTracking()
                .FirstOrDefaultAsync(cd => cd.ComponentCode.Equals(componentCode), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
