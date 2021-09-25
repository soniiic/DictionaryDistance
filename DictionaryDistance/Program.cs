using System;
using System.IO;
using System.Linq;

namespace DictionaryDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            var srcFile = File.ReadAllText(args[0]);
            var words = srcFile.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            var sourceWord = args[2];
            var destWord = args[3];

            IStrategy strategy = new BranchingStrategy(words, sourceWord, destWord);

            var distance = strategy.FindDistance();

            if (distance != null)
            {
                Console.WriteLine($"Success! Found connection at distance: {distance.Depth}");
                foreach (var word in distance.Path)
                {
                    Console.WriteLine(word);
                }
                
                File.WriteAllLines(args[1], distance.Path);
            }
            else
            {
                Console.WriteLine("Search exhausted");
            }

            Console.ReadKey();
        }
    }
}
