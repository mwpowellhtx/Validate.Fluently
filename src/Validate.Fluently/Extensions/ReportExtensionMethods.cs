using System;
using System.Collections.Generic;

namespace Validation
{
    using static String;

    public static class ReportValidationExtensionMethods
    {
        /// <summary>
        /// Gets or Sets the ReportedMessages for internal use.
        /// We leave this unassigned on purpose for Release purposes.
        /// </summary>
        internal static ICollection<string> ReportedMessages { get; set; }

        /// <summary>
        /// Notifies <see cref="ReportedMessages"/> of the <paramref name="message"/>
        /// based on the <paramref name="condition"/>.
        /// </summary>
        /// <param name="message">The Message to Report.</param>
        /// <param name="condition">Whether to Report the Message.</param>
        /// <returns>The <paramref name="message"/> after the <paramref name="condition"/> based Notification.</returns>
        /// <remarks>The back story here is that the CoreFx team decided to remove the
        /// System.Diagnostics.Debug.Listeners API from the API. Furthermore, digging
        /// further, attempts to leverage TraceListenerCollection are met with an inconsistent
        /// API exposure, at best. Which leaves us with this shimmy for a workaround in this
        /// particular instance.</remarks>
        private static string Notify(this string message, bool condition = true)
        {
            if (condition)
            {
                // Do this optionally. We do not set this by default, but rather leave that for the Unit Tests.
                ReportedMessages?.Add(message);
            }

            return message;
        }

        /// <summary>
        /// Reports a certain failure.
        /// </summary>
        /// <param name="_">A do not care place holder rooting the extension method.</param>
        /// <param name="message">The message.</param>
        /// <returns>The <paramref name="_"/> place holder after Reporting the certain Failure.</returns>
        public static object ReportFail(this object _, FluentMessageCallback message = null)
        {
            void Evaluate(string s) => Report.Fail(s.Notify());
            Evaluate(message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(string s) => Report.Fail(s.Notify());
            Evaluate(message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.If(y, s.Notify(y));
            Evaluate(condition, message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.If(y, s.Notify(y));
            Evaluate(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.If(y, s.Notify(y));
            Evaluate(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.IfNot(y, s.Notify(!y));
            Evaluate(condition, message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.IfNot(y, s.Notify(!y));
            Evaluate(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
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
            void Evaluate(bool y, string s) => Report.IfNot(y, s.Notify(y));
            Evaluate(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        // TODO: TBD: we might be better off also using a String table resource... but this will work for the time being...
        /// <summary>
        /// &quot;Cannot find an instance of the {0} service.&quot;
        /// </summary>
        internal const string ServiceInstanceNotFoundFormat = "Cannot find an instance of the {0} service.";

        internal static string RenderNotPresent<T>() => Format(ServiceInstanceNotFoundFormat, typeof(T).FullName);

        /// <summary>
        /// Verifies that a <paramref name="component"/> is Not Null, and reports
        /// an error about the missing component otherwise.
        /// </summary>
        /// <typeparam name="T">The interface of the imported <paramref name="component"/>.</typeparam>
        /// <param name="component">The imported component</param>
        /// <returns>The <paramref name="component"/> after Reporting If Not Present.</returns>
        /// <see cref="!:https://github.com/AArnott/Validation/blob/master/src/Validation/Report.cs#L22"/>
        public static T ReportIfNotPresent<T>(this T component)
        {
            void Evaluate(bool y, string s)
            {
                if (y)
                {
                    s.Notify();
                }

                // Unlike many of the other API, this one actually does not receive any messages.
                Report.IfNotPresent(component);
            }

            // This one got a little ugly, short of peeling layers back on the onion, which we were forced to do anyway.
            Evaluate(component == null, RenderNotPresent<T>());
            return component;
        }
    }
}
