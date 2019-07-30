using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesFailExtensionTests : AssumesExtensionsTestFixtureBase
    {
        // TODO: TBD: showAssert coverage?

        public AssumesFailExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionThrows<TException>(object root, Action<object> action, string expectedMessage = DefaultMessage)
            where TException : Exception
        {
            Action wrapper = () => action.AssertNotNull().Invoke(root);

            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(expectedMessage));
        }

        private static void VerifyAssumptionThrows<TRoot, TException>(TRoot root, Action<TRoot> action, string expectedMessage = DefaultMessage)
            where TException : Exception
        {
            Action wrapper = () => action.AssertNotNull().Invoke(root);

            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(expectedMessage));
        }

        private static void VerifyAssumptionThrows<TException>(object root
            , Exception innerException, Action<object, Exception> action, string expectedMessage = DefaultMessage)
            where TException : Exception
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Action wrapper = () => action.AssertNotNull().Invoke(root, innerException);

            // ReSharper disable once ImplicitlyCapturedClosure
            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex =>
                {
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(expectedMessage);
                    ex.InnerException.AssertNotNull().AssertSame(innerException.AssertNotNull());
                });
        }

        private static void VerifyAssumptionThrows<TRoot, TException>(TRoot root
            , Exception innerException, Action<TRoot, Exception> action, string expectedMessage = DefaultMessage)
            where TException : Exception
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Action wrapper = () => action.AssertNotNull().Invoke(root, innerException);

            // ReSharper disable once ImplicitlyCapturedClosure
            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex =>
                {
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(expectedMessage);
                    ex.InnerException.AssertNotNull().AssertSame(innerException.AssertNotNull());
                });
        }

        [Fact]
        public void Root_Assumes_Fail_Throws()
            => VerifyAssumptionThrows<Exception>(Root
                , _ => _.AssumesFail(() => ThisIsATest), ThisIsATest);

        [Fact]
        public void Null_Root_Assumes_Fail_Throws()
            => VerifyAssumptionThrows<Exception>(NullRoot
                , _ => _.AssumesFail(() => ThisIsATest), ThisIsATest);

        [Fact]
        public void Typed_Root_Assumes_Fail_Throws()
            => VerifyAssumptionThrows<int, Exception>(LifeTheUniverseAndEverything
                , _ => _.AssumesFail(() => ThisIsATest), ThisIsATest);

        [Fact]
        public void Root_InnerException_Assumes_Fail_Throws()
            => VerifyAssumptionThrows<Exception>(Root, InnerException
                , (_, __)=> _.AssumesFail(() => ThisIsATest, __), ThisIsATest);

        [Fact]
        public void Null_Root_InnerException_Assumes_Fail_Throws()
            => VerifyAssumptionThrows<Exception>(NullRoot, InnerException
                , (_, __) => _.AssumesFail(() => ThisIsATest, __), ThisIsATest);

        // TODO: TBD: invoke this one? VerifyAssumptionThrows<TRoot, TException> ...
    }
}
