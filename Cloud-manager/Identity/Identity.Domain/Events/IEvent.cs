namespace Identity.Domain.Events;

public interface IEvent
{
    string Route { get; }
}