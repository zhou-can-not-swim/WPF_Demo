using System.Windows;
using System.Windows.Controls;
using WpfD.ViewModel;

namespace WpfD.ViewPage
{
    /// <summary>
    /// Page4.xaml 的交互逻辑
    /// </summary>
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();

            Class2VModel class2VModel = new Class2VModel();
            this.DataContext = class2VModel;
            Loaded+= Page4_Loaded;
        }

        

        private void Page4_Loaded(object sender, RoutedEventArgs e)
        {
            List<Model.SoftWare> softWares = App.Database.GetAllSoftWares();
            var items = softWares.Select(sw => new
            {
                sw.Name,
                sw.Detail
            }).ToList();

            datagrid.ItemsSource = softWares;
        }
    }
}
