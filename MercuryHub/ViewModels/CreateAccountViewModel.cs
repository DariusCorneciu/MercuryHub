using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub.ViewModels
{
   
    public class UserDTO
    {
        public string userName {  get; set; }
        public string RoleName { get; set; }
    }

    public class CreateAccountViewModel:ViewModelBase
    {
        private string _username;
        private string _password;
        public int _roleId;

        public int _selectedUserId;
        public List<KeyValuePair<int, string>> RolesList { get; } = new List<KeyValuePair<int, string>>();

        public List<string> TakenNames;

        private ObservableCollection<UserDTO> _users;
        private Visibility _visibiliy;
        private Visibility _visibiliyEdit;
        public ObservableCollection<UserDTO> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public Visibility VisibilityCreate
        {
            get => _visibiliy;
            set
            {
                _visibiliy = value;
                OnPropertyChanged(nameof(VisibilityCreate));
            }
        }
        public Visibility VisibilityEdit
        {
            get => _visibiliyEdit;
            set
            {
                _visibiliyEdit = value;
                OnPropertyChanged(nameof(VisibilityEdit));
            }
        }
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public int SelectedRoleId
        {
            get => _roleId;
            set { _roleId = value; OnPropertyChanged(nameof(SelectedRoleId)); }
        }
        public int SelectedUserId
        {
            get => _selectedUserId;
            set { _selectedUserId = value; OnPropertyChanged(nameof(SelectedUserId)); }
        }



        public ICommand CreateAccountCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand PostEditUserCommand { get; }
        public CreateAccountViewModel()
        {
            VisibilityCreate = Visibility.Visible;
            VisibilityEdit = Visibility.Collapsed;
            PostEditUserCommand = new RelayCommand(PostEditUser);
            CancelEditCommand = new RelayCommand(CancelCommand);
            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            CreateAccountCommand = new RelayCommand(ExecuteCreateCommand,CanExecuteCreateCommand);
            RolesList.Clear();
             Users = new ObservableCollection<UserDTO>();
            using (var context = new ApplicationDbContext())
            {
               var roles = context.Roles.ToList();
                foreach (var role in roles) {

                    RolesList.Add(new KeyValuePair<int, string>(role.Id, role.Name));
                }
                TakenNames = new List<string>();
                var users = context.Users.Include(r=>r.Role).ToList();
                foreach (var user in users) {
                    Users.Add(new UserDTO
                    {
                        userName = user.userName,
                        RoleName = user.Role.Name
                    });

                    TakenNames.Add(user.userName);
                }

            }



        }

        private void PostEditUser(object obj)
        {
            int userId = (int)obj;
            using var context = new ApplicationDbContext();
            var editUser = context.Users.Include(r=>r.Role).Where(u => u.Id == userId).FirstOrDefault();
            if (editUser == null)
            {
                CancelCommand(obj);
            }
            else
            {
                if (!string.IsNullOrEmpty(Username) && editUser.userName != Username)
                {
                    editUser.userName = Username;
                }
                if (!string.IsNullOrEmpty(Password))
                {
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    editUser.password = passwordHasher.HashPassword(editUser,Password);
                }
                if(SelectedRoleId != editUser.RoleId && editUser.Role.Name != "Admin") 
                {
                    editUser.RoleId = SelectedRoleId;
                }
                context.Users.Update(editUser);
                context.SaveChanges();
                CancelCommand(obj);

            }


        }

        private void EditUser(object user)
        {
            var userName = (string)user;
            VisibilityCreate = Visibility.Collapsed;
            VisibilityEdit = Visibility.Visible;

            using var context = new ApplicationDbContext();
            var editUser = context.Users.Where(u => u.userName == userName).FirstOrDefault();

            if(editUser == null)
            {
                CancelCommand(user);
            }
            else
            {
                Username = editUser.userName;
                SelectedUserId = editUser.Id;
                SelectedRoleId = editUser.RoleId;
            }

              


        }
        private void CancelCommand(object user)
        {
            VisibilityCreate = Visibility.Visible;
            VisibilityEdit = Visibility.Collapsed;
            RefreshData();
        }
        private void DeleteUser(object user)
        {
            string userName = (string)user;
            using var context = new ApplicationDbContext();
            var deleteUser = context.Users.Where(u => u.userName == userName).FirstOrDefault();
            var principal = Thread.CurrentPrincipal.Identity;

            if(deleteUser == null || principal.Name == deleteUser.userName )
            {
                ToastManager.Show("User does not exists!, refresh the page", ToastType.Error);
            }
            else
            {
                context.Users.Remove(deleteUser);
                context.SaveChanges();
                ToastManager.Show("User deletet succesfuly!", ToastType.Success);
                RefreshData();
            }
               


        }
        private bool CanExecuteCreateCommand(object obj)
        {
            var principal = Thread.CurrentPrincipal;
            if (principal==null || !principal.IsInRole("Admin"))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;

            if (TakenNames != null && TakenNames.Any(name => string.Equals(name, Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            return true;
        }

      
        private void ExecuteCreateCommand(object obj)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            using var context = new ApplicationDbContext();
            var dbUser = new ApplicationUser
            {
                userName = Username,
                RoleId = SelectedRoleId
            };
            dbUser.password = passwordHasher.HashPassword(dbUser, Password);
            context.Users.Add(dbUser);
            context.SaveChanges();
            ToastManager.Show("User created succesfuly!", ToastType.Success);
            RefreshData();
        }

        private void RefreshData()
        {

            Username = "";
            Password = "";
            var context = new ApplicationDbContext();
            var users = context.Users.Include(r => r.Role).ToList();
            var newUsers = new ObservableCollection<UserDTO>();
            foreach (var user in users)
            {
                newUsers.Add(new UserDTO
                {
                    userName = user.userName,
                    RoleName = user.Role.Name
                });

                TakenNames.Add(user.userName);
            }

            Users = new ObservableCollection<UserDTO>(newUsers);
        }
    }
}
