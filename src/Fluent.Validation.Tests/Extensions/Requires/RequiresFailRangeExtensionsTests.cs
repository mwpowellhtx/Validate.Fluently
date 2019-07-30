using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresFailRangeExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresFailRangeExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyRequirement<T>(T _, string argumentName, FluentRangeVerificationCallback<T> evaluate, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());

            return () => _.RequiresFailRange(argumentName, evaluate, () => expectedMessage);
        }

        private static Exception VerifyRequirement<T>(T _, string argumentName, FluentRangeVerificationCallback<T> evaluate, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return _.RequiresFailRange(argumentName, evaluate, () => expectedMessage);
        }

        [Fact]
        public void Invalid_Argument_Range_Fails()
            => VerifyRequirement(NullRoot, ArgumentName, _ => false, out var renderedMessage)
                .AssertThrows<ArgumentOutOfRangeException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    ex.Message.AssertEqual(renderedMessage.AssertNotNull().AssertNotEmpty());
                    ex.ActualValue.AssertNull();
                });

        [Fact]
        public void Valid_Argument_Range_Passes()
            => VerifyRequirement(NullRoot, ArgumentName, _ => true).AssertNull();
    }
}
