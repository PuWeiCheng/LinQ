using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LinQ_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string Files = (@"C:\Users\wayne\source\repos\product.csv");
            var text = File.ReadAllLines("C:\\Users\\wayne\\source\\repos\\product (1).csv").Skip(1).ToList();
            List<Product> list = new List<Product> { };

            foreach (var item in text)
            {
                list.AddRange(CreateList(item));
                //Console.WriteLine(item);
            }
            //////////////////////////////////////////
            var pricetotal = list.Sum((x) => x.Price*x.Amount);
            Console.WriteLine($"全部商品總價格:{pricetotal}"); //計算商品總價格
            decimal avePrice = list.Average((x) => x.Price);
            Console.WriteLine($"全部商品平均價格{avePrice:N2}");//平均價格
            int totalAmount = list.Sum((b) => b.Amount);
            Console.WriteLine($"商品的總數量{totalAmount}");//計算商品總數量
            double avetotalAmount = list.Average((x) => x.Amount);
            Console.WriteLine($"商品的平均數量:{avetotalAmount:N2}");//計算商品平均數量
            Product expensive = list.OrderByDescending((x) => x.Price).First();
            Console.WriteLine($"商品最貴是:{expensive.Name}");//最貴商品
            Product Cheapest = list.OrderBy((x) => x.Price).First();
            Console.WriteLine($"商品最便宜是:{Cheapest.Name}");//最便宜商品
            decimal totalValuesof3C = list.Where((x) => x.Category == "3C").Sum((x) => x.Price * x.Amount);
            Console.WriteLine($" 3C類的商品總價:{totalValuesof3C}");//3C類產品總價
            decimal totalfood_drink = list.Where((x) => x.Category == "食品"||x.Category == "飲料").Sum((x) => x.Price * x.Amount);
            Console.WriteLine($"食物飲料類的商品總價:{totalfood_drink}");//食物和飲料產品總價
            Console.WriteLine("-------------------------------");

            var foodwithlarge = list.Where((x) => x.Category == "食品" && x.Amount > 100); //9
            Console.WriteLine("食品並商品數量大於100:");//商品大於100的商品(食品)
            foreach (var large_food in foodwithlarge)
            {
                Console.WriteLine(large_food.Name);
            }

            Console.WriteLine("-------------------------------");

            var expensiveProductsByCategory = list //10
                .Where(p => p.Price > 1000)
                .GroupBy(p => p.Category)
                .ToList();
            Console.WriteLine($"價格大於 1000 的商品: ");
            foreach (var groups in expensiveProductsByCategory)
            {
                Console.WriteLine($"{groups.Key}類 ");
                foreach (var product1 in groups)
                {
                    Console.WriteLine($"{product1.No} {product1.Name} {product1.Amount} {product1.Price}");
                }
            }

            Console.WriteLine("-------------------------------");

            var avgPriceByCategory = list //11
                        .GroupBy(p => p.Category)
                        .Select(g => new
                        {
                            Category = g.Key,
                            AveragePrice = g.Average(p => p.Price)
                        })
                        .ToList();
            Console.WriteLine($"各類別的平均價格: ");
            foreach (var group in avgPriceByCategory)
            {
                Console.WriteLine($"{group.Category}: {group.AveragePrice:N2}");
            }

            Console.WriteLine("-------------------------------");

            Console.WriteLine("商品價格由高到低排序:");
            var productbypricedes = list.OrderByDescending((x) => x.Price);//12
            foreach (var z in productbypricedes)
            {
                Console.WriteLine($"{z.No} {z.Name} {z.Amount} {z.Price}");
            }
            Console.WriteLine("-------------------------------");

            Console.WriteLine("商品數量由低到高排序:");
            var productsSortedByQuantityAscending = list//13
                .OrderBy(p => p.Amount)
                .ToList();
            foreach (var product in productsSortedByQuantityAscending)
            {
                Console.WriteLine($"{product.No} {product.Name} {product.Amount} {product.Price}");
            }

            Console.WriteLine("-------------------------------");

            var mostExpensiveProductsByCategory = list//14
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(p => p.Price).FirstOrDefault());
            foreach (var kvp in mostExpensiveProductsByCategory)
            {
                Console.WriteLine("{0}類", kvp.Key);
                Console.WriteLine("最貴的商品: {0}", kvp.Value.Name);
            }

            Console.WriteLine("-------------------------------");

            var mostCheaptestProductsByCategory = list //15
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.OrderBy(p => p.Price).FirstOrDefault());
            foreach (var kvps in mostCheaptestProductsByCategory)
            {
                Console.WriteLine("{0}類", kvps.Key);
                Console.WriteLine("最貴的商品: {0}", kvps.Value.Name);
            }

            Console.WriteLine("-------------------------------");
            Console.WriteLine("價格小於等於 10000 的商品");
            var inexpensiveProducts = from p in list //16
                                      where p.Price <= 10000
                                      select p;

            foreach (var p in inexpensiveProducts)
            {
                Console.WriteLine("{0},{1},{2},{3}, {4}",
                                  p.No, p.Name, p.Amount, p.Price, p.Category);
            }

            Console.WriteLine("-------------------------------");

            int totalItems = list.Count;


            int itemsPerPage = 4;

 
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

  
            int currentPage = 1;

            while (true)
            {
    
                var currentPageData = list.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

 
                Console.WriteLine("Page {0}", currentPage);
                foreach (var product in currentPageData)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}", product.No, product.Name, product.Amount, product.Price, product.Category);
                }

                
                if (currentPage == totalPages)
                {
                    break;
                }

                Console.WriteLine("按下鍵下一頁，按上鍵上一頁");
                //ConsoleKeyInfo key = Console.ReadKey();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
  
                    currentPage++;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
 
                    currentPage--;
                }
                else
                {
 
                    break;
                }
            }

            //Console.ReadLine();
        }

        static List<Product> CreateList(string read)
        {

            string[] save = read.Split(',');
            return new List<Product>()
            {
                new Product
                {
                    No = save[0],
                    Name = save[1],
                    Amount = int.Parse(save[2]),
                    Price = Convert.ToDecimal(save[3]),
                    Category = save[4]
                }
            };




        }
    }
}
