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
		public  IEnumerable<DbResource> GetResourceByString(string subStr)
		{
			DataSet ret= _connection.GetResourceByString(subStr);

			MapperConfiguration configuration= new MapperConfiguration(a => { a.AddProfile(new SimpleInvestorProfile()); });
			IMapper mapper= configuration.CreateMapper();
			List<DbResource> result= mapper.Map<List<DataRow>, List<DbResource>>(new List<DataRow>(ret.Tables[0].Select().ToList()));

			return result;
		}
		public bool CreateResource(DbResource res)
		{
			 return  _connection.CreateResource(res);

		}
		public  DbResource DeleteResource(DbResource res)
		{
			var ret = _connection.DeleteResource(res);
			 return res;
		}
	}
}
