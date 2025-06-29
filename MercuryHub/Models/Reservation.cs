using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MercuryHub.Services.BookingService;

namespace MercuryHub.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string BookingCode { get; set; }
        public string source { get; set; }
        public double reservationCost { get; set; }
        public int numberOfGuests { get; set; }
        public DateOnly checkIn { get; set; }
        public DateOnly checkOut { get; set; }
        public int? ClientId { get; set; }

        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        
        public string concatenatedNotes { get; set; }
        public int? RemoteReservationId { get; set; }
        public virtual Client? Client { get; set; }
        public ReservationStatus reservationStatus { get; set; }
        public int? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<RequestedRooms>? requestedRooms { get; set; }
    }
}
