using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class VerifyFailOperationExtensionsTests : VerifyExtensionsTestFixtureBase
    {
        public VerifyFailOperationExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyExtensionDelegation(object _, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => _.VerifyFailOperation(() => expectedMessage);
        }

        [Fact]
        public void Unconditionally_Verify_Object_FailOperation_Correct()
            => VerifyExtensionDelegation(NullRoot)
                .AssertThrows<InvalidOperationException>().Verify(ex =>
                {
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage);
                });

        private static Action VerifyExtensionDelegation(object _, FluentConditionCallback<object> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => _.VerifyFailOperation(condition, () => expectedMessage);
        }

        private static Exception VerifyExtension(object _, FluentConditionCallback<object> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return _.VerifyFailOperation(condition, () => expectedMessage);
        }

        [Fact]
        public void Conditionally_Verify_Object_True_FailOperation_Correct()
            => VerifyExtension(NullRoot, _ => true).AssertNull();

        [Fact]
        public void Conditionally_Verify_Object_False_FailOperation_Correct()
            => VerifyExtensionDelegation(NullRoot, _ => false)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));

        private static Action VerifyExtensionDelegation<T>(T _, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => _.VerifyFailOperation(() => expectedMessage);
        }

        [Fact]
        public void Unconditionally_Verify_Typed_FailOperation_Correct()
            => VerifyExtensionDelegation<int>(default)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));

        private static Action VerifyExtensionDelegation<T>(T _, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return () => _.VerifyFailOperation(condition, () => expectedMessage);
        }

        private static Exception VerifyExtension<T>(T _, FluentConditionCallback<T> condition, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            return _.VerifyFailOperation(condition, () => expectedMessage);
        }

        [Fact]
        public void Conditionally_Verify_Typed_True_FailOperation_Correct()
            => VerifyExtension<int>(default, _ => true).AssertNull();

        [Fact]
        public void Conditionally_Verify_Typed_False_FailOperation_Correct()
            => VerifyExtensionDelegation<int>(default, _ => false)
                .AssertThrows<InvalidOperationException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));
    }
}
