using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

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
            var bgWorker = new BookUpdate
            {
                callback = OperationContext.Current.GetCallbackChannel<IBookWishlistCallback>()
            };
            var thread = new Thread(delegate() { bgWorker.AddBook(b); });
            thread.IsBackground = true;
            thread.Start();
        }

        public void wishlistAdd(Book b, string user)
        {
            Console.WriteLine(b.Author, b.Title);
            Console.WriteLine("Requesting wishlist add for user " + user);
            var bgWorker = new BookUpdate();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookWishlistCallback>();
            var thread = new Thread(delegate() { bgWorker.WishlistAdd(b, user); });
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

        public int id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string published { get; set; }
        public string publishedDate { get; set; }
        public string language { get; set; }
        public string description { get; set; }
        public string pageCount { get; set; }
        public string categories { get; set; }
        public string averageRating { get; set; }
        public string maturityRating { get; set; }
        public string imageLinks { get; set; }
        public int isAvailable { get; set; }
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

        [OperationContract]
        AktienInfo.ServiceData getLogin(string username, string password);
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
        void loginUser(AktienInfo.ServiceData status, AktienInfo.ReturnedBooks rb);

        [OperationContract(IsOneWay = true)]
        void loadBooks(DataSet ds);
    }

    public class BookUpdate
    {
        private static readonly string password = "Linkstart1";

        private readonly string connStr =
            "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password +
            ";convert zero datetime=True;convert zero datetime=True";

        public IBookWishlistCallback callback;

        public string ean { get; set; }

        public IBookWishlistCallback Callback
        {
            get { return callback; }
            set { callback = value; }
        }

        public void AddBook(Book b)
        {
            Console.WriteLine("Insert Book");
            var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd =
                new MySqlCommand(
                    "INSERT INTO `books`(`ean`, `title`, `author`, `isAvailable`, `lastUpdate`, `existsSince`) VALUES ('" +
                    b.ean + "','" + b.Title + "','" + b.Author + "'," + b.isAvailable + ",'" + b.lastUpdated + "','" +
                    b.existsSince + "')", conn);
            cmd.ExecuteNonQuery();
            callback.AddBook();
            Console.WriteLine("called callback\n-------------------------------\n");
        }

        public void WishlistAdd(Book b, string user)
        {
            Console.WriteLine("Insert Book into wishlist");
            var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd =
                new MySqlCommand(
                    "INSERT INTO `wishlist`(`uid`, `bid`, `dateAdded`) VALUES ('" + user + "','" + b.Title + "','" +
                    DateTime.Now + "')", conn);

            cmd.ExecuteNonQuery();
            callback.AddBook();
            Console.WriteLine("called callback\n-------------------------------\n");
        }
    }

    public class AktienInfo : IAktienInfo
    {
        public void RegisterForUpdate(string symbol, string username)
        {
            Console.WriteLine("Requesting registering for update");
            var bgWorker = new Update {Aktie = symbol};
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            var t = new Thread(bgWorker.SendUpdateToClient);
            t.IsBackground = true;
            t.Start();
        }

        public void getUsers(string username)
        {
            Console.WriteLine("Requesting registered Users from " + username);
            var bgWorker = new Update();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            var t = new Thread(bgWorker.getUsers);
            t.IsBackground = true;
            t.Start();
        }

        public void getBooks(string username)
        {
            Console.WriteLine("Requesting registered Books from " + username);
            var bgWorker = new Update();
            bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
            var t = new Thread(bgWorker.getBooks);
            t.IsBackground = true;
            t.Start();
        }

        public ServiceData getLogin(string user, string pwd)
        {
            var SD = new ServiceData();
            try
            {
                Console.WriteLine("Requesting login for user " + user);
                var bgWorker = new Update();
                bgWorker.callback = OperationContext.Current.GetCallbackChannel<IBookUpdateCallback>();
                var thread = new Thread(delegate() { SD = bgWorker.Login(user, pwd); });
                thread.IsBackground = true;
                thread.Start();

                return SD;
            }
            catch (Exception e)
            {
                SD.Result = false;
                SD.ErrorMessage =
                    "Can't login, please try again with other credentials or check your network connectivity.";
                SD.ErrorDetails = e.ToString();
                throw new FaultException<ServiceData>(SD, e.ToString());
            }
        }

        public void test()
        {
        }

        [DataContract]
        public class ServiceData
        {
            [DataMember]
            public bool Result { get; set; }

            [DataMember]
            public string ErrorMessage { get; set; }

            [DataMember]
            public string ErrorDetails { get; set; }
        }

        [DataContract]
        public class ReturnedBooks
        {
            [DataMember]
            public bool hasReturned { get; set; }

            [DataMember]
            public string Books { get; set; }
        }


        public static class F
        {
            public static string Dump(object obj)
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        public class Update
        {
            private static readonly string password = "Linkstart1";

            private readonly string connStr =
                "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password +
                ";convert zero datetime=True";

            public IBookUpdateCallback callback;

            public string Aktie { get; set; }

            public void getBooks()
            {
                Console.WriteLine("Send Books");
                var conn = new MySqlConnection(connStr);
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM books", conn);
                var adp = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adp.Fill(ds, "LoadBooks");
                callback.loadBooks(ds);
                Console.WriteLine("called callback-------------------------------\n");
            }

            public void getUsers()
            {
                Console.WriteLine("Send Users");
                var conn = new MySqlConnection(connStr);
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM users", conn);
                var adp = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adp.Fill(ds, "LoadUsers");
                callback.UpdateUsers(ds);
                Console.WriteLine("called callback-------------------------------\n");
            }

            public ServiceData Login(string user, string pwd)
            {
                var sdc = new ServiceData();
                Console.WriteLine("Process Login for user " + user);
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var checkUser =
                    new MySqlCommand(
                        "SELECT COUNT(*) FROM users where username = '" + user + "' and pwd = '" + pwd + "'", conn);
                var status = int.Parse(checkUser.ExecuteScalar().ToString());


                if (status > 0)
                {
                    var comd = new MySqlCommand("SELECT * FROM logs", conn);
                    var addp = new MySqlDataAdapter(comd);
                    var dds = new DataSet();
                    addp.Fill(dds, "LoadLogs");


                    var cmd = new MySqlCommand("SELECT * FROM wishlist where uid = '" + user + "'", conn);
                    var adp = new MySqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adp.Fill(ds, "LoadWishlist");

                    var ldr = new List<string>();
                    foreach (DataRow item in dds.Tables["LoadLogs"].Rows)
                        try
                        {
                            ldr.Add(
                                ds.Tables["LoadWishlist"].Select("bid = '" + item[1] + "' AND dateAdded > '" + item[2] +
                                                                 "'")[0][2].ToString());
                        }
                        catch (Exception e)
                        {
                        }

                    var stat = ldr.Count > 0;
                    Console.WriteLine(status);
                    var rb = new ReturnedBooks();
                    rb.hasReturned = false;
                    if (stat)
                    {
                        foreach (var item in ldr)
                            rb.Books += item + ";";
                        rb.hasReturned = true;
                    }

                    sdc.Result = true;
                    callback.loginUser(sdc, rb);
                    Console.WriteLine("Successfully logged in user " + user);
                    return sdc;
                }
                sdc.Result = false;
                sdc.ErrorMessage = "Can't process login.";
                sdc.ErrorDetails = "Wrong username or password specified or check your internet connectivity.";
                var rbt = new ReturnedBooks();
                rbt.hasReturned = false;
                callback.loginUser(sdc, rbt);
                Console.WriteLine("Error during login user " + user);
                return sdc;
            }

            public void SendUpdateToClient()
            {
                while (true)
                {
                    Console.WriteLine("Check book");
                    Thread.Sleep(1000);

                    var conn = new MySqlConnection(connStr);
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM users", conn);

                    var adp = new MySqlDataAdapter(cmd);

                    var ds = new DataSet();
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

                    
                }
            }
        }
    }
}