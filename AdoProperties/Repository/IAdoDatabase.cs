using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoProperties.Repository
{
    public interface IAdoDatabase
    {
        void SqlCommand(string query);
        List<T> SqlRead<T>(string query);
        T SqlReadScaler<T>(string query);
    }
}
