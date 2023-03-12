namespace NEvo.Polling;

public class MessagePollerOptions
{
    public string? DefaultPollerType { get; set; }
    public IEnumerable<TopicPollerOptions> Topics { get; set; } = new List<TopicPollerOptions>();
}

public class TopicPollerOptions
{
    public string Name { get; set; }
    public int Threads { get; set; } = 1;
    public string? PollerType { get; set; }
}
