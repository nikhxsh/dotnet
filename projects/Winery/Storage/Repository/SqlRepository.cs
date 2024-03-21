using Dapper;
using WineryAPI.Storage.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;

namespace WineryAPI.Storage.Repository
{
	/// <summary>
	/// - Dapper is an open-source object-relational mapping (ORM) library for .NET and .NET Core applications
	/// - Dapper falls into a family of tools known as micro-ORMs. These tools perform only a subset of the functionality of full-blown 
	///   Object Relations Mappers, such as Entity Framework Core, but Dapper is known for its speed and simple implementation compared to others.
	/// - Dapper allows you to execute raw SQL queries, map the results to objects, and execute stored procedures, among other things
	///    - Dapper is lightweight and fast, making it an ideal choice for applications that require low latency and high performance.
	///    - It is a simple yet powerful object mapping tool for any.NET language, such as C#, that enables developers to quickly and easily 
	///		 map query results from ADO.NET data readers to instances of business objects
	///	   - It has excellent support for both asynchronous and synchronous database queries and batching multiple queries together into a single call.
	///	   - Additionally, dapper supports parameterized queries to help protect against SQL injection attacks
	///	- Dapper works with an ADO.NET IDbConnection object, which means that it will work with any database system for which there is an ADO.NET provider
	///	   
	///  Note: Dapper only map queries result to objects hence called micro-ORM
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SqlRepository<T> : IRepository<T> where T : class, new()
	{
		private readonly IDapperContext _dapperContext;

		public SqlRepository(IDapperContext dapperContext)
		{
			_dapperContext = dapperContext;
		}


		public int Count()
		{
			var result = Enumerable.Empty<int>();
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var query = $"SELECT COUNT(*) FROM {tableName}";

				using var connection = _dapperContext.CreateConnection();
				result = connection.Query<int>(query);
			}
			catch (Exception)
			{
				throw;
			}

			return result.FirstOrDefault();
		}

		public bool Exists(Guid id)
		{
			var result = Enumerable.Empty<int>();
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var query = $"SELECT {keyColumn} FROM {tableName} WHERE {keyColumn} = '{id}'";

				using var connection = _dapperContext.CreateConnection();
				result = connection.Query<int>(query);
			}
			catch (Exception)
			{
				throw;
			}

			return result.Count() == 1;
		}

		public IEnumerable<T> GetAll(
				int skip,
				int take
			)
		{
			var result = Enumerable.Empty<T>();
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var query = $"SELECT * " +
					$"FROM {tableName} " +
					$"ORDER BY {keyColumn} " +
					$"OFFSET {skip} ROWS " +
					$"FETCH NEXT {take} ROWS ONLY";

				using var connection = _dapperContext.CreateConnection();
				result = connection.Query<T>(query);
			}
			catch (Exception)
			{
				throw;
			}

			return result;
		}

		public T GetById(Guid id)
		{
			var result = Enumerable.Empty<T>();
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{id}'";

				using var connection = _dapperContext.CreateConnection();
				result = connection.Query<T>(query);
			}
			catch (Exception)
			{
				throw;
			}

			return result.FirstOrDefault() ?? new T();
		}

		public bool Add(T entity)
		{
			int rowsAffected = 0;
			try
			{
				var tableName = GetTableName();
				var columns = GetColumns(true);
				var properties = GetPropertyNames(true);

				var query = $"INSERT INTO {tableName} ({columns}) " +
							$"VALUES ({properties})";

				using var connection = _dapperContext.CreateConnection();
				rowsAffected = connection.Execute(query, entity);
			}
			catch (Exception)
			{
				throw;
			}
			return rowsAffected > 0;
		}

		public bool Update(T entity)
		{
			int rowsEffected = 0;
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var keyProperty = GetKeyPropertyName();

				var query = new StringBuilder();
				query.Append($"UPDATE {tableName} SET ");

				foreach (var property in GetProperties(true))
				{
					var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

					var propertyName = property.Name;
					var columnName = columnAttr?.Name;

					query.Append($"{columnName} = @{propertyName},");
				}

				query.Remove(query.Length - 1, 1);

				query.Append($" WHERE {keyColumn} = @{keyProperty}");

				using var connection = _dapperContext.CreateConnection();
				rowsEffected = connection.Execute(query.ToString(), entity);
			}
			catch (Exception)
			{
				throw;
			}

			return rowsEffected > 0;
		}

		public bool Delete(Guid id)
		{
			int rowsEffected = 0;
			try
			{
				var tableName = GetTableName();
				var keyColumn = GetKeyColumnName();
				var keyProperty = GetKeyPropertyName();
				var query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{id}";

				using var connection = _dapperContext.CreateConnection();
				rowsEffected = connection.Execute(query);
			}
			catch (Exception)
			{
				throw;
			}

			return rowsEffected > 0;
		}

		#region Supporting Methods
		protected string GetTableName()
		{
			var type = typeof(T);
			var tableAttr = type.GetCustomAttribute<TableAttribute>();
			if (tableAttr != null)
			{
				return tableAttr.Name;
			}
			return string.Empty;
		}

		protected string? GetKeyColumnName()
		{
			var properties = typeof(T).GetProperties();

			foreach (var property in properties)
			{
				var keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

				if (keyAttributes != null && keyAttributes.Length > 0)
				{
					var columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

					if (columnAttributes != null && columnAttributes.Length > 0)
					{
						var columnAttribute = (ColumnAttribute)columnAttributes[0];
						return columnAttribute.Name;
					}
					else
					{
						return property.Name;

					}
				}
			}
			return null;
		}

		protected string? GetColumns(bool excludeKey = false)
		{
			var type = typeof(T);
			var objects = type.GetProperties()
				.Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
				.Select(c =>
				{
					var columAttr = c.GetCustomAttribute<ColumnAttribute>();
					return columAttr != null ? columAttr.Name : c.Name;
				});

			var columns = string.Join(", ", objects);
			return columns;
		}

		protected string? GetPropertyNames(bool excludeKey = false)
		{
			var type = typeof(T);
			var properties = type.GetProperties()
				.Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
				.Select(c =>
				{
					return $"@{c.Name}";
				});
			var values = string.Join(", ", properties);
			return values;
		}

		protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
		{
			var type = typeof(T);
			var properties = type.GetProperties()
				.Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);
			return properties;
		}

		protected string? GetKeyPropertyName()
		{
			var type = typeof(T);
			var properties = type.GetProperties()
				.Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

			if (properties.Any())
			{
				return properties.FirstOrDefault()?.Name;
			}

			return null;
		}
		#endregion
	}
}
