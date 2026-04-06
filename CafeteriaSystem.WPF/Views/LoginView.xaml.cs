using System.Windows;
using System.Windows.Controls;
using CafeteriaSystem.WPF.ViewModels;

namespace CafeteriaSystem.WPF.Views;

public partial class LoginView : UserControl
{
    public LoginView() => InitializeComponent();

    private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.SetPasswordFromView(TxtPassword.Password);
    }
}
