using System;
using System.Collections.Generic;

namespace Validation
{
    public static partial class FluentValidationExtensionMethods
    {
        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <param name="_">A do not care place holder in order to root the extension method.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <param name="showAssert">Whether to Show an Assert.</param>
        /// <returns>Nothing, as this method always throws. The signature allows for
        /// &quot;throwing&quot; Fail so CSharp knows execution will stop.</returns>
        /// <see cref="Assumes.Fail(string,bool)"/>
        public static Exception AssumesFail(this object _, FluentMessageCallback message, bool showAssert = true)
            => Assumes.Fail(message.Invoke(), showAssert);

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <typeparam name="T">The place holder type.</typeparam>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <param name="showAssert">Whether to Show an Assert.</param>
        /// <returns>Nothing, as this method always throws. The signature allows for
        /// &quot;throwing&quot; Fail so CSharp knows execution will stop.</returns>
        /// <see cref="Assumes.Fail(string,bool)"/>
        public static Exception AssumesFail<T>(this T _, FluentMessageCallback message, bool showAssert = true)
            => Assumes.Fail(message.Invoke(), showAssert);

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <param name="_">A do not care place holder in order to root the extension method.</param>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <param name="innerException">An inner <see cref="Exception"/> for use with the thrown exception.</param>
        /// <param name="showAssert">Whether to Show an Assert.</param>
        /// <returns>Nothing, as this method always throws. The signature allows for
        /// &quot;throwing&quot; Fail so C# knows execution will stop.</returns>
        /// <see cref="Assumes.Fail(string,Exception,bool)"/>
        public static Exception AssumesFail(this object _, FluentMessageCallback message, Exception innerException, bool showAssert = true)
            => Assumes.Fail(message.Invoke(), innerException, showAssert);

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <typeparam name="T">The place holder type.</typeparam>
        /// <param name="message">The message to use for the thrown exception.</param>
        /// <param name="innerException">An inner <see cref="Exception"/> for use with the thrown exception.</param>
        /// <param name="showAssert">Whether to Show an Assert.</param>
        /// <returns>Nothing, as this method always throws. The signature allows for
        /// &quot;throwing&quot; Fail so C# knows execution will stop.</returns>
        /// <see cref="Assumes.Fail(string,Exception,bool)"/>
        public static Exception AssumesFail<T>(this T _, FluentMessageCallback message, Exception innerException, bool showAssert = true)
            => Assumes.Fail(message.UseCallbackOrDefault().Invoke(), innerException, showAssert);

        /// <summary>
        /// Throws an public exception if a condition evaluates to false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <see cref="Assumes.True(bool,string)"/>
        public static bool AssumesTrue(this bool condition, FluentMessageCallback message = null)
        {
            Assumes.True(condition, message.UseCallbackOrDefault().Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <see cref="Assumes.False(bool,string)"/>
        public static bool AssumesFalse(this bool condition, FluentMessageCallback message = null)
        {
            Assumes.False(condition, message.UseCallbackOrDefault().Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an public exception if a <paramref name="condition"/> Evaluates to False.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <see cref="Assumes.True(bool,string)"/>
        public static T AssumesTrue<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message = null)
        {
            Assumes.True(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Throws an public exception if a <paramref name="condition"/> Evaluates to True.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <see cref="Assumes.False(bool,string)"/>
        public static T AssumesFalse<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message = null)
        {
            Assumes.False(condition.Invoke(value), message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <see cref="object"/> is not of a given type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type the Value is expected to be.</typeparam>
        /// <param name="value">The Value to test.</param>
        /// <returns>The <paramref name="value"/> after <typeparamref name="T"/> Is Assumed.</returns>
        /// <see cref="Assumes.Is{T}"/>
        public static T AssumesIs<T>(this object value)
        {
            Assumes.Is<T>(value);
            return (T) value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null.
        /// </summary>
        /// <typeparam name="T">The type of Value to test.</typeparam>
        /// <param name="value">The Value.</param>
        /// <returns>The <paramref name="value"/> after .</returns>
        /// <see cref="Assumes.NotNull{T}"/>
        public static T AssumesNotNull<T>(this T value)
            where T : class
        {
            Assumes.NotNull(value);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Not Null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/> to test.</typeparam>
        /// <param name="value">The Value.</param>
        /// <returns>The <paramref name="value"/> after Null is Assumed.</returns>
        /// <see cref="Assumes.Null{T}"/>
        public static T AssumesNull<T>(this T value)
            where T : class
        {
            Assumes.Null(value);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="values"/> are Null or Empty.
        /// </summary>
        /// <typeparam name="T">The type of value elements to test.</typeparam>
        /// <param name="values">The Values.</param>
        /// <returns>The <paramref name="values"/> after Null or Empty are Assumed.</returns>
        /// <see cref="Assumes.NotNullOrEmpty"/>
        public static ICollection<T> AssumesNotNullOrEmpty<T>(this ICollection<T> values)
        {
            Assumes.NotNullOrEmpty(values);
            return values;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="values"/> are Null or Empty.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="values"/> to test.</typeparam>
        /// <param name="values">The Values.</param>
        /// <returns>The <paramref name="values"/> after Null or Empty are Assumed.</returns>
        /// <see cref="Assumes.NotNullOrEmpty"/>
        public static IEnumerable<T> AssumesNotNullOrEmpty<T>(this IEnumerable<T> values)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assumes.NotNullOrEmpty(values);
            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null or Empty.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <returns>The <paramref name="value"/> after Null or Empty is Assumed.</returns>
        /// <see cref="Assumes.NotNullOrEmpty"/>
        public static string AssumesNotNullOrEmpty(this string value)
        {
            Assumes.NotNullOrEmpty(value);
            return value;
        }

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <param name="_">A do not care place holder to root the extension method.</param>
        /// <returns>Nothing. This method always throws. But the signature allows calling
        /// code to &quot;throw&quot; this method for CSharp syntax reasons.</returns>
        /// <see cref="Assumes.NotReachable"/>
        public static Exception AssumesNotReachable(this object _) => Assumes.NotReachable();

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <typeparam name="T">The place holder type.</typeparam>
        /// <param name="_">A do not care place holder to root the extension method.</param>
        /// <returns>Nothing. This method always throws. But the signature allows calling
        /// code to &quot;throw&quot; this method for CSharp syntax reasons.</returns>
        /// <see cref="Assumes.NotReachable"/>
        public static Exception AssumesNotReachable<T>(this T _) => Assumes.NotReachable();

        /// <summary>
        /// Verifies that a <paramref name="component"/> is Not Null, otherwise throws
        /// an exception about the missing component.
        /// </summary>
        /// <typeparam name="T">The interface of the <paramref name="component"/>.</typeparam>
        /// <param name="component">A Component instance.</param>
        /// <returns>The <paramref name="component"/> after Present is Assumed.</returns>
        /// <see cref="Assumes.Present{T}"/>
        public static T AssumesPresent<T>(this T component)
        {
            Assumes.Present(component);
            return component;
        }
    }
}
