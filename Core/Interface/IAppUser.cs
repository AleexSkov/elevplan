using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using MongoDB.Bson;

namespace Core.Interface
{
    public interface IAppUser
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(ObjectId id);
        Task<AppUser?> GetByEmailAsync(string email);
        Task CreateAsync(AppUser user);
        Task<bool> EmailExistsAsync(string email);
        
        Task UpdateAsync(ObjectId id, AppUser updatedUser);

       
    }
}