using System.Data;
using System.Data.SqlClient;

namespace ObjectOriented.CreationalPatterns
{
	/// <summary>
	/// - Creational Patterns
	/// - In Factory pattern, we create object without exposing the creation logic. In this pattern, an interface is used for creating an object,
	///   but let subclass decide which class to instantiate. 
	/// - The creation of object is done when it is required. The Factory method allows a class later instantiation to subclasses.
	/// </summary>
	class FactoryPattern
	{
		public FactoryPattern()
		{
			var sqlconnection = ConnectionFactory.GetConnection(ConnectionType.SQL, "sql-connection-string");
			var postgreconnection = ConnectionFactory.GetConnection(ConnectionType.PostGre, "postgre-connection-string");
		}

		/// <summary>
		/// The 'Product' abstract class
		/// </summary>
		interface IConnection
		{
			IDbConnection Connection { get; }
		}

		/// <summary>
		/// A 'ConcreteProduct' class
		/// </summary>
		class SqlDBConnection : IConnection
		{
			private readonly string _connectionString;

			public SqlDBConnection(string connectionString)
			{
				_connectionString = connectionString;
			}

			public IDbConnection Connection
			{
				get
				{
					return new SqlConnection(_connectionString);
				}
			}
		}

		/// <summary>
		/// A 'ConcreteProduct' class
		/// </summary>
		class PostgreDBConnection : IConnection
		{
			private readonly string _connectionString;

			public PostgreDBConnection(string connectionString)
			{
				_connectionString = connectionString;
			}

			public IDbConnection Connection
			{
				get
				{
					return new SqlConnection(_connectionString);
				}
			}
		}

		static class ConnectionFactory
		{
			public static IConnection GetConnection(ConnectionType type, string connectionString) {
				switch (type)
				{
					case ConnectionType.SQL:
						return new SqlDBConnection(connectionString);
					case ConnectionType.PostGre:
						return new PostgreDBConnection(connectionString);
					default:
						return null;
				}
			}
		}

		enum ConnectionType { SQL, PostGre }
	}
}
