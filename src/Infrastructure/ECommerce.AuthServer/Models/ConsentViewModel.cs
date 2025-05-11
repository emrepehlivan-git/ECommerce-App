namespace ECommerce.AuthServer.Models;

public sealed class ConsentViewModel
{
    public required string ApplicationName { get; set; }
    public required string Scope { get; set; }
}