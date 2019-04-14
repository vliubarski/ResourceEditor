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

		public bool CreateResource(DbResource res)
		{
			if (ResourceExists(res))
			{
				return false;
			}

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
				return false;
			}
			finally
			{
				_connection.Dispose();
			}
			return true;
		}

		private bool ResourceExists(DbResource res)
		{
			string sql = $"select * from obi.vu_resources where RESOURCE_type = '{res.ResourceType}' " +
			             $"and RESOURCE_KEY = '{res.ResourceKey}' and CULTURE_CODE = '{res.CultureCode}'";
			var cmd = new OracleCommand(sql, _connection) {CommandType = CommandType.Text};
			var dataAdapter = new OracleDataAdapter(cmd);
			var dt = new DataTable();

			try
			{
				_connection.Open();
				dataAdapter.Fill(dt);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				_connection.Dispose();
			}

			return dt.Rows.Count > 0;
		}

		public bool DeleteResource(DbResource res)
		{
			string deleteFromType = $"delete from OBI.RESOURCE_TYPE where RESOURCE_TYPE = '{res.ResourceType}'";
			string deleteFromKey = $"delete from OBI.RESOURCE_key where RESOURCE_TYPE = '{res.ResourceType}' and RESOURCE_KEY = '{res.ResourceKey}'";
			string deleteFromText =
				$"delete from OBI.RESOURCE_TEXT where RESOURCE_VALUE = '{res.ResourceValue}' and CULTURE_ID = (select CULTURE_ID from obi.culture where CULTURE_NAME ='{res.CultureCode}')";

			var cmdDelete = new OracleCommand();
			cmdDelete.Connection = _connection;

			try
			{
				_connection.Open();
				cmdDelete.CommandText = deleteFromText;
				cmdDelete.ExecuteNonQuery();

				cmdDelete.CommandText = deleteFromType;
				cmdDelete.ExecuteNonQuery();

				cmdDelete.CommandText = deleteFromKey;
				cmdDelete.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
			finally
			{
				cmdDelete.Dispose();
				_connection.Dispose();
			}
			return true;
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
