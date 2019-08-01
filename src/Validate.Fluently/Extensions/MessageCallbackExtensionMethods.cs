namespace Validation
{
    internal static class MessageCallbackExtensionMethods
    {
        private static FluentMessageCallback GetDefaultCallback(string message = null) => () => message;
        public static FluentMessageCallback UseCallbackOrDefault(this FluentMessageCallback callback, string message = null)
            => callback ?? GetDefaultCallback(message);
    }
}
