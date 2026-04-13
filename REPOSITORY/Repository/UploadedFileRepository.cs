using MODEL.Eneities;
using REPOSITORY.IRepository;

namespace REPOSITORY.Repository
{
    internal class UploadedFileRepository : GenericRepository<UploadedFile>, IUploadedFileRepository
    {
        public UploadedFileRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
