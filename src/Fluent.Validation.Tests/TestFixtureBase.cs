using System;
using System.Collections.Generic;

namespace Validation
{
    using Xunit.Abstractions;

    public abstract class TestFixtureBase : IDisposable
    {
        protected ITestOutputHelper OutputHelper { get; }

        protected TestFixtureBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> Range corresponding to the
        /// <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Disposes the Test Fixture.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets whether the Test Fixture IsDisposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
