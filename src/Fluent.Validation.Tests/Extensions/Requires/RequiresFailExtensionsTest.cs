using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresFailExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresFailExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyRequirement<T>(T _, FluentMessageCallback message, Exception innerException = null)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Action GetRequiresFail() => () => _.RequiresFail(message);
            Action GetRequiresFailWithInnerException() => () => _.RequiresFail(innerException, message);
            return innerException == null ? GetRequiresFail() : GetRequiresFailWithInnerException();
        }

        // TODO: TBD: we are fairly certain this may be an oversight in the Validation framework.
        // TODO: TBD: i.e. there are no Argument Names being indicated to the Requires.Fail ...
        // TODO: TBD: i.e. never minding the fact that `Requires´ does not really speak to `Arguments´
        [Fact]
        public void Fail_Throws_ArgumentException()
            => VerifyRequirement(NullRoot, () => DefaultMessage)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNull();
                    ex.Message.AssertNotNull().AssertEqual(DefaultMessage);
                });

        [Fact]
        public void Fail_With_InnerException_Throws_ArgumentException()
        {
            void Verify(Exception innerException)
            {
                VerifyRequirement(NullRoot, () => DefaultMessage, innerException)
                    .AssertThrows<ArgumentException>().Verify(ex =>
                    {
                        ex.AssertNotNull().ParamName.AssertNull();
                        ex.Message.AssertNotNull().AssertEqual(DefaultMessage);
                        ex.InnerException.AssertNotNull().AssertSame(innerException.AssertNotNull());
                    });
            }

            Verify(InnerException);
        }
    }
}
