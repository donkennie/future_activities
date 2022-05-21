using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;
using Reactivties.Application.Activities;

namespace Reactivities.API.Controllers
{
   

    public class ActivitiesController : BaseAPIController
    {
      
        [HttpGet]

        public async Task<IActionResult> GetActivities()
        {
            return HandleResult( await Mediator.Send(new List.Query()));
        }


        [HttpGet("{id}")] //activities/id

        public async Task<IActionResult> GetActivity(Guid id)
        {

           return HandleResult(await Mediator.Send(new Details.Query { Id = id }));

        }


        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity}));   
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;

            return Ok(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
        

    }
}
