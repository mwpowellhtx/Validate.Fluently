namespace Validation
{
    using Xunit.Abstractions;

    public abstract class RequiresExtensionsTestFixtureBase : ExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;arg&quot;
        /// </summary>
        protected const string ArgumentName = "arg";

        /// <summary>
        /// &quot;Value cannot be null.&quot;
        /// </summary>
        protected const string ValueCannotBeNullMessage = "Value cannot be null.";

        protected RequiresExtensionsTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Renders the <paramref name="baseMessage"/> given the <paramref name="argumentName"/>.
        /// </summary>
        /// <param name="baseMessage"></param>
        /// <param name="argumentName"></param>
        /// <returns></returns>
        protected static string RenderArgumentMessage(string baseMessage, string argumentName)
            => $"{baseMessage}\r\nParameter name: {argumentName}";
    }
}
