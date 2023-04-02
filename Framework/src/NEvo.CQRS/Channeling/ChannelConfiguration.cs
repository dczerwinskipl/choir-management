namespace NEvo.CQRS.Channeling;

public class ChannelConfiguration
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class ChannelsConfiguration : Dictionary<string, ChannelConfiguration>
{

}