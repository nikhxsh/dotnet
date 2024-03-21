using System;

namespace ObjectOriented.CreationalPatterns
{
	/// <summary>
	/// - Creational Patterns
	/// - Allows you to create families of related objects that have a common theme or purpose
	/// - Encapsulates the creation logic and hides the specific concrete classes from the client, promoting loose coupling
	/// - Enables easy substitution of one family of objects with another, as long as they adhere to the common interface or abstract class
	/// - Provides a structured approach to create and manage related objects, making it easier to extend or modify the family of objects
	/// </summary>
	class AbstractFactoryPattern
    {

        public AbstractFactoryPattern()
		{
			Console.WriteLine("--- Windows Gui ---");
			var windowsFactory = new WindowsGuiFactory();
			var widowsUserInterface = new UserInterface(windowsFactory);
			widowsUserInterface.Render();


			Console.WriteLine("--- Mac Gui ---");
			var macFactory = new MacGuiFactory();
			var macUserInterface = new UserInterface(macFactory);
			macUserInterface.Render();
		}


		/// <summary>
		/// AbstractProductA
		/// </summary>
		interface IButton
        {
            void Render();
        }


		/// <summary>
		/// AbstractProductB
		/// </summary>
		interface ICheckbox
		{
			void Render();
		}

		/// <summary>
		/// AbstractProductA's ProductA1
		/// </summary>
		class WindowsButton : IButton
		{
			public void Render()
			{
                Console.WriteLine("Rendering a Windows button");
			}
		}

		/// <summary>
		/// AbstractProductA's ProductA2
		/// </summary>
		class MacButton : IButton
		{
			public void Render()
			{
				Console.WriteLine("Rendering a Mac button");
			}
		}

		/// <summary>
		/// AbstractProductB's ProductB1
		/// </summary>
		class WindowsCheckbox : ICheckbox
		{
			public void Render()
			{
				Console.WriteLine("Rendering a Windows checkbox");
			}
		}

		/// <summary>
		/// AbstractProductB's ProductB2
		/// </summary>
		class MacCheckbox : ICheckbox
		{
			public void Render()
			{
				Console.WriteLine("Rendering a Mac checkbox");
			}
		}

		/// <summary>
		/// Abstract factory
		/// </summary>
		interface IGuiFactory
        {
			IButton CreateButton();
			ICheckbox CreateCheckbox();
        }

		/// <summary>
		/// Concrete Factory - WindowsFactory
		/// </summary>
		class WindowsGuiFactory : IGuiFactory
		{
			public IButton CreateButton()
			{
				return new WindowsButton();
			}

			public ICheckbox CreateCheckbox()
			{
                return new WindowsCheckbox();
			}
		}

		/// <summary>
		/// Concrete Factory - MacFactory
		/// </summary>
		class MacGuiFactory : IGuiFactory
		{
			public IButton CreateButton()
			{
				return new MacButton();
			}

			public ICheckbox CreateCheckbox()
			{
				return new MacCheckbox();
			}
		}

		/// <summary>
		/// The 'Client' class 
		/// </summary>
		class UserInterface
		{
			private readonly IButton button;
			private readonly ICheckbox checkbox;

			public UserInterface(IGuiFactory guiFactory)
			{
				button = guiFactory.CreateButton();
				checkbox = guiFactory.CreateCheckbox();
			}

			public void Render()
			{
				button.Render();
				checkbox.Render();
			}
		}
    }
}
