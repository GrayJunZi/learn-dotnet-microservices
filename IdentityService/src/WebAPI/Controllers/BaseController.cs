using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
public class BaseController<T> : ControllerBase
{
    private ISender _sender = null;

    public ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
}
