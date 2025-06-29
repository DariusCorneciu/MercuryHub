using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MercuryHub.CustomControls;
using MercuryHub.Services;

namespace MercuryHub.ViewModels
{
    public class GuestDetailsPanelViewModel:ViewModelBase
    {

        public GuestDTO OriginalGuest { get; }

        private readonly Action<object> _searchGuestsAction;
        private GuestDTO _guest;
        public GuestDTO Guest {
            get => _guest;
            set
            {
                _guest = value;
                OnPropertyChanged(nameof(Guest));
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

        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveChangesCommand{ get; }
        public GuestDetailsPanelViewModel(GuestDTO guest)
        {

            Guest = guest;
            OriginalGuest = new GuestDTO();
            OriginalGuest.CopyGuest(guest);
            CloseCommand = new RelayCommand(ExecuteClose);
            VisibilityPanel = Visibility.Visible;
            CanSubmit = false;
            EditCommand = new RelayCommand(ExecuteEdit);
            VisibilityButton = Visibility.Hidden;
            SaveChangesCommand = new RelayCommand(SaveChanges, CanExecuteSave);
           
        }

        private bool CanExecuteSave(object obj)
        {
            return !Guest.SameGuest(OriginalGuest);
        }

        private void SaveChanges(object obj)
        {
            using var _localContext = new ApplicationDbContext();
            var client = _localContext.Clients.Where(c => c.Id == OriginalGuest.id).FirstOrDefault();
            if(client != null)
            {
                client.Email = Guest.Email;
                client.CNP = Guest.CNP;
                client.Address = Guest.Address;
                client.PhoneNumber = Guest.PhoneNumber;
                client.FirstName = Guest.FirstName;
                client.LastName = Guest.LastName;
                _localContext.SaveChanges();
                ToastManager.Show($"The guest {client.FirstName} {client.LastName} was updated!", ToastType.Success);


            }
            else
            {
                ToastManager.Show($"The guest was not found!", ToastType.Error);

            }
                


        }

        private void ExecuteEdit(object obj)
        {
            CanSubmit = !CanSubmit;

            if(VisibilityButton == Visibility.Hidden)
            {
                VisibilityButton = Visibility.Visible;
            }
            else
            {
                Guest.CopyGuest(OriginalGuest);
                Guest = Guest;
                VisibilityButton = Visibility.Hidden;
            }
            
        }

        private void ExecuteClose(object obj)
        {
            VisibilityPanel = Visibility.Hidden;
        }


        
    }
}
