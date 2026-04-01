using Agricultural_For_CV_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
    }
}
