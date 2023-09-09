using Atlas.Data.Access.Context;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public abstract class DataBase<T> : IDisposable
    {
        protected readonly ApplicationDbContext applicationDbContext;
        protected readonly ILogger<T> logger;
        private bool disposedValue;

        protected DataBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    applicationDbContext.Dispose();
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
