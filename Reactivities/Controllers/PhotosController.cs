using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivties.Application.Photos;

namespace Reactivities.API.Controllers
{
    public class PhotosController : BaseAPIController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

    }
}
