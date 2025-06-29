using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryHub.Models
{
   public  class ApplicationUser
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
