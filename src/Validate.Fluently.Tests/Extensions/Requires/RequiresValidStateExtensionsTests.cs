using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresValidStateExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        public RequiresValidStateExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyRequirementDelegation(bool condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => condition.RequiresValidState(() => expectedMessage);
        }

        private static bool VerifyRequirement(bool condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return condition.RequiresValidState(() => expectedMessage);
        }

        [Fact]
        public void True_Condition_Requires_ValidState_Correct() => VerifyRequirement(true).AssertTrue();

        [Fact]
        public void False_Condition_Requires_That_Correct()
            => VerifyRequirementDelegation(false)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.Message.AssertNotNull().AssertEqual(DefaultMessage));

        private static Action VerifyRequirementDelegation<T>(T value, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => value.RequiresValidState(condition, () => expectedMessage);
        }

        private static T VerifyRequirement<T>(T value, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return value.RequiresValidState(condition, () => expectedMessage);
        }

        [Fact]
        public void Typed_True_Condition_Requires_That_Correct() =>
            VerifyRequirement<int>(default, x => x == default).AssertEqual(default);

        [Fact]
        public void Typed_False_Condition_Requires_That_Correct()
            => VerifyRequirementDelegation<int>(default, x => x != default)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.Message.AssertNotNull().AssertEqual(DefaultMessage));
    }
}
