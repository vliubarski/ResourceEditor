using System.Collections.Generic;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IResourceRepository
	{
		IEnumerable<DbResource> GetResourceByString(string str);
		DbResource CreateResource(DbResource subStr);
		DbResource DeleteResource(DbResource subStr);
	}
}
