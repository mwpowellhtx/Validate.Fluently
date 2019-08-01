using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesPresentExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesPresentExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionDoesNotThrow<T>(T value, Action<T> action) => action.Invoke(value);

        private static void VerifyAssumptionThrows<T, TException>(T value, Action<T> action, string expectedMessage)
            where TException : Exception
        {
            Action wrapper = () => action.Invoke(value);

            wrapper.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(expectedMessage.AssertNotNull()));
        }

        [Fact]
        public void Not_Null_Component_Present_Does_Not_Throw()
        {
            void Verify<T>(T component) => VerifyAssumptionDoesNotThrow(component, x => x.AssumesPresent().AssertSame(x));
            Verify(InnerException);
        }

        [Fact]
        public void Null_Component_Present_Throws()
            => VerifyAssumptionThrows<Exception, Exception>(null, x => x.AssumesPresent(), PresentDefaultMessage);
    }
}
