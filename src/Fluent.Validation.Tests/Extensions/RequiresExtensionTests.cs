using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public RequiresExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
