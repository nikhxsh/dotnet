using MigrationService.Models;
using Npgsql;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MigrationService.Repository
{
	public class PostgresRepository
	{
		public readonly string postgres;
		public PostgresRepository(string ConnectionString)
		{
			postgres = ConnectionString;
		}

		public async Task<int> CreateRequestEntry(PostgresModelMongo postgresModel)
		{
			try
			{
				using (var connection = new NpgsqlConnection(postgres))
				{
					connection.Open();
					var sqlQuery = $"INSERT INTO public.\"Request\" ( \"TargetDbType\", \"SourceDbType\", \"Count\",\"Template\", \"Status\") VALUES(\'{postgresModel.TargetDatabase}\', \'{postgresModel.SourceDatabase}\', {postgresModel.count}, \'{JsonSerializer.Serialize(postgresModel.sqlToMongoTemplate)}\', \'{postgresModel.Status}\') returning \"RequestId\"";
					var com = new NpgsqlCommand(sqlQuery, connection);
					var tempobject = com.ExecuteScalar();
					connection.Close();

					return Convert.ToInt32(tempobject);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception();
			}
			
			
		}

		public async Task<int> CreateRequestEntry(PostgresModelPostgres postgresModel)
		{
			try
			{
				using (var connection = new NpgsqlConnection(postgres))
				{
					connection.Open();
					var sqlQuery = $"INSERT INTO public.\"Request\" ( \"TargetDbType\", \"SourceDbType\", \"Count\",\"Template\", \"Status\") VALUES(\'{postgresModel.TargetDatabase}\', \'{postgresModel.SourceDatabase}\', {postgresModel.count}, \'{JsonSerializer.Serialize(postgresModel.sqlToMongoTemplate)}\', \'{postgresModel.Status}\') returning \"RequestId\"";
					var com = new NpgsqlCommand(sqlQuery, connection);
					var tempobject = com.ExecuteScalar();
					connection.Close();

					return Convert.ToInt32(tempobject);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception();
			}


		}

		public async Task<PostgresRequestModel> GetRequestEntry(int requestId)
		{
			try
			{
				var requestModel = new PostgresRequestModel();
				using (var connection = new NpgsqlConnection(postgres))
				{
					connection.Open();

					var sqlQuery = $"SELECT \"RequestId\", \"Count\", \"Status\" FROM public.\"Request\" WHERE \"RequestId\" = {requestId}";

					using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
					{
						NpgsqlDataReader reader = command.ExecuteReader();
						while (reader.Read())
						{
							requestModel.RequestId = Int32.Parse(reader[0].ToString());
							requestModel.Count = Int32.Parse(reader[1].ToString());
							requestModel.Status = reader[2].ToString();
						}

						connection.Close();
					}

					return requestModel;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception();
			}
		}
	}
}
