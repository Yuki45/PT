using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInspection
{
    public class TestResultInfo
    {
        public string Name;
        public string SpecMin;
        public string SpecMax;
        public string Measured;
        public bool TestResult;

        public TestResultInfo(string _name, string _measured, string _min, string _max, bool result)
        {
            Name = _name;
            SpecMin = _min;
            SpecMax = _max;
            Measured = _measured;
            TestResult = result;
        }
    }

    public class ErrorInfo
    {
        public string Name;
        public string Msg;

        public ErrorInfo(string _name, string _msg)
        {
            Name = _name;
            Msg = _msg;
        }
    }
}
