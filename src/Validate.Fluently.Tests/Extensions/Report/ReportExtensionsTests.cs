using System.Collections.Generic;
using System.Linq;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class ReportExtensionsTests : ReportExtensionTestFixtureBase
    {
        public ReportExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            ReportValidationExtensionMethods.ReportedMessages = new List<string>();
        }

        private static ICollection<string> ReportedMessages => ReportValidationExtensionMethods.ReportedMessages.AssertNotNull();

        private static void VerifyReportedMessages(string expected, bool condition = true)
        {
            if (condition)
            {
                ReportedMessages.AssertTrue(x => x.Any()).AssertContains(x => x == expected);
            }
            else
            {
                ReportedMessages.AssertFalse(x => x.Any());
            }
        }

        [Fact]
        public void ReportedMessage_Instance_Not_Null()
        {
            ReportedMessages.AssertEmpty();
        }

        [Fact]
        public void Object_Fail_Correct()
        {
            void Verify(string s)
            {
                NullRoot.ReportFail(() => s).AssertNull();
                VerifyReportedMessages(s);
            }

            Verify(ThisIsATest);
        }

        [Fact]
        public void Typed_Fail_Correct()
        {
            void Verify(string s)
            {
                default(int).ReportFail(() => s).AssertEqual(default);
                VerifyReportedMessages(s);
            }

            Verify(ThisIsATest);
        }

        [Theory
         , InlineData(true)
         , InlineData(false)]
        public void Boolean_If_Correct(bool condition)
        {
            void Verify(string s)
            {
                VerifyReportedMessages(s, condition.ReportIf(() => s).AssertEqual(condition));
            }

            Verify(ThisIsATest);
        }

        [Theory
         , InlineData(true)
         , InlineData(false)]
        public void Object_If_Correct(bool condition)
        {
            void Verify(string s)
            {
                NullRoot.ReportIf(_ => condition, () => s).AssertNull();
                VerifyReportedMessages(s, condition);
            }

            Verify(ThisIsATest);
        }

        [Theory
         , InlineData(default(int), default(int))
         , InlineData(default(int), 1)]
        public void Typed_If_Correct(int expected, int actual)
        {
            void Verify(string s)
            {
                actual.ReportIf(x => x == expected, () => s).AssertEqual(actual);
                VerifyReportedMessages(s, actual == expected);
            }
        }

        [Theory
         , InlineData(true)
         , InlineData(false)]
        public void Boolean_IfNot_Correct(bool condition)
        {
            void Verify(string s)
            {
                VerifyReportedMessages(s, !condition.ReportIfNot(() => s).AssertEqual(condition));
            }

            Verify(ThisIsATest);
        }

        [Theory
         , InlineData(true)
         , InlineData(false)]
        public void Object_IfNot_Correct(bool condition)
        {
            void Verify(string s)
            {
                NullRoot.ReportIfNot(_ => condition, () => s).AssertNull();
                VerifyReportedMessages(s, !condition);
            }

            Verify(ThisIsATest);
        }

        [Theory
         , InlineData(default(int), default(int))
         , InlineData(default(int), 1)]
        public void Typed_IfNot_Correct(int expected, int actual)
        {
            void Verify(string s)
            {
                actual.ReportIfNot(x => x == expected, () => s).AssertEqual(actual);
                VerifyReportedMessages(s, actual != expected);
            }
        }
    }
}
