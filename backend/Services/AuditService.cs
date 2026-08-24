using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class AuditService : IAuditService
{
    private readonly AucaDbContext _context;

    public AuditService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task LogActionAsync(
        int? userId,
        string action,
        string entityType,
        int? entityId = null,
        string? oldValue = null,
        string? newValue = null,
        string? ipAddress = null,
        string? userAgent = null,
        bool isSuccessful = true,
        string severity = "info")
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            OldValue = oldValue,
            NewValue = newValue,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsSuccessful = isSuccessful,
            Severity = severity
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLogDTO>> GetAuditLogsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? action = null,
        string? entityType = null,
        bool? isSuccessful = null,
        int? limit = null)
    {
        var query = _context.AuditLogs
            .Include(a => a.User)
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(a => a.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.CreatedAt <= toDate.Value);

        if (!string.IsNullOrEmpty(action))
            query = query.Where(a => a.Action == action);

        if (!string.IsNullOrEmpty(entityType))
            query = query.Where(a => a.EntityType == entityType);

        if (isSuccessful.HasValue)
            query = query.Where(a => a.IsSuccessful == isSuccessful.Value);

        if (limit.HasValue && limit.Value > 0)
            query = query.Take(limit.Value);

        var logs = await query
            .Select(a => new AuditLogDTO
            {
                Id = a.Id,
                UserId = a.UserId,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                OldValue = a.OldValue,
                NewValue = a.NewValue,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                CreatedAt = a.CreatedAt,
                IsSuccessful = a.IsSuccessful,
                Severity = a.Severity
            })
            .ToListAsync();

        return logs;
    }

    public async Task<AuditLogDTO> GetAuditLogByIdAsync(int id)
    {
        var log = await _context.AuditLogs
            .Include(a => a.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (log == null)
        {
            throw new Exception("Audit log entry not found");
        }

        return new AuditLogDTO
        {
            Id = log.Id,
            UserId = log.UserId,
            Action = log.Action,
            EntityType = log.EntityType,
            EntityId = log.EntityId,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            IpAddress = log.IpAddress,
            UserAgent = log.UserAgent,
            CreatedAt = log.CreatedAt,
            IsSuccessful = log.IsSuccessful,
            Severity = log.Severity
        };
    }

    public async Task<List<string>> GetActionTypesAsync()
{
    var actions = await _context.AuditLogs
        .Select(a => a.Action)
        .Distinct()
        .OrderBy(a => a)
        .ToListAsync();

    return actions;
}
}