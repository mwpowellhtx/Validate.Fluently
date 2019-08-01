namespace Validation
{
    /// <summary>
    /// Returns the <see cref="bool"/> Condition given <paramref name="obj"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate bool FluentConditionCallback<in T>(T obj);
}
