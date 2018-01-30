using System;
using System.Data;
using System.ServiceModel;
using System.Windows;
using Client.ServiceReference1;
using Kundenservice;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für BookManagement.xaml
    /// </summary>
    public partial class BookManagement : Window, IBookUpdateCallback, IBookWishlistCallback
    {
        public DataSet books;
        public InstanceContext context;
        public AktienInfoClient proxy;
        public ServiceReference2.OrderServiceClient client;
        public BookManagement()
        {
            InitializeComponent();
            context = new InstanceContext(this);
            client = new ServiceReference2.OrderServiceClient(context);
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
            var outtext = "";
            Book b = new Book();
            var ctr = 0;

            foreach (var item in dataGrid.SelectedCells)
            {var obj = (item.Column.GetCellContent(item.Item) as TextBlock).Text;
                if (ctr == 1)
                {
                    b.ean = obj;
                }else if(ctr == 2)
                {
                    b.Title = obj;
                }else if(ctr == 3)
                {
                    b.Author = obj;
                }else if(ctr == 4){
                    b.isAvailable = int.Parse(obj);
                }else if(ctr== 5)
                {
                    b.lastUpdated = obj;
                }else if(ctr == 6)
                {
                    b.existsSince = Convert.ToDateTime(obj);
                }
                ctr++;
            }

            Console.WriteLine("Test");

        }

        public void AddBook()
        {
            //Book tmpBook = new Book()
            //client.addBook()
        }

        public void DelBook()
        {
            throw new NotImplementedException();
        }

        public void FindBook()
        {
            throw new NotImplementedException();
        }
    }
}