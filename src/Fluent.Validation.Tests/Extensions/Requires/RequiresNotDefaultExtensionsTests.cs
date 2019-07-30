using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class RequiresNotDefaultExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;&apos;{0}&apos; cannot be the default value defined by &apos;{1}&apos;&quot;
        /// </summary>
        private const string NotDefaultExpectedMessageFormat = "'{0}' cannot be the default value defined by '{1}'.";

        public RequiresNotDefaultExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static string RenderExpectedMessage<T>(string argumentName)
            => RenderArgumentMessage(Format(NotDefaultExpectedMessageFormat, argumentName, typeof(T).FullName)
                , argumentName);

        private static Action VerifyRequirement<T>(T value, string argumentName, out string renderedMessage)
            where T : struct
        {
            renderedMessage = RenderExpectedMessage<T>(argumentName);
            return () => value.RequiresNotDefault(argumentName);
        }

        [Theory, InlineData(1)]
        public void Not_Default_Value_Does_Not_Throw(int value) =>
            VerifyRequirement(value, ArgumentName, out _).Invoke();

        [Theory, InlineData(default(int))]
        public void Default_Value_Throws(int value)
            => VerifyRequirement(value, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(ArgumentName);
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });
    }
}
