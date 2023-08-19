using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DataAccess.Data;
using TaskManagementSystem.DataAccess.Repository.IRepository;
using TaskManagementSystem.Models.Models;

namespace TaskManagementSystem.DataAccess.Repository
{
    public class TasksRepository : Repository<Tasks>, ITasksRepository
    {
        private readonly ApplicationDbContext _context;

        public TasksRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

       

        public async Task<Tasks> UpdateAsync(Tasks task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<Tasks>> Search(string searchTerm)
        {
            return await _context.Tasks.Where(task => task.Title.ToLower().Contains(searchTerm.ToLower()) 
            || task.Description.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> SortByPriority()
        {
            return await _context.Tasks.OrderBy(task => task.Priority).ToListAsync();
        }


        public async Task<IEnumerable<Tasks>> SortByDueDate()
        {
            return await _context.Tasks.OrderBy(task => task.DueDate).ToListAsync();
        }
    }
}
