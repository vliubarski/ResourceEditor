using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IResourceRepository
	{
		IEnumerable<DbResource> GetResourceByString(string str);
		DbResource CreateResource(DbResource subStr);
	}
}
