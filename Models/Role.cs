namespace AucaJobPortalDotNet.Models;
public class Role
{
    public int Id{get;private set;}
    public string Name{get;set;}=string.Empty;
    public string Description{get;set;}=string.Empty;
    public bool IsSystemRole{get;set;}=false;//when u create a new role,it is user created not system/built_in
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
}