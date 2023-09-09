using Atlas.Data.Access.Context;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public abstract class DataBase<T> : IDisposable
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        protected readonly ILogger<T> _logger;
        private bool disposedValue;

        protected DataBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
