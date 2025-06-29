using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryHub.Models
{
    public class RequestedRooms
    {
        public int Id { get; set; }
        public string RoomType { get; set; }
        public int ?RoomId { get; set; }
        public virtual Room? room { get; set; }
        public int ReservationId { get; set; }

        public Reservation? reservation { get; set; }
        
    }
}
