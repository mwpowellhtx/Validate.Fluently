using System;

namespace Validation
{
    using Xunit.Abstractions;

    public abstract class ExtensionsTestFixtureBase : TestFixtureBase
    {
        /// <summary>
        /// Gets a default <see cref="object"/> instance for Root purposes.
        /// </summary>
        protected object Root { get; } = new object();

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        /// <summary>
        /// Gets a Null <see cref="object"/> Root for test purposes.
        /// </summary>
        protected object NullRoot { get; }

        /// <summary>
        /// 42
        /// </summary>
        protected const int LifeTheUniverseAndEverything = 42;

        /// <summary>
        /// 24
        /// </summary>
        protected const int TwentyFour = 24;

        /// <summary>
        /// false
        /// </summary>
        protected const bool False = false;

        /// <summary>
        /// true
        /// </summary>
        protected const bool True = true;

        /// <summary>
        /// &quot;An internal error occurred. Please contact customer support.&quot;
        /// </summary>
        protected const string DefaultMessage = "An internal error occurred. Please contact customer support.";

        /// <summary>
        /// &quot;Cannot find an instance of the System.Exception service.&quot;
        /// </summary>
        protected const string PresentDefaultMessage = "Cannot find an instance of the System.Exception service.";

        /// <summary>
        /// &quot;This is a test&quot;, original, right.
        /// </summary>
        protected const string ThisIsATest = "This is a test";

        /// <summary>
        /// Gets an InnerException instance for internal use.
        /// </summary>
        protected static Exception InnerException => new Exception();

        protected ExtensionsTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
