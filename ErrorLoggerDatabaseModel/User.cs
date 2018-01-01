using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ErrorLoggerDatabaseModel
{
    public partial class User
    {
        public User()
        {
            Applications = new HashSet<Application>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [Required]
        [StringLength(20)]
        public string UserStatus { get; set; }

        [Required]
        [StringLength(20)]
        public string LastLoginDate { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
