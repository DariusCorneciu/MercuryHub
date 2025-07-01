using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MercuryHub.Models;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub.Services
{
    public class BookingService
    {
        private readonly HttpClient _client;

        public class HotelDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class RoomDTO
        {
            public int PropertyId { get; set; }
            public int capacity { get; set; }
            public string roomType { get; set; }
            public int id { get; set; }
        }

        public class HubReservationDTO
        {
            public int reservationId { get; set; }
            public int propertyId { get; set; }
            public List<WantedRoomDTO> wantedRooms { get; set; }
            public DateOnly checkIn { get; set; }
            public DateOnly checkOut { get; set; }
            public string BookingCode { get; set; }
            public double reservationPayed { get; set; }
            public int numberOfGuests { get; set; }
            public string concatenatedNote { get; set; }
            public ReservationStatus reservationStatus { get; set; }
        }
        public class WantedRoomDTO
        {
            public int quantity { get; set; }
            public double unitPrice { get; set; }
            public string roomType { get; set; }

        }
        public enum ReservationStatus
        {
            NoPayment,
            Pending,
            Confirmed,
            Canceled,
            Completed
        }

        public class ConfirmReservationDTO
        {
            public int reservationId { get; set; }
            public List<RoomAssignmentDTO> rooms { get; set; }
        }
        public class RoomAssignmentDTO
        {
            public string roomType { get; set; }
            public int roomId { get; set; }
        }

        public class RejectReservationDTO
        {
            public string reason { get; set; }
            public int reservationId { get; set; }
        }

        public class UpdateReservationDTO
        {
            public int reservationId { get; set; }
            public double? reservationCost { get; set; }
            public DateOnly? checkIn { get; set; }
            public DateOnly? checkOut { get; set; }
            public List<RoomAssignmentDTO> rooms { get; set; }
        }

        public BookingService(string apiKey)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7194/"); 
            _client.DefaultRequestHeaders.Add("MercuryApiKey", apiKey);
        }

        public async Task<bool> TestApiKeyAsync()
        {
            var response = await _client.GetAsync("api/Hub/testKey");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ConfirmReservationAsync(ConfirmReservationDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Hub/ConfirmReservation", content);
            if (response.IsSuccessStatusCode)
            {

                return true;
            }
            return false;
            
        }

        public async Task<bool> UpdateReservationAsync(UpdateReservationDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync("api/Hub/UpdateReservation", content);
                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                return false;
            }
            catch (Exception ex) { 
            return false;
            }
           
        }
            

        public async Task<bool> RejectReservationAsync(RejectReservationDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Hub/RejectReservation", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }

        public async Task SyncReservationsAsync()
        {
            var response = await _client.GetAsync("api/Hub/reservations");
            using var _localContext = new ApplicationDbContext();
            if (response.IsSuccessStatusCode)
            {
                

                List<Property> properties = await _localContext.Properties.ToListAsync();
                var json = await response.Content.ReadAsStringAsync();
                    var remoteReservations = JsonSerializer.Deserialize<List<HubReservationDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });


                if (remoteReservations == null || !remoteReservations.Any())
                    return;

                foreach (var remoteReservation in remoteReservations)
                {
                    var localReservation = await _localContext.Reservations
                                                                    .Include(r => r.requestedRooms)
                        .FirstOrDefaultAsync(r => r.RemoteReservationId == remoteReservation.reservationId);
                    if (localReservation == null)
                    {
                        var newReservation = MapHubReservationToLocalReservation(remoteReservation, properties);
                        _localContext.Reservations.Add(newReservation);
                    }
                    else
                    {

                        UpdateLocalReservation(localReservation, remoteReservation);
                        if(remoteReservation.reservationStatus == ReservationStatus.Canceled)
                        {
                            foreach (var rem in localReservation.requestedRooms)
                                _localContext.RequestedRooms.Remove(rem);
                        }
                        
                    }
                }

                await _localContext.SaveChangesAsync();
            }

        }
        public async Task CreateFontDeskRolesAsync()
        {
           
            var response = await _client.GetAsync("api/Hub/hotels");

            using var db = new ApplicationDbContext();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var hotels = JsonSerializer.Deserialize<List<HotelDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (hotels != null)
                {
                         foreach (var hotel in hotels)
                        {
                            if(!db.Properties.Any(hr=>hr.SiteId == hotel.Id))
                            {
                                var aded = new Property
                                {
                                    Name = hotel.Name,
                                    SiteId = hotel.Id,

                                };
                                db.Properties.Add(aded);
                                db.Roles.Add(new Role
                                {
                                    Property = aded,
                                    Name = $"FontDesk {hotel.Name}"
                                });
                                await db.SaveChangesAsync();
                            }

                            
                            
                        }

                        
                   
                }



                if (!db.Rooms.Any())
                {
                    await CreateRoomsAsync();
                }
                    
               
                    
            }
        }



        private Reservation MapHubReservationToLocalReservation(HubReservationDTO remote, List<Property> properties)
        {
            var reservation = new Reservation
            {
                reservationStatus = remote.reservationStatus,
                RemoteReservationId = remote.reservationId,
                BookingCode = remote.BookingCode,
                checkIn = remote.checkIn,
                checkOut = remote.checkOut,
                numberOfGuests = remote.numberOfGuests,
                reservationCost = remote.reservationPayed,
                concatenatedNotes = remote.concatenatedNote,
                Property = properties.Where(p => p.SiteId == remote.propertyId).First(),
                // setează alte proprietăți dacă ai în local
                source = "Web"
            };

            var listRequestedRooms = new List<RequestedRooms>();
            foreach(var room in remote.wantedRooms)
            {
                listRequestedRooms.Add(new RequestedRooms
                {
                    reservation = reservation,
                    RoomType = room.roomType,
                });
            }

            reservation.requestedRooms = listRequestedRooms;
            return reservation;
        }
        private void UpdateLocalReservation(Reservation local, HubReservationDTO remote)
        {
            
            local.BookingCode = remote.BookingCode;
            local.checkIn = remote.checkIn;
            local.checkOut = remote.checkOut;
            local.numberOfGuests = remote.numberOfGuests;
            local.concatenatedNotes = remote.concatenatedNote;
            
            local.reservationCost = remote.reservationPayed;
            if(local.reservationStatus != remote.reservationStatus)
            {
                local.reservationStatus = remote.reservationStatus;
            }
        }
        




        public async Task CreateRoomsAsync()
        {
            var response = await _client.GetAsync("api/Hub/rooms");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var rooms = JsonSerializer.Deserialize<List<RoomDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (rooms != null)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var properties = db.Properties.ToList();
                        foreach (var room in rooms)
                        {

                            var requestedProperty = properties.Find(r => r.SiteId == room.PropertyId);
                            db.Rooms.Add(new Room
                            {
                                RoomType = room.roomType,
                                Property = requestedProperty,
                                SiteId = room.id,
                                capacity = room.capacity,
                            });

                        }

                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
