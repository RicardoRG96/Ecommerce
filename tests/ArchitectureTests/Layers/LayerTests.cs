using NetArchTest.Rules;
using Shouldly;

namespace ArchitectureTests.Layers
{
    public class LayerTests : BaseTest
    {
        [Fact]
        public void DomaninLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_PresentationLayer()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }

        [Fact]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }
    }
}
