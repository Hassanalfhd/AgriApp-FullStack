using System;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.LogsDtos;

namespace Agricultural_For_CV_BLL.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditLogDto dto);
    }
}
