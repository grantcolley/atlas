using Atlas.Data.Access.Context;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Base
{
    public abstract class DataBase<T>(ApplicationDbContext applicationDbContext, ILogger<T> logger) : IDisposable
    {
        protected readonly ApplicationDbContext _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        protected readonly ILogger<T> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
