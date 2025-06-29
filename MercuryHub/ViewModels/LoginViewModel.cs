using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MercuryHub.CustomControls;

namespace MercuryHub.ViewModels;
public class LoginViewModel : ViewModelBase
{
    private string _username;
    private SecureString _password;
    
    private string _errorMessage;
    private bool _isVisible;

    public event EventHandler<Tuple<string,string>>? LoginSucceeded;

   
    public bool IsVisible
    {
        get { return _isVisible; }
        set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
    }
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
    }

    public SecureString Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(nameof(Password)); }
    }

    public ICommand LoginCommand { get;  }
   // public ICommand RecoverPasswordCommand { get;  }
    public ICommand ShowPasswordCommand { get;  }
    public ICommand RememberPasswordCommand { get;  }

    public LoginViewModel()
    {
        
        height = 550;
        width = 400;
        LoginCommand = new RelayCommand(ExecuteLogginCommand, CanExecuteLoginCommand);
        _isVisible = true;
    }

    private bool CanExecuteLoginCommand(object obj)
    {
        bool validData;
        if(string.IsNullOrEmpty(Username) || Password == null)
        {
            validData = false;
        }
        else
        {
            validData = true;
        }
        return validData;

        
    }

    private void ExecuteLogginCommand(object obj)
    {
        using var context = new ApplicationDbContext();

        // Include the Role navigation property
        var user = context.Users
            .Where(u => u.userName == Username)
            .Include(u => u.Role)
            .SingleOrDefault();

        if (user == null)
        {
            ToastManager.Show("Invalid username or password.", ToastType.Error);
            return;
        }

        var hasher = new PasswordHasher<ApplicationUser>();

        // Convert SecureString to string  
        string passwordString = new System.Net.NetworkCredential(string.Empty, Password).Password;

        var result = hasher.VerifyHashedPassword(user, user.password, passwordString);

        if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
        {
           
            if (user.Role != null)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), [user.Role.Name]);
                IsVisible = false;
                ToastManager.Show($"Welcome back, {Username}!", ToastType.Success);
                LoginSucceeded?.Invoke(this,new Tuple<string, string> (Username, user.Role.Name));
            }
            else
            {
                ToastManager.Show("User role is not assigned.", ToastType.Warning);
            }
        }
        else
        {
            ToastManager.Show("Invalid username or password.", ToastType.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
