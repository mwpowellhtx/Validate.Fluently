using System;
using System.Collections.Generic;
using System.Linq;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;

    public class AssumesCollectionExtensionTests : AssumesExtensionsTestFixtureBase
    {
        public AssumesCollectionExtensionTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private static void VerifyRangeDoesNotThrow<T, TRange>(TRange values, Action<TRange> action)
            where TRange : IEnumerable<T> => action.Invoke(values);

        private static void VerifyRangeThrows<T, TRange, TException>(TRange values, Action<TRange> action)
            where TRange : IEnumerable<T>
            where TException : Exception
        {
            Action wrapper = () => action.Invoke(values);

            wrapper.AssertThrowsAny<TException>()
                .Verify(ex => ex.AssertNotNull().Message.AssertNotNull().AssertEqual(DefaultMessage));
        }

        private static IEnumerable<int> NullRange => null;

        private static IEnumerable<int> EmptyRange => GetRange<int>();

        private static IEnumerable<int> Range => GetRange(1, 2, 3);

        private static ICollection<int> NullCollection => null;

        private static ICollection<int> EmptyCollection => EmptyRange.ToList();

        private static ICollection<int> Collection => Range.ToList();

        // TODO: TBD: could potentially look at abstracting out bits of the AssumesString/Collection tests into something a bit more generic...
        [Fact]
        public void Range_Assumes_Not_Null_Or_Empty_Does_Not_Throw()
            => VerifyRangeDoesNotThrow<int, IEnumerable<int>>(Range, x => x.AssumesNotNullOrEmpty().AssertSame(x));

        [Fact]
        public void Empty_Range_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyRangeThrows<int, IEnumerable<int>, Exception>(EmptyRange, x => x.AssumesNotNullOrEmpty());

        [Fact]
        public void Null_Range_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyRangeThrows<int, IEnumerable<int>, Exception>(NullRange, x => x.AssumesNotNullOrEmpty());

        [Fact]
        public void Collection_Assumes_Not_Null_Or_Empty_Does_Not_Throw()
            => VerifyRangeDoesNotThrow<int, ICollection<int>>(Collection, x => x.AssumesNotNullOrEmpty().AssertSame(x));

        [Fact]
        public void Empty_Collection_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyRangeThrows<int, ICollection<int>, Exception>(EmptyCollection, x => x.AssumesNotNullOrEmpty());

        [Fact]
        public void Null_Collection_Assumes_Not_Null_Or_Empty_Throws()
            => VerifyRangeThrows<int, ICollection<int>, Exception>(NullCollection, x => x.AssumesNotNullOrEmpty());
    }
}
