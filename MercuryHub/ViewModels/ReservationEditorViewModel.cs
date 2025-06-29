using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub.ViewModels
{
    public class ReservationEditorDTO
    {
        public string? BookingCode { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? numberOfGuests { get; set; }
        public double? price { get; set; }
    }

   
   public class ReservationEditorViewModel : ViewModelBase
    {

        public ObservableCollection<AvabileRoomDTO> AvabileRooms { get; set; } = new();
        public ObservableCollection<AvabileRoomDTO> SelectedRooms { get; set; } = new();

        public ObservableCollection<Reservation> Reservations { get; set; } = new();
        private Property _property { get; set; }
        public Property Property
        {
            get { return _property; }
            set { _property = value; OnPropertyChanged(nameof(Property)); }
        }

        private ReservationEditorDTO _reservationDetalis { get; set; }
        public ReservationEditorDTO ReservationDetalis
        {
            get { return _reservationDetalis; }
            set { _reservationDetalis = value; OnPropertyChanged(nameof(ReservationDetalis));}
        }

        private string _fromName;
        public string FromName
        {
            get => _fromName;
            set
            {
                _fromName = value;
                OnPropertyChanged(nameof(FromName));
            }
        }

        private PersonalInformationDTO _persInfo;
        public PersonalInformationDTO PersonalInformation
        {
            get => _persInfo;
            set
            {
                _persInfo = value;
                OnPropertyChanged(nameof(PersonalInformation));
            }
        }
        private bool _avalability;
        public bool Avalability
        {
            get => _avalability;
            set
            {
                _avalability = value;
                OnPropertyChanged(nameof(Avalability));
            }
        }

        private Visibility _visibilitySearch;
        public Visibility VisibilitySearch
        {
            get => _visibilitySearch;
            set
            {
                _visibilitySearch = value;
                OnPropertyChanged(nameof(VisibilitySearch));
            }
        }
        private Visibility _visibilityCreate;
        public Visibility VisibilityCreate
        {
            get => _visibilityCreate;
            set
            {
                _visibilityCreate = value;
                OnPropertyChanged(nameof(VisibilityCreate));
            }
        }
        private Visibility _visibilityComplete;
        public Visibility VisibilityComplete
        {
            get => _visibilityComplete;
            set
            {
                _visibilityComplete = value;
                OnPropertyChanged(nameof(VisibilityComplete));
            }
        }
        public ICommand CreateReservationCommand { get; }
        public ICommand SearchReservtionCommand { get; }
        public ICommand ExecuteSearchCommand { get; }
        public ICommand ExecuteAvalabilityCommand { get; }
        public ICommand CompleteCommand { get; }
        public ICommand ToggleRoomCommand { get; }
        public ReservationEditorViewModel(Property property)
        {
            _property = property;
            CreateReservationCommand = new RelayCommand(ExecuteChangeToCreate);
            SearchReservtionCommand = new RelayCommand(ExecuteChangeToSearch);
            FromName = "Search Reservation";
            VisibilitySearch = Visibility.Visible;
            VisibilityCreate = Visibility.Collapsed;
            VisibilityComplete = Visibility.Collapsed;
            PersonalInformation = new PersonalInformationDTO();
            ReservationDetalis = new ReservationEditorDTO();
            ExecuteSearchCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
            ExecuteAvalabilityCommand = new RelayCommand(ExecuteAvalability, CanExecuteAvalability);
            CompleteCommand = new RelayCommand(ExecuteComplete, CanExecuteComplite);
            ToggleRoomCommand = new RelayCommand<AvabileRoomDTO>(ToggleRoom);
            Avalability = true;


        }

        private bool CanExecuteComplite(object obj)
        {
            bool havePersonal = 
                 !string.IsNullOrWhiteSpace(PersonalInformation.FirstName) &&
                 !string.IsNullOrWhiteSpace(PersonalInformation.LastName) &&
                 !string.IsNullOrWhiteSpace(PersonalInformation.PhoneNumber) &&
                 !string.IsNullOrWhiteSpace(PersonalInformation.Email);

            bool haveReservation = ReservationDetalis.numberOfGuests.HasValue && 
                ReservationDetalis.price.HasValue &&
                 ReservationDetalis.CheckIn.HasValue &&
                 ReservationDetalis.CheckOut.HasValue;

            bool testDates = ReservationDetalis.CheckIn < ReservationDetalis.CheckOut &&
                ReservationDetalis.CheckIn >= DateTime.Now.Date;

            return haveReservation && testDates && havePersonal &&(SelectedRooms.Count > 0);
        }

        private void ExecuteComplete(object obj)
        {
            using var _localContext = new ApplicationDbContext();

            /// for price and numberOfGuests tackes the last valid input
            /// t@mail.com;Admin User;0733412469
        
            var random = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            var dbReservation = new Reservation
            {
                checkIn = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckIn),
                checkOut = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckOut),
                reservationCost = (double)ReservationDetalis.price,
                concatenatedNotes = $"{PersonalInformation.Email};" +
                $"{PersonalInformation.FirstName} {PersonalInformation.LastName};" +
                $"{PersonalInformation.PhoneNumber}",
                numberOfGuests = (int)ReservationDetalis.numberOfGuests,
                source = "Local",
                PropertyId = Property.Id,
                BookingCode="LOC",
                reservationStatus = BookingService.ReservationStatus.Pending,
                
            };
            _localContext.Reservations.Add(dbReservation);
            _localContext.SaveChanges();
            dbReservation.BookingCode = $"LOC{dbReservation.Id}{random}";
            foreach(var room in SelectedRooms)
            {
                var dbRequestedRoom = new RequestedRooms
                {
                    reservation = dbReservation,
                    RoomType = room.Name,
                };
                _localContext.RequestedRooms.Add(dbRequestedRoom);
            }
            _localContext.SaveChanges();

            ToastManager.Show($"Reservation was created succesfuly! CODE:{dbReservation.BookingCode}",ToastType.Success);
            ExecuteChangeToSearch(obj);
        }

        private bool CanExecuteAvalability(object obj)
        {
       

            bool atleastOneReservation =
                 ReservationDetalis.CheckIn.HasValue &&
                 ReservationDetalis.CheckOut.HasValue;

            bool testDates = ReservationDetalis.CheckIn < ReservationDetalis.CheckOut &&
                ReservationDetalis.CheckIn >= DateTime.Now.Date;

                return atleastOneReservation && testDates;

        }
        private void ToggleRoom(AvabileRoomDTO room)
        {
            if (room.IsSelected)
            {
                if (!SelectedRooms.Contains(room))
                    SelectedRooms.Add(room);
            }
            else
            {
                SelectedRooms.Remove(room);
            }
        }
        private void ExecuteAvalability(object obj)
        {
            VisibilityComplete = Visibility.Visible;
            Avalability = false;
            using var _localContext = new ApplicationDbContext();

            //var
            var checkIn = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckIn);
            var checkOut = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckOut);
            var occupiedRoomIds = _localContext.Reservations
                .Where(r =>
                    r.reservationStatus == BookingService.ReservationStatus.Confirmed &&
                    r.checkIn >= checkIn &&
                    r.checkOut <= checkOut &&
                    r.PropertyId == Property.Id)
                .SelectMany(r => r.requestedRooms)
                .Select(rr => rr.RoomId)
                .Distinct()
                .ToList();

            var availableRooms = _localContext.Rooms
                .Where(r => !occupiedRoomIds.Contains(r.Id)).ToList();

            AvabileRooms.Clear();

            foreach (var room in availableRooms)
            {
                AvabileRooms.Add(new AvabileRoomDTO
                {
                    Id = room.Id,
                    SiteRoomId = room.SiteId,
                    capacity = room.capacity,
                    Name = room.RoomType

                });
            }

            ToastManager.Show($"Found {AvabileRooms.Count} rooms avabile for this reservation!", ToastType.Info);



        }

        private bool CanExecuteSearch(object obj)
        {
            bool atleastOnePersonal = !string.IsNullOrWhiteSpace(PersonalInformation.CNP) ||
                 !string.IsNullOrWhiteSpace(PersonalInformation.FirstName) ||
                 !string.IsNullOrWhiteSpace(PersonalInformation.LastName) ||
                 !string.IsNullOrWhiteSpace(PersonalInformation.PhoneNumber) ||
                 !string.IsNullOrWhiteSpace(PersonalInformation.Email);

            bool atleastOneReservation = !string.IsNullOrWhiteSpace(ReservationDetalis.BookingCode) ||
                 ReservationDetalis.CheckIn.HasValue ||
                 ReservationDetalis.CheckOut.HasValue;
            
            return atleastOnePersonal && atleastOneReservation;

        }

        private void ExecuteSearch(object obj)
        {
            using var _localContext = new ApplicationDbContext();

            var querry = _localContext.Reservations.Include(r=>r.requestedRooms).Include(r => r.Client).AsQueryable();


            if (!string.IsNullOrWhiteSpace(PersonalInformation.CNP))
            {
                querry = querry.Where(r => r.Client.CNP.Contains(PersonalInformation.CNP));
            }
            if (!string.IsNullOrWhiteSpace(PersonalInformation.LastName))
            {
                querry = querry.Where(r => r.concatenatedNotes
                .ToLower().Contains(PersonalInformation.LastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PersonalInformation.FirstName))
            {
                querry = querry.Where(r => r.concatenatedNotes
                .ToLower().Contains(PersonalInformation.FirstName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PersonalInformation.PhoneNumber))
            {
                querry = querry.Where(r => r.concatenatedNotes
                .Contains(PersonalInformation.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(PersonalInformation.Email))
            {
                querry = querry.Where(r => r.concatenatedNotes.ToLower()
                .Contains(PersonalInformation.Email.ToLower()));
            }


            if (!string.IsNullOrWhiteSpace(ReservationDetalis.BookingCode))
            {
                querry = querry.Where(r => r.BookingCode.ToLower()
                .Contains(ReservationDetalis.BookingCode.ToLower()));
            }

            if (ReservationDetalis.CheckIn.HasValue)
            {
                var date = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckIn);
                querry = querry.Where(r => r.checkIn >= date);
            }
            if (ReservationDetalis.CheckOut.HasValue)
            {
                var date = DateOnly.FromDateTime((DateTime)ReservationDetalis.CheckOut);
                querry = querry.Where(r => r.checkOut <= date);
            }
            var result = querry.ToList();
            Reservations.Clear();
            foreach (var r in result)
            {
                Reservations.Add(r);
            }

            ToastManager.Show($"Found {Reservations.Count} reservation for your search!");

        }

        private void ExecuteChangeToSearch(object obj)
        {
            FromName = "Search Reservation";
            VisibilitySearch = Visibility.Visible;
            VisibilityCreate = Visibility.Collapsed;
            VisibilityComplete = Visibility.Collapsed;
            Avalability = true;
            Reservations.Clear();
        }

        private void ExecuteChangeToCreate(object obj)
        {
            FromName = "Create Reservation";
            VisibilitySearch = Visibility.Collapsed;
            VisibilityCreate = Visibility.Visible;
            VisibilityComplete = Visibility.Collapsed;
            Avalability = true;

        }
    }
}
