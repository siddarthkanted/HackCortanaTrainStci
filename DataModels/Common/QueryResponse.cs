using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Common
{
    public class QueryResponse<T>
    {
        public string Query { get; set; }

        public int State { get; set; }

        public T Response { get; set; }
    }
}
