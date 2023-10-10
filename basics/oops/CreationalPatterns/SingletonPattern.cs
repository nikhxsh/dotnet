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
		}

		sealed class LoadBalancer
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
		/// We can choose to create the instance of Singleton class when the class is loaded.This is thread-safe without using locking.
		/// </summary>
		public sealed class EarlySingleton
		{
			//create instance eagerly
			private static EarlySingleton instance = new EarlySingleton();

			private EarlySingleton() { }

			public static EarlySingleton GetInstance()
			{
				return instance;//just return the instance
			}
		}

		/// <summary>
		/// - It's simple and performs well. It also allows you to check whether or not the instance has been created yet with the IsValueCreated property
		/// - The code above implicitly uses LazyThreadSafetyMode.ExecutionAndPublication as the thread safety mode for the Lazy<Singleton>
		/// </summary>
		public sealed class LazySingleton
		{
			//create instance eagerly
			private static readonly Lazy<LazySingleton> lazySingleton = new Lazy<LazySingleton>(() => new LazySingleton());

			private LazySingleton() { }

			public static LazySingleton GetInstance
			{
				get
				{
					return lazySingleton.Value;
				}
			}
		}
	}
}
