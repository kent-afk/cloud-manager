namespace Identity.Domain.Events;

public record Loging_User_Event(Guid Id, string Email): IEvent
{
    public string Route => "user.login";
}