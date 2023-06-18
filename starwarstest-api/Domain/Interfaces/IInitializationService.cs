namespace Domain.Interfaces{
public interface IInitializationService
{
    //created bool to tell if the process is complete or not. injected into another service.
    bool InitializationComplete { get; set; }
}
}