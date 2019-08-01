using System;
using System.Collections.Generic;
using System.Linq;

namespace Validation
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class RequiresDefinedExtensionsTests : RequiresExtensionsTestFixtureBase
    {
        /// <summary>
        /// &quot;&apos;{0}&apos; must be set to a value defined by the enum &apos;{1}&apos;.&quot;
        /// </summary>
        private const string EnumExpectedMessageFormat = "'{0}' must be set to a value defined by the enum '{1}'.";

        public RequiresDefinedExtensionsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// 1
        /// </summary>
        private const int AppleOrdinalValue = 1;

        /// <summary>
        /// 2
        /// </summary>
        /// <remarks>Which is also implied by the <see cref="Fruit"/> declaration.</remarks>
        private const int OrangeOrdinalValue = 2;

        // TODO: TBD: do we need to explore Flags based Enumerated values?
        /// <summary>
        /// Fruit verifies for Ordinal Enumerated values using <see cref="AppleOrdinalValue"/>
        /// as the <see cref="Apples"/> Ordinal starting value.
        /// </summary>
        /// <see cref="AppleOrdinalValue"/>
        private enum Fruit
        {
            Apples = AppleOrdinalValue,
            Oranges
        }

        /// <summary>
        /// Gets the <see cref="Fruit"/> Ordinal Values.
        /// </summary>
        private static IEnumerable<int> FruitOrdinalValues
        {
            get
            {
                int GetValue(Fruit value) => (int) value;

                foreach (var x in GetRange(Fruit.Apples, Fruit.Oranges))
                {
                    yield return GetValue(x);
                }
            }
        }

        // TODO: TBD: may refactor this callback...
        private delegate T EnumConversionCallback<out T>(int value)
#if NETSTANDARD1_0 || PROFILE259 // In the event that we decide to ...
            where T : struct, IComparable, IFormattable // i.e. Enum
#else // IConvertible missing from netstandard1.2
            where T : struct, IComparable, IFormattable, IConvertible // i.e. Enum
#endif
        ;

        private static void VerifyValueEnumRequirementCorrect<T>(int expectedValue, EnumConversionCallback<T> conversion, string _)
#if NETSTANDARD1_0 || PROFILE259 // Ditto...
            where T : struct, IComparable, IFormattable // i.e. Enum
#else // IConvertible missing from netstandard1.2
            where T : struct, IComparable, IFormattable, IConvertible // i.e. Enum
#endif
        {
            expectedValue = expectedValue.AssertTrue(x => FruitOrdinalValues.Any(y => y == x));
            conversion.AssertNotNull().Invoke(expectedValue).RequiresDefined(_.AssertNotNull().AssertNotEmpty());
        }

        /// <summary>
        /// Renders the Enum Expected Message given <paramref name="argumentName"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentName"></param>
        /// <returns></returns>
        /// <see cref="EnumExpectedMessageFormat"/>
        private static string RenderEnumExpectedMessage<T>(string argumentName)
            => RenderArgumentMessage($"{Format(EnumExpectedMessageFormat, argumentName, typeof(T).FullName)}"
                , argumentName);

        private static Action VerifyValueEnumRequirementInvalid<T>(int invalidValue,
            EnumConversionCallback<T> conversion, string argumentName)
#if NETSTANDARD1_0 || PROFILE259 // Ditto...
            where T : struct, IComparable, IFormattable // i.e. Enum
#else // IConvertible missing from netstandard1.2
            where T : struct, IComparable, IFormattable, IConvertible // i.e. Enum
#endif
        {
            invalidValue = invalidValue.AssertFalse(x => FruitOrdinalValues.Any(y => y == x));
            return () => conversion.Invoke(invalidValue).RequiresDefined(argumentName);
        }

        [Theory
         , InlineData(AppleOrdinalValue)
         , InlineData(OrangeOrdinalValue)
        ]
        public void Enum_Ordinal_Value_Is_Defined(int expectedValue)
            => VerifyValueEnumRequirementCorrect(expectedValue, x => (Fruit) x, ArgumentName);

        /// <summary>
        /// 0
        /// </summary>
        private const int FirstInvalidOrdinalValue = 0;

        /// <summary>
        /// 3
        /// </summary>
        private const int SecondInvalidOrdinalValue = 3;

        [Theory
         , InlineData(FirstInvalidOrdinalValue)
         , InlineData(SecondInvalidOrdinalValue)
        ]
        public void Enum_Ordinal_Value_Is_Not_Defined(int invalidValue)
            => VerifyValueEnumRequirementInvalid(invalidValue, x => (Fruit) x, ArgumentName)
                .AssertThrows<ArgumentException>().Verify(ex =>
                {
                    ex.AssertNotNull().ParamName.AssertNotNull().AssertEqual(ArgumentName);
                    var expectedMessage = RenderEnumExpectedMessage<Fruit>(ArgumentName).AssertNotNull().AssertNotEmpty();
                    ex.Message.AssertNotNull().AssertEqual(expectedMessage);
                });
    }
}
