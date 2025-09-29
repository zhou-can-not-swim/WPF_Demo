using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfD.Model;
using WpfD.Utils;

namespace WpfD.ViewModel
{
    public class ClassVModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        public ICommand AddUserCommand { get; set; }


        public ICommand TestCommand { get; set; }

        //属性,_name 存储当前值，value 是要设置的新值
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string? _email;
        public string? Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }


        public ClassVModel()
        {
            Users = UserManager.GetUsers();
            AddUserCommand = new RelayCommand(AddUser, CanAddUser);
            TestCommand = new RelayCommand(Test, CanTest);
        }

        private bool CanTest(object obj)
        {
            return true;
        }

        private void Test(object obj)
        {
            Name = "小1";
            Email = "111@qq.com";
        }



        private bool CanAddUser(object obj)
        {
            return true;
        }

        private void AddUser(object obj)
        {
            User user = new User();
            user.Name = Name;
            user.Email = Email;
            UserManager.AddUser(user);
        }



        ////这一步与INotifyPropertyChanged对应, 主要是为了实现数据的双向绑定
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
