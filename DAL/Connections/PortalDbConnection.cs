using System;
using System.Data;
using System.Data.Common;
using DAL.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace DAL.Connections
{
	public class PortalDbConnection : DbConnection, IPortalDbConnection
	{
		private readonly OracleConnection _connection;

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

		public DataSet GetResource(string sqlCommand)
		{
			var cmd2 = new OracleCommand(sqlCommand, _connection) { CommandType = CommandType.Text };
			var dataAdapter = new OracleDataAdapter(cmd2);
			var ds = new DataSet();

			try
			{
				dataAdapter.Fill(ds);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return ds;
		}

		public bool CreateResource(string sqlCommand)
		{
			var cmd = new OracleCommand(sqlCommand, _connection) { CommandType = CommandType.Text };
			decimal res;
			try
			{
				res = (decimal)cmd.ExecuteScalar();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return res == 1;
		}

		public bool DeleteResource(string sqlCommand)
		{
			var cmdDelete = new OracleCommand();
			cmdDelete.Connection = _connection;
			int res;
			try
			{
				cmdDelete.CommandText = sqlCommand;
				res = cmdDelete.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
			return res == 1;
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
