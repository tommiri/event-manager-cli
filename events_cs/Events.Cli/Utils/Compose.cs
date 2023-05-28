namespace Events.Utils;

/// <summary>
/// Utility class for composing functions
/// </summary>
public static class Compose
{
    /// <summary>
    /// Generic method for composing a function by short-circuiting logical AND
    /// </summary>
    /// <param name="previous">previous function</param>
    /// <param name="next">new function</param>
    /// <typeparam name="T">type of functions</typeparam>
    /// <returns>composed function if previous is not null, otherwise new function</returns>
    public static Func<T, bool> And<T>(Func<T, bool>? previous, Func<T, bool> next)
    {
        return previous == null ? next : x => previous(x) && next(x);
    }

    /// <summary>
    /// Generic method for composing a function by short-circuiting logical OR
    /// </summary>
    /// <param name="previous">previous function</param>
    /// <param name="next">new function</param>
    /// <typeparam name="T">type of functions</typeparam>
    /// <returns>composed function if previous is not null, otherwise new function</returns>
    public static Func<T, bool> Or<T>(Func<T, bool>? previous, Func<T, bool> next)
    {
        return previous == null ? next : x => previous(x) || next(x);
    }
}