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

namespace DynamicsPhone.Views
{
    public partial class OrderDetails : PhoneApplicationPage
    {
        public OrderDetails()
        {
            InitializeComponent();

            this.DataContext = OrderViewModel.viewModel.SelectedOrder;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.cbDelivered.IsEnabled = false;
            OrderViewModel.viewModel.ConfirmDelivery();
        }
    }
}