﻿using System.Collections.Generic;
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

		public bool CreateResource(DbResource newResource)
		{
			return _repository.CreateResource(newResource);
		}

		public bool UpdateResource(DbResource newResource)
		{
			return _repository.UpdateResource(newResource);
		}

		public bool DeleteResource(DbResource newResource)
		{
			return _repository.DeleteResource(newResource);
		}
	}
}
