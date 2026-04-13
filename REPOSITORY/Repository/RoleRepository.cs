using MODEL.Eneities;
using REPOSITORY.IRepository;

namespace REPOSITORY.Repository
{
    internal class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
