using Atlas.Data.Access.Context;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Base
{
    public abstract class DataBase<T> : IDisposable
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        protected readonly ILogger<T> _logger;
        private bool _disposedValue;

        protected DataBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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
