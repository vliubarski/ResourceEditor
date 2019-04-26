using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{

	public sealed class SimpleInvestorProfile : Profile
	{
		public SimpleInvestorProfile()
		{
			IMappingExpression<DataRow, DbResource> mappingExpression;

			mappingExpression = CreateMap<DataRow, DbResource>();
			mappingExpression.ForMember(d => d.CultureCode, o => o.MapFrom(s => s["CultureCode"]));
			mappingExpression.ForMember(d => d.ResourceKey, o => o.MapFrom(s => s["ResourceKey"]));
			mappingExpression.ForMember(d => d.ResourceType, o => o.MapFrom(s => s["ResourceType"]));
			mappingExpression.ForMember(d => d.ResourceValue, o => o.MapFrom(s => s["ResourceValue"]));
		}
	}

	public class ResourceRepository : IResourceRepository
	{
		private readonly IPortalDbConnection _connection;

		public ResourceRepository(IPortalDbConnection connection)
		{
			_connection = connection;
		}
		public IEnumerable<DbResource> GetResourceByString(string subStr)
		{
			string sql = $"select RESOURCE_TYPE as ResourceType, RESOURCE_KEY as ResourceKey, RESOURCE_Value as ResourceValue, CULTURE_CODE as CultureCode from OBI.vu_resources where resource_value like '%{subStr}%'";
			DataSet resFromDb;

			try
			{
				_connection.Open();
				resFromDb = _connection.GetResource(sql);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new List<DbResource>();
			}
			finally
			{
				_connection.Dispose();
			}

			MapperConfiguration configuration = new MapperConfiguration(a => { a.AddProfile(new SimpleInvestorProfile()); });
			IMapper mapper = configuration.CreateMapper();
			List<DbResource> result = mapper.Map<List<DataRow>, List<DbResource>>(new List<DataRow>(resFromDb.Tables[0].Select().ToList()));

			return result;
		}

		public bool CreateResource(DbResource res)
		{
			try
			{
				_connection.Open();
				if (EntryNumber(res) > 0)
				{
					return false;
				}

				string sqlCommand2 = $"SELECT obi.dt_update_resource('{res.ResourceType}', '{res.CultureCode}', '{res.ResourceKey}', '{res.ResourceValue}', 1, 1) AS result FROM dual";
				return _connection.CreateResource(sqlCommand2);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			finally
			{
				_connection.Close();
			}
		}

		private int EntryNumber(DbResource res)
		{
			string sqlCommand = $"select * from obi.vu_resources where RESOURCE_type = '{res.ResourceType}' " +
			                    $"and RESOURCE_KEY = '{res.ResourceKey}' and CULTURE_CODE = '{res.CultureCode}'";

			DataSet dataSet = _connection.GetResource(sqlCommand);
			return dataSet.Tables[0].Rows.Count;
		}

		public bool DeleteResource(DbResource res)
		{
			string fromType = $"delete from OBI.RESOURCE_TYPE where RESOURCE_TYPE = '{res.ResourceType}'";
			string fromKey = $"delete from OBI.RESOURCE_key where RESOURCE_TYPE = '{res.ResourceType}' and RESOURCE_KEY = '{res.ResourceKey}'";
			string fromText = $"delete from OBI.RESOURCE_TEXT where RESOURCE_VALUE = '{res.ResourceValue}'" +
								   $"and CULTURE_ID = (select CULTURE_ID from obi.culture where CULTURE_NAME ='{res.CultureCode}')";
			bool resultText;
			try
			{
				_connection.Open();
				resultText = _connection.DeleteResource(fromText);
				if (LastResourceWithTheSameResKeyId(res))
				{
					return resultText
						   && _connection.DeleteResource(fromKey)
						   && _connection.DeleteResource(fromType);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
			finally
			{
				_connection.Close();
			}

			return resultText;
		}

		private bool LastResourceWithTheSameResKeyId(DbResource res)
		{
			string fromText = "select * from OBI.RESOURCE_TEXT where RESOURCE_KEY_ID = (select RESOURCE_KEY_ID from OBI.RESOURCE_key " +
								$"where RESOURCE_key = '{res.ResourceKey}' and RESOURCE_type = '{res.ResourceType}')";

			DataSet dataSet = _connection.GetResource(fromText);
			return dataSet.Tables[0].Rows.Count == 0;
		}
	}
}
