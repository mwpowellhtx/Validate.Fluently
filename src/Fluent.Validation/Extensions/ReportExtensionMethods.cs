namespace Validation
{
    public static partial class FluentValidationExtensionMethods
    {
        /// <summary>
        /// Reports a certain failure.
        /// </summary>
        /// <param name="_">A do not care place holder rooting the extension method.</param>
        /// <param name="message">The message.</param>
        /// <returns>The <paramref name="_"/> place holder after Reporting the certain Failure.</returns>
        public static object ReportFail(this object _, FluentMessageCallback message = null)
        {
            Report.Fail(message.UseCallbackOrDefault().Invoke());
            return _;
        }

        /// <summary>
        /// Reports a certain failure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_">A do not care place holder rooting the extension method.</param>
        /// <param name="message">The message.</param>
        /// <returns>The <paramref name="_"/> place holder after Reporting the certain Failure.</returns>
        public static T ReportFail<T>(this T _, FluentMessageCallback message = null)
        {
            Report.Fail(message.UseCallbackOrDefault().Invoke());
            return _;
        }

        /// <summary>
        /// Reports an error if the <paramref name="condition"/> Evaluates to True.
        /// </summary>
        /// <param name="condition">An error is reported when <paramref name="condition"/> is False.</param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="condition"/> after Reporting If met.</returns>
        public static bool ReportIf(this bool condition, FluentMessageCallback message = null)
        {
            Report.If(condition, message.UseCallbackOrDefault().Invoke());
            return condition;
        }

        /// <summary>
        /// Reports an error if the <paramref name="condition"/> Evaluates to True.
        /// </summary>
        /// <param name="value">The Value being Reported upon.</param>
        /// <param name="condition">An error is reported when <paramref name="condition"/> returns False.</param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="value"/> after Reporting If <paramref name="condition"/> met.</returns>
        public static object ReportIf(this object value, FluentConditionCallback<object> condition, FluentMessageCallback message = null)
        {
            Report.If(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Reports an error if the <paramref name="condition"/> Evaluates to True.
        /// </summary>
        /// <typeparam name="T">The <paramref name="value"/> type.</typeparam>
        /// <param name="value">The Value being Reported upon.</param>
        /// <param name="condition">An error is reported when <paramref name="condition"/> returns False.</param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="value"/> after Reporting If <paramref name="condition"/> met.</returns>
        public static T ReportIf<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message = null)
        {
            Report.If(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Reports an error if a <paramref name="condition"/> does not Evaluate to True.
        /// </summary>
        /// <param name="condition">If set to false, an error is reported.</param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="condition"/> after Reporting If Not met.</returns>
        public static bool ReportIfNot(this bool condition, FluentMessageCallback message = null)
        {
            Report.IfNot(condition, message.UseCallbackOrDefault().Invoke());
            return condition;
        }

        /// <summary>
        /// Reports an error if a <paramref name="condition"/> does not Evaluate to True.
        /// </summary>
        /// <param name="value">The Value being Reported upon.</param>
        /// <param name="condition">An error is reported when returning False.</param>
        /// <param name="message">The formatted message.</param>
        /// <returns></returns>
        /// <returns>The <paramref name="value"/> after Report If Not meeting the <paramref name="condition"/>.</returns>
        public static object ReportIfNot(this object value, FluentConditionCallback<object> condition, FluentMessageCallback message = null)
        {
            Report.IfNot(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Reports an error if a <paramref name="condition"/> does not Evaluate to True.
        /// </summary>
        /// <typeparam name="T">The <paramref name="value"/> type.</typeparam>
        /// <param name="value">The Value being Reported upon.</param>
        /// <param name="condition">An error is reported when returning False.</param>
        /// <param name="message">The formatted message.</param>
        /// <returns>The <paramref name="value"/> after Report If Not meeting the <paramref name="condition"/>.</returns>
        public static T ReportIfNot<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message = null)
        {
            Report.IfNot(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Verifies that a <paramref name="component"/> is Not Null, and reports
        /// an error about the missing component otherwise.
        /// </summary>
        /// <typeparam name="T">The interface of the imported <paramref name="component"/>.</typeparam>
        /// <param name="component">The imported component</param>
        /// <returns>The <paramref name="component"/> after Reporting If Not Present.</returns>
        public static T ReportIfNotPresent<T>(this T component)
        {
            Report.IfNotPresent(component);
            return component;
        }
    }
}
