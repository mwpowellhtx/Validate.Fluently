using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresArgumentExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresArgumentExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyRequirement(bool condition, string argumentName, string expectedMessage = null)
        {
            // TODO: TBD: we could potentially break this up a little bit into success/failure methods along the same lines as the typed methods...
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();

            // ReSharper disable ImplicitlyCapturedClosure
            Action action = () => condition.RequiresArgument(argumentName, () => expectedMessage).AssertEqual(condition);

            void VerifyRequirementSuccess() => action.Invoke();
            // ReSharper restore ImplicitlyCapturedClosure

            void VerifyRequirementFailure()
            {
                var renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());

                action.AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(argumentName);
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });
            }

            if (!condition)
            {
                VerifyRequirementFailure();
                return;
            }

            VerifyRequirementSuccess();
        }

        // TODO: TBD: we might redress the Assumes tests with an approach something like this...
        private static T VerifyRequirement<T>(T value, FluentConditionCallback<T> condition, string argumentName, string expectedMessage = null)
            => value.RequiresArgument(condition.AssertNotNull(), argumentName.AssertNotNull().AssertNotEmpty()
                , () => (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty());

        private static Action VerifyRequirementExpectingException<T>(T value, FluentConditionCallback<T> condition
            , string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName.AssertNotNull().AssertNotEmpty());
            return () => VerifyRequirement(value, condition, argumentName, expectedMessage);
        }

        [Theory
         , InlineData(true, ArgumentName, DefaultMessage)
         , InlineData(false, ArgumentName, DefaultMessage)
        ]
        public void Boolean_Argument_Condition_Correct(bool condition, string argumentName, string expectedMessage)
            => VerifyRequirement(condition, argumentName, expectedMessage);

        [Fact]
        public void Typed_Argument_Successful_Condition_Correct()
        {
            void Verify<T>(T value)
            {
                VerifyRequirement(value, x => x != null, ArgumentName).AssertSame(value);
            }

            Verify(InnerException);
        }

        [Fact]
        public void Typed_Argument_Failure_Condition_Correct()
            => VerifyRequirementExpectingException<Exception>(null, x => x != null, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(ArgumentName);
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage.AssertNotNull().AssertNotEmpty());
                });
    }
}
