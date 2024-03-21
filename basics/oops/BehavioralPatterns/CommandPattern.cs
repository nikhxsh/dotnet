using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ObjectOriented.BehavioralPatterns
{
	/// <summary>
	/// - Behavioral Pattern
	/// - Encapsulate a command request as an object
	/// - In this pattern, a request is wrapped under an object as a command and passed to invoker object. 
	///   Invoker object pass the command to the appropriate object which can handle it and that object executes the command. 
	/// - This handles the request in traditional ways like as queuing and callbacks.
	/// - This pattern is commonly used in the menu systems of many applications such as Editor, IDE etc.
	/// </summary>
	class CommandPattern
    {
        public CommandPattern()
        {
            // Create user and let her compute
            
            User user = new();

            // User presses calculator buttons
            user.Compute('+', 100);
            user.Compute('-', 50);
            user.Compute('*', 10);
            user.Compute('/', 2);

            // Undo 4 commands
            user.Undo(4);

            // Redo 3 commands
            user.Redo(3);


            // In memory Transactions

            var tableJson = JObject.Parse(@"{ ""users"": [] }");

            var transaction = new JsonTransaction(tableJson);
            transaction.Begin();
            transaction.AddRow("roles", new { id = 1234, name = "Admin" });
            transaction.Commit();
            transaction.Print();
			transaction.AddRow("subs", new { id = "S001", name = "_key" });
			transaction.Rollback();
		}

        /// <summary>
        /// The 'Command' abstract class
        /// </summary>
        interface ICommand
        {
            void Execute();
            void UnExecute();
        }

		#region calculator
		/// <summary>
		/// The 'ConcreteCommand' class
		/// </summary>
		class CalculatorCommand : ICommand
        {
            private char _operator;
            private int _operand;
            private Calculator _calculator;
            public CalculatorCommand(Calculator calculator, char @operator, int operand)
            {
                this._calculator = calculator;
                this._operator = @operator;
                this._operand = operand;
            }

            // Gets operator
            public char Operator
            {
                set { _operator = value; }
            }

            // Get operand
            public int Operand
            {
                set { _operand = value; }
            }

            // Execute new command
            public void Execute()
            {
                _calculator.Operation(_operator, _operand);
            }

            // Unexecute last command
            public void UnExecute()
            {
                _calculator.Operation(Undo(_operator), _operand);
            }

            // Returns opposite operator for given operator
            private char Undo(char @operator)
            {
                switch (@operator)
                {
                    case '+': return '-';
                    case '-': return '+';
                    case '*': return '/';
                    case '/': return '*';
                    default:
                        throw new ArgumentException("@operator");
                }
            }
        }

        /// <summary>
        /// The 'Receiver' class
        /// </summary>
        class Calculator
        {
            private int _curr = 0;

            public void Operation(char @operator, int operand)
            {
                switch (@operator)
                {
                    case '+': _curr += operand; break;
                    case '-': _curr -= operand; break;
                    case '*': _curr *= operand; break;
                    case '/': _curr /= operand; break;
                }
                Console.WriteLine("Current value = {0,3} (following {1} {2})", _curr, @operator, operand);
            }
        }

        /// <summary>
        /// The 'Invoker' class
        /// </summary>
        class User
        {
            private Calculator _calculator = new Calculator();
            private List<ICommand> _commands = new List<ICommand>();
            private int _current = 0;

            public void Redo(int levels)
            {
                Console.WriteLine("\n---- Redo {0} levels ", levels);
                for (int i = 0; i < levels; i++)
                {
                    if (_current < _commands.Count - 1)
                    {
                        ICommand command = _commands[_current++];
                        command.Execute();
                    }
                }
            }

            public void Undo(int levels)
            {
                Console.WriteLine("\n---- Undo {0} levels ", levels);
                for (int i = 0; i < levels; i++)
                {
                    if (_current > 0)
                    {
                        ICommand command = _commands[--_current];
                        command.UnExecute();
                    }
                }
            }

            public void Compute(char @operator, int operand)
            {
                // Create command operation and execute it
                ICommand command = new CalculatorCommand(
                  _calculator, @operator, operand);
                command.Execute();

                // Add command to undo list
                _commands.Add(command);
                _current++;
            }
        }
		#endregion

		#region Transaction

        class JsonTransaction
		{
			private readonly JObject _data;
			private readonly List<ICommand> commands;
            private bool _transactionStarted = false;

            public JsonTransaction(JObject data)
            {
                _data = data;
				commands = new List<ICommand>();
			}

            public void Begin()
            {
                _transactionStarted = true;
			}

            public void AddRow(string key, object values)
            {
                commands.Add(new AddRowCommand(_data, key, values));
			}

            public void Commit()
            {
                if(_transactionStarted)
                {
                    foreach (var command in commands)
                    {
                        command.Execute();
					}
					_transactionStarted = false;
				}
            }

            public void Rollback()
			{
				if (_transactionStarted)
				{
					foreach (var command in commands)
					{
						command.UnExecute();
					}
					_transactionStarted = false;
				}
			}

            public void Print()
            {
                Console.WriteLine(_data.ToString(Formatting.Indented));
            }
        }

		class AddRowCommand : ICommand
		{
			private readonly JObject _data;
			private readonly string _key;
			private readonly object _values;

			public AddRowCommand(JObject originalData, string key, object values)
            {
                _data = originalData;
                _key = key;
                _values = values;
			}

			public void Execute()
			{
				if (_data != null && !_data.ContainsKey(_key))
				{
					_data.Add(_key, JToken.FromObject(new object[] { _values }));
				}
			}

			public void UnExecute()
			{
				if (_data != null && _data.ContainsKey(_key))
				{
					_data.Remove(_key);
				}
			}
		}

		#endregion
	}
}
