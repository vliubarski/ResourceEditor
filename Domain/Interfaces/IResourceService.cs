
using System.Collections.Generic;
using DAL.Models;

namespace DAL
{
	public interface IResourceService
	{
		IEnumerable<object> GetAnyResource(string subString);
		bool CreateResource(DbResource newResource);
		DbResource DeleteResource(DbResource newResource);

	}
}
