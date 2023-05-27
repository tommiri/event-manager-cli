namespace Events.Utils;

public static class Compose
{
    public static Func<T, bool> And<T>(Func<T, bool>? previous, Func<T, bool> next)
    {
        return previous == null ? next : x => previous(x) && next(x);
    }

    public static Func<T, bool> Or<T>(Func<T, bool>? previous, Func<T, bool> next)
    {
        return previous == null ? next : x => previous(x) || next(x);
    }
}