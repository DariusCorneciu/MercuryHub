using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.EntityFrameworkCore;
using static MercuryHub.Services.BookingService;

namespace MercuryHub.ViewModels
{

    public class AvabileRoomDTO
    {
        public int Id { get; set; }
        public int SiteRoomId { get; set; }
        public string Name { get; set; }
        public int capacity { get; set; }
        public bool IsSelected { get; set; } = false;
    }
    public class ReservationDetailPanelViewModel : ViewModelBase
    {
    
        public ReservationDTO Reservation { get; }
        private string _checkOut;
        public string checkOut
        {
            get => _checkOut;
            set
            {
                _checkOut = value;
                OnPropertyChanged(nameof(checkOut));
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

        private string _searchBox;
        private List<Client> _clients;
        public List<Client> MatchingClients
        {
            get { return _clients; }
            set { _clients = value; OnPropertyChanged(nameof(MatchingClients)); }
        }

        private Client _client;
        public Client SelectedClient
        {
            get { return _client; }
            set { _client = value; OnPropertyChanged(nameof(SelectedClient));
                CommandManager.InvalidateRequerySuggested();

            }
        }

        private bool _executeGenerate = false;
        public string SearchBox
        {
            get { return _searchBox; }
            set { _searchBox = value; OnPropertyChanged(nameof(SearchBox)); }
        }

        private Visibility _visibilityClients;
        public Visibility VisibilityClients
        {
            get => _visibilityClients;
            set
            {
                _visibilityClients = value;
                OnPropertyChanged(nameof(VisibilityClients));
            }
        }

        private Visibility _visibilityRooms;
        public Visibility VisibilityRooms
        {
            get => _visibilityRooms;
            set
            {
                _visibilityRooms = value;
                OnPropertyChanged(nameof(VisibilityRooms));
            }
        }
        private Visibility _visibilityCalledGuest;
        public Visibility VisibilityCalledGuest
        {
            get => _visibilityCalledGuest;
            set
            {
                _visibilityCalledGuest = value;
                OnPropertyChanged(nameof(VisibilityCalledGuest));
            }
        }

        private bool _calledGuest;
        public bool CalledGuest
        {
            get => _calledGuest;
            set
            {
                _calledGuest = value;
                OnPropertyChanged(nameof(CalledGuest));
            }
        }


        private string _rejectReason;
        public string RejectReason
        {
            get { return _rejectReason; }
            set { _rejectReason = value; OnPropertyChanged(nameof(RejectReason)); }
        }

        public ObservableCollection<AvabileRoomDTO> AvabileRooms { get; set; } = new();
        public ObservableCollection<AvabileRoomDTO> SelectedRooms { get; set; } = new();
        public ICommand GenerateCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand FinishCommand { get; }
        public ICommand ToggleRoomCommand { get; }
        public ICommand RejectCommand { get; }
        public ReservationDetailPanelViewModel(ReservationDTO reservationDTO)
        {
            _searchBox = "";
            Reservation = reservationDTO;
            SearchCommand = new RelayCommand(ExecuteSeachCommand);
            GenerateCommand = new RelayCommand(ExecuteGenerateCommand,CanExecuteGenerate);
            NextCommand = new RelayCommand(ExecuteNext, CanExecuteNext);
            CloseCommand = new RelayCommand(ExecuteClose);
            FinishCommand = new RelayCommand(ExecuteFinish, CanExecuteFinish);
            RejectCommand = new RelayCommand(ExecuteReject, CanExecuteReject);
            VisibilityPanel = Visibility.Visible;
            VisibilityClients = Visibility.Visible;
            VisibilityRooms = Visibility.Collapsed;
            ToggleRoomCommand = new RelayCommand<AvabileRoomDTO>(ToggleRoom);
            checkOut = reservationDTO.StartDate.AddDays(reservationDTO.DurationDays).ToString("dd MMM yyyy");
            VisibilityCalledGuest = Visibility.Collapsed;
            CalledGuest = false;
            


        }

        private bool CanExecuteReject(object obj)
        {
            return !string.IsNullOrWhiteSpace(RejectReason);
        }

        private async void ExecuteReject(object obj)
        {
            using var _localcontext = new ApplicationDbContext();
            var reservation = _localcontext.Reservations
                .Include(r => r.requestedRooms)
                .Where(r => r.Id == Reservation.Id && r.reservationStatus != ReservationStatus.Canceled)
                .FirstOrDefault();

            if(reservation == null)
            {
                ToastManager.Show("Reservation was canceled or does not exists anymore!");
                VisibilityPanel = Visibility.Hidden;
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
            
            
            if(reservation.RemoteReservationId != null)
            {
                BookingService _bookingService =
                new BookingService(MercuryHub.Properties.Settings.Default.ApiKey);

                var request = new RejectReservationDTO
                {
                    reason = RejectReason,
                    reservationId = (int)reservation.RemoteReservationId
                };
                var result = await _bookingService.RejectReservationAsync(request);

                if (result)
                {
                    ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                    VisibilityPanel = Visibility.Hidden;
                    _localcontext.SaveChanges();
                    ToastManager.Show("Reservation canceled and synced!", ToastType.Success);
                }

            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                _localcontext.SaveChanges();
                VisibilityPanel = Visibility.Hidden;
                ToastManager.Show("Reservation canceled and synced!", ToastType.Success);
            }
            

            

        }

        private void ToggleRoom(AvabileRoomDTO room)
        {
             if (room.IsSelected){
                if (!SelectedRooms.Contains(room))
                    SelectedRooms.Add(room);
            }
            else{
                SelectedRooms.Remove(room);
            }
        }

        private bool CanExecuteFinish(object obj)
        {
            if(SelectedRooms.Count < Reservation.PreferredRooms.Count)
            {
                return false;
            }

            else
            {

                var dictionaryPreferredRooms = Reservation.PreferredRooms
        .GroupBy(room => room)
        .ToDictionary(group => group.Key, group => group.Count());

                var dictionarySelected = SelectedRooms
                    .GroupBy(room => room.Name)
                    .ToDictionary(group => group.Key, group => group.Count());

                bool areIdentical = dictionaryPreferredRooms.Count == dictionarySelected.Count &&
                                    dictionaryPreferredRooms.All(kv =>
                                        dictionarySelected.TryGetValue(kv.Key, out int count) &&
                                        count == kv.Value);


                if (areIdentical)
                {
                    VisibilityCalledGuest = Visibility.Collapsed;
                    CalledGuest = false;
                    return true;
                }
                VisibilityCalledGuest = Visibility.Visible;
                return CalledGuest;
            }
        }

        private async void ExecuteFinish(object obj)
        {
            using var _localContext = new ApplicationDbContext();
            var reservation = _localContext.Reservations
                .Include(r => r.requestedRooms)
                .FirstOrDefault(r => r.Id == Reservation.Id && r.reservationStatus == BookingService.ReservationStatus.Pending);

            if (reservation == null)
            {
                ToastManager.Show("Reload page, reservation was confirmed/canceled!");
                return;
            }

            reservation.requestedRooms.Clear();
            foreach (var selected in SelectedRooms)
            {
                reservation.requestedRooms.Add(new RequestedRooms
                {
                    RoomId = selected.Id,
                    RoomType = selected.Name
                });
            }
            reservation.reservationStatus = BookingService.ReservationStatus.Confirmed;

            var principal = Thread.CurrentPrincipal;
            var user = _localContext.Users.Where(u => u.userName == principal.Identity.Name).FirstOrDefault();

            reservation.Client = SelectedClient;
            reservation.UserId = user.Id;
            
            


            if (reservation.RemoteReservationId.HasValue)
            {
                var rooms = new List<BookingService.RoomAssignmentDTO>();
                foreach (var selected in SelectedRooms)
                {
                    rooms.Add(new RoomAssignmentDTO
                    {
                        roomId = selected.SiteRoomId,
                        roomType = selected.Name,
                    });
                }

                var updateBookindDB = new BookingService.ConfirmReservationDTO
                {
                    reservationId = (int)reservation.RemoteReservationId,
                    rooms = rooms,
                };

                BookingService _bookingService = new BookingService(MercuryHub.Properties.Settings.Default.ApiKey);

                var result  = await _bookingService.ConfirmReservationAsync(updateBookindDB);

                if (result)
                {
                    ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                    VisibilityPanel = Visibility.Hidden;
                    _localContext.SaveChanges();
                    ToastManager.Show("Reservation saved and synced!",ToastType.Success);

                }

            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).SyncAndUpdateViewModel();
                VisibilityPanel = Visibility.Hidden;
                _localContext.SaveChanges();
                ToastManager.Show("Reservation saved and synced!", ToastType.Success);
            }
            

           
        }


        private void ExecuteClose(object obj)
        {
            VisibilityPanel = Visibility.Hidden;
        }

        private bool CanExecuteNext(object obj)
        {
            return SelectedClient != null;
        }

        private void ExecuteNext(object obj)
        {
            using var _localContext = new ApplicationDbContext();

            VisibilityClients = Visibility.Collapsed;
            VisibilityRooms = Visibility.Visible;

            var reservationCheckOut = Reservation.StartDate.AddDays(Reservation.DurationDays);

          
            var occupiedRoomIds = _localContext.Reservations
                .Where(r =>
                    r.reservationStatus == BookingService.ReservationStatus.Confirmed &&
                    r.checkIn >= Reservation.StartDate &&
                    r.checkOut <= reservationCheckOut &&
                    r.PropertyId == Reservation.propertyId)
                .SelectMany(r => r.requestedRooms)
                .Select(rr => rr.RoomId)
                .Distinct()
                .ToList();

            var availableRooms = _localContext.Rooms
                .Where(r => !occupiedRoomIds.Contains(r.Id))
                .ToList();
           
            AvabileRooms.Clear();

            foreach(var room in availableRooms)
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
        private bool CanExecuteGenerate(object obj)
        {
            return _executeGenerate;
        }

        private void ExecuteGenerateCommand(object obj)
        {
           
            var noteList = Reservation.concatenatedNotes.Split(';');

            string? email = noteList.Length > 0 ? noteList[0] : null;
            string? fullName = noteList.Length > 1 ? noteList[1] : null;
            string? phone = noteList.Length > 2 ? noteList[2] : null;

                
           if(string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(fullName) 
                && string.IsNullOrWhiteSpace(phone)){
                ToastManager.Show("Can't generate partial user! To frew information about reservation!",ToastType.Error);
            }
            else{
                using var _localContext = new ApplicationDbContext();
                var nameList = fullName.Split(' ');
                var clientDB = new Client
                {
                    Email = email,
                    FirstName = nameList[0],
                    LastName = nameList[1],
                    PhoneNumber = phone

                };
                _localContext.Clients.Add(clientDB);
                _localContext.SaveChanges();
                ToastManager.Show("Createad partial user, search again!", ToastType.Success);
            }

               


            _executeGenerate = false;
        }

        private void ExecuteSeachCommand(object sender)
        {
            VisibilityClients = Visibility.Visible;
            VisibilityRooms = Visibility.Collapsed;
            VisibilityCalledGuest = Visibility.Collapsed;
            CalledGuest = false;
            SelectedRooms.Clear();

            string query = SearchBox.Trim();
            
            var reservation = Reservation;
            var concatenatedNotes = reservation.concatenatedNotes;
            var context = new ApplicationDbContext();

            IQueryable<Client> guestsQuery = context.Clients;

            if (!string.IsNullOrEmpty(concatenatedNotes))
            {
                var noteList = concatenatedNotes.Split(';');  // email;fullName;phoneNumber

                string? email = noteList.Length > 0 ? noteList[0] : null;
                string? fullName = noteList.Length > 1 ? noteList[1] : null;
                string? phone = noteList.Length > 2 ? noteList[2] : null;

                if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(fullName) || !string.IsNullOrWhiteSpace(phone))
                {
                    guestsQuery = guestsQuery.Where(g =>
                        (!string.IsNullOrWhiteSpace(email) && g.Email.Contains(email)) ||
                        (!string.IsNullOrWhiteSpace(fullName) && (g.FirstName + " " + g.LastName).Contains(fullName)) ||
                        (!string.IsNullOrWhiteSpace(phone) && g.PhoneNumber.Contains(phone))
                    );
                }

            }


            if (!string.IsNullOrEmpty(query))
            {
                guestsQuery = guestsQuery.Where(g =>
                                    g.Email.Contains(query) ||
                                    g.FirstName.Contains(query) ||
                                    g.LastName.Contains(query) ||
                                    g.CNP.Contains(query) ||
                                    g.Address.Contains(query));
            }

            MatchingClients = guestsQuery.ToList();

            if (MatchingClients.Any())
            {
                _executeGenerate = false;
                ToastManager.Show($"Possible Guests Found!", ToastType.Success);
            }
            else
            {
                _executeGenerate = true;
               ToastManager.Show($"No Guest Found!", ToastType.Error);
            }

        }
    }
}
