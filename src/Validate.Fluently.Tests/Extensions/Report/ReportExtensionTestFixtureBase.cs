namespace Validation
{
    using Xunit.Abstractions;

    public abstract class ReportExtensionTestFixtureBase : ExtensionsTestFixtureBase
    {
        protected ReportExtensionTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
