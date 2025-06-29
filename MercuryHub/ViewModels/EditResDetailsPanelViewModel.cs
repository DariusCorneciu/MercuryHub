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
using static MercuryHub.Services.BookingService;

namespace MercuryHub.ViewModels
{
    public class EditReservationDTO{
    public int ?numberOfGuests { get; set; }
        public double ?reservationCost { get; set; }
        public string bookingCode { get; set; }
        public DateTime ?checkIn { get; set; }
        public DateTime? checkOut { get; set; }
       
    
    }
    public class EditResDetailsPanelViewModel:ViewModelBase
    {
        public Reservation OriginalReservation { get; }

        //private readonly Action<object> _searchGuestsAction;
        private EditReservationDTO _reservation;
        public EditReservationDTO Reservation
        {
            get => _reservation;
            set
            {
                _reservation = value;
                OnPropertyChanged(nameof(Reservation));
            }
        }

       

        private Visibility _visibilityPanel;
        public Visibility VisibilityPanel
        {
            get => _visibilityPanel;
            set
            {
                _visibilityPanel = value;
                OnPropertyChanged(nameof(VisibilityPanel));
            }
        }
        private Visibility _visibilityButton;
        public Visibility VisibilityButton
        {
            get => _visibilityButton;
            set
            {
                _visibilityButton = value;
                OnPropertyChanged(nameof(VisibilityButton));
            }
        }

        private Visibility _visibilityFinal;
        public Visibility VisibilityFinal
        {
            get => _visibilityFinal;
            set
            {
                _visibilityFinal = value;
                OnPropertyChanged(nameof(VisibilityFinal));
            }
        }


        public ICommand CheckAvalability { get; }
        private bool _canSubmit;
        public bool CanSubmit
        {
            get => _canSubmit;
            set
            {
                _canSubmit = value;
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public ObservableCollection<AvabileRoomDTO> AvabileRooms { get; set; } = new();
        public ObservableCollection<AvabileRoomDTO> SelectedRooms { get; set; } = new();

        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ToggleRoomCommand { get; }
        public ICommand EditReservationCommand { get; }

        public EditResDetailsPanelViewModel(Reservation res)
        {
            OriginalReservation = res;
            VisibilityPanel = Visibility.Visible;
            VisibilityButton = Visibility.Hidden;
            Reservation = new EditReservationDTO
            {
                numberOfGuests = res.numberOfGuests,
                
                reservationCost = res.reservationCost,
                checkIn = res.checkIn.ToDateTime(TimeOnly.MinValue),
                checkOut = res.checkOut.ToDateTime(TimeOnly.MinValue),
                bookingCode = res.BookingCode

            };

            CanSubmit = false;
            VisibilityFinal = Visibility.Collapsed;
            EditCommand = new RelayCommand(ExecuteEdit);
            CloseCommand = new RelayCommand(ExecuteClose);
            CheckAvalability = new RelayCommand(ExecuteAvalabiliy, CanExecuteAvalability);
            ToggleRoomCommand = new RelayCommand<AvabileRoomDTO>(ToggleRoom);
            EditReservationCommand = new RelayCommand(ExecuteEditReservation, CanExecuteEditReservation);
        }

        private bool CanExecuteEditReservation(object obj)
        {
            /// sa fie la acelasi numar de mere.
            return OriginalReservation.requestedRooms.Count <= SelectedRooms.Count;
        }

        private async void ExecuteEditReservation(object obj)
        {

            using var _localContext = new ApplicationDbContext();

            var reservation = _localContext.Reservations
                .Where(r => r.Id == OriginalReservation.Id).Include(r=>r.requestedRooms).FirstOrDefault();
            if(reservation == null)
            {
                ToastManager.Show("Reservation was invalid, pleace sync!");
                VisibilityPanel = Visibility.Collapsed;
                return;
            }

            if (reservation.reservationCost <= Reservation.reservationCost)
            {
                reservation.reservationCost = (double)Reservation.reservationCost;
            }
            if (reservation.numberOfGuests >= Reservation.numberOfGuests)
            {
                reservation.numberOfGuests = (int)Reservation.numberOfGuests;
            }
            reservation.checkIn = DateOnly.FromDateTime(Reservation.checkIn.Value);
            reservation.checkOut = DateOnly.FromDateTime(Reservation.checkOut.Value);
            if(reservation.requestedRooms!= null)
                    reservation.requestedRooms.Clear();
            foreach(var room in SelectedRooms)
            {
                var dbRequestedRoom = new RequestedRooms
                {
                    reservation = reservation,
                    RoomId = room.Id,
                    RoomType = room.Name,

                };
                _localContext.Add(dbRequestedRoom);
            }

            if (reservation.RemoteReservationId != null)
            {
                //UpdateReservationAsync
                var _bookingService = 
                    new BookingService(MercuryHub.Properties.Settings.Default.ApiKey);

                var roomsSelected = new List<RoomAssignmentDTO>();

                foreach(var r in SelectedRooms)
                {
                    roomsSelected.Add(new RoomAssignmentDTO
                    {
                        roomId = r.SiteRoomId,
                        roomType = r.Name
                    });
                }

                var request = new UpdateReservationDTO
                {
                    checkIn = DateOnly.FromDateTime(Reservation.checkIn.Value),
                    checkOut = DateOnly.FromDateTime(Reservation.checkOut.Value),
                    reservationCost = Reservation.reservationCost,
                    reservationId = (int)reservation.RemoteReservationId,
                    rooms = roomsSelected
                };


                var result =  await _bookingService.UpdateReservationAsync(request);
                if (result)
                {
                    _localContext.SaveChanges();
                    ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                    ToastManager.Show("Reservation was succesfuly updated!", ToastType.Success);
                    VisibilityPanel = Visibility.Collapsed;
                }
                else
                {
                    ToastManager.Show("Error at api!", ToastType.Error);
                }

            }
            else
            {
                _localContext.SaveChanges();
                ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                ToastManager.Show("Reservation was succesfuly updated!", ToastType.Success);
                VisibilityPanel = Visibility.Collapsed;

            }

                

         
           


        }

        private void ExecuteAvalabiliy(object obj)
        {
            VisibilityFinal = Visibility.Visible;
            using var _localContext = new ApplicationDbContext();
            var checkIn = DateOnly.FromDateTime((DateTime)Reservation.checkIn);
            var checkOut = DateOnly.FromDateTime((DateTime)Reservation.checkOut);
            var occupiedRoomIds = _localContext.Reservations
                .Where(r =>
                    r.reservationStatus == BookingService.ReservationStatus.Confirmed &&
                    r.checkIn >= checkIn &&
                    r.checkOut <= checkOut &&
                    r.PropertyId == OriginalReservation.PropertyId && r.Id != OriginalReservation.Id)
                .SelectMany(r => r.requestedRooms)
                .Select(rr => rr.RoomId)
                .Distinct()
                .ToList();

            var selecedRequestedRooms = new List<int?>();
            if (OriginalReservation.requestedRooms!= null)
            {
                selecedRequestedRooms = OriginalReservation.requestedRooms.Select(r => r.RoomId).ToList();
            }
            


                var availableRooms = _localContext.Rooms
                    .Where(r => !occupiedRoomIds.Contains(r.Id)).ToList();

            AvabileRooms.Clear();
            SelectedRooms.Clear();
            foreach (var room in availableRooms)
            {
                var avabileRoom = new AvabileRoomDTO
                {
                    Id = room.Id,
                    SiteRoomId = room.SiteId,
                    capacity = room.capacity,
                    Name = room.RoomType,
                    IsSelected = selecedRequestedRooms.Contains(room.Id)

                };

                AvabileRooms.Add(avabileRoom);
                if (avabileRoom.IsSelected)
                {
                    SelectedRooms.Add(avabileRoom);
                }
            }

            ToastManager.Show($"Found {AvabileRooms.Count} rooms avabile for this reservation!", ToastType.Info);


        }

        private bool CanExecuteAvalability(object obj)
        {
            if (!Reservation.checkIn.HasValue || !Reservation.checkOut.HasValue)
                return false;

            if (Reservation.checkIn.Value > Reservation.checkOut.Value){
                return false;

            }
            if (Reservation.checkIn.Value.Date <= DateTime.Now.Date &&
               Reservation.checkOut.Value.Date <= DateTime.Now.Date)
            {
                return false;
            }
            if (Reservation.checkIn.Value.Date >= DateTime.Now.Date)
            {
                return true;
            }
            if (Reservation.checkOut.Value.Date < DateTime.Now.Date)
            {
                return true;
            }

           

            return false;

        }

        private void ExecuteEdit(object obj)
        {
            CanSubmit = !CanSubmit;

            if (VisibilityButton == Visibility.Hidden)
            {
                VisibilityButton = Visibility.Visible;
            }
            else
            {
                Reservation = new EditReservationDTO
                {
                   
                    reservationCost = OriginalReservation.reservationCost,
                    checkIn = OriginalReservation.checkIn.ToDateTime(TimeOnly.MinValue),
                    checkOut = OriginalReservation.checkOut.ToDateTime(TimeOnly.MinValue),
                    numberOfGuests = OriginalReservation.numberOfGuests,
                    
                };

                VisibilityButton = Visibility.Hidden;
            }

        }

        private void ExecuteClose(object obj)
        {
            VisibilityPanel = Visibility.Hidden;
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

    }



}
