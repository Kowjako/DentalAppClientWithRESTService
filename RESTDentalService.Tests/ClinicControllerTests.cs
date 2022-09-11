using RESTDentalService.Tests.ClassFixtures;
using Xunit;

namespace RESTDentalService.Tests
{
    public class ClinicControllerTests : IClassFixture<ClinicControllerFixture>
    {
        private ClinicControllerFixture _target;

        public ClinicControllerTests(ClinicControllerFixture target)
        {
            _target = target;
        }


    }
}
