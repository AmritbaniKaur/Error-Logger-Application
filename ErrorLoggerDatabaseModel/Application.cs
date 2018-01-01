using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ErrorLoggerDatabaseModel
{
    public partial class Application
    {
        public Application()
        {
            Users = new HashSet<User>();
        }
   
        public int ApplicationId { get; set; }

        [Required]
        [StringLength(15)]
        public string ApplicationName { get; set; }

        [Required]
        [StringLength(15)]
        public string ApplicationType { get; set; }

        [Required]
        [StringLength(20)]
        public string ApplicationStatus { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
