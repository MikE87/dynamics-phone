using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using DynamicsPhone.DynamicsServiceRef;

namespace DynamicsPhone.Model
{
    [Table]
    public class User: INotifyPropertyChanged
    {
        public User() { }

        public User(NavUser user)
        {
            this.Address = user.Address;
            this.Balance = user.Balance;
            this.City = user.City;
            this.CountryCode = user.CountryCode;
            this.Email = user.Email;
            this.IsCompany = user.Company;
            this.Name = user.Name;
            this.No = user.No;
            this.PostalCode = user.PostalCode;
        }


        private int _no;
        [Column(IsPrimaryKey=true)]
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

        private string _email;
        [Column(CanBeNull=false)]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;

                NotifyPropertyChanged("Email");
            }
        }

        public string _name;
        [Column(CanBeNull=false)]
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
        [Column]
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
        [Column]
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
        [Column]
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
        [Column]
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
        [Column]
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
        [Column]
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table]
    public class OrderItem : INotifyPropertyChanged
    {
        public OrderItem() { }

        public OrderItem(NavOrderItem item)
        {
            this.Cost = item.Cost;
            this.Count = item.Count;
            this.No = item.No;
            this.OrderNo = item.OrderNo;
            this.ProductNo = item.ProductNo;
            this.PurchasePrice = item.PurchasePrice;
        }


        private int _no;
        [Column(IsPrimaryKey = true)]
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

        private decimal _cost;
        [Column]
        public decimal Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;

                NotifyPropertyChanged("Cost");
            }
        }

        private int _count;
        [Column]
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;

                NotifyPropertyChanged("Count");
            }
        }

        private decimal _purchasePrice;
        [Column]
        public decimal PurchasePrice
        {
            get
            {
                return _purchasePrice;
            }
            set
            {
                _purchasePrice = value;

                NotifyPropertyChanged("PurchasePrice");
            }
        }

        private int _orderNo;
        [Column]
        public int OrderNo
        {
            get
            {
                return _orderNo;
            }
            set
            {
                _orderNo = value;

                NotifyPropertyChanged("OrderNo");
            }
        }

        private int _productNo;
        [Column]
        public int ProductNo
        {
            get
            {
                return _productNo;
            }
            set
            {
                _productNo = value;

                NotifyPropertyChanged("ProductNo");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table]
    public class Order : INotifyPropertyChanged
    {
        public Order() { }

        public Order(NavOrder order)
        {
            if (order.IsDelivered)
                this.DeliveryDate = new DateTime(order.OrderDeliveryDate.Year, order.OrderDeliveryDate.Month, order.OrderDeliveryDate.Day);
            else
                this.DeliveryDate = new DateTime(2000, 1, 1);

            this.OrderDate = new DateTime(order.OrderDate.Year, order.OrderDate.Month, order.OrderDate.Day);
            this.CustomerNo = order.CustomerNo;
            this.IsDelivered = order.IsDelivered;
            this.No = order.No;
            this.TotalCost = order.TotalCost;
        }

        private int _no;
        [Column(IsPrimaryKey = true)]
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

        private int _customerNo;
        [Column]
        public int CustomerNo
        {
            get
            {
                return _customerNo;
            }
            set
            {
                _customerNo = value;
                NotifyPropertyChanged("CustomerNo");
            }
        }

        private bool _isDelivered;
        [Column]
        public bool IsDelivered
        {
            get
            {
                return _isDelivered;
            }
            set
            {
                _isDelivered = value;
                NotifyPropertyChanged("IsDelivered");
            }
        }

        private DateTime _orderDate;
        [Column]
        public DateTime OrderDate
        {
            get
            {
                return _orderDate;
            }
            set
            {
                _orderDate = value;
                NotifyPropertyChanged("OrderDate");
            }
        }

        private DateTime _deliveryDate;
        [Column]
        public DateTime DeliveryDate
        {
            get
            {
                return _deliveryDate;
            }
            set
            {
                _deliveryDate = value;
                NotifyPropertyChanged("DeliveryDate");
            }
        }

        private decimal _totalCost;
        [Column]
        public decimal TotalCost
        {
            get
            {
                return _totalCost;
            }
            set
            {
                _totalCost = value;
                NotifyPropertyChanged("TotalCost");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table]
    public class Product : INotifyPropertyChanged
    {
        public Product() { }

        public Product(NavProduct product)
        {
            this.No = product.No;
            this.Name = product.Name;
            this.Count = product.Count;
            this.Description = product.Description;
            this.Price = product.Price;
            this.VendorNo = product.VendorNo;
        }

        private int _no;
        [Column(IsPrimaryKey = true)]
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

        private string _name;
        [Column(CanBeNull=false)]
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

        private string _desc;
        [Column]
        public string Description
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
                NotifyPropertyChanged("Description");
            }
        }

        private decimal _price;
        [Column(CanBeNull=false)]
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                NotifyPropertyChanged("Price");
            }
        }

        private int _count;
        [Column(CanBeNull=false)]
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                NotifyPropertyChanged("Count");
            }
        }

        private int _vendorNo;
        [Column]
        public int VendorNo
        {
            get
            {
                return _vendorNo;
            }
            set
            {
                _vendorNo = value;
                NotifyPropertyChanged("VendorNo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public class DynamicsDataContex : DataContext
    {
        public DynamicsDataContex(string connectionString)
            : base(connectionString)
        {
        }

        public Table<User> Users;
        public Table<Order> Orders;
        public Table<OrderItem> OrderItems;
        public Table<Product> Products;
    }
}
