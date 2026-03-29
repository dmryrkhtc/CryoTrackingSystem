using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string? IPAddress { get; set; }

        //navigation properties
        public User User { get; set; }

    }
}
