using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public StatusType Status { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityType Priority { get; set; }

        
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }


    }
}
