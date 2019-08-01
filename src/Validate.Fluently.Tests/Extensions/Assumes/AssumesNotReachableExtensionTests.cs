using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesNotReachableExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesNotReachableExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionThrows<T, TException>(T value, Action<T> action)
            where TException : Exception
        {
            Action wrapper = () => action.Invoke(value);

            wrapper.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));
        }

        [Fact]
        public void Null_Object_Root_Not_Reachable_Throws()
            => VerifyAssumptionThrows<object, Exception>(NullRoot, x => x.AssumesNotReachable());

        [Fact]
        public void Not_Null_Object_Root_Not_Reachable_Throws()
            => VerifyAssumptionThrows<object, Exception>(Root, _ => _.AssumesNotReachable());

        [Fact]
        public void Null_Class_Root_Not_Reachable_Throws()
            => VerifyAssumptionThrows<Exception, Exception>(null, _ => _.AssumesNotReachable());

        [Fact]
        public void Not_Null_Class_Root_Not_Reachable_Throws()
            => VerifyAssumptionThrows<Exception, Exception>(InnerException, _ => _.AssumesNotReachable());
    }
}
