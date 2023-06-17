using Domain.Interfaces;

namespace Services
{
public class InitializationService : IInitializationService
{
    public bool InitializationComplete { get; set; }
}
}