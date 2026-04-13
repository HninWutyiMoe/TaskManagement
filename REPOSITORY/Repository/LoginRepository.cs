using MODEL.DTO;
using REPOSITORY.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.Repository
{
    internal class LoginRepository : GenericRepository<LoginDTO>, ILoginRepository
    {
        public LoginRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
