using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class VerifyOperationExtensionsTests : VerifyExtensionsTestFixtureBase
    {
        public VerifyOperationExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyOperationDelegation<T>(T value, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => value.VerifyOperation(condition, () => expectedMessage);
        }

        private static T VerifyOperation<T>(T value, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return value.VerifyOperation(condition, () => expectedMessage);
        }

        [Fact]
        public void Typed_Operation_Throws_Correctly()
            => VerifyOperationDelegation<int>(default, _ => false, DefaultMessage)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));

        [Fact]
        public void Typed_Operation_Would_Not_Throw()
            => VerifyOperation<int>(default, _ => true).AssertEqual(default);
    }
}
