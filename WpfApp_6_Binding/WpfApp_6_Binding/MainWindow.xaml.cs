using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace WpfApp_6_Binding
{
    // 定义一个名为 ObservableObject 的公共类
    // 它实现了 INotifyPropertyChanged 接口，这个接口是.NET中数据绑定通知的核心
    public class ObservableObject : INotifyPropertyChanged
    {
        // 声明一个公开的事件，名为 PropertyChanged
        // 事件类型是 PropertyChangedEventHandler（属性改变事件处理器）
        // 这个事件是接口要求必须实现的。当属性的值发生变化时，会触发这个事件来通知UI更新
        public event PropertyChangedEventHandler PropertyChanged;

        // 定义一个公共方法，用于触发PropertyChanged事件
        // [CallerMemberName] 是一个编译器特性（Attribute）
        // 它表示：如果调用此方法时不传入参数，编译器会自动将调用此方法的那个属性名（即Name, Age, Address）作为参数传进来
        // string propertyName = "" 给参数一个默认值（空字符串），使参数变为可选参数
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            // 1. `PropertyChanged?` ： 这是空值传播运算符（Null-conditional operator）
            //    意思是：先检查 PropertyChanged 事件是否为null（即有没有事件订阅者/监听者）
            //    如果为null（没人监听），则什么都不做，直接跳过。
            //    如果不为null（有人监听），则执行 `.Invoke(...)`
            // 2. `this` : 指当前类的实例，即哪个对象发出了属性改变的通知
            // 3. `new PropertyChangedEventArgs(propertyName)` : 创建一个事件参数对象，其中包含了发生改变的属性名称
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Person : ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                // 将传入的值（value关键字代表赋的值）赋给私有字段 name
                name = value;
                // 调用从父类继承来的 RaisePropertyChanged 方法
                // 由于使用了 [CallerMemberName]，编译器会自动将当前属性名 "Name" 作为参数传入
                // 这会触发 PropertyChanged 事件，通知所有订阅者（比如UI界面）：“Name”属性已经变了，请更新！
                RaisePropertyChanged();
            }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; RaisePropertyChanged(); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; RaisePropertyChanged(); }
        }
    }

    // 定义一个名为 MainViewModel 的公共类（视图模型）
    // 它继承自 ObservableObject，因此具备属性变化通知的能力
    public class MainViewModel : ObservableObject
    {
        // 声明一个私有字段，用于存储 Person 对象的引用
        private Person person;

        // 声明一个公共属性，也叫 Person，用于对外暴露私有字段
        public Person Person
        {
            // 当外部读取时，返回私有字段 person
            get { return person; }
            // 当外部设置时
            set
            {
                // 1. 将传入的值赋给私有字段
                person = value;
                // 2. 发出通知：告诉UI"Person"这个属性本身发生了变化
                //    （比如整个Person对象被替换成了另一个新的）
                RaisePropertyChanged();
            }
        }

        // 构造函数：在创建 MainViewModel 实例时自动执行
        public MainViewModel()
        {
            // 初始化私有字段 person，创建一个新的 Person 对象
            person = new Person
            {
                // 使用对象初始化器，为Person对象的属性赋初始值
                Name = "张三",      // 设置姓名为"张三"
                Age = 50,          // 设置年龄为50
                Address = "居无定所", // 设置地址为"居无定所"
            };
        }
    }


    // 这是局部类定义，由Visual Studio自动生成，与XAML文件配合构成一个完整的窗口
    public partial class MainWindow : Window
    {
        // 窗口的构造函数
        public MainWindow()
        {
            // 初始化组件：这个方法由编译器自动生成
            // 它会加载对应的MainWindow.xaml文件，创建其中定义的按钮、文本框等控件
            InitializeComponent();

            // 设置窗口的 DataContext（数据上下文）
            // 将一个新的 MainViewModel 实例赋值给 DataContext
            // 这意味着：这个窗口以及窗口内的所有控件，默认的数据源都是这个 MainViewModel 对象
            this.DataContext = new MainViewModel();
        }

        // 按钮点击事件的处理方法
        // 当用户在界面上点击按钮时，会自动调用这个方法
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前窗口的数据上下文（DataContext），并尝试将其转换为 MainViewModel 类型
            var vm = DataContext as MainViewModel;

            // 安全判断：如果转换失败（vm为null），则直接返回，不执行后面的代码
            if (vm == null) return;

            // 1. 修改ViewModel中Person的Age属性
            //    生成一个1到99之间的随机数，并赋值
            vm.Person.Age = new Random().Next(1, 100);

            // 2. 修改ViewModel中Person的Address属性
            //    将当前的时间日期转换成字符串，并赋值
            vm.Person.Address = DateTime.Now.ToString();

            // 注意：这里修改的是 Person 对象的 Age 和 Address 属性
            // 由于 Person 类本身也继承自 ObservableObject，它的每个属性在set时都会调用 RaisePropertyChanged()
            // 所以UI上绑定到 Person.Age 和 Person.Address 的控件会自动更新显示
        }
    }
}