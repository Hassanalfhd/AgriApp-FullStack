using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _db;

        public AuditLogRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(AuditLogs log)
        {
            _db.AuditLogs.Add(log);

            await _db.SaveChangesAsync();
        }
    }
}
