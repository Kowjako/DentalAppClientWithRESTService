using RESTDentalService.Tests.ClassFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RESTDentalService.Tests
{
    public class OperationControllerTests : IClassFixture<OperationControllerFixture>
    {
        private OperationControllerFixture _target;

        public OperationControllerTests(OperationControllerFixture target)
        {
            _target = target;
        }
    }
}
