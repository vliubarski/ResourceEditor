
using System.Collections.Generic;
using DAL.Models;

namespace DAL
{
	public interface IResourceService
	{
		IEnumerable<object> GetAnyResource(string subString);
		DbResource CreateResource(DbResource newResource);
		DbResource DeleteResource(DbResource newResource);

	}
}
