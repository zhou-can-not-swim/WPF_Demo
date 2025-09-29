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

namespace WpfApp_13_sqlite
{
    public partial class MainWindow : Window
    {
        private SQLiteDatabaseHelper dbHelper;
        private SoftWare selectedSoftWare;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new SQLiteDatabaseHelper();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                List<SoftWare> softWares = dbHelper.GetAllSoftWares();
                dgUsers.ItemsSource = softWares;
                tbStatus.Text = $"已加载 {softWares.Count} 个软件";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载软件时出错: {ex.Message}");
                tbStatus.Text = "加载软件时出错";
            }
        }

        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSoftWare = dgUsers.SelectedItem as SoftWare;
            if (selectedSoftWare != null)
            {
                txtName.Text = selectedSoftWare.Name;
                txtEmail.Text = selectedSoftWare.IconUrl;
                txtAge.Text = selectedSoftWare.DownloadUrl.ToString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    User newUser = new User
                    {
                        Name = txtName.Text,
                        Email = txtEmail.Text,
                        Age = int.Parse(txtAge.Text)
                    };

                    dbHelper.AddUser(newUser);
                    ClearForm();
                    LoadUsers();
                    tbStatus.Text = "用户添加成功";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"添加用户时出错: {ex.Message}");
                    tbStatus.Text = "添加用户时出错";
                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSoftWare == null)
            {
                MessageBox.Show("请先选择一个用户");
                return;
            }

            if (ValidateInput())
            {
                try
                {
                    selectedSoftWare.Name = txtName.Text;
                    selectedSoftWare.Email = txtEmail.Text;
                    selectedSoftWare.Age = int.Parse(txtAge.Text);

                    dbHelper.UpdateUser(selectedSoftWare);
                    ClearForm();
                    LoadUsers();
                    tbStatus.Text = "用户更新成功";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"更新用户时出错: {ex.Message}");
                    tbStatus.Text = "更新用户时出错";
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSoftWare == null)
            {
                MessageBox.Show("请先选择一个用户");
                return;
            }

            if (MessageBox.Show($"确定要删除用户 '{selectedSoftWare.Name}' 吗?", "确认删除",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    dbHelper.DeleteUser(selectedSoftWare.Id);
                    ClearForm();
                    LoadUsers();
                    tbStatus.Text = "用户删除成功";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"删除用户时出错: {ex.Message}");
                    tbStatus.Text = "删除用户时出错";
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入姓名");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("请输入邮箱");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age <= 0)
            {
                MessageBox.Show("请输入有效的年龄");
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtAge.Clear();
            selectedSoftWare = null;
        }
    }
}