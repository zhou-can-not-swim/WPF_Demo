using System.Text;
using System.Windows;
using WpfApp_9_mvvm.Model;
using WpfApp_9_mvvm.Repository;

namespace WpfApp_9_mvvm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserRepository _userRepository = new UserRepository();

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = _userRepository.GetAllUsers();
            // 绑定到UI控件，如DataGrid
            dataGrid.ItemsSource = users;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Name = nameTextBox.Text,
                Email = emailTextBox.Text,
                Age = int.TryParse(ageTextBox.Text, out int age) ? age : (int?)null,//尝试将文本框 ageTextBox 中的文本转换为整数
                Address = addressTextBox.Text
            };

            _userRepository.AddUser(user);
            LoadUsers(); // 刷新列表
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is User selectedUser)
            {
                _userRepository.DeleteUser(selectedUser.Name);
                LoadUsers(); // 刷新列表
            }
        }
    }
}