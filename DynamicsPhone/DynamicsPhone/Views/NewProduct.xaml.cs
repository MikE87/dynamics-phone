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
using DynamicsPhone.DynamicsServiceRef;
using DynamicsPhone.ViewModels;

namespace DynamicsPhone.Views
{
    public partial class NewProduct : PhoneApplicationPage
    {
        public NewProduct()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            decimal price;
            if(!decimal.TryParse(tbPrice.Text, out price))
            {
                tbStatus.Text = "Bad price";
                return;
            }

            int count;
            if(!int.TryParse(tbCount.Text, out count))
            {
                tbStatus.Text = "Bad stock";
                return;
            }

            if(tbName.Text.Length == 0)
            {
                tbStatus.Text = "Name is required.";
                return;
            }

            tbStatus.Text = "";

            NavProduct p = new NavProduct()
            {
                Name = tbName.Text,
                Description = tbDesc.Text,
                Price = price,
                Count = count,
                VendorNo = UserViewModel.viewModel.No
            };

            ProductViewModel.viewModel.AddProduct(p);
            NavigationService.GoBack();
        }
    }
}