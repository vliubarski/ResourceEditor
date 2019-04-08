using System.Collections.Generic;
using DAL;
using DAL.Interfaces;
using DAL.Models;

namespace Domain.Services
{
	public class ResourceService:IResourceService
	{
		private readonly IResourceRepository _repository;

		public ResourceService(IResourceRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<object> GetAnyResource(string subString)
		{
			return _repository.GetResourceByString(subString);
		}

		public DbResource CreateResource(DbResource newResource)
		{
			return _repository.CreateResource(newResource);
		}
	}
}
