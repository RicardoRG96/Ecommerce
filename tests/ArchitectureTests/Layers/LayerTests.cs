using NetArchTest.Rules;
using Shouldly;

namespace ArchitectureTests.Layers
{
    public class LayerTests : BaseTest
    {
        [Fact]
        public void DomaninLayer_Should_NotHaveDependencyOnApplication()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn("Application")
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }
    }
}
