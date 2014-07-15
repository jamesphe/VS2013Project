using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECWebMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ECWebMonitor.Tests
{
    [TestClass()]
    public class Service1Tests
    {
        [TestMethod()]
        public void IsAccessibleTest()
        {
            var myService1=new Service1();

            myService1.IsAccessible("http://sales.donper.com/");


            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            Service1.Execute("net stop http", 3000);
            Assert.Fail();
        }

        [TestMethod()]
        public void RestartServiceTest()
        {
            Service1.RestartService("HTTP Service", 10 * 1000);
            Assert.Fail();
        }
    }
}
