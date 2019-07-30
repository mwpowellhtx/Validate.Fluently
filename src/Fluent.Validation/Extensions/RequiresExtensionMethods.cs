using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Validation
{
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class FluentValidationExtensionMethods
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if a <paramref name="condition"/>
        /// does not evaluate to True.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool RequiresArgument(this bool condition, string argumentName, FluentMessageCallback message)
        {
            Requires.Argument(condition, argumentName, message.Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if a <paramref name="condition"/>
        /// does not evaluate to True.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="condition"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T RequiresArgument<T>(this T argument, FluentConditionCallback<T> condition, string argumentName, FluentMessageCallback message)
        {
            Requires.Argument(condition(argument), argumentName, message.Invoke());
            return argument;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/>
        /// is not defined by the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of enum the <paramref name="value"/> is constrained to be defined within.</typeparam>
        /// <param name="value">The Value that must be defined in <typeparamref name="T"/>.</param>
        /// <param name="argumentName">The name of the argument that supplied the value.</param>
        /// <returns>The <paramref name="value"/> after Defined is Required.</returns>
        /// <see cref="!:https://portablelibraryprofiles.stephencleary.com/">Portable Class Library (PCL) profiles for purposes of aligning with the Validation callbacks.</see>
        public static T RequiresDefined<T>(this T value, string argumentName)
#if NETSTANDARD1_0 || PROFILE259
            where T : struct, IComparable, IFormattable // i.e. Enum
#else // IConvertible missing from netstandard1.2
            where T : struct, IComparable, IFormattable, IConvertible // i.e. Enum
#endif
        {
            Requires.Defined(value, argumentName);
            return value;
        }

        // TODO: TBD: this is work, and it is mostly just meticulous, very detail oriented... not that I mind it... but I want to refocus on the CG/Sat OrTools stuff...
        // TODO: TBD: pick up testing here ...
        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="message"></param>
        /// <returns>Nothing. It always throws.</returns>
        public static Exception RequiresFail<T>(this T _, FluentMessageCallback message)
            => Requires.Fail(message.UseCallbackOrDefault().Invoke());

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        /// <returns>Nothing. This method always throws. But the signature allows calling
        /// code to &quot;throw&quot; this method for CSharp syntax reasons.</returns>
        public static Exception RequiresFail<T>(this T _, Exception innerException, FluentMessageCallback message)
            => Requires.Fail(innerException, message.UseCallbackOrDefault().Invoke());

        // TODO: TBD: `out of range´ of what? would it not be worth specifying in terms of at least an IEnumerable<T>?
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/>
        /// <paramref name="evaluate"/> returns false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        /// <param name="evaluate"></param>
        /// <param name="message"></param>
        /// <returns>Nothing. This method always throws.</returns>
        public static Exception RequiresFailRange<T>(this T value, string argumentName, FluentRangeVerificationCallback<T> evaluate, FluentMessageCallback message = null)
            => evaluate.Invoke(value) ? default : Requires.FailRange(argumentName, message.UseCallbackOrDefault().Invoke());

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if a <paramref name="condition"/>
        /// Does Not Evaluate to True.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="condition"/> after Range is Required.</returns>
        public static bool RequiresRange(this bool condition, string argumentName, FluentMessageCallback message = null)
        {
            Requires.Range(condition, argumentName, message.UseCallbackOrDefault().Invoke());
            return condition;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if a <paramref name="condition"/>
        /// Does Not Evaluate to True.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        /// <returns>The <paramref name="value"/> after Range <paramref name="condition"/> is Required.</returns>
        public static T RequiresRange<T>(this T value, FluentConditionCallback<T> condition, string argumentName, FluentMessageCallback message = null)
        {
            Requires.Range(condition.Invoke(value), argumentName, message.UseCallbackOrDefault().Invoke());
            return value;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/>
        /// is Equal to the Default Value of the <see cref="Type"/> <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Not Default is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is Not Default.</exception>
        public static T RequiresNotDefault<T>(this T value, string argumentName)
            where T : struct
        {
            Requires.NotDefault(value, argumentName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null.
        /// </summary>
        /// <typeparam name="T">The type of the return value of the <see cref="Task{TResult}"/>.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Not Null is Required.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is Null.</exception>
        /// <remarks>This method allows async methods to use <see cref="Requires.NotNull{T}(T,string)"/>
        /// without having to assign the result to local variables to avoid CSharp warnings.</remarks>
        public static Task<T> RequiresNotNull<T>(this Task<T> value, string argumentName)
        {
            Requires.NotNull(value, argumentName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null.
        /// </summary>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Not Null is Required.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is Null.</exception>
        /// <remarks>This method allows async methods to use <see cref="Requires.NotNull{T}(T,string)"/>
        /// without having to assign the result to local variables to avoid CSharp warnings.</remarks>
        public static Task RequiresNotNull(this Task value, string argumentName)
        {
            Requires.NotNull(value, argumentName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see cref="IntPtr.Zero"/>.</exception>
        public static IntPtr RequiresNotNull(this IntPtr value, string argumentName) => Requires.NotNull(value, argumentName);

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns> value of the parameter.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is Null.</exception>
        public static T RequiresNotNull<T>(this T value, string argumentName)
            where T : class
            => Requires.NotNull(value, argumentName);

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is Null.</exception>
        /// <remarks>This method exists for callers who themselves only know the type as a generic
        /// argument which may or may not be a class, but certainly cannot be Null.</remarks>
        public static T RequiresNotNullAllowStructs<T>(this T value, string argumentName)
            => Requires.NotNullAllowStructs(value, argumentName);

        /// <summary>
        /// Throws an exception if the specified <paramref name="values"/> is Null,
        /// has no elements or has an element with a Null value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="values">The Values of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="values"/> after Not Null Empty or Null Elements is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if the tested condition is false.</exception>
        public static IEnumerable<T> RequiresNotNullEmptyOrNullElements<T>(this IEnumerable<T> values, string argumentName)
            where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            Requires.NotNullEmptyOrNullElements(values, argumentName);
            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="values"/> is Null,
        /// has no elements or has an element with a Null value.
        /// </summary>
        /// <param name="values">The Values of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="values"/> after Not Null or Empty is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if the tested condition is false.</exception>
        public static IEnumerable RequiresNotNullOrEmpty(this IEnumerable values, string argumentName)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Requires.NotNullOrEmpty(values, argumentName);
            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null or Empty.
        /// </summary>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Not Null or Empty is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is Null, Empty or White Space.</exception>
        public static string RequiresNotNullOrEmpty(this string value, string argumentName)
        {
            Requires.NotNullOrEmpty(value, argumentName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="value"/> is Null, Empty,
        /// or White Space.
        /// </summary>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="value"/> after Not Null, Empty or White Space is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is Null or Empty.</exception>
        public static string RequiresNotNullOrWhiteSpace(this string value, string argumentName)
        {
            Requires.NotNullOrWhiteSpace(value, argumentName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified <paramref name="values"/> is Not Null,
        /// and Has an Element with a Null value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="values">The Values of the argument.</param>
        /// <param name="argumentName">The name of the argument to include in any thrown exception.</param>
        /// <returns>The <paramref name="values"/> after Not Null or Not Null Elements is Required.</returns>
        /// <exception cref="ArgumentException">Thrown if the tested condition is false.</exception>
        public static IEnumerable<T> RequiresNullOrNotNullElements<T>(this IEnumerable<T> values, string argumentName)
            where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            Requires.NullOrNotNullElements(values, argumentName);
            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Validates some expression describing the acceptable <paramref name="condition"/>
        /// for an argument Evaluates to True.
        /// </summary>
        /// <param name="condition">The expression that must Evaluate to True to avoid an <see cref="ArgumentException"/>.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="message">The message.</param>
        /// <returns>The <paramref name="condition"/> after That <paramref name="condition"/> is Required.</returns>
        public static bool RequiresThat(this bool condition, string argumentName, FluentMessageCallback message)
        {
            Requires.That(condition, argumentName, message.Invoke());
            return condition;
        }

        /// <summary>
        /// Validates some expression describing the acceptable <paramref name="condition"/>
        /// for an argument <paramref name="value"/> Evaluates to True.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="condition">The Condition to Evaluate to True to avoid an <see cref="ArgumentException"/>.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="message">The message.</param>
        /// <returns>The <paramref name="value"/> after That <paramref name="value"/> is Required.</returns>
        public static T RequiresThat<T>(this T value, FluentConditionCallback<T> condition, string argumentName, FluentMessageCallback message)
        {
            Requires.That(condition.Invoke(value), argumentName, message.Invoke());
            return value;
        }

        /// <summary>
        /// Validates some expression describing the acceptable <paramref name="condition"/>
        /// for an argument Evaluates to True.
        /// </summary>
        /// <param name="condition">The expression that must Evaluate to True to avoid an <see cref="InvalidOperationException"/>.</param>
        /// <param name="message">The message to include with the exception.</param>
        /// <returns>The <paramref name="condition"/> after Valid State is Required.</returns>
        public static bool RequiresValidState(this bool condition, FluentMessageCallback message)
        {
            Requires.ValidState(condition, message.Invoke());
            return condition;
        }

        /// <summary>
        /// Validates some expression describing the acceptable <paramref name="condition"/>
        /// for an argument Evaluates to True.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The Value of the argument.</param>
        /// <param name="condition">The expression that must Evaluate to True to avoid an <see cref="InvalidOperationException"/>.</param>
        /// <param name="message">The message to include with the exception.</param>
        /// <returns>The <paramref name="value"/> after Valid State <paramref name="condition"/> is Required.</returns>
        public static T RequiresValidState<T>(this T value, FluentConditionCallback<T> condition, FluentMessageCallback message)
        {
            Requires.ValidState(condition.Invoke(value), message.Invoke());
            return value;
        }
    }
}
