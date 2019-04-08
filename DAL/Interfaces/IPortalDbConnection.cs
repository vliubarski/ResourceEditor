using System.Collections.Generic;
using System.Data;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IPortalDbConnection: IDbConnection
	{
		//IEnumerable<DbResource> GetResourceByString(string str);
		DataSet GetResourceByString(string str);
		DataSet CreateResource(DbResource subStr);
	}
}
