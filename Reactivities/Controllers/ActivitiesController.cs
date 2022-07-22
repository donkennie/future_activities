using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;
using Reactivties.Application.Activities;
using Reactivties.Application.Core;

namespace Reactivities.API.Controllers
{
   

    public class ActivitiesController : BaseAPIController
    {
      
        [HttpGet]
        
        public async Task<IActionResult> GetActivities([FromQuery] PagingParams param)  
        {
            return HandlePagedResult( await Mediator.Send(new List.Query { Params = param}));
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


        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;

            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }


        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult (await Mediator.Send(new Delete.Command { Id = id }));
        }


        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }

    }
}
