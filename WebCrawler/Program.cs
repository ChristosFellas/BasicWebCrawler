using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {

            var shoeList = StartCrawl("https://www.flightclub.com/adidas/adidas-yeezy");
            Console.ReadLine();
        }

        private static async Task<List<Shoe>> StartCrawl(string url)
        {
            var httpClient = new HttpClient();

            var html = await  httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var resultsList = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("item-container")).ToList();


            var shoes = new List<Shoe>();

            foreach (var item in resultsList)
            {
                var shoe = new Shoe();

                shoe.Name = item.Descendants("p").FirstOrDefault().InnerHtml;
                shoe.Price = item.Descendants("span").
                    Where(node => node.GetAttributeValue("class", "")
                    .Equals("price")).FirstOrDefault().InnerHtml;

                StringBuilder sb = new StringBuilder(shoe.Price);

                sb.Remove(0, 1);

                shoe.Price = sb.ToString();

                shoe.ImageUrl = item.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value;

                shoes.Add(shoe);
            }

            return shoes;
        }
    }
}
