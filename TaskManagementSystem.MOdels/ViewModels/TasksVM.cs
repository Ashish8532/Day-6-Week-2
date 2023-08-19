using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ViewModels
{
    public class TasksVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // Use string to hold the status name
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }
    }
}
