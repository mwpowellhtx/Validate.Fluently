using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresRangeExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresRangeExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyFailRangeRequirement<T>(T _, string argumentName, FluentRangeVerificationCallback<T> evaluate, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());

            return () => _.RequiresFailRange(argumentName, evaluate, () => expectedMessage);
        }

        private static Exception VerifyFailRangeRequirement<T>(T _, string argumentName, FluentRangeVerificationCallback<T> evaluate, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return _.RequiresFailRange(argumentName, evaluate, () => expectedMessage);
        }

        [Fact]
        public void Fail_Range_Invalid_Argument_Range_Fails()
            => VerifyFailRangeRequirement(NullRoot, ArgumentName, _ => false, out var renderedMessage)
                .AssertThrows<ArgumentOutOfRangeException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    ex.Message.AssertEqual(renderedMessage.AssertNotNull().AssertNotEmpty());
                    ex.ActualValue.AssertNull();
                });

        [Fact]
        public void Fail_Range_Valid_Argument_Range_Passes()
            => VerifyFailRangeRequirement(NullRoot, ArgumentName, _ => true).AssertNull();

        private static Action VerifyRangeRequirement(bool condition, string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName);
            return () => condition.RequiresRange(argumentName, () => expectedMessage);
        }

        private static bool VerifyRangeRequirement(bool condition, string argumentName) => condition.RequiresRange(argumentName);

        [Fact]
        public void Invalid_Range_Condition_Throws()
            => VerifyRangeRequirement(false, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentOutOfRangeException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Valid_Range_Condition_Does_Not_Throw() => VerifyRangeRequirement(true, ArgumentName).AssertTrue();

        private static Action VerifyRangeRequirement<T>(T value, FluentConditionCallback<T> condition, string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName);
            return () => value.RequiresRange(condition, argumentName, () => expectedMessage);
        }

        private static T VerifyRangeRequirement<T>(T _, FluentConditionCallback<T> condition, string argumentName) => _.RequiresRange(condition, argumentName);

        [Fact]
        public void Invalid_Typed_Range_Condition_Throws()
            => VerifyRangeRequirement<int>(default, _ => false, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentOutOfRangeException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Valid_Typed_Range_Condition_Does_Not_Throw() => VerifyRangeRequirement<int>(default, _ => true, ArgumentName).AssertEqual(default);
    }
}
