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
using System.IO.IsolatedStorage;
using DynamicsPhone.DynamicsServiceRef;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using DynamicsPhone.Model;
using System.Linq;

namespace DynamicsPhone.ViewModels
{
    public class UserViewModel: BaseViewModel
    {
        public static UserViewModel viewModel = new UserViewModel();
        private DynamicsServicesClient _client;
        
        public UserViewModel()
        {
            this._client = new DynamicsServicesClient();
            this.Products = new ObservableCollection<NavProduct>();
            this.Orders = new ObservableCollection<NavOrder>();


            this._client.LogInCompleted += 
                new EventHandler<LogInCompletedEventArgs>(_client_LogInCompleted);
            this._client.AddUserCompleted += 
                new EventHandler<AddUserCompletedEventArgs>(_client_AddUserCompleted);
            this._client.GetUserProductsCompleted += 
                new EventHandler<GetUserProductsCompletedEventArgs>(_client_GetUserProductsCompleted);
            this._client.GetUserOrdersCompleted += 
                new EventHandler<GetUserOrdersCompletedEventArgs>(_client_GetUserOrdersCompleted);

            if (this.Email != null && this.Pass != null)
            {
                this.Login(this.Email, this.Pass);
            }
        }

        void _client_GetUserOrdersCompleted(object sender, GetUserOrdersCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                this.Orders = new ObservableCollection<NavOrder>();
                App.ViewModel.DynDB.Orders.DeleteAllOnSubmit(App.ViewModel.DynDB.Orders);
                App.ViewModel.DynDB.OrderItems.DeleteAllOnSubmit(App.ViewModel.DynDB.OrderItems);
                
                foreach(NavOrder o in e.Result)
                {
                    this.Orders.Add(o);
                    App.ViewModel.DynDB.Orders.InsertOnSubmit(new Order(o));
                    
                    foreach(NavOrderItem item in o.OrderItems)
                    {
                        App.ViewModel.DynDB.OrderItems.InsertOnSubmit(new OrderItem(item));
                    }
                }
                 
                App.ViewModel.DynDB.SubmitChanges();
            }
        }

        void _client_GetUserProductsCompleted(object sender, GetUserProductsCompletedEventArgs e)
        {
            if(e.Error == null && e.Result != null)
            {
                this.Products = new ObservableCollection<NavProduct>();
                App.ViewModel.DynDB.Products.DeleteAllOnSubmit(App.ViewModel.DynDB.Products);

                foreach(NavProduct p in e.Result)
                {
                    this.Products.Add(p);
                    App.ViewModel.DynDB.Products.InsertOnSubmit(new Product(p));
                }
                App.ViewModel.DynDB.SubmitChanges();
            }
        }

        void _client_AddUserCompleted(object sender, AddUserCompletedEventArgs e)
        {
            if(e.Error == null && e.Result)
            {
                this.RegisterStatus = "Registered successfully";
                this.Login(this.Email, this.Pass);
            }
            else if(e.Error != null)
            {
                this.RegisterStatus = e.Error.Message;
                this.Email = null;
                this.Pass = null;
            }
            else
            {
                this.RegisterStatus = "Connection error !";
                this.Email = null;
                this.Pass = null;
            }
        }

        void _client_LogInCompleted(object sender, LogInCompletedEventArgs e)
        {
            this.LoginStatus += ".";

            if (e.Error == null && e.Result != null)
            {
                this.LoginStatus += ".";
                this.No = e.Result.No;
                this.Address = e.Result.Address;
                this.City = e.Result.City;
                this.CountryCode = e.Result.CountryCode;
                this.IsCompany = e.Result.Company;
                this.PostalCode = e.Result.PostalCode;
                this.Name = e.Result.Name;
                this.Balance = e.Result.Balance;
                this.LoginStatus = "Logged in successfully.";
                this.UserLogged = true;

                if (this.Email == e.Result.Email)
                {
                    this.LoadLocalData();
                }
                else
                {
                    this.Email = e.Result.Email;
                    this.ReloadData();
                }
            }
            else if (e.Error != null)
            {
                this.LoginStatus = e.Error.Message;
                this.Pass = null;
                this.UserLogged = false;
            }
            else
            {
                this.LoginStatus = "Wrong email or password!";
                this.Pass = null;
                this.UserLogged = false;
            }
        }



        private string _loginStatus;
        public string LoginStatus
        {
            get { return _loginStatus; }
            set
            {
                _loginStatus = value;
                NotifyPropertyChanged("LoginStatus");
            }
        }

        private string _registerStatus;
        public string RegisterStatus
        {
            get { return _registerStatus; }
            set
            {
                _registerStatus = value;
                NotifyPropertyChanged("RegisterStatus");
            }
        }

        private bool _userLogged;
        public bool UserLogged
        {
            get { return _userLogged; }
            set
            {
                _userLogged = value;
                NotifyPropertyChanged("UserLogged");
            }
        }


        public string Email
        {
            get 
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("Email"))
                {
                    return IsolatedStorageSettings.ApplicationSettings["Email"] as string;
                }
                else
                {
                    return null;
                }
            }
            set 
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("Email"))
                {
                    IsolatedStorageSettings.ApplicationSettings["Email"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("Email", value);
                }

                IsolatedStorageSettings.ApplicationSettings.Save();

                NotifyPropertyChanged("Email");
            }
        }

        public string Pass
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("Pass"))
                {
                    return IsolatedStorageSettings.ApplicationSettings["Pass"] as string;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("Pass"))
                {
                    IsolatedStorageSettings.ApplicationSettings["Pass"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("Pass", value);
                }

                IsolatedStorageSettings.ApplicationSettings.Save();

                NotifyPropertyChanged("Pass");
            }
        }

        private int _no;
        public int No
        {
            get
            {
                return _no;
            }
            set
            {
                _no = value;

                NotifyPropertyChanged("No");
            }
        }

        public string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                NotifyPropertyChanged("Name");
            }
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;

                NotifyPropertyChanged("Address");
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;

                NotifyPropertyChanged("City");
            }
        }

        private string _postCode;
        public string PostalCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                _postCode = value;

                NotifyPropertyChanged("PostalCode");
            }
        }

        private string _country;
        public string CountryCode
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;

                NotifyPropertyChanged("CountryCode");
            }
        }

        private bool _isC;
        public bool IsCompany
        {
            get
            {
                return _isC;
            }
            set
            {
                _isC = value;

                NotifyPropertyChanged("IsCompany");
            }
        }

        private decimal _balance;
        public decimal Balance 
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;

                NotifyPropertyChanged("Balance");
            }
        }


        public ObservableCollection<NavOrder> Orders { get; private set; }
        public ObservableCollection<NavProduct> Products { get; private set; }


        
        public void Login(string email, string pass)
        {
            this.LoginStatus = "Logging in";
            this.Pass = pass;
            this._client.LogInAsync(email, pass);
        }

        public void Register(string countryCode, string address, string city, string postalCode, 
                                string email, string pass, string name, bool isCorporate)
        {
            this.RegisterStatus = "Registering";

            if(name == "")
            {
                this.RegisterStatus = "Name is required";
                return;
            }
            else if(email == "")
            {
                this.RegisterStatus = "E-mail is required";
                return;
            }
            else if (!ValidateEmail(email))
            {
                this.RegisterStatus = "Bad email format!";
                return;
            }
            else if(pass == "")
            {
                this.RegisterStatus = "Password is required";
            }

            NavUser user = new NavUser()
            {
                Address = address,
                City = city,
                Company = isCorporate,
                CountryCode = countryCode,
                Email = email,
                Name = name,
                PostalCode = postalCode
            };

            this.Email = email;
            this.Pass = pass;
            this._client.AddUserAsync(user, pass);
        }

        internal void LogOut()
        {
            this.No = -1;
            this.Pass = null;
            this.Name = null;
            this.Address = null;
            this.City = null;
            this.CountryCode = null;
            this.IsCompany = false;
            this.PostalCode = null;
            this.Products = new ObservableCollection<NavProduct>();
            this.Orders = new ObservableCollection<NavOrder>();
            this.UserLogged = false;
            this.LoginStatus = "Logged out";
            this.RegisterStatus = "";
        }

        public bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email,
              @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }


        /// <summary>
        /// Reloads product and order data from the server.
        /// </summary>
        public void ReloadData()
        {
            _client.GetUserOrdersAsync(this.No);
            _client.GetUserProductsAsync(this.No);
        }

        /// <summary>
        /// Loads data from local storage. 
        /// 
        /// if there is no products or orders it reloads data from the server. 
        /// </summary>
        private void LoadLocalData()
        {
            this.Products = new ObservableCollection<NavProduct>();
            this.Orders = new ObservableCollection<NavOrder>();

            var products = App.ViewModel.DynDB.Products.Where(x => x.VendorNo == this.No);
            var orders = App.ViewModel.DynDB.Orders.Where(x => x.CustomerNo == this.No);

            if(products == null && orders == null)
            {
                this.ReloadData();
                return;
            }

            foreach(Product p in products)
            {
                this.Products.Add(new NavProduct() 
                { 
                    Count = p.Count,
                    Description = p.Description,
                    Name = p.Name,
                    No = p.No,
                    Price = p.Price,
                    VendorNo = p.VendorNo
                });
            }

            foreach(Order o in orders)
            {
                NavOrder order = new NavOrder()
                {
                    CustomerNo = o.CustomerNo,
                    IsDelivered = o.IsDelivered,
                    No = o.No,
                    OrderDate = o.OrderDate.Date,
                    OrderDeliveryDate = o.DeliveryDate.Date,
                    TotalCost = o.TotalCost,
                    OrderItems = new ObservableCollection<NavOrderItem>()
                };
                
                var items = App.ViewModel.DynDB.OrderItems.Where(x => x.OrderNo == o.No);
                foreach(OrderItem item in items)
                {
                    order.OrderItems.Add(new NavOrderItem() 
                    { 
                        Cost = item.Cost,
                        Count = item.Count,
                        No = item.No,
                        OrderNo = item.OrderNo,
                        ProductNo = item.ProductNo,
                        PurchasePrice = item.PurchasePrice
                    });
                }

                this.Orders.Add(order);
            }
        }
    }
}
