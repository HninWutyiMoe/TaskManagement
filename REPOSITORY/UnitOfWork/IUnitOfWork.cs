using REPOSITORY.IRepository;

namespace REPOSITORY.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository Role {  get; }
        IDepartmentRepository Department { get; }
        IUserRepository User { get; }
        ITaskDetailRepository TaskDetail { get; }
        ITaskHeaderRepository TaskHeader { get; }
        IUploadedFileRepository UploadedFile { get; }
        Task<int> SaveChangesAsync();
    }
}
