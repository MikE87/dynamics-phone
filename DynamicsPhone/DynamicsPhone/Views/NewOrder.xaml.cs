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
    public partial class NewOrder : PhoneApplicationPage
    {
        public NewOrder()
        {
            InitializeComponent();

            this.DataContext = OrderViewModel.viewModel.NewOrder;
            this.lpProducts.ItemsSource = ProductViewModel.viewModel.ProductsToOrder;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            int count;
            if (!int.TryParse(tbCount.Text, out count))
                return;
            
            NavProduct p = this.lpProducts.SelectedItem as NavProduct;
            if(OrderViewModel.viewModel.NewOrder.OrderItems.Count >0 && OrderViewModel.viewModel.NewOrder.OrderItems.Last().ProductNo == p.No)
            {
                OrderViewModel.viewModel.NewOrder.OrderItems.Last().Count += count;
                OrderViewModel.viewModel.NewOrder.OrderItems.Last().Cost += p.Price * count;
                OrderViewModel.viewModel.NewOrder.TotalCost += p.Price * count;
                return;
            }

            decimal cost = count * p.Price;

            OrderViewModel.viewModel.NewOrder.OrderItems.Add(new NavOrderItem() 
            { 
                ProductNo = p.No,
                PurchasePrice = p.Price,
                OrderNo = OrderViewModel.viewModel.NewOrder.No,
                Count = count,
                Cost = cost
            });

            OrderViewModel.viewModel.NewOrder.TotalCost += cost;
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (OrderViewModel.viewModel.NewOrder.TotalCost > 0)
            {
                OrderViewModel.viewModel.AddOrder();
                NavigationService.GoBack();
            }
        }
    }
}