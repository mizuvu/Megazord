using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Zord.Core.Domain.Interfaces;

namespace Zord.EntityFrameworkCore
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        protected UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _context.Dispose();
                }
            }
            //dispose unmanaged resources
            _disposed = true;
        }
    }
}
