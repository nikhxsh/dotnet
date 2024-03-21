using System;
using System.Collections.Generic;

namespace ObjectOriented.CreationalPatterns
{

	/// <summary>
	/// - Creational Patterns
	/// - Ensure a class has only one instance and provide a global point of access to it.
	/// </summary>
	class SingletonPattern
	{
		public SingletonPattern()
		{
			//LoadBalancer x = new LoadBalancer();
			LoadBalancer b1 = LoadBalancer.GetInstance();
			LoadBalancer b2 = LoadBalancer.GetInstance();
			LoadBalancer b3 = LoadBalancer.GetInstance();
			LoadBalancer b4 = LoadBalancer.GetInstance();

			if (b1 == b2 && b2 == b3 && b3 == b4)
				Console.WriteLine("Same innstance");

			LoadBalancer balancer = LoadBalancer.GetInstance();

			for (int i = 0; i < 20; i++)
			{
				string server = balancer.Server;
				Console.WriteLine($"Dispatch Request {server}");
			}

			Console.WriteLine($"EarlySingleton Init counter > {EarlySingleton.counter}");

			var earlySingleton1 = EarlySingleton.GetInstance();
			earlySingleton1.Log("New instace earlySingleton1");
			Console.WriteLine($"Instance counter > {EarlySingleton.counter}");

			var earlySingleton2 = EarlySingleton.GetInstance();
			earlySingleton1.Log("New instace earlySingleton2");
			Console.WriteLine($"Instance counter > {EarlySingleton.counter}");


			Console.WriteLine($"LazySingleton Init counter > {LazySingleton.counter}");

			var lazySingleton1 = LazySingleton.GetInstance();
			lazySingleton1.Log("New instace lazySingleton1");
			Console.WriteLine($"Instance counter > {LazySingleton.counter}");

			var lazySingleton2 = LazySingleton.GetInstance();
			lazySingleton1.Log("New instace lazySingleton2");
			Console.WriteLine($"Instance counter > {LazySingleton.counter}");

		}

		public sealed class LoadBalancer
		{
			private static LoadBalancer _loadBalancer;
			private List<string> _servers = new List<string>();
			private Random _random = new Random();

			private static object syncLock = new object();
			private LoadBalancer()
			{
				_servers.Add("Server 1");
				_servers.Add("Server 2");
				_servers.Add("Server 3");
				_servers.Add("Server 4");
				_servers.Add("Server 5");
			}

			public static LoadBalancer GetInstance()
			{
				if (_loadBalancer == null)
				{
					lock (syncLock)
					{
						if (_loadBalancer == null)
							_loadBalancer = new LoadBalancer();
					}
				}

				return _loadBalancer;
			}

			public string Server
			{
				get
				{
					int r = _random.Next(_servers.Count);
					return _servers[r].ToString();
				}
			}
		}


		/// <summary>
		/// - The instance of the singleton class is created eagerly during the class loading process, regardless of whether it is needed or not
		/// - The instance is created as soon as the class is loaded
		/// - The above class creates an instance as soon as we access any static property or method
		/// - Within the GetInstance() method, we don’t need to write the object initialization, null checking, and thread-safety code, as the CLR will 
		///	  handle all these things
		///	- Use eager loading when you have plenty of resources available and want to ensure that the Singleton instance is ready for use immediately
		///	- Opt for eager loading if the Singleton initialization is quick and inexpensive
		///	- Eager loading consumes memory when the application starts because the instance is created upfront
		/// </summary>
		public sealed class EarlySingleton
		{
			public static int counter = 0;

			//create instance eagerly
			private static readonly EarlySingleton instance = new EarlySingleton();

			// This will called only once
			private EarlySingleton()
			{
				Console.WriteLine("EarlySingleton Instance Created");
				counter++;
			}

			public static EarlySingleton GetInstance()
			{
				return instance;
			}

			public void Log(string message)
			{
				Console.WriteLine(message);
			}
		}

		/// <summary>
		/// - It's simple and performs well
		/// - It also allows you to check whether or not the instance has been created yet with the IsValueCreated property
		/// - The code above implicitly uses LazyThreadSafetyMode.ExecutionAndPublication as the thread safety mode for the Lazy<Singleton>, so the Lazy<T> objects are, 
		///   by default, thread-safe
		/// - Use lazy loading to conserve resources and only create the Singleton instance when needed
		/// - Choose lazy loading if the initialization of the Singleton instance involves significant computational or time-consuming tasks
		/// - Lazy loading can save memory because the Singleton instance is created only when needed
		/// </summary>
		public sealed class LazySingleton
		{
			public static int counter = 0;
			//create instance eagerly
			private static readonly Lazy<LazySingleton> lazySingleton = new Lazy<LazySingleton>(() => new LazySingleton());

			private LazySingleton()
			{
				Console.WriteLine("LazySingleton Instance Created");
				counter++;
			}

			public static LazySingleton GetInstance()
			{
				return lazySingleton.Value;
			}

			public void Log(string message)
			{
				Console.WriteLine(message);
			}
		}
	}
}
