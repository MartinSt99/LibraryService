using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;

namespace Client
{
    class BookLookup
    {
        public static BooksService service = new BooksService(
            new BaseClientService.Initializer
            {
                ApplicationName = "git-server",
                ApiKey = "AIzaSyCj1CyB6GBbejRkSD2sV9XAqcS7QzeVHE8"
            });
        public static async Task<Volume> SearchISBN(string isbn)
        {
            
            Console.WriteLine("Executing a book search request…");
           
            var result = service.Volumes.List(isbn).Execute();
            if(result != null && result.Items != null)
            {
                var item = result.Items[1];
                return item;
            }
            return null;
        }
    }
}
