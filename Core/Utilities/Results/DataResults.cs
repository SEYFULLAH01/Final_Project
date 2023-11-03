using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class DataResults<T> : Result, IDataResult<T>
    {
        public DataResults(T data, bool success, string message):base(success,message)
        {
            Data = data;
        }
        public DataResults(T data, bool success):base(success) 
        {
            Data = data;
        }
        public T Data { get; }
    }
}
