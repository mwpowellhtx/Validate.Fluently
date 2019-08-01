using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesStringExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesStringExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyAssumptionDoesNotThrow(string value, Action<string> action) => action.Invoke(value);

        private static void VerifyAssumptionThrows<TException>(string value, Action<string> action)
            where TException : Exception
        {
            Action wrapper = () => action.Invoke(value);

            wrapper.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));
        }

        private static string NullString => null;

        /// <summary>
        /// &quot;&quot;
        /// </summary>
        private const string EmptyString = "";

        // ReSharper disable once CommentTypo
        /// <summary>
        /// &quot;abcd&quot;
        /// </summary>
        private const string ValidString = "abcd";

        // TODO: TBD: could potentially look at abstracting out bits of the AssumesString/Collection tests into something a bit more generic...
        [Fact]
        public void String_Assumes_Not_Null_Or_Empty_Does_Not_Throw()
            => VerifyAssumptionDoesNotThrow(ValidString, x => x.AssumesNotNullOrEmpty().AssertSame(x));

        [Fact]
        public void Empty_String_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyAssumptionThrows<Exception>(EmptyString, x => x.AssumesNotNullOrEmpty());

        [Fact]
        public void Null_String_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyAssumptionThrows<Exception>(NullString, x => x.AssumesNotNullOrEmpty());
    }
}
