using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.EntityFrameworkCore;
using static MercuryHub.Services.BookingService;

namespace MercuryHub.ViewModels
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DurationDays { get; set; }
        public Brush Color { get; set; }
        public int propertyId { get; set; }
        public string ShowedName { get; set; }
        public int guestNumber { get; set; }

        // CalendarStart trebuie setat din exterior (ex: la încărcare viewmodel)
        public static DateOnly CalendarStart { get; set; }

        public double Offset { get; set; } = 0;
       
        public double DurationWidth => DurationDays * 100;

        public List<string> PreferredRooms { get; set; } = new(); // pentru tooltip
        public bool IsUnassigned { get; set; }
        public string concatenatedNotes { get; set; } = string.Empty;
    }


    public class RoomDTO
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string RoomType { get; set; }
        public ObservableCollection<ReservationDTO> Reservations { get; set; } = new();
    }
    public class ReservationViewModel: ViewModelBase
    {

        private Property _property {  get; set; }
        public ObservableCollection<RoomDTO> Rooms { get; set; } = new();
        public ObservableCollection<DateOnly> Days { get; set; } = new();
        public ObservableCollection<ReservationDTO> UnassignedReservations { get; set; } = new();
        private ObservableCollection<Reservation> _allReservations;
        public ICollectionView FilteredReservations { get; private set; }
        public Property Property
        {
            get { return _property; }
            set { _property = value; OnPropertyChanged(nameof(Property)); }
        }

        private DateOnly _calendarStartDate { get; set; }
        public DateOnly CalendarStartDate
        {
            get { return _calendarStartDate; }
            set { _calendarStartDate = value; OnPropertyChanged(nameof(CalendarStartDate)); }
        }
        private bool _isPopupOpen { get; set; }

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); }
        }

        private ReservationDTO _cancelReservation { get; set; }

        public ReservationDTO CancelReservation
        {
            get { return _cancelReservation; }
            set { _cancelReservation = value; OnPropertyChanged(nameof(CancelReservation)); }
        }
        private string _cancelReason { get; set; }

        public string CancelReason
        {
            get { return _cancelReason; }
            set { _cancelReason = value; OnPropertyChanged(nameof(CancelReason)); }
        }
        public ICommand BackMonth { get; }
        public ICommand NextMonth { get; }
        public ICommand CancelCommand { get; }

        public ReservationViewModel(Property property, ObservableCollection<Reservation> reservations)
        {
            _property = property;
            _allReservations = reservations;
            FilteredReservations = CollectionViewSource.GetDefaultView(_allReservations);
            FilteredReservations.Filter = r =>
            {
                var res = r as Reservation;
                return res != null && res.PropertyId == property.Id;
            };

            using var _localContext = new ApplicationDbContext();

            var roomsDB = _localContext.Rooms.Where(r => r.PropertyId == property.Id).ToList();

            var now = DateTime.Now;
            var start = new DateOnly(now.Year, now.Month, 1);
            var end = start.AddDays(31);
            CalendarStartDate = start;
            ReservationDTO.CalendarStart = start;
            foreach (var room in roomsDB)
            {
                var roomDto = new RoomDTO
                {
                    Id = room.Id,
                    RoomType = room.RoomType,
                    Reservations = new ObservableCollection<ReservationDTO>()
                };

                Rooms.Add(roomDto);
            }


            UpdateDtoFromFilteredReservations();

            _allReservations.CollectionChanged += (s, e) => UpdateDtoFromFilteredReservations();

           
            

           

            
            for (var date = start; date < end; date = date.AddDays(1)){
                Days.Add(date);
            }

            BackMonth = new RelayCommand(ExecuteBackMonth);
            NextMonth = new RelayCommand(ExecuteNextMonth);
            CancelCommand = new RelayCommand(ExecuteCancel, CanExecuteCancel);


        }

        private async void ExecuteCancel(object obj)
        {
            using var _localcontext = new ApplicationDbContext();
            var reservation = _localcontext.Reservations
                .Include(r => r.requestedRooms)
                .Where(r => r.Id == CancelReservation.Id && r.reservationStatus != ReservationStatus.Canceled)
                .FirstOrDefault();

            if (reservation == null)
            {
                ToastManager.Show("Reservation was canceled or does not exists anymore!");
                IsPopupOpen = false;
                return;
            }
            var principal = Thread.CurrentPrincipal;
            var user = _localcontext.Users.Where(u => u.userName == principal.Identity.Name).FirstOrDefault();

            reservation.reservationStatus = ReservationStatus.Canceled;
            reservation.UserId = user.Id;
            if (reservation.requestedRooms != null)
            {
                reservation.requestedRooms.Clear();
            }


            if (reservation.RemoteReservationId != null)
            {
                BookingService _bookingService =
                new BookingService(MercuryHub.Properties.Settings.Default.ApiKey);

                var request = new RejectReservationDTO
                {
                    reason = CancelReason,
                    reservationId = (int)reservation.RemoteReservationId
                };
                var result = await _bookingService.RejectReservationAsync(request);

                if (result)
                {
                    ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                    IsPopupOpen = false;
                    _localcontext.SaveChanges();
                    ToastManager.Show("Reservation canceled and synced!", ToastType.Success);
                }

            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                IsPopupOpen = false;
                _localcontext.SaveChanges();
                ToastManager.Show("Reservation canceled and synced!", ToastType.Success);

            }




        }

        private bool CanExecuteCancel(object obj)
        {
            return !string.IsNullOrWhiteSpace(CancelReason);
        }

        private void ExecuteNextMonth(object obj)
        {
            CalendarStartDate = CalendarStartDate.AddMonths(1);
            ReservationDTO.CalendarStart = CalendarStartDate;
            UpdateDtoFromFilteredReservations();
        }

        private void ExecuteBackMonth(object obj)
        {
            CalendarStartDate = CalendarStartDate.AddMonths(-1);
            ReservationDTO.CalendarStart = CalendarStartDate;
            UpdateDtoFromFilteredReservations();
        }

        private void UpdateDtoFromFilteredReservations()
        {
            var filteredItems = FilteredReservations.Cast<Reservation>();
            UnassignedReservations.Clear();
            foreach (var room in Rooms)
            {
                room.Reservations.Clear();
            }
            foreach (var r in filteredItems) {

                var notes = r.concatenatedNotes.ToLower().Split(';');
                var concatenatedNote = r.concatenatedNotes;

                Brush reservationColor = Brushes.LightBlue;
                switch (r.source)
                {
                    case "Web":
                        reservationColor = Brushes.LightPink; break;

                }


                string? email = notes.Length > 0 ? notes[0] : null;
                string? fullName = notes.Length > 1 ? notes[1] : null;
                string? phone = notes.Length > 2 ? notes[2] : null;

                if(r.reservationStatus == Services.BookingService.ReservationStatus.Pending)
                {
                    
                    UnassignedReservations.Add(new ReservationDTO
                    {
                        Id = r.Id,
                        propertyId = _property.Id,
                        GuestName = fullName ?? "???",
                        StartDate = r.checkIn,
                        EndDate = r.checkOut,
                        DurationDays = (r.checkOut.ToDateTime(TimeOnly.MinValue) - r.checkIn.ToDateTime(TimeOnly.MinValue)).Days,
                        concatenatedNotes = concatenatedNote,
                        Color = reservationColor,
                        guestNumber = r.numberOfGuests,
                        PreferredRooms = r.requestedRooms!.Select(r => r.RoomType).ToList()
                    });
                }
                
                if (r.reservationStatus == Services.BookingService.ReservationStatus.Confirmed 
                    && (r.checkIn.Month == ReservationDTO.CalendarStart.Month 
                    || r.checkOut.Month == ReservationDTO.CalendarStart.Month) )
                {
                    var offsetDays = (r.checkIn.DayNumber - ReservationDTO.CalendarStart.DayNumber);
                    double offsetPx = offsetDays * 100;
                    
                    
                    foreach(var requestedRoom in r.requestedRooms)
                    {
                        Rooms.Where(room => requestedRoom.RoomId == room.Id).First().Reservations.Add(new ReservationDTO
                        {
                            Id = r.Id,
                            propertyId = _property.Id,
                            GuestName = fullName ?? "???",
                            ShowedName = $"{fullName} | {r.BookingCode}",
                            StartDate = r.checkIn,
                            EndDate = r.checkOut,
                            guestNumber = r.numberOfGuests,
                            DurationDays = (r.checkOut.ToDateTime(TimeOnly.MinValue) - r.checkIn.ToDateTime(TimeOnly.MinValue)).Days,
                            Offset = offsetPx+50,
                            Color = reservationColor,
                            PreferredRooms = r.requestedRooms!.Select(r => r.RoomType).ToList()
                        });

                    }
                   

                }





            }
        }
    }
    }

