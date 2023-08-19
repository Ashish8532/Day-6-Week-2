using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Models.Models;

namespace TaskManagementSystem.DataAccess.Repository.IRepository
{
    public interface ITasksRepository : IRepository<Tasks>
    {
        Task<Tasks> UpdateAsync(Tasks task);
        Task<IEnumerable<Tasks>> Search(string searchTerm);
        Task<IEnumerable<Tasks>> SortByPriority();
        Task<IEnumerable<Tasks>> SortByDueDate();
    }
}
