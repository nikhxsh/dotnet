using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ObjectOriented.StructuralPatterns
{
	/// <summary>
	/// - Structural Patterns
	/// - Add responsibilities to objects dynamically
	/// - Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative 
	///   to subclassing for extending functionality.
	/// - This structural code demonstrates the Decorator pattern which dynamically adds extra functionality to 
	///   an existing object.  
	/// </summary>
	class DecoratorPattern
	{
		public DecoratorPattern()
		{
			// Create book
			var sms = new SMS("SMS Sender", "Migration Created with Id 1234");
			sms.Display();

			var slack = new Slack("Nikhilesh", "dev-updates", "Merge to develop successfull");
			slack.Display();

			// Make slack trackable, then track activity
			Console.WriteLine("Making message trackable:");

			var trackMessage = new TrackMessage(slack);
			trackMessage.TrackActivity("127.0.0.1", "Logged in");
			trackMessage.TrackActivity("127.0.0.1", "Sent message to dev-updates");

			trackMessage.Display();
		}

		/// <summary>
		/// The 'Component' abstract class
		/// </summary>
		abstract class Notifier
		{
			protected string IP { get; set; }
			protected string Message { get; set; }
			protected DateTime Timestamp { get; set; }

			public abstract void Display();
		}

		/// <summary>
		/// The 'ConcreteComponent' class
		/// </summary>
		class SMS : Notifier
		{
			private string Sender { get; set; }
			private string Provider { get; set; }

			// Constructor
			public SMS(string sender, string message)
			{
				Sender = sender;
				Message = message;
				Timestamp = DateTime.Now;
			}

			public override void Display()
			{
				Console.WriteLine("---- Message ---- ");
				Console.WriteLine($"Sender: {Sender}");
				Console.WriteLine($"Message: {Message}");
				Console.WriteLine($"Time: {Timestamp}");
			}
		}

		/// <summary>
		/// The 'ConcreteComponent' class
		/// </summary>
		class Slack : Notifier
		{
			private string Sender { get; set; }
			private string Channel { get; set; }

			// Constructor
			public Slack(string sender, string channel, string message)
			{
				Sender = sender;
				Channel = channel;
				Message = message;
				Timestamp = DateTime.Now;
			}

			public override void Display()
			{
				Console.WriteLine("---- Message ---- ");
				Console.WriteLine($"Sender: {Sender}");
				Console.WriteLine($"Channel: {Channel}");
				Console.WriteLine($"Message: {Message}");
				Console.WriteLine($"Time: {Timestamp}");
			}
		}


		/// <summary>
		/// The 'Decorator' abstract class
		/// </summary>
		abstract class Decorator : Notifier
		{
			protected Notifier notifier;

			// Constructor
			public Decorator(Notifier notifier)
			{
				this.notifier = notifier;
			}

			public override void Display()
			{
				notifier.Display();
			}
		}

		/// <summary>
		/// The 'ConcreteDecorator' class
		/// </summary>
		class TrackMessage : Decorator
		{
			protected List<string> activities = new List<string>();

			// Constructor
			public TrackMessage(Notifier notifier)
			  : base(notifier)
			{
			}

			public void TrackActivity(string ip, string detail)
			{
				IP = ip;
				activities.Add(detail);
			}

			public override void Display()
			{
				base.Display();

				foreach (string activity in activities)
				{
					Console.WriteLine($"{IP} : {activity}");
				}
			}
		}
	}
}
