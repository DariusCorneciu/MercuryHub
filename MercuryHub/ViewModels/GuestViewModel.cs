using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MercuryHub.ViewModels
{
    public class GuestDTO {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CNP { get; set; }
        public int id { get; set; }

        public List<Reservation> reservations { get; set; } = new();
        public void CopyGuest(GuestDTO b)
        {
          
            FirstName = b.FirstName;
            LastName = b.LastName;
            Email = b.Email;
            PhoneNumber = b.PhoneNumber;
            Address = b.Address;
            id = b.id;
            CNP = b.CNP;
        }
        public bool SameGuest(GuestDTO guest)
        {
            if (guest == null)
                return false;

            return FirstName == guest.FirstName &&
                   LastName == guest.LastName &&
                   FullName == guest.FullName &&
                   Email == guest.Email &&
                   PhoneNumber == guest.PhoneNumber &&
                   Address == guest.Address &&
                   CNP == guest.CNP;

        }

    }

    public class PersonalInformationDTO{
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LastName { get; set; }
        public string? CNP { get; set; }

    }

    public class AddressDTO
    {
        public string? Country { get; set; }
        public string? Town { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Building { get; set; }
        public string? Scara { get; set; }
        public string? Apartment { get; set; }
    }

   


    public class GuestViewModel : ViewModelBase
    {
        public ObservableCollection<GuestDTO> PossibleGuests { get; set; } = new();
        public ICommand CreateGuestCommand { get; }
        public ICommand SearchGuestCommand { get; }
       
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

        public ICommand CreateGuestButton { get; }
        public ICommand SearchGuestButton { get; }

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
        private AddressDTO _address;
        public AddressDTO Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public GuestViewModel()
        {
            // PostEditUserCommand = new RelayCommand(PostEditUser);
            CreateGuestCommand = new RelayCommand(ChangeToCreateGuestForm);
            SearchGuestCommand = new RelayCommand(ChangeToSearchGuestForm);
            FromName = "Search Guest";
            VisibilitySearch = Visibility.Visible;
            VisibilityCreate = Visibility.Collapsed;
            SearchGuestButton = new RelayCommand(SearchGuests);
            PersonalInformation = new PersonalInformationDTO();


            CreateGuestButton = new RelayCommand(CreateGuest,ExecuteCreateGuest);
            Address = new AddressDTO();


        }
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? Email { get; set; }
        //public string? PhoneNumber { get; set; }
        private bool ExecuteCreateGuest(object obj)
        {
            if(string.IsNullOrEmpty(PersonalInformation.FirstName) 
                || string.IsNullOrEmpty(PersonalInformation.LastName)
                || string.IsNullOrEmpty(PersonalInformation.Email)
                || string.IsNullOrEmpty(PersonalInformation.PhoneNumber))
            {
                return false;
            }
            if (!PersonalInformation.PhoneNumber.All(char.IsDigit))
            {
                return false;
            }

            /// la adressa, daca sunt toate nulle e ok, dar daca exista una din ele trebuie sa fie minimul

            return true;
            
        }

        private void CreateGuest(object obj)
        {
            /// search if exists some guest with this properties
            /// cauta dupa email, nrTelefon, cnp(daca exista)

            using var _localContext = new ApplicationDbContext();
            var guests = _localContext.Clients.Where(p =>
            p.Email.Contains(PersonalInformation.Email.ToLower()) ||
            p.PhoneNumber.Contains(PersonalInformation.PhoneNumber) ||
            p.CNP.Contains(PersonalInformation.CNP)).ToList();

            if(guests.Count > 0){
                ToastManager.Show($"Found {guests.Count} possible guests!", ToastType.Info);
                ChangeToSearchGuestForm(obj);
                foreach (var guest in guests)
                {
                    PossibleGuests.Add(new GuestDTO
                    {
                        Email = guest.Email ?? "NotFound",
                        PhoneNumber = guest.PhoneNumber ?? "NotFound",
                        Address = guest.Address ?? "NotFound",
                        CNP = guest.CNP ?? "NotFound",
                        FirstName = guest.FirstName,
                        LastName = guest.LastName,
                        id = guest.Id
                    });
                }
            }
            else
            {
                var dbGuest = new Client
                {
                    FirstName = PersonalInformation.FirstName,
                    LastName = PersonalInformation.LastName,
                    Email = PersonalInformation.Email,
                    PhoneNumber = PersonalInformation.PhoneNumber,
                };
                if(!string.IsNullOrEmpty(PersonalInformation.CNP)
                    && PersonalInformation.CNP.All(char.IsDigit))
                {
                    dbGuest.CNP = PersonalInformation.CNP;
                   
                }
                string concatenatedAddress = GenerateConcatenatedAddress();
                if (!string.IsNullOrEmpty(concatenatedAddress))
                {
                    dbGuest.Address = concatenatedAddress;
                }

                _localContext.Clients.Add(dbGuest);
                _localContext.SaveChanges();
                ToastManager.Show($"The guest {dbGuest.FirstName} {dbGuest.LastName} was added into db!", ToastType.Success);
                ChangeToSearchGuestForm(obj);
            }

        }



        public void SearchGuests(object obj)
        {
            using var _localContext = new ApplicationDbContext();
            var query = _localContext.Clients.Include(c=>c.Reservations).AsQueryable();
            string? firstName = PersonalInformation.FirstName?.ToLower();
            string? lastName = PersonalInformation.LastName?.ToLower();
            string? email = PersonalInformation.Email?.ToLower();
            string? cnp = PersonalInformation.CNP;
            string? phone = PersonalInformation.PhoneNumber;


            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.ToLower().Contains(firstName));
            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.ToLower().Contains(lastName));
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.ToLower().Contains(email));

            if (!string.IsNullOrWhiteSpace(cnp) && cnp.All(char.IsDigit))
                query = query.Where(c => c.CNP.Contains(cnp));

            if (!string.IsNullOrWhiteSpace(phone) && phone.All(char.IsDigit))
                query = query.Where(c => c.PhoneNumber.Contains(phone));


            var guests = query.ToList();

            PossibleGuests.Clear();
            
            foreach (var guest in guests)
            {
                PossibleGuests.Add(new GuestDTO
                {
                    FirstName = guest.FirstName ?? "NotFound",
                    LastName = guest.LastName ?? "NotFound",
                    Email = guest.Email ?? "NotFound",
                    PhoneNumber = guest.PhoneNumber ?? "NotFound",
                    Address = guest.Address ?? "NotFound",
                    CNP = guest.CNP ?? "NotFound",
                    id = guest.Id,
                    reservations = guest.Reservations.Where(r=>r.checkIn.Year == DateTime.Now.Year).ToList()
                });
                

            }

            ToastManager.Show($"Found {guests.Count} possible guests!", ToastType.Info);
        }

        private void ChangeToSearchGuestForm(object obj)
        {
            FromName = "Search Guest";
            VisibilitySearch = Visibility.Visible;
            VisibilityCreate = Visibility.Collapsed;
            PossibleGuests.Clear();
           
        }

        private void ChangeToCreateGuestForm(object obj)
        {
            FromName = "Create Guest";
            VisibilitySearch = Visibility.Collapsed;
            VisibilityCreate = Visibility.Visible;
            
        }

        private string GenerateConcatenatedAddress()
        {
            string returnedAddress = "";

            if (!string.IsNullOrEmpty(Address.Country))
            {
                returnedAddress += Address.Country;
            }
            if (!string.IsNullOrEmpty(Address.Town))
            {
                returnedAddress +=$", {Address.Town}";
            }
            if (!string.IsNullOrEmpty(Address.Street))
            {
                returnedAddress += $", Str. {Address.Street}";
            }
            if (!string.IsNullOrEmpty(Address.Number))
            {
                returnedAddress += $", Nr. {Address.Number}";
            }
            if (!string.IsNullOrEmpty(Address.Building))
            {
                returnedAddress += $", Bld. {Address.Building}";
            }
            if (!string.IsNullOrEmpty(Address.Scara))
            {
                returnedAddress += $", Ent. {Address.Scara}";
            }
            if (!string.IsNullOrEmpty(Address.Apartment))
            {
                returnedAddress += $", Ap. {Address.Apartment}";
            }


            return returnedAddress;
        }
    }
}
