public static class StateArgs
{
    public static T Get<T>(object[] args, int index)
    {
        if (args == null || args.Length <= index)
        {
            throw new System.ArgumentOutOfRangeException(nameof(args), "Args array is null or index is out of range.");
        }
        if (args[index] is T value)
        {
            return value;
        }

        throw new System.InvalidCastException($"Cannot cast argument at index {index} to type {typeof(T)}.");
    }

    public static bool TryGet<T>(object[] args, int index, out T value)
    {
        value = default;
        if (args == null || args.Length <= index)
        {
            return false;
        }

        if (args[index] is T castedValue)
        {
            value = castedValue;
            return true;
        }

        return false;
    }
}