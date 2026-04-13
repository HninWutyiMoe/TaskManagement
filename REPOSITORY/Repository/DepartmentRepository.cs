using MODEL.Eneities;
using REPOSITORY.IRepository;

namespace REPOSITORY.Repository
{
    internal class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
