﻿using System;
using System.Collections.Generic;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class RequiresCollectionExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;&apos;{0}&apos; must contain at least one element.&quot;
        /// </summary>
        private const string EmptyCollectionMessageFormat = "'{0}' must contain at least one element.";

        /// <summary>
        /// &quot;&apos;{0}&apos; cannot contain a null (Nothing in Visual Basic) element.&quot;
        /// </summary>
        private const string CollectionCannotContainNullsFormat = "'{0}' cannot contain a null (Nothing in Visual Basic) element.";

        /// <summary>
        /// &quot;&apos;arg&apos; cannot be an empty string (\&quot;\&quot;) or start with the null character.&quot;
        /// </summary>
        private const string NullOrEmptyStringFormat = "'arg' cannot be an empty string (\"\") or start with the null character.";

        public RequiresCollectionExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private class A
        {
            // ReSharper disable once EmptyConstructor
            internal A()
            {
            }
        }

        private static IEnumerable<A> NullCollection => null;

        private static IEnumerable<A> EmptyCollection => GetRange<A>();

        private static IEnumerable<A> CollectionWithNulls => new A[] {null};

        private static IEnumerable<A> ValidCollection => GetRange(new A());

        private static Action VerifyNotNullEmptyOrNullElements<T>(IEnumerable<T> values, string argumentName, out string renderedMessage, string expectedMessage = null)
            where T : class
        {
            expectedMessage = (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderArgumentMessage(expectedMessage, argumentName);
            return () => values.RequiresNotNullEmptyOrNullElements(argumentName);
        }

        [Fact]
        public void Null_Collection_Throws()
            => VerifyNotNullEmptyOrNullElements(NullCollection, ArgumentName, out var renderedMessage, ValueCannotBeNullMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Empty_Collection_Throws()
            => VerifyNotNullEmptyOrNullElements(EmptyCollection, ArgumentName, out var renderedMessage
                    , Format(EmptyCollectionMessageFormat, ArgumentName))
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Collection_With_Null_Elements_Throws()
            => VerifyNotNullEmptyOrNullElements(CollectionWithNulls, ArgumentName, out var renderedMessage
                    , Format(CollectionCannotContainNullsFormat, ArgumentName))
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        private static IEnumerable<T> VerifyNotNullEmptyOrNullElements<T>(IEnumerable<T> values, string argumentName)
            where T : class
            => values.RequiresNotNullEmptyOrNullElements(argumentName);

        [Fact]
        public void Valid_Collection_Does_Not_Throw()
        {
            void Verify<T>(IEnumerable<T> values)
                where T : class
                => VerifyNotNullEmptyOrNullElements(values, ArgumentName).AssertNotNull().AssertSame(values);

            Verify(ValidCollection);
        }

        private static Action VerifyNotNullOrEmpty(string value, string argumentName, out string renderedMessage, string expectedMessage = null)
        {
            renderedMessage = RenderArgumentMessage(
                (expectedMessage ?? DefaultMessage).AssertNotNull().AssertNotEmpty()
                , argumentName.AssertNotNull().AssertNotEmpty());
            return () => value.RequiresNotNullOrEmpty(argumentName);
        }

        private static string VerifyNotNullOrEmpty(string value, string argumentName)
            => value.RequiresNotNullOrEmpty(argumentName);

        [Fact]
        public void Null_String_Throws()
            => VerifyNotNullOrEmpty(null, ArgumentName, out var renderedMessage, ValueCannotBeNullMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Empty_String_Throws()
            => VerifyNotNullOrEmpty(Empty, ArgumentName, out var renderedMessage, NullOrEmptyStringFormat)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Valid_String_Does_Not_Throw()
            => VerifyNotNullOrEmpty(ThisIsATest, ArgumentName)
                .AssertEqual(ThisIsATest);
    }
}
