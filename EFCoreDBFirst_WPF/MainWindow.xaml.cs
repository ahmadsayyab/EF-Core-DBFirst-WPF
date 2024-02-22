using MainLogicLibrary.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EFCoreDBFirst_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Age = int.Parse(txtAge.Text.Trim()),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Password.Trim(),
            };

            SaveUser(user);
            MessageBox.Show("User saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetControls();
            LoadData();
        }

        DbUserData db = new DbUserData();
        private void SaveUser(User user)
        {

            using (var context = new DbUserData())
            {
                // Check if the user already exists in the database
                var existingUser = context.Users.Find(user.Id);
                if (existingUser != null)
                {
                    // Update existing user
                    context.Entry(existingUser).CurrentValues.SetValues(user);
                }
                else
                {
                    // Add new user
                    context.Users.Add(user);
                }

                // Save changes
                context.SaveChanges();
            }


        }

        private User _selectedUser;
        private void dgData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is User user)
            {
                _selectedUser = user;
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtAge.Text = Convert.ToString(user.Age);
                txtAddress.Text = user.Address;
                txtEmail.Text = user.Email;
                txtPassword.Password = user.Password;

            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser != null)
            {
                _selectedUser.FirstName = txtFirstName.Text;
                _selectedUser.LastName = txtLastName.Text;
                _selectedUser.Age = int.Parse(txtAge.Text);
                _selectedUser.Address = txtAddress.Text;
                _selectedUser.Email = txtEmail.Text;
                _selectedUser.Password = txtPassword.Password;

                SaveUser(_selectedUser);
                MessageBox.Show("Data Updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshDataGrid();
                ResetControls();
            }
        }

        private void btnDeleteData_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button btn = sender as Button;
            if (btn != null && btn.DataContext is User userToDelete)
            {
                // Ask for confirmation
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Delete the user from the database
                    using (var context = new DbUserData())
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                    }

                    // Update the DataGrid
                    RefreshDataGrid();
                }
            }
        }


        private void LoadData()
        {
            var data = db.Users.ToList();

            dgData.ItemsSource = data;
        }

        private void RefreshDataGrid()
        {
            dgData.ItemsSource = null;
            LoadData();

        }

        private void ResetControls()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
        }
    }
}