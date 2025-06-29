using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryHub.Models
{
    public class Role
    {
        public int Id { get; set; }
        public int? PropertyId { get; set; }
        public virtual Property? Property { get; set; }
        public string Name { get; set; }
    }
}
