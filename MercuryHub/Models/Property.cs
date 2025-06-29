using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryHub.Models
{
   public class Property
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }

        public virtual ICollection<Room>? Rooms { get; set; }
        
        public virtual ICollection<Role>? Roles { get; set; }
    }
}
