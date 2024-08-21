namespace CreationsPlatformWebApplication.Models;

public class StatusMessageModel
{
    public StatusMessageModel() : this(null)
    {
        
    }

    public StatusMessageModel(string message) : this(message, false)
    {
    }

    public StatusMessageModel(string message, bool isError)
    {
        Message = message;
        IsError = isError;
    }
    public string Message { get; set; }
    public bool IsError { get; set; }
}