namespace CreationsPlatformWebApplication.Models;

public class StatusMessageModel(string message, bool isError)
{
    public StatusMessageModel() : this(null)
    {
        
    }

    public StatusMessageModel(string message) : this(message, false)
    {
    }

    public string Message { get; set; } = message;
    public bool IsError { get; set; } = isError;
}