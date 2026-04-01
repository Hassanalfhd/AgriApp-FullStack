using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agricultural_For_CV_DAL.Entities
{

    [Table("AuditLogs")]
    public class AuditLogs
    {
        public long Id { get; set; }
        public int? ActorId { get; set; }
        public string? ActorType { get; set; }     // user, system, service
        public string? Action { get; set; }        // create, update, delete, login...
        public string? ResourceType { get; set; }  // product, order, user...
        public int? ResourceId { get; set; }      // resource unique identifier
        public string? Before { get; set; }        // JSON string (old data)
        public string? After { get; set; }         // JSON string (new data)
        public string? Metadata { get; set; }      // JSON string (ip, useragent, etc.)
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
