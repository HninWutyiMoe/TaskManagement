using MODEL.Eneities;
using REPOSITORY.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.Repository
{
    internal class TaskHeaderRepository : GenericRepository<TaskHeader>, ITaskHeaderRepository
    {
        public TaskHeaderRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
