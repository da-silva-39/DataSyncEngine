namespace Views.Components;

public sealed record NotificationItem(DateTime At, string Message, ToastKind Kind);

public static class NotificationCenter
{
    public const int MaxItems = 100;

    private static readonly List<NotificationItem> _items = new();

    public static event Action? Changed;

    public static IReadOnlyList<NotificationItem> Items => _items;

    public static void Push(string message, ToastKind kind = ToastKind.Info)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        if (_items.Count > 0
            && _items[0].Message == message
            && (DateTime.Now - _items[0].At).TotalSeconds < 2)
        {
            return;
        }
        _items.Insert(0, new NotificationItem(DateTime.Now, message, kind));
        if (_items.Count > MaxItems) _items.RemoveRange(MaxItems, _items.Count - MaxItems);
        Changed?.Invoke();
    }

    public static void Clear()
    {
        _items.Clear();
        Changed?.Invoke();
    }
}
