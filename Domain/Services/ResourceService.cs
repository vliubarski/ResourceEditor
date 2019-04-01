using System.Collections.Generic;
using DAL;
using DAL.Interfaces;

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
	}
}
