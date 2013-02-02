using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DynamicsPhone.DynamicsServiceRef;
using DynamicsPhone.Model;
using System.Linq;

namespace DynamicsPhone.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public static OrderViewModel viewModel = new OrderViewModel();
        private DynamicsServiceRef.DynamicsServicesClient _client;

        public OrderViewModel()
        {
            _client = new DynamicsServicesClient();

            _client.AddOrderCompleted += 
                new EventHandler<AddOrderCompletedEventArgs>(_client_AddOrderCompleted);
            _client.ConfirmOrderDeliveryCompleted += 
                new EventHandler<ConfirmOrderDeliveryCompletedEventArgs>(_client_ConfirmOrderDeliveryCompleted);
        }

        void _client_ConfirmOrderDeliveryCompleted(object sender, ConfirmOrderDeliveryCompletedEventArgs e)
        {
            if (e.Error != null || !e.Result)
            {
                this.SelectedOrder.IsDelivered = false;
            }
            else
            {
                var o = App.ViewModel.DynDB.Orders.SingleOrDefault(x => x.No == this.SelectedOrder.No);
                o.IsDelivered = true;
                o.DeliveryDate = this.SelectedOrder.OrderDeliveryDate;
                App.ViewModel.DynDB.SubmitChanges();
            }
        }

        void _client_AddOrderCompleted(object sender, AddOrderCompletedEventArgs e)
        {
            if(e.Error == null && e.Result != null)
            {
                UserViewModel.viewModel.Orders.Add(e.Result);
                App.ViewModel.DynDB.Orders.InsertOnSubmit(new Order(e.Result));
                
                foreach(NavOrderItem item in e.Result.OrderItems)
                {
                    App.ViewModel.DynDB.OrderItems.InsertOnSubmit(new OrderItem(item));
                }
                
                App.ViewModel.DynDB.SubmitChanges();
            }
        }

        public void ConfirmDelivery()
        {
            this.SelectedOrder.IsDelivered = true;
            this.SelectedOrder.OrderDeliveryDate = DateTime.Now;
            this._client.ConfirmOrderDeliveryAsync(this.SelectedOrder);
        }

        public void AddOrder()
        {
            this.NewOrder.CustomerNo = App.ViewModel.User.No;
            this.NewOrder.OrderDate = DateTime.Now;
            this._client.AddOrderAsync(this.NewOrder);
        }

        private NavOrder _selectedOrder;
        public NavOrder SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;

                NotifyPropertyChanged("SelectedOrder");
            }
        }

        private NavOrder _newOrder;
        public NavOrder NewOrder
        {
            get { return _newOrder; }
            set
            {
                _newOrder = value;
                NotifyPropertyChanged("NewOrder");
            }
        }
    }
}
