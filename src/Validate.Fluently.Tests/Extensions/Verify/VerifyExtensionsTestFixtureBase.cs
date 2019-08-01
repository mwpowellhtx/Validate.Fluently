namespace Validation
{
    using Xunit.Abstractions;

    public abstract class VerifyExtensionsTestFixtureBase : ExtensionsTestFixtureBase
    {
        protected VerifyExtensionsTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
