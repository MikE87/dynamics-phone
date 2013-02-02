using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using System;
using DynamicsServices.UserPageRef;
using DynamicsServices.ProductPageRef;
using DynamicsServices.OrderPageRef;
using DynamicsServices.OrderedItemsPageRef;

namespace DynamicsServices
{
    [ServiceContract]
    interface IDynamicsServices
    {
        // User

        [OperationContract]
        IEnumerable<NavUser> GetAllUsers();

        [OperationContract]
        NavUser GetUserById(int id);

        [OperationContract]
        bool AddUser(NavUser user, string pass);

        [OperationContract]
        bool EditUser(NavUser user, string pass);

        [OperationContract]
        NavUser LogIn(string email, string pass);
        
        // Product
        
        [OperationContract]
        IEnumerable<NavProduct> GetAllProducts();

        [OperationContract]
        IEnumerable<NavProduct> GetProductsToOrder(int id);

        [OperationContract]
        IEnumerable<NavProduct> GetUserProducts(int id);

        [OperationContract]
        NavProduct GetProductById(int id);

        [OperationContract]
        NavProduct AddProduct(NavProduct product);

        [OperationContract]
        bool EditProduct(NavProduct product);

        // Order

        [OperationContract]
        IEnumerable<NavOrder> GetAllOrders();

        [OperationContract]
        IEnumerable<NavOrder> GetUserOrders(int id);

        [OperationContract]
        NavOrder GetOrderById(int id);

        [OperationContract]
        NavOrder AddOrder(NavOrder order);

        [OperationContract]
        bool EditOrder(NavOrder order);

        [OperationContract]
        bool ConfirmOrderDelivery(NavOrder order);

    }

    [DataContract]
    public class NavUser
    {
        public NavUser(User user)
        {
            No = user.No;
            Name = user.Nazwa;
            Address = user.Adres;
            Balance = user.Portfel;
            City = user.Miasto;
            Company = user.Firma;
            CountryCode = user.Kraj;
            Email = user.E_Mail;
            PostalCode = user.Kod_Pocztowy;
        }

        [DataMember]
        public int No { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public bool Company { get; set; }

        [DataMember]
        public decimal Balance { get; set; }
    }

    [DataContract]
    public class NavProduct
    {
        public NavProduct(Product p)
        {
            Count = p.Dostępna_ilość;
            Description = p.Opis;
            Name = p.Nazwa;
            No = p.No;
            Price = p.Cena;
            VendorNo = p.Dostawca;
        }

        [DataMember]
        public int No { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int VendorNo { get; set; }

        [DataMember]
        public byte[] Image { get; set; }
    }

    [DataContract]
    public class NavOrder
    {
        public NavOrder(Order o)
        {
            No = o.No;
            CustomerNo = o.Klient;
            IsDelivered = o.Potwierdzenie_dostawy;
            OrderDate = o.Data_zamówienia;
            OrderDeliveryDate = o.Data_dostarczenia;
            TotalCost = o.Kwota_do_zapłaty;
        }

        [DataMember]
        public int No { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public DateTime OrderDeliveryDate { get; set; }

        [DataMember]
        public int CustomerNo { get; set; }

        [DataMember]
        public bool IsDelivered { get; set; }

        [DataMember]
        public decimal TotalCost { get; set; }

        [DataMember]
        public List<NavOrderItem> OrderItems { get; set; }
    }

    [DataContract]
    public class NavOrderItem
    {
        public NavOrderItem(OrderedItems item)
        {
            No = item.No;
            Cost = item.Koszt;
            Count = item.Ilość;
            PurchasePrice = item.Cena_zakupu;
            OrderNo = item.Zamówienie;
            ProductNo = item.Towar;
            Key = item.Key;
        }

        [DataMember]
        public int No { get; set; }

        [DataMember]
        public decimal PurchasePrice { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Cost { get; set; }

        [DataMember]
        public int ProductNo { get; set; }

        [DataMember]
        public int OrderNo { get; set; }

        [DataMember]
        public string Key { get; set; }
    }
}
