using System;

namespace ObjectOriented.StructuralPatterns
{
	/// <summary>
	/// - Structural Patterns
	/// - Match interfaces of different classes
	/// - Convert the interface of a class into another interface that clients expect. 
	/// - Adapter lets classes work together that couldn't otherwise because of incompatible interfaces
	/// - To understand this definition, let's use a  real-world simple example. You know a language, "A". 
	///   You visit a place where language "B" is spoken. But you don't know how to speak that language. 
	///   So you meet a person who knows both languages, in other words A and B. So he can act as an adapter 
	///   for you to talk with the local people of that place and help you communicate with them.
	/// </summary>
	class AdapterPattern
	{
		public AdapterPattern()
		{
			var spanish = new Spanish();
			var hindi = new Hindi();

			Console.WriteLine("--- Spanish ---");
			Console.WriteLine(spanish.Hablar());

			Console.WriteLine("--- Hindi ---");
			Console.WriteLine(hindi.Namaste());

			Console.WriteLine("--- Spanish to Hindi ---");
			var targetAdapter = new SpanishAdapter(spanish);
			Console.WriteLine(targetAdapter.Namaste());
		}

		public interface ISpanish
		{
			public string Hablar();
		}

		public class Spanish : ISpanish
		{
			public string Hablar()
			{
				return "Hola Amigos!";
			}
		}

		public interface IHindi
		{
			public string Namaste();
		}

		public class Hindi : IHindi
		{
			public string Namaste()
			{
				return "Namaste!";
			}
		}

		public class SpanishAdapter : IHindi
		{
			private Spanish _spanish { get; set; }

			public SpanishAdapter(Spanish _spanish)
			{
				this._spanish = _spanish;
			}

			public string Namaste()
			{
				return _spanish.Hablar();
			}
		}
	}
}
