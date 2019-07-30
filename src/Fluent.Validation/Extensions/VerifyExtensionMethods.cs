using System;

namespace Validation
{
    public static partial class FluentValidationExtensionMethods
    {
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="_">A do not care place holder to root the extension method.</param>
        /// <param name="message">The message to use with the thrown exception.</param>
        /// <returns>The root place holder after Fail Operation is Verified.</returns>
        public static object VerifyFailOperation(this object _, FluentMessageCallback message)
        {
            Verify.FailOperation(message.Invoke());
            return _;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <typeparam name="T">The type of the root place holder.</typeparam>
        /// <param name="_">A do not care place holder to root the extension method.</param>
        /// <param name="message">The message to use with the thrown exception.</param>
        /// <returns>The root place holder after Fail Operation is Verified.</returns>
        public static T VerifyFailOperation<T>(this T _, FluentMessageCallback message)
        {
            Verify.FailOperation(message.Invoke());
            return _;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when <paramref name="condition"/>
        /// is False.
        /// </summary>
        /// <param name="condition">the condition which returns True to avoid <see cref="ObjectDisposedException"/>.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <returns>The <paramref name="condition"/> after Not Disposed is Verified.</returns>
        public static bool VerifyNotDisposed(this bool condition, FluentMessageCallback message)
        {
            Verify.NotDisposed(condition, message.Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when <paramref name="condition"/>
        /// is False.
        /// </summary>
        /// <param name="disposedValue">The Disposed Value.</param>
        /// <param name="condition">the condition which returns True to avoid <see cref="ObjectDisposedException"/>.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <returns>The <paramref name="disposedValue"/> after Not Disposed <paramref name="condition"/> is Verified.</returns>
        public static object VerifyNotDisposed(this object disposedValue, FluentConditionCallback<object> condition, FluentMessageCallback message)
        {
            Verify.NotDisposed(condition.Invoke(disposedValue), message.Invoke());
            return disposedValue;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when <paramref name="condition"/>
        /// is False.
        /// </summary>
        /// <typeparam name="T">The Value type.</typeparam>
        /// <param name="disposedValue">the Disposed Value.</param>
        /// <param name="condition">the condition which returns True to avoid <see cref="ObjectDisposedException"/>.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <returns>The <paramref name="disposedValue"/> after Not Disposed <paramref name="condition"/> is Verified.</returns>
        public static T VerifyNotDisposed<T>(this T disposedValue, FluentConditionCallback<T> condition, FluentMessageCallback message)
            where T : IDisposable
        {
            Verify.NotDisposed(condition.Invoke(disposedValue), message.Invoke());
            return disposedValue;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when <paramref name="condition"/>
        /// is False.
        /// </summary>
        /// <typeparam name="T">The Value type.</typeparam>
        /// <param name="disposedValue">The Disposed Value.</param>
        /// <param name="condition">The condition which returns True to avoid <see cref="ObjectDisposedException"/>.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <returns>The <paramref name="disposedValue"/> after Not Disposed <paramref name="condition"/> is Verified.</returns>
        public static T VerifyNotDisposedObservable<T>(this T disposedValue, FluentConditionCallback<T> condition, FluentMessageCallback message)
            where T : IDisposableObservable
        {
            Verify.NotDisposed(condition.Invoke(disposedValue), message.Invoke());
            return disposedValue;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if a <paramref name="condition"/>
        /// is False.
        /// </summary>
        /// <param name="condition">The Condition which returns True to avoid <see cref="InvalidOperationException"/>.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <returns>The <paramref name="condition"/> after Operation is Verified.</returns>
        public static bool VerifyOperation(this bool condition, FluentMessageCallback message)
        {
            Verify.Operation(condition, message.Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if a <paramref name="value"/>
        /// <paramref name="condition"/> is False.
        /// </summary>
        /// <typeparam name="T">The Value type.</typeparam>
        /// <param name="value">The Value.</param>
        /// <param name="condition">The Condition which returns True to avoid <see cref="InvalidOperationException"/>.</param>
        /// <param name="message">The message for use with the thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Operation <paramref name="condition"/> is Verified.</returns>
        public static T VerifyOperation<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message)
        {
            Verify.Operation(condition.Invoke(value), message.Invoke());
            return value;
        }
    }
}
