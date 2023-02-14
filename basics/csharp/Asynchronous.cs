using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    /// <summary>
    /// - https://learn.microsoft.com/en-us/dotnet/csharp/async
    /// - https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
    /// 
    /// - The core of async programming is the Task and Task<T> objects, which model asynchronous operations
    /// </summary>
    public class Asynchronous
	{
        public Asynchronous()
		{

            TaskBasics();
            _ = AccessWebAsync();
            TaskFactory();
            TaskContinuations();
            DetachedChildTask();
            AttachedChildTask();
        }

        /// <summary>
        /// - A task represents an operation that is running or going to run
        /// - The Task class represents a single operation that does not return a value and that usually executes asynchronously.
        /// - The work performed by a Task object typically executes asynchronously on a thread pool thread rather than synchronously 
        ///   on the main application thread, you can use the Status property, as well as the IsCanceled, IsCompleted, and IsFaulted 
        ///   properties, to determine the state of a task
        /// - Tasks typically run asynchronously on a thread pool thread, the thread that creates and starts the task continues execution 
        ///   as soon as the task has been instantiated
        /// </summary>
        public void TaskBasics()
        {
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                Task.CurrentId, obj,
                Thread.CurrentThread.ManagedThreadId);
            };

            // Create a task but do not start it.
            Task t1 = new Task(action, "alpha");

            // Construct a started task
            Task t2 = Task.Factory.StartNew(action, "beta");

            // - Your application's logic may require that the calling thread continue execution only when one or more tasks have completed execution.
            // - You can synchronize the execution of the calling thread and the asynchronous tasks it launches by calling a Wait method to wait for one
            //   or more tasks to complete
            // - A call to the Wait method blocks the calling thread until the single class instance has completed execution
            // - You can also supply a cancellation token by calling the Wait(CancellationToken) and Wait(Int32, CancellationToken) methods. If the token's
            //   IsCancellationRequested property is true or becomes true while the Wait method is executing, the method throws an OperationCanceledException
            // - You can also wait for all of a series of tasks to complete by calling the "WaitAll" method

            t2.Wait(); // Block the main thread to demonstrate that t2 is executing

            // Launch t1 
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})",
                              Thread.CurrentThread.ManagedThreadId);
            // Wait for the task to finish.
            t1.Wait();

            // Construct a started task using Task.Run.
            var taskData = "delta";

            // The Run method provides a simple way to start a task using default values and without requiring additional parameters
            Task t3 = Task.Run(() => {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}", Task.CurrentId, taskData, Thread.CurrentThread.ManagedThreadId);
            });
            // Wait for the task to finish.
            t3.Wait();

            // Construct an unstarted task
            Task t4 = new Task(action, "gamma");
            // Run it synchronously
            t4.RunSynchronously();
            // Although the task was run synchronously, it is a good practice
            // to wait for it in the event exceptions were thrown by the task.
            t4.Wait();
        }

        class CustomData
        {
            public long CreationTime;
            public int Name;
            public int ThreadNum;
        }

        /// <summary>
        /// - You can also use the TaskFactory.StartNew method to create and start a task in one operation. Use this method when creation and 
        /// - scheduling do not have to be separated and you require additional task creation options or the use of a specific scheduler, or when 
        /// - you need to pass additional state into the task that you can retrieve through its Task.AsyncState property
        /// </summary>
        public void TaskFactory()
        {
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
					if (obj is not CustomData data)
						return;

					data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                }, new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            Task.WaitAll(taskArray);
            foreach (var task in taskArray)
            {
                //This state is passed as an argument to the task delegate, and it can be accessed from the task object by using the Task.AsyncState property.
                var data = task.AsyncState as CustomData;
                if (data != null)
                    Console.WriteLine($"Task #{data.Name} created at {data.CreationTime}, ran on thread #{data.ThreadNum}.");
            }
        }

        /// <summary>
        /// - The Task.ContinueWith and Task<TResult>.ContinueWith methods let you specify a task to start when the antecedent task finishes. 
        /// - The delegate of the continuation task is passed a reference to the antecedent task so that it can examine the antecedent task's status 
        ///   and, by retrieving the value of the Task<TResult>.Result property, can use the output of the antecedent as input for the continuation.
        /// </summary>
        public void TaskContinuations()
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                var message = $"Task 1 from Thread {Thread.CurrentThread.ManagedThreadId}";
                return message;
            });

            // Execute the continuation when the antecedent (Task1) finishes
            var task2 = task1.ContinueWith((antecedent) =>
            {
                Console.WriteLine(antecedent.Result);
                Console.WriteLine($"Task 2 after antecedent Task1 finishes from Thread {Thread.CurrentThread.ManagedThreadId}");
                return 22;
            });

            var task3 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 3 from Thread {Thread.CurrentThread.ManagedThreadId}"); return 22; });
            var task4 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 4 from Thread {Thread.CurrentThread.ManagedThreadId}"); return 22; });
            var task5 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 5 from Thread {Thread.CurrentThread.ManagedThreadId}"); return 22; });

            Task all = Task.Factory.ContinueWhenAll(
            new[] { task2, task3, task4, task5 },
            tasks => Console.WriteLine($"All tasks returned with total {tasks.Sum(t => t.Result)}")
            );

            //Call to the Task.Wait method to ensure that the task completes execution before the console mode application or current application ends.
            task1.Wait();

            //If an antecedent throws and the continuation fails to query the antecedent’s Exception property 
            //(and the antecedent isn’t otherwise waited upon), the exception is considered unhandled and the application dies
            //A safe pattern is to rethrow antecedent exceptions. As long as the continuation is Waited upon, 
            //the exception will be propagated and rethrown to the Waiter
            bool error = false;
            var normalTask = Task.Factory.StartNew(() => { if (error) throw null; else return "Success"; });

            var catchTask = normalTask.ContinueWith((antecedent) => Console.WriteLine(antecedent.Exception.Message), TaskContinuationOptions.OnlyOnFaulted);

            var OkayTask = normalTask.ContinueWith((antecedent) => Console.WriteLine(antecedent.Result), TaskContinuationOptions.NotOnFaulted);
        }

        /// <summary>
        /// When user code that is running in a task creates a new task and does not specify the AttachedToParent option, the new task is not synchronized 
        /// with the parent task in any special way. This type of non-synchronized task is called a detached nested task or detached child task.
        /// </summary>
        public void DetachedChildTask()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task beginning."); //This print First
                for (int ctr = 1; ctr <= 10; ctr++)
                {
                    var child = Task.Factory.StartNew((x) =>
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine($"Attached child #{x} completed by Thread {Thread.CurrentThread.ManagedThreadId}."); //This prints afterwords
                    }, ctr);
                }
            });

            parent.ContinueWith(task => Console.WriteLine($"Parent task {task.Status}")); //This print second
        }

        /// <summary>
        /// 1. When user code that is running in a task creates a task with the AttachedToParent option, the new task is known as a attached child task of 
        ///    the parent task. You can use the AttachedToParent option to express structured task parallelism, because the parent task implicitly waits for 
        ///    all attached child tasks to finish
        /// 2. Parent task can use the TaskCreationOptions.DenyChildAttach option to prevent other tasks from attaching to the parent task
        /// </summary>
        public void AttachedChildTask()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task beginning.");
                for (int ctr = 1; ctr <= 10; ctr++)
                {
                    Task.Factory.StartNew((x) =>
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine($"Attached child #{x} completed by Thread {Thread.CurrentThread.ManagedThreadId}.");
                    },
                    ctr,
                    TaskCreationOptions.AttachedToParent);
                }
            });

            parent.ContinueWith(task => Console.WriteLine($"Parent task {task.Status}"));
        }

        /// <summary>
        /// - An async keyword is a method that performs asynchronous tasks such as fetching data from a database, reading a file, etc, they can be 
        ///   marked as “async”. Whereas await keyword making  “await” to a statement means suspending the execution of the async method it is residing 
        ///   in until the asynchronous task completes
        /// - After suspension, the control goes back to the caller method. Once the task completes, the control comes back to the states where await is 
        ///   mentioned and executes the remaining statements in the enclosing method
        /// - When using async and await the compiler generates a state machine in the background 
        ///   (https://devblogs.microsoft.com/premier-developer/dissecting-the-async-methods-in-c/)
        /// </summary>
        /// <returns></returns>
        public async Task AccessWebAsync()
        {
            var content = await AccessTheWebAsync();
            Console.WriteLine($"Content Length is {content}");
        }

        /// <summary>
        /// - The method usually includes at least one await expression, which marks a point where the method can't continue until the awaited asynchronous 
        ///   operation is complete. In the meantime, the method is suspended, and control returns to the method's caller
        /// - If GetStringAsync (and therefore getStringTask) completes before AccessTheWebAsync awaits it, control remains in AccessTheWebAsync 
        /// - The expense of suspending and then returning to AccessTheWebAsync would be wasted if the called asynchronous process (getStringTask) has already 
        ///   completed and AccessTheWebAsync doesn't have to wait for the final result
        /// </summary>
        async Task<int> AccessTheWebAsync()
        {
			using HttpClient client = new();
			Task<string> getStringTask = client.GetStringAsync("http://www.guimp.com/");

			string urlContents = await getStringTask;

			return urlContents.Length;
		}
    }
}
