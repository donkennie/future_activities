using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;
using Reactivties.Application.Core;
using Reactivties.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactivties.Application.Activities
{
    public class List
    {

        public class Query : IRequest<Result<PagedList<ActivityDTO>>>
        {
            public ActivityParams Params { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDTO>>>
        {

            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }


            public async Task<Result<PagedList<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query =  _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername()})
                    .AsQueryable();

                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
                }

                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
                }

                return Result<PagedList<ActivityDTO>>.Success(
                     await PagedList<ActivityDTO>.CreateAsync(query, request.Params.PageNumber,
                        request.Params.PageSize));

                
            }
        }
    }
}
