﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Tools.Models;

namespace CSharp
{
    class Concepts
    {
        public void Play()
        {
            //Delegates();
            //Events();
            //Actions();
            //Functions();
            //Identifiers();
            //Indexer();
            //Discards();
            //CovarianceAndContravariance();
            //TryCatch();
        }

        /// <summary>
        /// - Value types derive from System.ValueType, which derives from System.Object. Types that derive from System.ValueType 
        ///  have special behavior in the CLR. Value type variables directly contain their values, which means that the memory is 
        ///  allocated inline in whatever context the variable is declared. There is no separate heap allocation or garbage collection 
        ///  overhead for value-type variables.
        ///  
        /// - There are two categories of value types: struct and enum.
        ///  a. The built-in numeric types are structs, and they have properties and methods that you can access.
        ///  b. All enums inherit from System.Enum, which inherits from System.ValueType. All the rules that apply to structs also apply to enums.
        ///  
        /// - Value types are sealed, which means, for example, that you cannot derive a type from System.Int32, and you cannot define a 
        ///  struct to inherit from any user-defined class or struct because a struct can only inherit from System.ValueType. 
        /// </summary>
        private void ValueTypes()
        {
            // Static method on type byte.
            byte b = byte.MaxValue;

            byte num = 0xA;
            int i = 5;
            char c = 'Z';

            // Because literals are typed, and all types derive ultimately from System.Object, you can write and compile code such as
            string s = "The answer is " + 5.ToString();
            Type type = 12345.GetType();
        }

        /// <summary>
        /// - A type that is defined as a class, delegate, array, or interface is a reference type. At run time, when you declare a variable of a reference type, 
        ///  the variable contains the value null until you explicitly create an object by using the new operator, or assign it an object that has been created 
        ///  elsewhere by using new
        /// - When the object is created, the memory is allocated on the managed heap, and the variable holds only a reference to the location of the object. 
        ///  Types on the managed heap require overhead both when they are allocated and when they are reclaimed by the automatic memory management functionality 
        ///  of the CLR, which is known as garbage collection. 
        /// - However, garbage collection is also highly optimized, and in most scenarios it does not create a 
        ///  performance issue. 
        /// - All arrays are reference types, even if their elements are value types. Arrays implicitly derive from the System.Array class
        /// </summary>
        private void ReferenceTypes()
        {
            Singer singer = new Singer();

			// An interface must be initialized together with a class object that implements it. If Student implements IStudent, you create an instance of 
			// Student as shown in the following
			IStudent student = new Student();
        }

        private void Types()
        {
            // --Implicitly typed local variables--
            // The var keyword instructs the compiler to infer the type of the variable from the expression on the right side of the initialization statement. 
            // The inferred type may be a built-in type, an anonymous type, a user-defined type, or a type defined in the .NET Framework class library
            // var can only be used when a local variable is declared and initialized in the same statement; the variable cannot be initialized to null
            // var cannot be used on fields at class scope

            // i is compiled as an int
            var i = 5;

            // --Anonymous types --
            // provide a convenient way to encapsulate a set of read-only properties into a single object without having to explicitly define a type 
            // first. The type name is generated by the compiler and is not available at the source code level. The type of each property is inferred by the compiler.
            // Anonymous types are class types that derive directly from object, and that cannot be cast to any type except object. 
            var v = new { Amount = 108, Message = "Hello" };

            // --Nullable types--
            // instances of the System.Nullable<T> struct. Nullable types can represent all the values of an underlying type T, and an additional null value. 
            // The underlying type T can be any non-nullable value type. T cannot be a reference type.
            // The syntax T? is shorthand for Nullable<T>. The two forms are interchangeable.
            int? nullInt = null;
            Nullable<int> otherNullInt = null;

            // --Unmanaged types --
            // An unmanaged type is any type that isn't a reference type or constructed type (a type that includes at least one type argument), and doesn't 
            // contain reference type or constructed type fields at any level of nesting.In other words, an --unmanaged type is one of the following --:
            //  sbyte, byte, short, ushort, int, uint, long, ulong, char, float, double, decimal, or bool
            //  Any enum type
            //  Any pointer type
            //  Any user-defined struct type that is not a constructed type and contains fields of unmanaged types only
        }

        private void CastingAndtypeConversions()
        {
            // --Implicit conversions--
            // A long can hold any value an int can hold, and more!
            // https:// docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/implicit-numeric-conversions-table
            int num = 2147483647;
            long bigNum = num;

            // --Explicit conversions--
            // A cast is a way of explicitly informing the compiler that you intend to make the conversion and that you are aware that data loss might occur.
            // https:// docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/explicit-numeric-conversions-table
            double x = 1234.7;
            int a;
            // Cast double to int.
            a = (int)x;
            // Output: 1234

            // The type is a static type, but an object of type dynamic bypasses static type checking
            // At compile time, an element that is typed as dynamic is assumed to support any operation.
            dynamic dynamic_ec = new Student();
        }

        private void BoxingAndUnboxing()
        {
            // --Boxing -- is the process of converting a value type to the type object or to any interface type implemented by this value type
            // When the CLR boxes a value type, it wraps the value inside a System.Object instance and stores it on the managed heap
            // Boxing a value type allocates an object instance on the heap and copies the value into the new object.
            int i = 123;
            // The following line boxes i.
            object o = i;

            // --Unboxing -- extracts the value type from the object. Boxing is implicit; unboxing is explicit. The concept of boxing and unboxing 
            // underlies the C# unified view of the type system in which a value of any type can be treated as an object.
            // Checking the object instance to make sure that it is a boxed value of the given value type.
            // Copying the value from the instance into the value-type variable.
            o = 123;
            i = (int)o; // unboxing

            //Parameter overloading
            var boxingUnboxing = new BoxingUnboxing();
            object y = 12;
            boxingUnboxing.Play(y);
            int x = 12;
            boxingUnboxing.Play(x);
        }

        class BoxingUnboxing
        {
            public void Play(object param)
            {
                Console.WriteLine($"Object : {param}");
            }

            public void Play(int param)
            {
                Console.WriteLine($"Int : {param}");
            }
        }


        /// <summary>
        /// - Covariance and contravariance are supported for reference types, but they are not supported for value types.
        /// - In C#, covariance and contravariance enable implicit reference conversion for array types, delegate types,
        ///   and generic type arguments
        /// - Covariance preserves assignment compatibility and contravariance reverses it.
        /// - Covariance and contravariance are collectively referred to as variance. A generic type parameter that is not
        ///   marked covariant or contravariant is referred to as invariant
        /// </summary>
        private void CovarianceAndContravariance()
        {
            // Assignment compatibility.   
            string str = "test";
            // An object of a more derived type is assigned to an object of a less derived type.   
            object obj = str;

            // Covariance.   
            IEnumerable<string> strings = new List<string>();
            // An object that is instantiated with a more derived type argument   
            // is assigned to an object instantiated with a less derived type argument.   
            // Assignment compatibility is preserved.   
            IEnumerable<object> objects = strings;

            // For generic type parameters, the out keyword specifies that the type parameter is covariant. You can use the out keyword in generic interfaces and delegates.
            ICovariant<object> iobj1 = new Covariant<object>();
            ICovariant<string> istr1 = new Covariant<string>();

			// ICovariant<string> istr2 = istr1; // The following statement generates a compiler error because classes are invariant

			// You can assign istr1 to iobj1 because
			// the ICovariant interface is covariant.
			iobj1 = istr1;


            // Contravariance.             
            // Assume that the following method is in the class:   
            static void SetObject(object o) { }
            Action<object> actObject = SetObject;
            // An object that is instantiated with a less derived type argument   
            // is assigned to an object instantiated with a more derived type argument.   
            // Assignment compatibility is reversed.   
            Action<string> actString = actObject;

            //For generic type parameters, the in keyword specifies that the type parameter is contravariant. You can use the in keyword in generic interfaces and delegates.
            IContravariant<object> iobj2 = new Sample2<object>();
            IContravariant<string> istr2 = new Sample2<string>();

			// You can assign iobj2 to istr2 because
			// the IContravariant interface is contravariant.
			istr2 = iobj2;
        }

		// For generic type parameters, the in keyword specifies that the type parameter is covariant
		interface ICovariant<out R>
		{
			R GetSomething();
		}
		class Covariant<R> : ICovariant<R>
		{
			R ICovariant<R>.GetSomething()
			{
				return default(R);
			}
		}

		// For generic type parameters, the in keyword specifies that the type parameter is contravariant
		interface IContravariant<in A> { }
        class Sample2<A> : IContravariant<A> { }

        /// <summary>
        /// - Array types are reference types derived from the abstract base type Array. 
        /// - Since this type implements IEnumerable and IEnumerable<T>, you can use foreach iteration on all arrays in C#.
        /// </summary>
        private void Arrays()
        {
            // Declare a single-dimensional array. 
            int[] array1 = new int[5];

            // Declare and set array element values.
            int[] array2 = new int[] { 1, 3, 5, 7, 9 };

            // Alternative syntax.
            int[] array3 = { 1, 2, 3, 4, 5, 6 };

            // Declare a two dimensional array.
            int[,] multiDimensionalArray1 = new int[2, 3];

            // Declare and set array element values.
            int[,] multiDimensionalArray2 = { { 1, 2, 3 }, { 4, 5, 6 } };

            // Declare a jagged array.
            int[][] jaggedArray = new int[6][];

            // Set the values of the first array in the jagged array structure.
            jaggedArray[0] = new int[4] { 1, 2, 3, 4 };
        }

        private void Operators()
        {
            // The --default --operator produces the default value of a type
            // The argument to the default operator must be the name of a type or a type parameter.
            Console.WriteLine(default(int)); // output: 0
            Console.WriteLine(default(object)); // output: null

            // The --delegate operato --r creates an anonymous method that can be converted to a delegate type
            Func<int, int, int> sum = delegate (int a, int b) { return a + b; };
            Action greet = delegate { Console.WriteLine("Hello!"); };

            // The --nameof -- operator obtains the name of a variable, type, or member as the string constant
            Console.WriteLine(nameof(System.Collections.Generic)); // output: Generic
            Console.WriteLine(nameof(List<int>)); // output: List
            Console.WriteLine(nameof(List<int>.Count)); // output: Count
            Console.WriteLine(nameof(List<int>.Add)); // output: Add
            var numbers = new List<int> { 1, 2, 3 };
            Console.WriteLine(nameof(numbers)); // output: numbers
            Console.WriteLine(nameof(numbers.Count)); // output: Count
            Console.WriteLine(nameof(numbers.Add)); // output: Add

            // The --sizeof --operator returns the number of bytes occupied by a variable of a given type.
            // The argument to the sizeof operator must be the name of an unmanaged type or a type parameter that is constrained 
            // to be an unmanaged type.
            Console.WriteLine(sizeof(byte)); // output: 1
            Console.WriteLine(sizeof(double)); // output: 8
            unsafe
            {
                Console.WriteLine(sizeof(IntPtr*)); // output: 8
            }

            // The --stackalloc-- operator allocates a block of memory on the stack. A stack allocated memory block created during the method 
            // execution is automatically discarded when that method returns. You cannot explicitly free memory allocated with the stackalloc 
            // operator. A stack allocated memory block is not subject to garbage collection and doesn't have to be pinned with the fixed 
            // statement.
            // Span<int> numbers = stackalloc int[12];
        }

        private void Identifiers()
        {
            //verbatim identifier
            Console.WriteLine($"-- Identifiers --");
            // The-- @ special character-- serves as a verbatim identifier

            // The @ character prefixes a code element that the compiler is to interpret as an identifier rather than a C# keyword
            string[] @for = { "John", "James", "Joan", "Jamie" };
            for (int ctr = 0; ctr < @for.Length; ctr++)
            {
                Console.WriteLine($"Here is your gift, {@for[ctr]}!");
            }

            // Simple escape sequences (such as "\\" for a backslash), hexadecimal escape sequences (such as "\x0041" for an uppercase A), 
            // and Unicode escape sequences (such as "\u0041" for an uppercase A) are interpreted literally
            string filename1 = @"c:\documents\files\u0066.txt";

            // To enable the compiler to distinguish between attributes in cases of a naming conflict. An attribute is a class that derives from Attribute.
        }

        /// <summary>
        /// Generics introduce to the .NET Framework the concept of type parameters, which make it possible to design classes and methods that defer the 
        /// specification of one or more types until the class or method is declared and instantiated by client code.
        /// </summary>
        private void Generics()
        {
            var genericArray1 = new Generic<int>(5);
            genericArray1.Add(2);
            genericArray1.Add(4);
            genericArray1.Add(5);

            var genericArray2 = new Generic<double>(5);
            genericArray2.Add(55.6);

            // var newContraint = new Generic<Base>(5);
            // var classContraint = new Generic<int>(5);
            // var structContraint = new Generic<Student>(5);
            // var baseContraint = new Generic<Student>(5);
            // var interfaceContraint = new Generic<Category>(5);
        }

        /// <summary>
        /// - Indexers enable indexed properties: properties referenced using one or more arguments. Those arguments provide an index into some 
        ///  collection of values.
        /// </summary>
        private void Indexer()
        {
            Console.WriteLine($"-- Indexer --");
            var student = new Student { StudentId = 10002, Marks = 99 };
            student.AssignedSubject = new string[] { "Physics", "Chemistry", "Maths" };
            Console.WriteLine(student[0]); // Physics
            Console.WriteLine(student[2]); // Maths
        }

        /// <summary>
        /// - Starting with C# 7.0, C# supports discards, which are temporary, dummy variables that are intentionally unused in application code
        /// </summary>
        private void Discards()
        {
            Console.WriteLine($"-- Discards --");
            var student = new Student { StudentId = 10002, Name = "RAW", Marks = 99, StudentGrade = Grade.A };

            // --Deconstruct-- the person object.
            var (_, name, _, grade) = student;
            Console.WriteLine($"{name} has grade {grade}!");

            var choice = 2;
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case object _: // Some object type without format information
                    break;
            }

            _ = Task.Run(() => { Console.WriteLine($"{name} has grade {grade}!"); });
        }

        /// <summary>
        /// - A delegate is a type that represents references to methods with a particular parameter list and return type. 
        /// - When you instantiate a delegate, you can associate its instance with any method with a compatible signature and return type. 
        /// - You can invoke (or call) the method through the delegate instance.
        /// - Event handlers are nothing more than methods that are invoked through delegates.
        /// - Any method from any accessible class or struct that matches the delegate type can be assigned to the delegate. The method can be 
        ///   either static or an instance method. This makes it possible to programmatically change method calls, and also plug new code into 
        ///   existing classes.
        /// - In the context of method overloading, the signature of a method does not include the return value. But in the context of delegates, 
        ///   the signature does include the return value. In other words, a method must have the same return type as the delegate.
        /// - https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/ms173173(v=vs.100)
        /// </summary>
        public delegate int PerformCalculation(int x, int y);
        private void Delegates()
        {
            PerformCalculation handler = (a, b) => a + b;
            Console.WriteLine($"(Using delegate) 10 + 40 = {handler(10, 40)}");
            CalculateWithCallback(10, 50, handler);

            Console.WriteLine();

            //A delegate can call more than one method when invoked. This is referred to as multicasting.
            PerformCalculation calculationHandler = new PerformCalculation(Add);
            calculationHandler += Sub;
            calculationHandler += Divide;
            //All methods are called in FIFO (First in First Out) order.
            Console.WriteLine("Multicast Invokation");
            calculationHandler(100, 20);
            Console.WriteLine("Removing Divide Method.");
            calculationHandler -= Divide;
            calculationHandler(100, 20);
        }

        private void CalculateWithCallback(int param1, int param2, PerformCalculation callback)
        {
            Console.WriteLine($"(Using delegate callback) 10 + 50 = {callback(param1, param2)}");
        }

        private int Add(int param1, int param2)
        {
            Console.WriteLine($"{param1} + {param2} is {param1 + param2}");
            return param1 + param2;
        }

        private int Sub(int param1, int param2)
        {
            Console.WriteLine($"{param1} - {param2} is {param1 - param2}");
            return param1 - param2;
        }
        private int Multiply(int param1, int param2)
        {
            Console.WriteLine($"{param1} * {param2} is {param1 * param2}");
            return param1 / param2;
        }

        private int Divide(int param1, int param2)
        {
            Console.WriteLine($"{param1} / {param2} is {param1 / param2}");
            return param1 / param2;
        }

        /// <summary>
        /// - Events enable a class or object to notify other classes or objects when something of interest occurs. 
        /// - The class that sends (or raises) the event is called the publisher and the classes that receive (or handle) the event 
        ///   are called subscribers.
        /// - The publisher determines when an event is raised; the subscribers determine what action is taken in response to the event.
        /// - An event can have multiple subscribers. A subscriber can handle multiple events from multiple publishers.
        /// </summary>
        private void Events()
        {
            var emailPublisher = new EmailPublisher();
            var emailSubscriber1 = new EmailSubscriber { EventId = 1000 };
            var emailSubscriber2 = new EmailSubscriber { EventId = 2000 };
            emailSubscriber1.SubscribeToEmailNotification(emailPublisher);
            emailSubscriber2.SubscribeToEmailNotification(emailPublisher);

            emailPublisher.Send("Nikhilesh");
            //emailPublisher.Send("Jon Snow");
        }

        private class NotifyArgs : EventArgs
        {
            public NotifyArgs(string s)
            {
                Message = s;
            }
            public string Message { get; set; }
        }

        private class EmailPublisher
        {
            //Represents the method that will handle an event that has no event data.
            public event EventHandler<NotifyArgs> RaiseEmailNotificationEvent;
            public void Send(string name)
            {
                OnRaiseEmailNotificationEvent(new NotifyArgs(name));
            }

            protected virtual void OnRaiseEmailNotificationEvent(NotifyArgs e)
            {
                EventHandler<NotifyArgs> handler = RaiseEmailNotificationEvent;
                if (handler != null)
                {
                    e.Message += $" sent email at {DateTime.Now}";
                    handler(this, e);
                }
            }
        }

        private class EmailSubscriber
        {
            public int EventId { get; set; }

            public void SubscribeToEmailNotification(EmailPublisher emailPublisher)
            {
                emailPublisher.RaiseEmailNotificationEvent += HandleEmailNotification;
            }

            void HandleEmailNotification(object sender, NotifyArgs e)
            {
                Console.WriteLine($"{EventId}: {e.Message}");
            }
        }

        /// <summary>
        /// - Encapsulates a method that has no parameters and does not return a value.
        /// - When you use the Action delegate, you do not have to explicitly define a delegate that encapsulates a 
        ///   parameterless procedure
        /// </summary>
        private void Actions()
        {
            Action<string> printToConsole2 = PrintMessage;
            printToConsole2("Using Action<String>");

            //Starting with C# 7.0, C# supports local functions. Local functions are private methods of a type that are nested in another member
            void printToConsole3(string message) { PrintMessage(message); }
            printToConsole3("Using Action delegate");

            void printToConsole4(string message) => PrintMessage(message);
            printToConsole4("Using Action Lambda expression");
        }

        private void PrintMessage(string message)
        {
            Console.WriteLine($"Hello {message}!");
        }

        /// <summary>
        /// - Encapsulates a method that has one parameter and returns a value of the type specified by the TResult parameter.
        /// </summary>
        private void Functions()
        {
            //Func<string, string> selector = str => str.ToUpper();
            //Local function
            string selector(string str) => str.ToUpper();

            string[] words = { "orange", "apple", "Article", "elephant" };

            IEnumerable<string> upperCaseWords = words.Select(selector);
            foreach (var word in upperCaseWords)
                Console.WriteLine(word);

            Func<string, int> contentData = GetContentLength;
            Console.WriteLine($"You are Hired!.Length() > { contentData("You are Hired!")}");

            Func<string, string> convert = delegate (string s)
            {
                return s.ToUpper();
            };

            Console.WriteLine($"Hello Brother > {convert("hello brother!")}");
        }

        private int GetContentLength(string content)
        {
            return content.Length;
        }

        /// <summary>
        /// - Lazy initialization is a technique that defers the creation of an object until the first time it is needed. In other words, 
        ///   initialization of the object happens only on demand.
        /// - The System.Lazy<T> class simplifies the work of performing lazy initialization and instantiation of objects. 
        /// - By initializing objects in a lazy manner, you can avoid having to create them at all if they are never needed, 
        ///   or you can postpone their initialization until they are first accessed
        /// </summary>

        private Lazy<LazyClass> lazyClass = new Lazy<LazyClass>(() => new LazyClass());

        private void LazyInitialization()
        {
            //Lazy initialization occurs the first time the Lazy.Value property is accessed or the Lazy.ToString method is called
            var lazyInit1 = lazyClass.Value;
            var lazyInit2 = lazyClass.ToString();
        }

        public class LazyClass
        {
            public long[] HugeAllocation { get; set; } = new long[1000000];
        }

        private void Attributes()
        {
			//C# enables programmers to invent new kinds of declarative information, called attributes. Programmers can then attach attributes 
			//to various program entities, and retrieve attribute information in a run-time environment. 

			//For instance, a framework might define a HelpAttribute attribute that can be placed on certain program elements (such as classes and methods) 
			//to provide a mapping from those program elements to their documentation.
		}


		private string TryCatch()
		{
			try
			{
				return "a";
			}
			catch (Exception)
			{
				return "b";
			}
			finally
			{
				//Compile time error: controll can not leave the body of Finally clause
				//return "c";
			}
		}

		class Generic<T>
			where T : struct // The type argument must be a value type. Any value type except Nullable<T>
							 // where T : Student 
							 // where T: class // The type argument must be a reference type. This constraint applies also to any class, interface, delegate, or array type
							 // where T : unmanaged // The type argument must be an unmanaged type.(Beginning with C# 7.3)
							 // where T : new() // The type argument must have a public parameterless constructor. with other constraints, the new() constraint must be last.
							 // where T : Base // The type argument must be or derive from the specified base class.
							 // where T : IStudent // The type argument must be or implement the specified interface
							 // where T : Delegate // enables you to write code that works with delegates in a type-safe manner(Beginning with C# 7.3)
							 // where T : Enum // Enum provide type-safe programming to cache results from using the static methods in System.Enum(Beginning with C# 7.3)
		{
			public T[] Array { get; set; }
			private int Index { get; set; }

			public Generic(int length)
			{
				Array = new T[length];
			}

			public void Send<TData>(ref TData param1)
			{

			}

			public void Add(T content)
			{
				Array[Index] = content;
			}
		}

		class Base
		{
			public Base(int x)
			{
			}
		}
		// You can apply constraints to multiple parameters, and multiple constraints to a single parameter
		class Test<T, U>
			 where U : struct
			 where T : Base, new()
		{ }

		/// <summary>
		/// Generic Interfaces
		/// </summary>
		interface IMonth<T> { }
		interface IMarch<T> : IMonth<T> { }
		interface IJanuary : IMonth<int> { }

		private void CSharpVersion6()
        {
            //Auto-property initializers
            //public ICollection<double> Grades { get; } = new List<double>();

            //Expression-bodied function members
            //public override string ToString() => $"{LastName}, {FirstName}";

            //using static
            //The using static enhancement enables you to import the static methods of a single class
            //using static System.Math;

            //Null-conditional operators
            //var first = person?.FirstName;

            //Null-conditional operators
            //public string FullName => $"{FirstName} {LastName}";

            //Exception filters
            //catch (System.Net.Http.HttpRequestException e) when (e.Message.Contains("301"))
            //{
            //   return "Site Moved";
            //}

            //The nameof expression
            //if (IsNullOrWhiteSpace(lastName))
            // throw new ArgumentException(message: "Cannot be blank", paramName: nameof(lastName));

            //Await in Catch and Finally blocks

            //Initialize associative collections using indexers
            //You can use them with Dictionary<TKey,TValue> collections and other types where the accessible Add method accepts more 
            //than one argument.
            Dictionary<int, string> webErrors = new Dictionary<int, string>
            {
                [404] = "Page not Found",
                [302] = "Page moved, but left a forwarding address.",
                [500] = "The web server can't come out to play today."
            };
        }

        private void CSharpVersion7()
        {
            int input = 123, sum = 0;
            //out variables
            if (int.TryParse("123", out int result))
                Console.WriteLine(result);
            else
                Console.WriteLine("Could not parse input");

            // Tuples
            // (string Alpha, string Beta) namedLetters = ("a", "b");
            // Console.WriteLine($"{namedLetters.Alpha}, {namedLetters.Beta}");

            // Pattern Matching constructs
            // - Patterns test that a value has a certain shape, and can extract information from the value when it has the matching shape.

            // The is type pattern expression
            if (input is int count)
                sum += count;

            switch (new Student { Marks = 30 })
            {
                //when clauses in case expressions
                case Student s when s.Marks == 0:
                    break;
                //var declarations in case expressions
                case var o when o.AssignedSubject.Length == 0:
                    break;
            }

            // Local functions
            // Local functions enable you to declare methods inside the context of another method
            var index = alphabetSubsetImplementation(1, 10);
            IEnumerable<int> alphabetSubsetImplementation(int start, int end)
            {
                for (var c = start; c < end; c++)
                    yield return c;
            }

            // More expression-bodied members
            // Expression-bodied constructor
            //public ExpressionMembersExample(string label) => this.Label = label;

            // Expression-bodied finalizer
            //~ExpressionMembersExample() => Console.Error.WriteLine("Finalized!");

            // Expression-bodied get/set accessors.
            //  public string Label
            //  {
            //    ...get => label;
            //    ..set => this.label = value ?? "Default label";
            //  }

            //Generalized async return types
            //async ValueTask<int> Func()
            //  {
            //   await Task.Delay(100);
            //   return 5;
            //  }
        }

		private void CSharpVersion9() // .NET5
		{
			// Record types
			// - You use the record keyword to define a reference type that provides built-in functionality for encapsulating data.
			// - You can create record types with immutable properties by using positional parameters or standard property syntax
			Person person = new(123) { FirstName = "Nancy", LastName = "Davolio" };

			// Init only setters
			// - you can create init accessors instead of set accessors for properties and indexers
			var now = new WeatherObservation
			{
				RecordedAt = DateTime.Now,
				TemperatureInCelsius = 20,
				PressureInMillibars = 998.0m
			};

			// Top-level statements
			// - Top-level statements remove unnecessary ceremony from many applications

			// Fit and finish features
			// - you can omit the type in a new expression when the created object's type is already known
			private List<WeatherObservation> _observations = new();
	    }

		public record Person(long Id)
		{
			public required string FirstName { get; set; }
			public required string LastName { get; set; }
		};

		/// <summary>
		/// A record can inherit from another record. However, a record can't inherit from a class, and a class can't inherit from a record.
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="Grade"></param>
		public record Teacher(long Id, int Grade) : Person(Id)
		{
		}

		public struct WeatherObservation
		{
			public DateTime RecordedAt { get; init; }
			public decimal TemperatureInCelsius { get; init; }
			public decimal PressureInMillibars { get; init; }

			public override string ToString() =>
				$"At {RecordedAt:h:mm tt} on {RecordedAt:M/d/yyyy}: " +
				$"Temp = {TemperatureInCelsius}, with {PressureInMillibars} pressure";
		}

		private void CSharpVersion11() // .NET7
		{
			// Generic attributes
			// public class GenericAttribute<T> : Attribute { }
			// [GenericAttribute<string>()]
			// public string Method() => default;

			// Raw string literals
			string longMessage = """
                This is a long message.
                It has several lines.
                    Some are indented
                            more than others.
                Some should start at the first column.
                Some have "quoted text" in them.
                """;
		}

		private void CSharpVersion12() // .NET8
		{
			// Primary constructors
			// - You can now create primary constructors in any class and struct. Primary constructors are no longer restricted to record types

			// Collection expressions
			// - Collection expressions introduce a new terse syntax to create common collection values. Inlining other collections into these values
			//   is possible using a spread operator ...
			//int[] row0 = [1, 2, 3];
			//int[] row1 = [4, 5, 6];
			//int[] row2 = [7, 8, 9];
			//int[] single = [..row0, ..row1, ..row2];

			// Inline arrays
			// - Inline arrays enable a developer to create an array of fixed size in a struct type

		}
	}
}
