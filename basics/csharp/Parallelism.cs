using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace CSharp
{
	class Parallelism
    {
        /// <summary>
        /// - https://www.infoworld.com/article/3614819/how-to-use-parallelfor-and-parallelforeach-in-csharp.html
        /// </summary>
        public void Play()
        {
            ParallelFor();
            ParallelForEach();
            PLINQ();
            ParallelInvoke();
        }

        /// <summary>
        /// - The For method will perform the same kind of iteration you would normally make using a for loop but each iteration would be executed in a new task. 
        /// </summary>
        public void ParallelFor()
        {
            var rnd = new int[] { 4, 1, 6, 2, 9, 5, 10, 3 };
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4 //it is the upper limit for the entire parallel operation, irrespective of the number of cores, max threads are 4.
            };

            Console.WriteLine($"Your CPU has {Environment.ProcessorCount} Cores");
            // Executes up to 20 iterations of a loop in parallel.
            var result = Parallel.For( 
                0,
                rnd.Count(),
                options,
                (index) =>
                {
                    Console.WriteLine($"Beginning Iteration {index}");
                    var delay = rnd[index];
                    Thread.Sleep(delay);
                    Console.WriteLine($"Total execution time by Thread {Thread.CurrentThread.ManagedThreadId} of Iteration {index} is {MockDataUtility.EllapsedTime(delay)}");
                    Console.WriteLine($"Iteration {index} Completed");
                }
            );
        }

        /// <summary>
        ///  - This example sums up the elements of an int[] in parallel.
        ///  - Each thread maintains a local sum. When a thread is initialized, that local sum is set to 0.
        ///    On every iteration the current element is added to the local sum.
        ///  - When a thread is done, it safely adds its local sum to the global sum.
        ///  - After the loop is complete, the global sum is printed out.
        /// </summary>
        public void ParallelForEach()
        {
            // The sum of these elements is 40.
            int[] input = { 4, 1, 6, 2, 9, 5, 10, 3 };
            int sum = 0;

            try
            {
                Parallel.ForEach(
                        input,// source collection
                        () => 0,// thread local initializer
                        (n, loopState, localSum) =>// body
                        {
                            localSum += n;
                            Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId}, n={n}, localSum={localSum}");
                            return localSum;
                        },
                        (localSum) => Interlocked.Add(ref sum, localSum)// thread local aggregator
                    );

                Console.WriteLine("\nSum={0}", sum);
            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException e)
            {
                Console.WriteLine("Parallel.ForEach has thrown an exception. THIS WAS NOT EXPECTED.\n{0}", e);
            }
        }

        /// <summary>
        /// Parallel LINQ (PLINQ) is a parallel implementation of LINQ to Objects.
        /// 
        /// var queryA = from num in numberList.AsParallel()
        ///              select ExpensiveFunction(num); //good for PLINQ
        /// var queryB = from num in numberList.AsParallel()
        ///              where num % 2 > 0
        ///              select num; //not as good for PLINQ 
        /// </summary>
        public void PLINQ()
        {
            Console.WriteLine("Calculate Prime Numbers of range (3 t0 1000)");
            var numbers = Enumerable.Range(3, 100);

            MockDataUtility.Watch.Start();
            var sequentialQuery = from n in numbers
                                  where ExpensiveFunction(n)
                                  select n;
            Console.WriteLine($"Total Prime Numbers: {sequentialQuery.Count()}");
            MockDataUtility.Watch.Stop();
            Console.WriteLine($"Sequential LINQ Execution took {MockDataUtility.EllapsedTime(MockDataUtility.Watch.ElapsedMilliseconds)}s");

            MockDataUtility.Watch.Reset();
            MockDataUtility.Watch.Start();
            var parallerQuery = from n in numbers.AsParallel()
                                where ExpensiveFunction(n)
                                select n;
            Console.WriteLine($"Total Prime Numbers: {parallerQuery.Count()}");
            MockDataUtility.Watch.Stop();
            Console.WriteLine($"Parallel LINQ Execution took {MockDataUtility.EllapsedTime(MockDataUtility.Watch.ElapsedMilliseconds)}s");


            //Parallel.Invoke executes an array of Action delegates in parallel, and then waits for them to complete
            Parallel.Invoke(() =>
                {
                    Thread.Sleep(2000); Console.WriteLine("Invoke 1");
                },
                    () =>
                    {
                        Thread.Sleep(1000); Console.WriteLine("Invoke 2");
                    }
            );

            Console.WriteLine($"");
            //Parallel.For and Parallel.ForEach perform the equivalent of a C# for and foreach loop, 
            //but with each iteration executing in parallel instead of sequentially.
            MockDataUtility.Watch.Reset();
            var totalIterations = 3000;

            MockDataUtility.Watch.Start();
            for (int i = 0; i < totalIterations; i++) { }
            MockDataUtility.Watch.Stop();
            Console.WriteLine($"for(,,) loop took {MockDataUtility.EllapsedTime(MockDataUtility.Watch.ElapsedMilliseconds)}s for {totalIterations} iterations");

            MockDataUtility.Watch.Reset();
            MockDataUtility.Watch.Start();
            Parallel.For(0, totalIterations, i => { });
            MockDataUtility.Watch.Stop();
            Console.WriteLine($"Parallel.For loop took {MockDataUtility.EllapsedTime(MockDataUtility.Watch.ElapsedMilliseconds)}s for {totalIterations} iterations");
        }

        private bool ExpensiveFunction(int value)
        {
            Thread.Sleep(100);
            //Console.WriteLine("Method=Theta, Thread={0}", Thread.CurrentThread.ManagedThreadId);
            return Enumerable.Range(2, (int)Math.Sqrt(value)).All(x => value % x > 0);
        }

        /// <summary>
        /// Executes each of the provided actions, possibly in parallel.
        /// No guarantees are made about the order in which the operations execute or whether they execute in parallel. 
        /// This method does not return until each of the provided operations has completed, regardless of whether completion occurs 
        /// due to normal or exceptional termination.
        /// </summary>
        public void ParallelInvoke()
        {
            // Retrieve Goncharov's "Oblomov" from Gutenberg.org.
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            // Perform three tasks in parallel on the source array
            Parallel.Invoke(() =>
                                        {
                                            Console.WriteLine("Begin first task...");
                                            GetLongestWord(words);
                                        },// close first Action

                                        () =>
                                        {
                                            Console.WriteLine("Begin second task...");
                                            GetMostCommonWords(words);
                                        }, //close second Action

                                        () =>
                                        {
                                            Console.WriteLine("Begin third task...");
                                            GetCountForWord(words, "sleep");
                                        } //close third Action
                                    );//close parallel.invoke

            Console.WriteLine("Returned from Parallel.Invoke");
        }

        #region HelperMethods
        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;

            Console.WriteLine($@"Task 3 -- The word ""{term}"" occurs {findWord.Count()} times.");
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonWords = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 -- The most common words are:");
            foreach (var v in commonWords)
            {
                sb.AppendLine("  " + v);
            }
            Console.WriteLine(sb.ToString());
        }

        private static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                               orderby w.Length descending
                               select w).First();

            Console.WriteLine($"Task 1 -- The longest word is {longestWord}.");
            return longestWord;
        }

        // An http request performed synchronously for simplicity.
        private static string[] CreateWordArray(string uri)
        {
            Console.WriteLine($"Retrieving from {uri}");

            // Download a web page the easy way.
            string s = new WebClient().DownloadString(uri);

            // Separate string into an array of words, removing some common punctuation.
            return s.Split(
                 new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' },
                 StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion
    }
}