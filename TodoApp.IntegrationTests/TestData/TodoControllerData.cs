using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.IntegrationTests.TestData
{
    public static class TodoControllerData
    {
        public static IEnumerable<object[]> InvalidCreateData =>
            new List<object[]>
            {
                new object[] { "", DateTime.Today },
                new object[] { null, DateTime.Today },
                new object[] { "Test", null },
            };
    }
}
