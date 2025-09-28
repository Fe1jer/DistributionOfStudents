using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        bool Commit();
    }
}
