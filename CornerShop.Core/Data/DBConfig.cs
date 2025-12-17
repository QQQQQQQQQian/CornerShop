using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornerShop.Core.Data
{
    public static class DBConfig
    {
        public static bool UseCloud { get; set; }=false;

        public const  string LocalConnection= "";
        private const string CloudConnection = "";
        public static string ConnectionString
        {
            get
            {
                return UseCloud ? CloudConnection : LocalConnection;
            }
        }
        public const string AdminApiUrl = "";
    }
}
