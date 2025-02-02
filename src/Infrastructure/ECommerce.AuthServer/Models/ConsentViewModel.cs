using System.ComponentModel.DataAnnotations;

namespace ECommerce.AuthServer.Models;

public sealed class ConsentViewModel
{
    [Display(Name = "Application")]
    public string ApplicationName { get; set; } = null!;

    [Display(Name = "Scope")]
    public string Scope { get; set; } = null!;
}