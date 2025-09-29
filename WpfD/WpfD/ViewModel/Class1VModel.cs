using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WpfD.Model;

namespace WpfD.ViewModel
{
    public partial class Class1VModel : ObservableObject
    {
        public ObservableCollection<User> Users { get; set; }

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? email;


        public Class1VModel()
        {
            Users = UserManager.GetUsers();
        }

        [RelayCommand]
        private void Test(object obj)
        {
            Name = "小1";
            Email = "111@qq.com";
        }

        [RelayCommand]
        private void AddUser(object obj)
        {
            User user = new User();
            user.Name = Name;
            user.Email = Email;
            UserManager.AddUser(user);
        }

        //private RelayCommand? addUserCommand;
        //public IRelayCommand AddUserCommand => addUserCommand ??= new RelayCommand(AddUser);
    }

}
