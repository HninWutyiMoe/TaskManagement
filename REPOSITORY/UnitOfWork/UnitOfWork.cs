using REPOSITORY.IRepository;
using REPOSITORY.Repository;

namespace REPOSITORY.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            Role = new RoleRepository(_dataContext);
            Department = new DepartmentRepository(_dataContext);
            User = new UserRepository(_dataContext);    
            TaskHeader = new TaskHeaderRepository(_dataContext);
            TaskDetail = new TaskDetailRepository(_dataContext);
            UploadedFile = new UploadedFileRepository(_dataContext);
        }

        public IRoleRepository Role { get; private set; }
        public IDepartmentRepository Department {  get; private set; }
        public IUserRepository User { get; private set; }
        public ITaskHeaderRepository TaskHeader { get; private set; }
        public ITaskDetailRepository TaskDetail { get; private set; }
        public IUploadedFileRepository UploadedFile { get; private set; }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }
    }
}
