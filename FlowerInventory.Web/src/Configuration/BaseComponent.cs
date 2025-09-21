namespace FlowerInventory.Web.Configuration;

/* Abstract class for a global logger */
public abstract class BaseComponent<T>
{
    protected static readonly ILogger Logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<T>();
}