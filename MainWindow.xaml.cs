using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VendorRad.Models;
using VendorRad.ViewModels;

namespace VendorRad
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            timer = new DispatcherTimer();
            StartClock();
        }

        private void ToggleContactType_Checked(object sender, RoutedEventArgs e)
        {
            if (VendorSpecificFields == null)
                return; // Exit if controls are not initialized

            if (ToggleContactType.IsChecked == true)
            {
                ToggleContactType.Content = "Customer";
                VendorSpecificFields.Visibility = Visibility.Collapsed;
            }
            else
            {
                ToggleContactType.Content = "Vendor";
                VendorSpecificFields.Visibility = Visibility.Visible;
            }
        }

        // Event handler for saving contact
        private void SaveContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleContactType.IsChecked == true)
            {
                // Save as Customer
                var customer = new Customer
                {
                    Name = ContactName.Text,
                    Company = ContactCompany.Text,
                    PhoneNumber = ContactPhoneNumber.Text,
                    Address = ContactAddress.Text,
                    SalesNotes = ContactNotes.Text
                };

                viewModel.AddContact(customer);
                MessageBox.Show("Customer saved successfully!");
                ClearContactFields();
            }
            else
            {
                var masterVendor = VendorCompanyDropdown.SelectedItem as MasterVendor;

                if (masterVendor == null)
                {
                    MessageBox.Show("Please select a master vendor.");
                    return; // Exit the method if no vendor is selected
                }

                var vendor = new Vendor
                {
                    Name = ContactName.Text,
                    Company = masterVendor.CompanyName,
                    PhoneNumber = ContactName.Text,
                    Address = ContactAddress.Text,
                    MasterVendor = masterVendor
                };

                viewModel.AddContact(vendor);

                MessageBox.Show("Vendor saved successfully!");
                ClearContactFields();
            }
            MessageBox.Show("Contact saved successfully!");
        }

        private void AddMasterVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var companyName = NewMasterVendorCompanyName.Text;
            var vendorCode = NewMasterVendorCode.Text;

            if (!string.IsNullOrEmpty(companyName) && !string.IsNullOrEmpty(vendorCode))
            {
                if (!viewModel.IsMasterVendorExists(companyName))
                {
                    viewModel.AddNewMasterVendor(companyName, vendorCode);
                    MessageBox.Show("New master vendor added successfully!");
                }
                else
                {
                    MessageBox.Show("Master vendor already exists.");
                }
            }
            else
            {
                MessageBox.Show("Please enter both company name and vendor code.");
            }
        }

        // Clear contact input fields
        private void ClearContactFields()
        {
            ContactName.Clear();
            ContactCompany.Clear();
            ContactPhoneNumber.Clear();
            ContactAddress.Clear();
            ContactNotes.Clear();
        }

        // Start the clock display
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