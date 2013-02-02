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
using System.Collections.ObjectModel;

namespace DynamicsPhone.ViewModels
{
    public class ProductViewModel: BaseViewModel
    {
        public static ProductViewModel viewModel = new ProductViewModel();

        private DynamicsServicesClient _client;

        public ObservableCollection<NavProduct> ProductsToOrder;

        public ProductViewModel()
        {
            this._client = new DynamicsServicesClient();
            this.ProductsToOrder = new ObservableCollection<NavProduct>();

            this._client.AddProductCompleted += 
                new EventHandler<AddProductCompletedEventArgs>(_client_AddProductCompleted);
            this._client.GetProductsToOrderCompleted += 
                new EventHandler<GetProductsToOrderCompletedEventArgs>(_client_GetProductsToOrderCompleted);
        }

        void _client_GetProductsToOrderCompleted(object sender, GetProductsToOrderCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                foreach (NavProduct p in e.Result)
                {
                    this.ProductsToOrder.Add(p);
                }
            }
        }

        void _client_AddProductCompleted(object sender, AddProductCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                App.ViewModel.DynDB.Products.InsertOnSubmit(new Product(e.Result));
                App.ViewModel.User.Products.Add(e.Result);
                App.ViewModel.DynDB.SubmitChanges();
            }
        }

        public void AddProduct(NavProduct product)
        {
            _client.AddProductAsync(product);
        }

        public void ReloadProductsToOrder()
        {
            _client.GetProductsToOrderAsync(App.ViewModel.User.No);
        }
    }
}
