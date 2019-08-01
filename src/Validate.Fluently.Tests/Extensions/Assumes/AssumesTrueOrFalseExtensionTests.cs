using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesTrueOrFalseExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesTrueOrFalseExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionDoesNotThrow(bool condition, Action<bool> action)
            => action.AssertNotNull().Invoke(condition);

        private static void VerifyAssumptionThrows<TException>(bool condition, Action<bool> action, string expectedMessage = null)
            where TException : Exception
        {
            expectedMessage = expectedMessage ?? DefaultMessage;

            Action wrapper = () => action.AssertNotNull().Invoke(condition);

            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertEqual(expectedMessage));
        }

        private static void VerifyAssumptionThrows<T, TException>(T value, Action<T> action, string expectedMessage = null)
            where TException : Exception
        {
            expectedMessage = expectedMessage ?? DefaultMessage;

            Action wrapper = () => action.AssertNotNull().Invoke(value);

            wrapper.AssertNotNull().AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertEqual(expectedMessage));
        }

        [Fact]
        public void False_Assumes_False_Does_Not_Throw() => VerifyAssumptionDoesNotThrow(False, x => x.AssumesFalse());

        [Theory
         , InlineData(null)
         , InlineData(ThisIsATest)]
        public void True_Assumes_False_Throws(string expectedMessage)
        {
            switch (expectedMessage)
            {
                case null:
                    VerifyAssumptionThrows<Exception>(True, x => x.AssumesFalse());
                    break;

                default:
                    VerifyAssumptionThrows<Exception>(True, x => x.AssumesFalse(() => expectedMessage), expectedMessage);
                    break;
            }
        }

        [Fact]
        public void True_Assumes_True_Does_Not_Throw() => VerifyAssumptionDoesNotThrow(True, x => x.AssumesTrue());

        [Theory
         , InlineData(null)
         , InlineData(ThisIsATest)]
        public void False_Assumes_True_Throws(string expectedMessage)
        {
            switch (expectedMessage)
            {
                case null:
                    VerifyAssumptionThrows<Exception>(False, x => x.AssumesTrue());
                    break;

                default:
                    VerifyAssumptionThrows<Exception>(False, x => x.AssumesTrue(() => expectedMessage), expectedMessage);
                    break;
            }
        }

        [Theory
         , InlineData(LifeTheUniverseAndEverything, TwentyFour, null)
         , InlineData(TwentyFour, LifeTheUniverseAndEverything, null)
         , InlineData(LifeTheUniverseAndEverything, TwentyFour, ThisIsATest)
         , InlineData(TwentyFour, LifeTheUniverseAndEverything, ThisIsATest)]
        public void Typed_Assumes_True_First_Does_Not_Equal_Second_Throws(int first, int second, string expectedMessage)
        {
            switch (expectedMessage)
            {
                case null:
                    // ReSharper disable once ImplicitlyCapturedClosure
                    VerifyAssumptionThrows<int, Exception>(first, x => x.AssumesTrue(y => y == second));
                    break;

                default:
                    VerifyAssumptionThrows<int, Exception>(first, x => x.AssumesTrue(y => y == second, () => expectedMessage), expectedMessage);
                    break;
            }
        }

        [Theory
         , InlineData(LifeTheUniverseAndEverything, TwentyFour, null)
         , InlineData(TwentyFour, LifeTheUniverseAndEverything, null)
         , InlineData(LifeTheUniverseAndEverything, TwentyFour, ThisIsATest)
         , InlineData(TwentyFour, LifeTheUniverseAndEverything, ThisIsATest)]
        public void Typed_Assumes_True_First_Does_Not_Not_Equal_Second_Throws(int first, int second, string expectedMessage)
        {
            switch (expectedMessage)
            {
                case null:
                    // ReSharper disable once ImplicitlyCapturedClosure
                    VerifyAssumptionThrows<int, Exception>(first, x => x.AssumesFalse(y => y != second));
                    break;

                default:
                    VerifyAssumptionThrows<int, Exception>(first, x => x.AssumesFalse(y => y != second, () => expectedMessage), expectedMessage);
                    break;
            }
        }
    }
}
