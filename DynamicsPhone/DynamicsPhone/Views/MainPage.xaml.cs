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
using System.ServiceModel;
using DynamicsPhone.ViewModels;
using DynamicsPhone.DynamicsServiceRef;
using System.Collections.ObjectModel;

namespace DynamicsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        } 
        

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void buttonLogOut_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel.viewModel.LogOut();
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new Uri("/Views/LoginRegistration.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void lbOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderViewModel.viewModel.SelectedOrder = e.AddedItems[0] as NavOrder;
            NavigationService.Navigate(new Uri("/Views/OrderDetails.xaml", UriKind.Relative));
        }

        private void btnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NewProduct.xaml", UriKind.Relative));
        }

        private void buttonSync_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.User.ReloadData();
        }

        private void buttonNewOrder_Click(object sender, RoutedEventArgs e)
        {
            ProductViewModel.viewModel.ReloadProductsToOrder();
            OrderViewModel.viewModel.NewOrder = new NavOrder();
            OrderViewModel.viewModel.NewOrder.OrderItems = new ObservableCollection<NavOrderItem>();
            OrderViewModel.viewModel.NewOrder.No = int.Parse
                ("" + 
                DateTime.Now.Year + 
                (DateTime.Now.Month + 
                DateTime.Now.Day + 
                DateTime.Now.Hour +
                DateTime.Now.Minute +
                DateTime.Now.Second) +
                DateTime.Now.Millisecond);
            NavigationService.Navigate(new Uri("/Views/NewOrder.xaml", UriKind.Relative));
        }
    }
}