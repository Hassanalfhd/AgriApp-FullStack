using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLogs log);
    }
}
