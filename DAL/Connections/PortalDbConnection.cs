using System;
using System.Data;
using System.Data.Common;
using DAL.Interfaces;
using DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace DAL.Connections
{
	public class PortalDbConnection : DbConnection, IPortalDbConnection
	{
		private readonly OracleConnection _connection;
		private const string ConnString = "Data Source=STAGING_12C.WORLD;User Id=OBI;Password=OBI;";

		public PortalDbConnection():this(ConnString)
		{
		}
		public PortalDbConnection(string connectionString  )
		{
			_connection = new OracleConnection(connectionString);
		}

		public override string ConnectionString
		{
			get => _connection.ConnectionString;
			set => _connection.ConnectionString = value;
		}

		public override string Database => _connection.Database;
		public override ConnectionState State => _connection.State;

		//public IEnumerable<DbResource> GetResourceByString(string str)
		public DataSet GetResourceByString(string subStr)
		{
			string sql = $"select RESOURCE_TYPE as ResourceType, RESOURCE_KEY as ResourceKey, RESOURCE_Value as ResourceValue, CULTURE_CODE as CultureCode from OBI.vu_resources where resource_value like '%{subStr}%'";
			var cmd = new OracleCommand(sql, _connection) { CommandType = CommandType.Text };
			var dataAdapter = new OracleDataAdapter(cmd);
			var ds = new DataSet();

			try
			{
				_connection.Open();
				dataAdapter.Fill(ds);
			}
			catch (Exception e)
			{
				//log error
				throw;
			}
			finally
			{
				_connection.Dispose();
			}
			return ds;
		}

		public DataSet CreateResource(DbResource res)
		{
			string sql = $"SELECT obi.dt_update_resource('{res.ResourceType}', '{res.CultureCode}', '{res.ResourceKey}', '{res.ResourceValue}', 1, 1) AS result FROM dual";

			var cmd = new OracleCommand(sql, _connection) { CommandType = CommandType.Text };
			var dataAdapter = new OracleDataAdapter(cmd);
			var ds = new DataSet();

			try
			{
				_connection.Open();
				dataAdapter.Fill(ds);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				_connection.Dispose();
			}
			return ds;
		}

		public override string DataSource => _connection.DataSource;
		public override string ServerVersion => _connection.ServerVersion;

		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			return _connection.BeginTransaction(isolationLevel);
		}

		public override void ChangeDatabase(string databaseName)
		{
			_connection.ChangeDatabase(databaseName);
		}

		public override void Close()
		{
			_connection.Close();
		}

		public override void Open()
		{
			_connection.Open();
		}

		protected override DbCommand CreateDbCommand()
		{
			return _connection.CreateCommand();
		}
	}
}
