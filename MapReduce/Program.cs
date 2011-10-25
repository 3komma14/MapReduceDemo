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

            // Map
            var mappedResult = Map(lines, GetWords);
            PrintMapResult(mappedResult);
            Console.ReadLine();

            // Reduce
            var result = Reduce(mappedResult, GetWordCount("Lorem"));
            PrintReduceResult(result);
            Console.ReadLine();
        }

        #region Map

        private static IEnumerable<ResultItem> Map(IList<string> lines, Func<string, IEnumerable<ResultItem>> function)
        {
            var result = new List<ResultItem>();
            var tasks = new Task<IEnumerable<ResultItem>>[lines.Count];

            // Split
            for (var i = 0; i < lines.Count; i++)
            {
                var index = i;
                var data = lines[i];
                tasks[i] = Task.Factory.StartNew(() => function(data));
            }

            Task.WaitAll(tasks);

            // Combine
            foreach (var task in tasks)
            {
                result.AddRange(task.Result);
            }

            return result;
        }

        private static IEnumerable<ResultItem> GetWords(string input)
        {
            var tokenizer = new StringTokenizer(input);
            var words = tokenizer.GetTokens();
            return words.Select(token => new ResultItem(token, 1));
        }

        private static void PrintMapResult(IEnumerable<ResultItem> mappedResult)
        {
            Console.WriteLine("Map result: Word count = {0}", mappedResult.Count());
            foreach (var resultItem in mappedResult.Take(10))
            {
                Console.WriteLine("{0} = {1}", resultItem.Word, resultItem.Value);
            }
            Console.WriteLine("...");
        }

        #endregion

        #region Reduce

        private static int Reduce(IEnumerable<ResultItem> mappedResult, Func<ResultItem, int> function)
        {
            return mappedResult.Sum(function);
        }

        private static Func<ResultItem, int> GetWordCount(string word)
        {
            return x => string.Compare(x.Word, word) == 0 ? x.Value : 0;
        }

        private static void PrintReduceResult(int result)
        {
            Console.WriteLine("Reduce result: Lorem found {0} times", result);
        }

        #endregion

    }
}
