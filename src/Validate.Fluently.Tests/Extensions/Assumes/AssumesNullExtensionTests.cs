using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesNullExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesNullExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionDoesNotThrow<T>(T value, Action<T> action)
            where T : class
            => action.Invoke(value);

        private static void VerifyAssumptionThrows<T, TException>(T value, Action<T> action)
            where T : class
            where TException : Exception
        {
            Action wrapper = () => action.Invoke(value);

            wrapper.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));
        }

        [Fact]
        public void Not_Null_Assumes_Not_Null_Does_Not_Throw()
        {
            void Verify(Exception exception) => VerifyAssumptionDoesNotThrow(exception, x => x.AssumesNotNull().AssertSame(exception));
            Verify(InnerException);
        }

        [Fact]
        public void Null_Assumes_Null_Does_Not_Throw() => VerifyAssumptionDoesNotThrow<Exception>(null, x => x.AssumesNull().AssertNull());

        [Fact]
        public void Not_Null_Assumes_Null_Throws() => VerifyAssumptionThrows<Exception, Exception>(InnerException, x => x.AssumesNull());

        [Fact]
        public void Null_Assumes_Not_Null_Throws() => VerifyAssumptionThrows<Exception, Exception>(null, x => x.AssumesNotNull());
    }
}
