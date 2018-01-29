using System.Data;
using System.ServiceModel;
using System.Windows;
using Client.ServiceReference1;
using Kundenservice;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für BookManagement.xaml
    /// </summary>
    public partial class BookManagement : Window, IBookUpdateCallback
    {
        public DataSet books;
        public InstanceContext context;
        public AktienInfoClient proxy;

        public BookManagement()
        {
            InitializeComponent();
            context = new InstanceContext(this);
            proxy = new AktienInfoClient(context);
        }

       
       
    
        
        public void loadBooks(DataSet ds)
        {
            dataGrid.ItemsSource = books.Tables["LoadBooks"].DefaultView;
        }

        public void loginUser(int status)
        {
        }

        public void UpdateUsers(DataSet ds)
        {
        }

        public void BookUpdate(string x, double test, DataSet ds)
        {
        }

        private void btnAddWishlist_Click(object sender, RoutedEventArgs e)
        {
            proxy.
        }
    }
}