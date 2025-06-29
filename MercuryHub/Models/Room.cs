using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryHub.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomType { get; set; }
        public string ?Name { get; set; }
        public int SiteId { get; set; }
        public int capacity { get; set; }
       public ICollection<RequestedRooms>? requestedRooms { get; set; }
        public int PropertyId { get; set; }
        public virtual Property? Property { get; set; }
    }
}
