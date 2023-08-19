using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string? Content { get; set; }

        public int TasksId { get; set; }
        public Tasks Tasks { get; set; }

    }
}
