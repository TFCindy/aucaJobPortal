namespace AucaJobPortalDotNet.Models;
public class Permission
{
    public int Id{get;private set;}
    public string Name{get;set;}=string.Empty;
    public string? Description{get;set;}
    public string? Category{get;set;}
    public DateTime CreatedAt{get;private set;}
}