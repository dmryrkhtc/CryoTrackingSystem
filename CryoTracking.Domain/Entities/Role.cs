using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
   public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        // Navigation
        public ICollection<User> Users { get; set; }
    }
}
