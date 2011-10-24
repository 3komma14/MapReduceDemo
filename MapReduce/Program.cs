using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapReduce
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input reader
            var lines = File.ReadAllLines("LoremIpsum.txt");

            #region Non Task Version

            // Map
            var startMapNonTask = DateTime.Now;
            var mappedResultNonTask = MapNonTask(lines);
            var endMapNonTask = DateTime.Now;
            Console.WriteLine("Word count = {0}", mappedResultNonTask.Count());
            Console.WriteLine("Non Task Time: {0}", endMapNonTask.Subtract(startMapNonTask).Milliseconds);
            Console.ReadLine();

            #endregion            
            
            // Map
            var startMap = DateTime.Now;
            var mappedResult = Map(lines);
            var endMap = DateTime.Now;
            Console.WriteLine("Word count = {0}", mappedResult.Count());
            Console.WriteLine("Time: {0}", endMap.Subtract(startMap).Milliseconds);
            Console.ReadLine();

            // Reduce
            var result = Reduce(mappedResult);
            Console.WriteLine("Reduce result: Lorem found {0} times", result);
            Console.ReadLine();
        }

        private static IEnumerable<WordItem> Map(IList<string> lines)
        {
            var result = new List<WordItem>();
            var tasks = new Task<IEnumerable<WordItem>>[lines.Count];

            // Split
            for (var i = 0; i < lines.Count; i++)
            {
                var index = i;
                var data = lines[i];
                tasks[i] = Task.Factory.StartNew(() => PartialMap(index, data));
            }

            Task.WaitAll(tasks);
            
            // Combine
            foreach (var task in tasks)
            {
                result.AddRange(task.Result);
            }

            return result;
        }        
        
        private static IEnumerable<WordItem> MapNonTask(IList<string> lines)
        {
            var result = new List<WordItem>();

            // Split
            for (var i = 0; i < lines.Count; i++)
            {
                result.AddRange(PartialMap(i, lines[i]));
            }

            return result;
        }

        private static int Reduce(IEnumerable<WordItem> mappedResult)
        {
            return mappedResult.Sum(x => string.Compare(x.Key, "Lorem") == 0 ? x.Value : 0);
        }

        private static IEnumerable<WordItem> PartialMap(int id, string input)
        {
            Console.WriteLine("Map(id = {0})", id);
            Thread.Sleep(10); 
            var tokenizer = new StringTokenizer(input);
            return tokenizer.GetTokens().Select(token => new WordItem(token, 1));
        }
    }
}
