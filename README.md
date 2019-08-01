# Validate Fluently - Fluent Validation

We add *Fluent* extension methods building on the [Validation](https://github.com/AArnott/Validation/) project.

[![NuGet package](https://img.shields.io/nuget/v/Validate.Fluently.svg)](https://nuget.org/packages/Validate.Fluently)
[![Validation NuGet package](https://img.shields.io/nuget/v/Validation.svg)](https://nuget.org/packages/Validation)

This project is available as the [*Validate.Fluently*][1] NuGet package.

## Fluent Philosophy

Couple of operational notes. We make assumptions about:

1. Names. Specify names such as Arguments and so forth at key moments.
2. Messages. We approach this issue using delegated callbacks. We want to avoid things such as string formatting and interpolation until the last possible moment.
3. Conditions. We approach this issue also using delegated callbacks, for the same reasons.
4. Values. Whenever possible, we provide the extension method anchor values, objects, etc, for the delegated conditions. When validation passes you can leverage *Fluent* style with the return value.

In general, the approach we took with this is simple, for you to be able to validate your bits early and often, then keep on composing your phrases, sentences, and the rest of your code, with as minimal impact as possible.

## Some Examples

We can validate some basic *Requirements*, which end up throwing [`System.ArgumentException`][2].

```C#
arg1.RequiresNotNull(nameof(arg1));
arg2.RequiresNotNullOrEmpty(nameof(arg2));
````

Fluent state validation via the *Verification* throws [`System.InvalidOperationException`][3]

```C#
condition.VerifyOperation(() => "some error occurred.");
```

Internal integrity checks occur via the *Assumptions* and throws a [Validation.InternalErrorException][4]. Testing this one can be tricky, however; we recommend simply looking for instances deriving from [`Exception`][5].

```C#
condition.AssumesTrue(() => "some error");
```

Warning signs that should not throw an [`Exception`][5] via the *Reporting* class.

```C#
condition.ReportIfNot(() => "some error");
```

[1]: https://nuget.org/packages/Validate.Fluently "Validate Fluently NuGet package"
[2]: https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception "System.ArgumentException"
[3]: https://docs.microsoft.com/en-us/dotnet/api/system.invalidoperationexception "System.InvalidOperationException"
[4]: https://github.com/AArnott/Validation/blob/master/src/Validation/Assumes.InternalErrorException.cs "Validation.Assumes.InternalErrorException"
[5]: https://docs.microsoft.com/en-us/dotnet/api/system.exception "System.Exception"
