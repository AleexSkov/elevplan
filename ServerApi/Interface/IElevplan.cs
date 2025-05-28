using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace ServerApi.Interface;

public interface IElevplan
{
    Task<List<Elevplan>> GetAllAsync();
    Task<Elevplan?> GetTemplateAsync();
    Task CreateAsync(Elevplan plan);
}