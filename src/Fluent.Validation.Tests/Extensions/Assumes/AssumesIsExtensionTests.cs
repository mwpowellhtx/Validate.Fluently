using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesIsExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesIsExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

#region Academic Illustration

        private abstract class A
        {
            protected A()
            {
            }
        }

        private class B : A
        {
            internal B()
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class C : A
        {
        }

        #endregion // Academic Illustration

        private static void VerifyAssumptionDoesNotThrow<T, TAssumption>(T value) => value.AssumesIs<TAssumption>();

        private static void VerifyAssumptionThrows<T, TExpected, TException>(T first, string expectedMessage = null)
            where TException : Exception
        {
            expectedMessage = expectedMessage ?? DefaultMessage;

            Action action = () => first.AssumesIs<TExpected>();

            action.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertEqual(expectedMessage.AssertNotNull()));
        }

        [Fact]
        public void Can_Assume_B_Is_A() => VerifyAssumptionDoesNotThrow<B, A>(new B());

        [Fact]
        public void Can_Assume_A_Is_B() => VerifyAssumptionDoesNotThrow<A, B>(new B());

        [Fact]
        public void Cannot_Assume_Int_Is_Double() => VerifyAssumptionThrows<int, double, Exception>(LifeTheUniverseAndEverything);

        [Fact]
        public void Cannot_Assume_B_Is_C() => VerifyAssumptionThrows<B, C, Exception>(new B());
    }
}
