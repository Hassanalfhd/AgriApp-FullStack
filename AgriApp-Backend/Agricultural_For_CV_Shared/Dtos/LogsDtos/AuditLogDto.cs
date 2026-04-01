using System;

namespace Agricultural_For_CV_Shared.Dtos.LogsDtos
{
    public class AuditLogDto
    {
        public string action { get; set; }
        public string ResourceType { get; set; }
        public int? ResourceId { get; set; }
        public object? Before { get; set; }
        public object? After { get; set; }
        public int? ActorId { get; set; }
        public string? ActorType { get; set; } = "user";
        public object? Metadata { get; set; }
    }
}
