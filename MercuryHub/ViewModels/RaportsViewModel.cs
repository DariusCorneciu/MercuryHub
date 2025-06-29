using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MercuryHub.Services;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub.ViewModels
{

    public class UserRaportDTO
    {
        public string userName { get; set; }
        public string roleName { get; set; }
        public int alocatedThisMonth { get; set; }
        public int rejectedThisMonth { get; set; }
        public double alocationRate { get; set; }
        public double rejectionRate { get; set; }
        public int grade { get; set; }

    }
   public class RaportsViewModel:ViewModelBase
    {

        public ObservableCollection<UserRaportDTO> UsersList { get; set; } = new();



        public RaportsViewModel() {
            GenerateUserList();
        }

        private void GenerateUserList()
        {
            using var _localContext = new ApplicationDbContext();
            var users = _localContext.Users.Include(r => r.Role).ToList();
            
            var thisYear = DateTime.Now.Year;
            var thisMonth = DateTime.Now.Month;

            var reservations = _localContext.Reservations
                .Where(r => r.checkIn.Year == thisYear && r.UserId != null)
                .GroupBy(r => r.UserId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var total = g.Count();
                        var alocatedThisMonth = g.Count(r =>
                            r.reservationStatus != BookingService.ReservationStatus.Canceled &&
                            (r.checkIn.Month == thisMonth || r.checkOut.Month == thisMonth));

                        var rejectionThisMonth = g.Count(r =>
                            r.reservationStatus == BookingService.ReservationStatus.Canceled &&
                            (r.checkIn.Month == thisMonth || r.checkOut.Month == thisMonth));

                        var allocationRate = total > 0
                            ? g.Count(r => r.reservationStatus != BookingService.ReservationStatus.Canceled) / (double)total
                            : 0;

                        var rejectionRate = total > 0
                            ? g.Count(r => r.reservationStatus == BookingService.ReservationStatus.Canceled) / (double)total
                            : 0;

                        var gradeRaw = (alocatedThisMonth * 0.5)
                                     - (rejectionThisMonth * 0.7)
                                     + (allocationRate * 5)
                                     - (rejectionRate * 3);

                        var grade = Math.Clamp((int)Math.Round(gradeRaw), 1, 10);

                        return new
                        {
                            alocatedThisMonth,
                            rejectionThisMonth,
                            allocationRate,
                            rejectionRate,
                            grade
                        };
                    });


            UsersList = new ObservableCollection<UserRaportDTO>();
            foreach (var us in users)
            {
                string role = "No Role";
                if (us.Role != null)
                {
                    role = us.Role.Name;
                }


                UsersList.Add(new UserRaportDTO
                {
                    userName = us.userName,
                    roleName = role,
                    alocatedThisMonth = reservations.ContainsKey(us.Id) ? reservations[us.Id].alocatedThisMonth : 0,
                    rejectedThisMonth = reservations.ContainsKey(us.Id) ? reservations[us.Id].rejectionThisMonth : 0,
                    alocationRate = reservations.ContainsKey(us.Id) ? reservations[us.Id].allocationRate : 0,
                    rejectionRate = reservations.ContainsKey(us.Id) ? reservations[us.Id].rejectionRate : 0,
                    grade = reservations.ContainsKey(us.Id) ? reservations[us.Id].grade : 0

                });

                
            }
        }
    }
}
