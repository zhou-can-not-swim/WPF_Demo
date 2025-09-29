using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp_12_View_ViewModel
{
    //INotifyPropertyChanged接口用于实现数据绑定中的属性更改通知,没有它，扩展123没办法实现        前端---->后端
    //当绑定到UI元素的数据源中的属性值发生更改时，INotifyPropertyChanged接口可以通知UI元素更新。
    public class MainViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        public ICommand AddUserCommand { get; set; }

        //扩展5：一定要注释这一行，要不然会有冲突
        //public string? Name { get; set; }
        //public string? Email { get; set; }

        //扩展1：首先添加一个测试命令：
        public ICommand TestCommand { get; set; }

        public MainViewModel()
        {
            Users = UserManager.GetUsers();
            AddUserCommand = new RelayCommand(AddUser, CanAddUser);//给外部的AddUserCommand赋值操作，前端点击“添加”按钮调用AddUser方法
            //扩展2：在构造函数中添加：
            TestCommand = new RelayCommand(Test, CanTest);
        }

        private bool CanAddUser(object obj)
        {
            return true;
        }

        private void AddUser(object obj)
        {
            User user = new User();
            user.Name = Name;      //调用方法后,往数组中添加值
            user.Email = Email;
            UserManager.AddUser(user);
        }

        //扩展3：实现Test与CanTest方法：
        private bool CanTest(object obj)
        {
            return true;
        }

        private void Test(object obj)
        {
            Name = "小1";
            Email = "111@qq.com";
        }

        //和INotifyPropertyChanged配套使用

        //扩展4:还需要添加Name和Email,这是和前端一致的

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
