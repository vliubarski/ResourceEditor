
using System.Collections.Generic;

namespace DAL
{
	public interface IResourceService
	{
		IEnumerable<object> GetAnyResource(string subString);
	}
}
