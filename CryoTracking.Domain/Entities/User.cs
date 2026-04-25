using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        // Navigation
        public Role Role { get; set; } = null!;
        public ICollection<Log> Logs { get; set; }=new List<Log>();
    }
}
