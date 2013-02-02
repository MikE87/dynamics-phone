using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net;
using DynamicsServices.UserPageRef;
using DynamicsServices.ProductPageRef;
using DynamicsServices.OrderPageRef;
using DynamicsServices.OrderedItemsPageRef;

namespace DynamicsServices
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, IncludeExceptionDetailInFaults=true)]
    class DynamicsServices: IDynamicsServices
    {
        private User_Service _user_service;
        private Product_Service _product_service;
        private Order_Service _order_service;
        private OrderedItems_Service _orderItems_service;

        public DynamicsServices()
        {
            _user_service = new User_Service();
            _product_service = new Product_Service();
            _order_service = new Order_Service();
            _orderItems_service = new OrderedItems_Service();

            _user_service.Credentials = CredentialCache.DefaultNetworkCredentials;
            _product_service.Credentials = CredentialCache.DefaultNetworkCredentials;
            _order_service.Credentials = CredentialCache.DefaultNetworkCredentials;
            _orderItems_service.Credentials = CredentialCache.DefaultNetworkCredentials;
        }

        public IEnumerable<NavUser> GetAllUsers()
        {
            try
            {
                User_Filter[] filter = { new User_Filter() };

                List<NavUser> n_users = new List<NavUser>();
                List<User> users = new List<User>(_user_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (User u in users)
                {
                    n_users.Add(new NavUser(u));
                }

                return n_users;
            }
            catch
            {
                return null;
            }
        }

        public NavUser GetUserById(int id)
        {
            try
            {
                return new NavUser( _user_service.Read(id) );
            }
            catch 
            {
                return null;
            }
        }

        public bool AddUser(NavUser user, string pass)
        {
            try
            {
                User_Filter[] filterEmail = { new User_Filter() { Criteria = "=" + user.Email, Field = User_Fields.E_Mail } };
                User_Filter[] filterName = { new User_Filter() { Criteria = "=" + user.Name, Field = User_Fields.Nazwa } };

                var checkUserEmail = _user_service.ReadMultiple(filterEmail, "", 1);
                var checkUserName = _user_service.ReadMultiple(filterName, "", 1);

                if (checkUserEmail.Length > 0)
                {
                    throw new FaultException("E-mail already exists !");
                }

                else if (checkUserName.Length > 0)
                {
                    throw new FaultException("Name already exists !");
                }

                User u = new User()
                {
                    Adres = user.Address,
                    E_Mail = user.Email,
                    Firma = user.Company,
                    Kod_Pocztowy = user.PostalCode,
                    Kraj = user.CountryCode,
                    Miasto = user.City,
                    Nazwa = user.Name,
                    Portfel = 0,
                    FirmaSpecified = true
                };

                _user_service.Create(ref u);

                
                var usr = _user_service.ReadMultiple(filterEmail, "", 1);

                _user_service.UpdatePassword(usr[0].Key, pass);

                return true;
            }
            catch 
            {
                throw;
            }
        }

        public bool EditUser(NavUser user, string pass)
        {
            try
            {
                User u = new User()
                {
                    No = user.No,
                    Adres = user.Address,
                    E_Mail = user.Email,
                    Firma = user.Company,
                    Kod_Pocztowy = user.PostalCode,
                    Kraj = user.CountryCode,
                    Miasto = user.City,
                    Nazwa = user.Name,
                    Portfel = 0
                };

                _user_service.Update(ref u);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public NavUser LogIn(string email, string pass)
        {
            try
            {
                User_Filter[] filter = { new User_Filter() { Criteria = "=" + email, Field = User_Fields.E_Mail } };
                var user = _user_service.ReadMultiple(filter, "", 1);

                if (_user_service.Login(user[0].Key, pass))
                    return new NavUser(user[0]);

                return null;
            }
            catch 
            {
                return null;
            } 
        }

        public IEnumerable<NavProduct> GetAllProducts()
        {
            try
            {
                Product_Filter[] filter = { new Product_Filter() };

                List<NavProduct> n_products = new List<NavProduct>();
                List<Product> products = new List<Product>(_product_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (Product p in products)
                {
                    n_products.Add(new NavProduct(p));
                }

                return n_products;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<NavProduct> GetProductsToOrder(int id)
        {
            try
            {
                Product_Filter[] filter = { new Product_Filter(){ Criteria = "<>" + id, Field = Product_Fields.Dostawca} };

                List<NavProduct> n_products = new List<NavProduct>();
                List<Product> products = new List<Product>(_product_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (Product p in products)
                {
                    n_products.Add(new NavProduct(p));
                }

                return n_products;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<NavProduct> GetUserProducts(int id)
        {
            try
            {
                Product_Filter[] filter = { new Product_Filter(){ Criteria = "=" + id, Field = Product_Fields.Dostawca} };

                List<NavProduct> n_products = new List<NavProduct>();
                List<Product> products = new List<Product>(_product_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (Product p in products)
                {
                    n_products.Add(new NavProduct(p));
                }

                return n_products;
            }
            catch
            {
                return null;
            }
        }

        public NavProduct GetProductById(int id)
        {
            try
            {
                return new NavProduct(_product_service.Read(id));
            }
            catch 
            {
                return null;
            }
        }

        public NavProduct AddProduct(NavProduct product)
        {
            try
            {
                Product p = new Product() { 
                    Cena = product.Price,
                    CenaSpecified = true,
                    Dostawca = product.VendorNo,
                    DostawcaSpecified = true,
                    Dostępna_ilość = product.Count,
                    Dostępna_ilośćSpecified = true,
                    Nazwa = product.Name,
                    Opis = product.Description
                };

                _product_service.Create(ref p);

                return product;
            }
            catch
            {
                return null;
            }
        }

        public bool EditProduct(NavProduct product)
        {
            try
            {
                Product p = new Product()
                {
                    No = product.No,
                    Cena = product.Price,
                    Dostawca = product.VendorNo,
                    Dostępna_ilość = product.Count,
                    Nazwa = product.Name,
                    Opis = product.Description
                };

                _product_service.Update(ref p);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<NavOrder> GetAllOrders()
        {
            try
            {
                Order_Filter[] filter = { new Order_Filter() };

                List<NavOrder> n_orders = new List<NavOrder>();
                List<Order> orders = new List<Order>(_order_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (Order o in orders)
                {
                    OrderedItems_Filter[] item_filter = { new OrderedItems_Filter() { Criteria = "=" + o.No, Field = OrderedItems_Fields.Zamówienie } };

                    List<NavOrderItem> n_orderItems = new List<NavOrderItem>();
                    List<OrderedItems> orderItems = new List<OrderedItems>(_orderItems_service.ReadMultiple(item_filter, "", int.MaxValue));

                    foreach (OrderedItems item in orderItems)
                    {
                        n_orderItems.Add(new NavOrderItem(item));
                    }

                    n_orders.Add(new NavOrder(o)
                    {
                        OrderItems = n_orderItems
                    });
                }

                return n_orders;
            }
            catch 
            {
                return null;
            }
        }

        public IEnumerable<NavOrder> GetUserOrders(int id)
        {
            try
            {
                Order_Filter[] filter = { new Order_Filter(){ Criteria = "=" + id, Field = Order_Fields.Klient} };

                List<NavOrder> n_orders = new List<NavOrder>();
                List<Order> orders = new List<Order>(_order_service.ReadMultiple(filter, "", int.MaxValue));

                foreach (Order o in orders)
                {
                    OrderedItems_Filter[] item_filter = { new OrderedItems_Filter() { Criteria = "=" + o.No, Field = OrderedItems_Fields.Zamówienie } };

                    List<NavOrderItem> n_orderItems = new List<NavOrderItem>();
                    List<OrderedItems> orderItems = new List<OrderedItems>(_orderItems_service.ReadMultiple(item_filter, "", int.MaxValue));

                    foreach (OrderedItems item in orderItems)
                    {
                        n_orderItems.Add(new NavOrderItem(item));
                    }

                    n_orders.Add(new NavOrder(o)
                    {
                        OrderItems = n_orderItems
                    });
                }

                return n_orders;
            }
            catch
            {
                return null;
            }
        }

        public NavOrder GetOrderById(int id)
        {
            try
            {
                NavOrder order = new NavOrder(_order_service.Read(id));
                OrderedItems_Filter[] item_filter = { new OrderedItems_Filter() { Criteria = "=" + order.No, Field = OrderedItems_Fields.Zamówienie } };
                List<NavOrderItem> n_orderItems = new List<NavOrderItem>();
                List<OrderedItems> orderItems = new List<OrderedItems>(_orderItems_service.ReadMultiple(item_filter, "", int.MaxValue));

                foreach (OrderedItems item in orderItems)
                {
                    n_orderItems.Add(new NavOrderItem(item));
                }

                order.OrderItems = n_orderItems;
                return order;
            }
            catch 
            {
                return null;
            }
        }

        public NavOrder AddOrder(NavOrder order)
        {
            try
            {
                Order o = new Order() {
                    No = order.No,
                    NoSpecified = true,
                    Klient = order.CustomerNo,
                    KlientSpecified = true,
                    Kwota_do_zapłaty = order.TotalCost,
                    Kwota_do_zapłatySpecified = true,
                    Potwierdzenie_dostawy = order.IsDelivered,
                    Potwierdzenie_dostawySpecified = true,
                    Data_zamówienia = DateTime.Now,
                    Data_zamówieniaSpecified = true
                };

                _order_service.Create(ref o);

                foreach (NavOrderItem item in order.OrderItems)
                {
                    OrderedItems oi = new OrderedItems() { 
                        Ilość = item.Count,
                        Towar = item.ProductNo,
                        Cena_zakupu = item.PurchasePrice,
                        Koszt = item.Cost,
                        Zamówienie = item.OrderNo,
                        IlośćSpecified = true,
                        TowarSpecified = true,
                        ZamówienieSpecified = true,
                        Cena_zakupuSpecified = true,
                        KosztSpecified = true
                    };

                    _orderItems_service.Create(ref oi);
                }

                return order;
            }
            catch
            {
                return null;
            }
        }

        public bool EditOrder(NavOrder order)
        {
            try
            {
                foreach(NavOrderItem noi in order.OrderItems)
                {
                    _orderItems_service.Delete(noi.Key);
                }

                Order o = new Order()
                {
                    No = order.No,
                    Data_dostarczenia = order.OrderDeliveryDate,
                    Data_zamówienia = order.OrderDate,
                    Klient = order.CustomerNo,
                    Kwota_do_zapłaty = order.TotalCost,
                    Potwierdzenie_dostawy = order.IsDelivered
                };

                _order_service.Update(ref o);

                foreach (NavOrderItem item in order.OrderItems)
                {
                    OrderedItems oi = new OrderedItems()
                    {
                        Cena_zakupu = item.PurchasePrice,
                        Ilość = item.Count,
                        Koszt = item.Cost,
                        Towar = item.ProductNo,
                        Zamówienie = item.OrderNo,
                        Key = item.Key
                    };

                    _orderItems_service.Create(ref oi);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ConfirmOrderDelivery(NavOrder order)
        {
            try
            {
                Order o = new Order()
                {
                    No = order.No,
                    NoSpecified = true,
                    Data_dostarczenia = order.OrderDeliveryDate,
                    Data_zamówienia = order.OrderDate,
                    Klient = order.CustomerNo,
                    Kwota_do_zapłaty = order.TotalCost,
                    Potwierdzenie_dostawy = order.IsDelivered,
                    Potwierdzenie_dostawySpecified = true
                };

                _order_service.Update(ref o);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }    
}
