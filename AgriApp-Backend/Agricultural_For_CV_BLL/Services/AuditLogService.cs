using Agricultural_For_CV_BLL.Interfaces;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.LogsDtos;
using Agricultural_For_CV_Shared.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Agricultural_For_CV_BLL.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;

        public AuditLogService(IAuditLogRepository repo)
        {
            _repo = repo;
        }

        public async Task LogAsync(AuditLogDto dto)
        {
            var log = new AuditLogs
            {
                ActorId = dto.ActorId,
                ActorType = dto.ActorType,
                Action = dto.action,
                ResourceType = dto.ResourceType,
                ResourceId = dto.ResourceId,
                Before = dto.Before != null ? JsonSerializer.Serialize(dto.Before) : null,
                After = dto.After != null ? JsonSerializer.Serialize(dto.After) : null,
                Metadata = dto.Metadata != null ? JsonSerializer.Serialize(dto.Metadata) : null,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _repo.AddAsync(log);
        }
    }
}
