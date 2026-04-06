using CafeteriaSystem.Application.DTOs;
using CafeteriaSystem.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CafeteriaSystem.WPF.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    public event Action<UserSessionDto>? LoginSucceeded;

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string _errorText = string.Empty;
    [ObservableProperty] private bool _isLoading;

    public LoginViewModel(AuthService authService) => _authService = authService;

    [RelayCommand]
    private async Task LoginAsync()
    {
        HasError = false;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            HasError = true; ErrorText = "Por favor ingresa usuario y contraseña."; return;
        }
        IsLoading = true;
        await Task.Delay(600); // Simulated delay for UX
        var (success, message, session) = await _authService.LoginAsync(Username, Password);
        IsLoading = false;
        if (!success) { HasError = true; ErrorText = message; return; }
        Password = string.Empty;
        LoginSucceeded?.Invoke(session!);
    }

    public void SetPasswordFromView(string pwd) => Password = pwd;
}
