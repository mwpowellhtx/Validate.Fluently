using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresThatExtensionTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresThatExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyRequirement(bool condition, string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());
            return () => condition.RequiresThat(argumentName, () => expectedMessage);
        }

        private static bool VerifyRequirement(bool condition, string argumentName, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return condition.RequiresThat(argumentName, () => expectedMessage);
        }

        [Fact]
        public void True_Condition_Requires_That_Correct() => VerifyRequirement(true, ArgumentName).AssertTrue();

        [Fact]
        public void False_Condition_Requires_That_Correct()
            => VerifyRequirement(false, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        private static Action VerifyRequirement<T>(T value, FluentConditionCallback<T> condition, string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());
            return () => value.RequiresThat(condition, argumentName, () => expectedMessage);
        }

        private static T VerifyRequirement<T>(T value, FluentConditionCallback<T> condition, string argumentName, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return value.RequiresThat(condition, argumentName, () => expectedMessage);
        }

        [Fact]
        public void Typed_True_Condition_Requires_That_Correct() =>
            VerifyRequirement<int>(default, x => x == default, ArgumentName).AssertEqual(default);

        [Fact]
        public void Typed_False_Condition_Requires_That_Correct()
            => VerifyRequirement<int>(default,  x=>x!=default,ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });
    }
}
