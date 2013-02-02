using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DynamicsPhone.ViewModels;
using DynamicsPhone.DynamicsServiceRef;
using DynamicsPhone.Model;


namespace DynamicsPhone
{
    public sealed class MainViewModel : BaseViewModel
    {

        private DynamicsDataContex dynDB;

        public MainViewModel(string connectionString)
        {
            this.dynDB = new DynamicsDataContex(connectionString);
        }

        public DynamicsDataContex DynDB
        {
            get { return dynDB; }
        }

        public UserViewModel User 
        {
            get { return UserViewModel.viewModel; }
        }
    }
}