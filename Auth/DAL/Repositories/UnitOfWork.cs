using DAL.Context;
using DAL.Repositories.Custom;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;

        private IUserRepository? _users;
        private IUserTokenRepository? _userTokens;

        private bool _disposed;

        public UnitOfWork(AuthDbContext context) => _context = context;

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IUserTokenRepository UserTokens => _userTokens ??= new UserTokenRepository(_context);

        public bool Commit()
        {
            return _context.SaveChanges() != 0;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public void Dispose()
        {
            Cleanup(true);
            GC.SuppressFinalize(this);
        }

        private void Cleanup(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        ~UnitOfWork()
        {
            Cleanup(false);
        }
    }
}
