namespace Validation
{
    using Xunit.Abstractions;

    public abstract class AssumesExtensionsTestFixtureBase : ExtensionsTestFixtureBase
    {
        protected AssumesExtensionsTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
