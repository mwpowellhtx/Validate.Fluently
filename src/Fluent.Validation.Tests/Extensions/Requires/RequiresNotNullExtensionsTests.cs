using System;
using System.Threading.Tasks;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class RequiresNotNullExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;Value cannot be null.&quot;
        /// </summary>
        private const string NotNullMessage = "Value cannot be null.";

        public RequiresNotNullExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static Action VerifyRequirement(IntPtr ptr, string argumentName, out string renderedMessage)
        {
            renderedMessage = RenderArgumentMessage(NotNullMessage, argumentName);
            return () => ptr.RequiresNotNull(argumentName);
        }

        [Fact]
        public void Null_IntPtr_Throws()
        {
            VerifyRequirement(IntPtr.Zero, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                    {
                        ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                        ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                    });
        }

        /// <summary>
        /// Does not matter what <paramref name="value"/> is, although we would
        /// not necessarily want to go dereferencing any of these addresses.
        /// </summary>
        /// <param name="value"></param>
        [Theory, InlineData(0x12121212L)]
        public void Not_Null_IntPtr_Does_Not_Throw(long value)
            => VerifyRequirement(new IntPtr(value), ArgumentName, out _).Invoke();

        private static Action VerifyClassRequirement<T>(T _, string argumentName, out string renderedMessage)
            where T : class
        {
            renderedMessage = RenderArgumentMessage(NotNullMessage, argumentName);
            return () => _.RequiresNotNull(argumentName);
        }

        private static T VerifyClassRequirement<T>(T _, string argumentName)
            where T : class
            => _.RequiresNotNull(argumentName);

        [Fact]
        public void Null_Class_Throws()
            => VerifyClassRequirement<Exception>(null, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });

        [Fact]
        public void Not_Null_Class_Throws()
        {
            void Verify<T>(T value)
                where T : class
                => VerifyClassRequirement(value, ArgumentName).AssertNotNull().AssertSame(value);

            Verify(InnerException);
        }

        /// <summary>
        /// Gets a Basic Task for testing purposes. We should be able to verify for both
        /// <see cref="Task{TResult}"/> as well as <see cref="Task"/> oriented Not Null
        /// Requirements.
        /// </summary>
        private static Task<int> IntegerTask => new Task<int>(() => default);

        private static Task<int> NullTask => null;

        private static Action VerifyTaskRequirement<T>(Task<T> task, string argumentName, out string renderedMessage)
        {
            renderedMessage = RenderArgumentMessage(NotNullMessage, argumentName);
            return () => task.RequiresNotNull(argumentName);
        }

        private static Action VerifyTaskRequirement(Task task, string argumentName, out string renderedMessage)
        {
            renderedMessage = RenderArgumentMessage(NotNullMessage, argumentName);
            return () => task.RequiresNotNull(argumentName);
        }

        private static Task<T> VerifyTaskRequirement<T>(Task<T> task, string argumentName)
            => task.RequiresNotNull(argumentName);

        private static Task VerifyTaskRequirement(Task task, string argumentName)
            => task.RequiresNotNull(argumentName);

        [Fact]
        public void Not_Null_Task_Does_Not_Throw()
        {
            void Verify(Task task) => VerifyTaskRequirement(task, ArgumentName).AssertNotNull().AssertSame(task);
            Verify(IntegerTask);
        }

        [Fact]
        public void Null_Task_Does_Not_Throw()
        {
            VerifyTaskRequirement((Task) NullTask, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });
        }

        [Fact]
        public void Not_Null_Integer_Task_Does_Not_Throw()
        {
            void Verify<T>(Task<T> task) => VerifyTaskRequirement(task, ArgumentName).AssertNotNull().AssertSame(task);
            Verify(IntegerTask);
        }

        [Fact]
        public void Null_Integer_Task_Does_Not_Throw()
        {
            VerifyTaskRequirement<int>(NullTask, ArgumentName, out var renderedMessage)
                .AssertThrows<ArgumentNullException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertEqual(ArgumentName);
                    renderedMessage.AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(renderedMessage);
                });
        }
    }
}
