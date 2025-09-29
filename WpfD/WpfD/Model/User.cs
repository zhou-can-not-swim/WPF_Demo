using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfD.Model
{
    ////页面2用这个
    //public class User
    //{
    //    public string? Name { get; set; }
    //    public string? Email { get; set; }
    //}

    //页面3用这个(页面2其实也可以使用这个)
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _email;
    }


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
