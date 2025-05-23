using Core.Models;
namespace ComwellElevplan.Service;

public interface IElevplan
{
    Task<List<Elevplan>> GetAllAsync();
}