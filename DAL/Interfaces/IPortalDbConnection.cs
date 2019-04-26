using System.Data;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IPortalDbConnection: IDbConnection
	{
		//IEnumerable<DbResource> GetResourceByString(string str);
		DataSet GetResource(string str);
		bool CreateResource(string sqlCommand);
		bool DeleteResource(string sqlCommand);
		//bool ResourceExists(string sqlCommand);
	}
}
