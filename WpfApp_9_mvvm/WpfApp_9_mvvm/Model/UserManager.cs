using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_9_mvvm.Model
{
    public static class UserManager
    {
        public static ObservableCollection<User> DataBaseUsers = new ObservableCollection<User>()
     {
         new User() { Name = "小王", Email = "123@qq.com" },
         new User() { Name = "小红", Email = "456@qq.com" },
         new User() { Name = "小五", Email = "789@qq.com" }
     };

        public static ObservableCollection<User> GetUsers()
        {
            return DataBaseUsers;
        }

        public static void AddUser(User user)
        {
            DataBaseUsers.Add(user);
        }
    }

}
