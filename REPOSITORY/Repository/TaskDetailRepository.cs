using MODEL.Eneities;
using REPOSITORY.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.Repository
{
    internal class TaskDetailRepository : GenericRepository<TaskDetail>, ITaskDetailRepository
    {
        public TaskDetailRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
