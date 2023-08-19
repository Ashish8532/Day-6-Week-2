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
    public class CommentRepository : Repository<Comments>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
