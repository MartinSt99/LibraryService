using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;

namespace Kundenservice
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IService1" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [DataContract]
    public class OrderBooks : IOrderService
    {
        
        private readonly List<Book> books = new List<Book>();

        public List<Book> getwishList()
        {
            return books;
        }

        public void addBook(Book b)
        {
            books.Add(b);
            Console.WriteLine("Requesting AddBook for user");
            BookUpdate bgWorker = new BookUpdate
            {
                callback = OperationContext.Current.GetCallbackChannel<IBookWishlistCallback>()
            };
            Thread thread = new Thread(delegate () { bgWorker.AddBook(b); });
            thread.IsBackground = true;
            thread.Start();
        }

        public void wishlistAdd(Book b, string user)
        {
            books.Add(b);
            Console.WriteLine("Requesting wishlist add for user " + user);
            BookUpdate bgWorker = new BookUpdate();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookWishlistCallback>();
            Thread thread = new Thread(delegate () { bgWorker.WishlistAdd(b, user); });
            thread.IsBackground = true;
            thread.Start();
        }

        public Book getBook(Guid id)
        {
            //return books.Find(x => x.ID == id);
            return new Book("a", "t", DateTime.Now, " ");
        }

        public void delBook(Guid id)
        {
            //books.RemoveAll(x => x.ID == id);
        }


        
    }

    [DataContract]
    public class Book
    {
        public Book()
        {

        }
        public Book(string a, string t, DateTime p, string id)
        {
            Author = a;
            Title = t;
            existsSince = p;
            ean = id;
        }
        public int isAvailable { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public DateTime existsSince { get; set; }
        public string ean { get; set; }
        public string lastUpdated { get; set; }

    }



    [ServiceContract(CallbackContract = typeof(IBookUpdateCallback))]
    public interface IAktienInfo
    {

        [OperationContract(IsOneWay = true)]
        void RegisterForUpdate(string username, string symbol);

        [OperationContract(IsOneWay = true)]
        void getUsers(string username);

        [OperationContract(IsOneWay = true)]
        void getBooks(string username);

        [OperationContract(IsOneWay = true)]
        void getLogin(string username, string password);


    }
    [ServiceContract(CallbackContract = typeof(IBookWishlistCallback))]
    public interface IOrderService
    {
        [OperationContract]
        List<Book> getwishList();

        [OperationContract]
        void addBook(Book b);

        [OperationContract]
        Book getBook(Guid b);

        [OperationContract]
        void delBook(Guid b);

        [OperationContract(IsOneWay = true)]
        void wishlistAdd(Book b, string user);


        // TODO: Hier Dienstvorgänge hinzufügen
    }

    [ServiceContract]
    public interface IBookWishlistCallback
    {
        [OperationContract(IsOneWay = true)]
        void AddBook();

        [OperationContract(IsOneWay = true)]
        void DelBook();

        [OperationContract(IsOneWay = true)]
        void FindBook();

    }

    [ServiceContract]
    public interface IBookUpdateCallback
    {
        [OperationContract(IsOneWay = true)]
        void BookUpdate(string symbol, double preis, DataSet ds);

        [OperationContract(IsOneWay = true)]
        void UpdateUsers(DataSet ds);

        [OperationContract(IsOneWay = true)]
        void loginUser(int status);

        [OperationContract(IsOneWay = true)]
        void loadBooks(DataSet ds);
    }

    public class BookUpdate
    {

        public string ean { get; set; }
        public IBookWishlistCallback Callback { get => callback; set => callback = value; }

        public IBookWishlistCallback callback = null;
        static string password = "Linkstart1";

        string connStr =
            "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password + ";convert zero datetime=True;convert zero datetime=True";

        public void AddBook(Book b)
        {
            Console.WriteLine("Insert Book");
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `books`(`ean`, `title`, `author`, `isAvailable`, `lastUpdate`, `existsSince`) VALUES ('" + b.ean + "','" + b.Title + "','" + b.Author + "'," + b.isAvailable + ",'" + b.lastUpdated + "','" + b.existsSince + "')", conn);
            cmd.ExecuteNonQuery();
            callback.AddBook();
            Console.WriteLine("called callback\n-------------------------------\n");
        }

        public void WishlistAdd(Book b, string user)
        {
            Console.WriteLine("Insert Book into wishlist");
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `wishlish`(`uid`, `bid`, `wishCreated`) VALUES ('" + "','" + b.Title +"','" + DateTime.Now + "')", conn);
            cmd.ExecuteNonQuery();
            callback.AddBook();
            Console.WriteLine("called callback\n-------------------------------\n");


        }
    }

    public class AktienInfo : IAktienInfo
    {
        
    public void test()
        {

        }
        public void RegisterForUpdate(string symbol, string username)
        {
            Console.WriteLine("Requesting registering for update");
            Update bgWorker = new Update() { Aktie = symbol };
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            Thread t = new Thread(bgWorker.SendUpdateToClient);
            t.IsBackground = true;
            t.Start();

        }

        public void getUsers(string username)
        {
            
            Console.WriteLine("Requesting registered Users from " + username);
            Update bgWorker = new Update();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            Thread t = new Thread(bgWorker.getUsers);
            t.IsBackground = true;
            t.Start();

        }

        public void getBooks(string username)
        {
            Console.WriteLine("Requesting registered Books from " + username);
            Update bgWorker = new Update();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            Thread t = new Thread(bgWorker.getBooks);
            t.IsBackground = true;
            t.Start();

        }

        public void getLogin(string user, string pwd)
        {
            Console.WriteLine("Requesting login for user " + user);
            Update bgWorker = new Update();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            Thread thread = new Thread(delegate () { bgWorker.Login(user,pwd); });
            thread.IsBackground = true;
            thread.Start();
                
        }
    }

    
    public class Update
    {
        
        public string Aktie { get; set; }
        public IBookUpdateCallback callback = null;
        static string password = "Linkstart1";
        string connStr =
            "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password + ";convert zero datetime=True";

        public void getBooks()
        {
            Console.WriteLine("Send Books");
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM books", conn);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds, "LoadBooks");
            callback.loadBooks(ds);
            Console.WriteLine("called callback-------------------------------\n");
        }
        public void getUsers()
        {
            Console.WriteLine("Send Users");
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users", conn);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds, "LoadUsers");
            callback.UpdateUsers(ds);
            Console.WriteLine("called callback-------------------------------\n");
        }

        public void Login(string user, string pwd)
        {
            Console.WriteLine("Process Login for user " + user);
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            MySqlCommand checkUser = new MySqlCommand("SELECT COUNT(*) FROM users where username = '" + user + "' and pwd = '" + pwd + "'", conn);
            var status = int.Parse(checkUser.ExecuteScalar().ToString());
            Console.WriteLine(status);
             if (status > 0)
             {
                 callback.loginUser(1);
                 Console.WriteLine("Successfully logged in user " + user);
             }
             else
             {
                 callback.loginUser(0);
                 Console.WriteLine("Error during login user " + user);
             }
            Console.WriteLine("called callback-------------------------------\n");

        }

        public void SendUpdateToClient()
        {
            while (true)
            {
                Console.WriteLine("Check book");
                Thread.Sleep(1000);

                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users", conn);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadUsers");
                Console.WriteLine(ds.Tables.Count);
                try
                {
                    callback.BookUpdate(Aktie, 100, ds);
                }
                catch (Exception e)
                {
                    break;
                }
                

                /*Random r = new Random();

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    try
                    {

                        

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Cannot send Message to client {0}", e);
                    }
                }*/
            }
        }

    }
}