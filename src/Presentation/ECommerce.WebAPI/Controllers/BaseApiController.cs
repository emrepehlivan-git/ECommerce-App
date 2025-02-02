using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected ILocalizationService LocalizationService => HttpContext.RequestServices.GetRequiredService<ILocalizationService>();
    protected L L => HttpContext.RequestServices.GetRequiredService<L>();
}
