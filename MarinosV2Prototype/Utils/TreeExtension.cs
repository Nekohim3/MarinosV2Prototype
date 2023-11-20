namespace MarinosV2Prototype.Utils;

public static class TreeExtension
{
    public static IEnumerable<TSource> FromChain<TSource>(
        this TSource           source,
        Func<TSource, TSource> nextItem,
        Func<TSource?, bool>   canContinue)
    {
        for (var current = source; canContinue(current); current = nextItem(current))
        {
            yield return current;
        }
    }

    public static IEnumerable<TSource> FromChain<TSource>(
        this TSource           source,
        Func<TSource, TSource> nextItem)
        where TSource : class
    {
        return FromChain(source, nextItem, s => s != null);
    }
}