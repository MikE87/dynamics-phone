using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using DynamicsPhone.ViewModels;

namespace DynamicsPhone
{
    public partial class LoginRegistration : PhoneApplicationPage
    {
        public LoginRegistration()
        {
            InitializeComponent();

            this.DataContext = UserViewModel.viewModel;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel.viewModel.Login(this.textBoxEmail.Text, this.textBoxPassword.Password);
        }

        private void userLogged_Checked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel.viewModel.Register(
                tbCountryCode.Text,
                tbAddress.Text,
                tbCity.Text,
                tbPostCode.Text,
                tbEmail.Text,
                tbPass.Password,
                tbName.Text,
                isCompany.IsChecked.Value
                );
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }
    }
}