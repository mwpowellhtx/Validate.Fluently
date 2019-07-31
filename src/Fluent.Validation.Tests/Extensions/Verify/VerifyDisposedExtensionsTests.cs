using System;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class VerifyDisposedExtensionsTests : VerifyExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;ObjectName&quot;
        /// </summary>
        private const string ObjectName = nameof(ObjectName);

        /// <summary>
        /// &quot;Cannot access a disposed object.&quot;
        /// </summary>
        private const string CannotAccessDisposedObject = "Cannot access a disposed object.";

        public VerifyDisposedExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <inheritdoc />
        private class D : IDisposableObservable
        {
            /// <summary>
            /// Gets a new Instance for Internal Use.
            /// </summary>
            internal static D Instance => new D();

            /// <summary>
            /// Private Constructor.
            /// </summary>
            private D()
            {
            }

            /// <inheritdoc />
            public bool IsDisposed { get; private set; }

            // ReSharper disable once UnusedParameter.Local
            /// <summary>
            /// Disposes the object.
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                // We would not normally conduct a Disposable pattern this way, but for these tests, this is acceptable.
                IsDisposed = true;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                if (IsDisposed)
                {
                    return;
                }

                Dispose(true);
            }
        }

        /// <summary>
        /// Provides a couple of Rendering channels to cope with the inconsistencies in the API.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="expectedMessage"></param>
        /// <returns></returns>
        private static string RenderDisposedMessage(string objectName = null, string expectedMessage = CannotAccessDisposedObject)
            => IsNullOrEmpty(objectName)
                ? $"{expectedMessage}"
                : $"{expectedMessage}\r\nObject name: '{objectName}'.";

        /// <summary>
        /// Provides a couple of Rendering channels to cope with the inconsistencies in the API.
        /// </summary>
        /// <param name="expectedMessage"></param>
        /// <returns></returns>
        private static string RenderDisposedMessageWithObjectType<T>(string expectedMessage = null)
            => $"{expectedMessage ?? CannotAccessDisposedObject}\r\nObject name: '{typeof(T).FullName}'.";

        private static Action VerifyDisposedDelegation(bool condition, string objectName, out string renderedMessage)
        {
            renderedMessage = RenderDisposedMessage(objectName);
            return () => condition.VerifyNotDisposed(objectName);
        }

        private static bool VerifyDisposed(bool condition, string objectName)
            => condition.VerifyNotDisposed(objectName);

        [Fact]
        public void Not_Disposed_Not_Disposed_Correct()
            => VerifyDisposedDelegation(false, ObjectName, out var renderedMessage)
                .AssertThrows<ObjectDisposedException>()
                .Verify(ex =>
                {
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Not_Disposed_Disposed_Correct() => VerifyDisposed(true, ObjectName).AssertTrue();

        private static Action VerifyValueNotDisposedConditionOnlyDelegation(object value, string objectName, FluentConditionCallback<object> condition, out string renderedMessage)
        {
            renderedMessage = RenderDisposedMessage(objectName);
            return () => value.VerifyValueNotDisposedConditionOnly(objectName, condition);
        }

        private static object VerifyValueNotDisposedConditionOnly(object value, string objectName, FluentConditionCallback<object> condition)
            => value.VerifyValueNotDisposedConditionOnly(objectName, condition);

        [Fact]
        public void Value_ConditionOnly_Not_Disposed_Not_Disposed_Correct()
            => VerifyValueNotDisposedConditionOnlyDelegation(NullRoot, ObjectName, _ => false, out var renderedMessage)
                .AssertThrows<ObjectDisposedException>()
                .Verify(ex =>
                {
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Value_ConditionOnly_Not_Disposed_Disposed_Correct() => VerifyValueNotDisposedConditionOnly(NullRoot, ObjectName, _ => true).AssertNull();

        private static Action VerifyValueNotDisposedDelegation(object value, FluentConditionCallback<object> condition, out string renderedMessage, string expectedMessage = null)
        {
            expectedMessage = (expectedMessage ?? CannotAccessDisposedObject).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderDisposedMessage();
            return () => value.VerifyValueNotDisposed(condition, () => expectedMessage);
        }

        private static object VerifyValueNotDisposed(object value, FluentConditionCallback<object> condition, string expectedMessage = null)
            => value.VerifyValueNotDisposed(condition, () => expectedMessage);

        [Fact]
        public void Value_Not_Disposed_Not_Disposed_Correct()
            => VerifyValueNotDisposedDelegation(NullRoot, _ => false, out var renderedMessage)
                .AssertThrows<ObjectDisposedException>()
                .Verify(ex =>
                {
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.AssertNotNull().Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Value_Not_Disposed_Disposed_Correct() => VerifyValueNotDisposed(NullRoot, _ => true).AssertNull();

        private static Action VerifyValueNotDisposedDelegation<T>(T value, FluentConditionCallback<T> condition, out string renderedMessage, string expectedMessage = null)
            where T : IDisposable
        {
            expectedMessage = (expectedMessage ?? CannotAccessDisposedObject).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderDisposedMessageWithObjectType<T>(expectedMessage);
            return () => value.VerifyValueNotDisposed(condition, () => expectedMessage);
        }

        private static T VerifyValueNotDisposed<T>(T value, FluentConditionCallback<T> condition)
            where T : IDisposable
            => value.VerifyValueNotDisposed(condition);

        [Fact]
        public void Disposable_Not_Disposed_Not_Disposed_Correct()
        {
            void Verify(D instance)
            {
                VerifyValueNotDisposedDelegation(instance.AssertNotNull()
                        , x => x.AssertNotNull().IsDisposed, out var renderedMessage)
                    .AssertThrows<ObjectDisposedException>()
                    .Verify(ex =>
                    {
                        renderedMessage.AssertNotNull().AssertNotEmpty();
                        ex.AssertNotNull().Message.AssertNotNull().AssertEqual(renderedMessage);
                    });
            }

            Verify(D.Instance);
        }

        [Fact]
        public void Disposable_Not_Disposed_Disposed_Correct()
        {
            void Verify(D instance)
            {
                instance.AssertNotNull().Dispose();
                VerifyValueNotDisposed(instance, x => x.AssertNotNull().IsDisposed)
                    .AssertNotNull().AssertSame(instance);
            }

            Verify(D.Instance);
        }

        private static Action VerifyObservableValueNotDisposedDelegation<T>(T value, out string renderedMessage, string expectedMessage = null)
            where T : IDisposableObservable
        {
            expectedMessage = (expectedMessage ?? CannotAccessDisposedObject).AssertNotNull().AssertNotEmpty();
            renderedMessage = RenderDisposedMessageWithObjectType<T>(expectedMessage);
            return () => value.VerifyObservableValueNotDisposed(() => expectedMessage);
        }

        private static T VerifyObservableValueNotDisposed<T>(T value, string expectedMessage = null)
            where T : IDisposableObservable
        {
            expectedMessage = (expectedMessage ?? CannotAccessDisposedObject).AssertNotNull().AssertNotEmpty();
            return value.VerifyObservableValueNotDisposed(() => expectedMessage);
        }

        [Fact]
        public void DisposableObservable_Not_Disposed_Not_Disposed_Correct()
        {
            void Verify(D instance)
            {
                VerifyObservableValueNotDisposed(instance, ObjectName)
                    .AssertNotNull().AssertSame(instance);
            }

            Verify(D.Instance);
        }

        [Fact]
        public void DisposableObservable_Not_Disposed_Disposed_Correct()
        {
            void Verify(D instance)
            {
                instance.AssertNotNull().Dispose();
                VerifyObservableValueNotDisposedDelegation(instance.AssertNotNull(), out var renderedMessage)
                    .AssertThrows<ObjectDisposedException>()
                    .Verify(ex =>
                    {
                        renderedMessage.AssertNotNull().AssertNotEmpty();
                        ex.AssertNotNull().Message.AssertNotNull().AssertEqual(renderedMessage);
                    });
            }

            Verify(D.Instance);
        }
    }
}
