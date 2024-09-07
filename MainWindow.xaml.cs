﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VendorRad.Models;
using VendorRad.ViewModels;

namespace VendorRad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            timer = new DispatcherTimer();
            DataContext = viewModel;
            StartClock();
        }

        // Event handler for saving customer contact
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer
            {
                Name = CustomerName.Text,
                Company = CustomerCompany.Text,
                PhoneNumber = CustomerPhoneNumber.Text,
                Address = CustomerAddress.Text,
                SalesNotes = CustomerSalesNotes.Text
            };

            viewModel.AddOrUpdateContact(customer);
            MessageBox.Show("Customer saved successfully!");
            ClearCustomerFields();
        }

        // Event handler for saving vendor contact
        private void SaveVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var vendor = new Vendor
            {
                Name = VendorName.Text,
                Company = VendorCompany.Text,
                PhoneNumber = VendorPhoneNumber.Text,
                Address = VendorAddress.Text
            };

            viewModel.AddOrUpdateContact(vendor);
            MessageBox.Show("Vendor saved successfully!");
            ClearVendorFields();
        }

        private void ClearCustomerFields()
        {
            CustomerName.Clear();
            CustomerCompany.Clear();
            CustomerPhoneNumber.Clear();
            CustomerAddress.Clear();
            CustomerSalesNotes.Clear();
        }

        private void ClearVendorFields()
        {
            VendorName.Clear();
            VendorCompany.Clear();
            VendorPhoneNumber.Clear();
            VendorAddress.Clear();
        }

        private void StartClock()
        {            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ClockDisplay.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}