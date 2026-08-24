using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface IAuditService
{
    Task LogActionAsync(int? userId, string action,
        string entityType,
        int? entityId = null,
        string? oldValue = null,
        string? newValue = null,
        string? ipAddress = null,
        string? userAgent = null,
        bool isSuccessful = true,
        string severity = "info");

    Task<List<AuditLogDTO>> GetAuditLogsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? action = null,
        string? entityType = null,
        bool? isSuccessful = null,
        int? limit = null);

    Task<AuditLogDTO> GetAuditLogByIdAsync(int id);

    Task<List<string>> GetActionTypesAsync();  

}