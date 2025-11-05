using NetArchTest.Rules;
using Shouldly;

namespace ArchitectureTests.Layers
{
    public class LayerTests : BaseTest
    {
        [Fact]
        public void DomaninLayer_Should_NotHaveDependencyOnApplicationLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOnInfrastructureLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOnPresentationLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }
    }
}
