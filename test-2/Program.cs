using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("歡迎來到1A2b遊戲!");
            Random random = new Random();
            int[] answer =Enumerable.Range(0,10).OrderBy(x => random.Next()).Take(4).ToArray();
            int guessCount = 0;

            foreach (var i in answer)
            {
                Console.WriteLine(i);
            }

            while (true)
            {
                Console.WriteLine("輸入一個四位數:");
                string input =Console.ReadLine();
                if (input.Length != 4 || input.Distinct().Count() != 4)
                {
                    Console.WriteLine("請輸入不重複數字");
                    continue;
                }
                int[] guess = input.Select((c) => c - '0').ToArray();
                int correctCount = guess.Where((digit, index) => digit == answer[index]).Count();
                int existCount = guess.Intersect(answer).Count() - correctCount;
                Console.WriteLine($"{correctCount}A{existCount}B");
                guessCount++;

                if (correctCount == 4)
                {
                    Console.WriteLine("恭喜你!答對了~~");
                    break;
                }

            }
            Console.Write("你要繼續玩嗎？(y/n): ");
            string playAgain = Console.ReadLine();
            if (playAgain.ToLower() == "n")
            {
                Console.WriteLine("遊戲結束，下次再來玩喔～");
                Thread.Sleep(3000);
            }
            else
            {
                Main(null);
            }
        }
    }
}
