using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntDevs.UnitTest
{
    public class FilterFlightStrategy
    {
        public void Execute(string xml)
        {         
            // to do FilterFlightStrategy
            ExecuteInfo.Add("A航空公司航班号AB1234已经过滤，执行策略前航班数为205，执行策略后航班总数为200");

        }

        private string _name = "FilterFlightStrategy";
        private string _description = "根据数据库配置过滤A航空公司的特定航班数据";
        private List<string> _executeInfo = new List<string>();

        //策略名称
        public string Name{ get { return _name; } }

        //策略描述
        public string Description { get { return _description; } }

        public List<string> ExecuteInfo
        {
            get { return _executeInfo; }
            private set { _executeInfo = value; }
        }
    }
}
