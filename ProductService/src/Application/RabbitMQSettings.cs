namespace Application;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public string ProductCreatedEventQueue { get; set; }
    public string ProductDeletedEventQueue { get; set; }

    public int RetryInMilliseconds { get; set; }
}
