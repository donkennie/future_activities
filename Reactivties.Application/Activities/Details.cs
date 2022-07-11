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
    public class Details
    {

        public class Query : IRequest<Result<ActivityDTO>>
        {

            public Guid Id { get; set; }
            public string Username { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result<ActivityDTO>>
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

            public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity= await _context.Activities
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername()}) // To remove the include function and make it done with the automapper.
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return  Result<ActivityDTO>.Success(activity);
            }
        }

    }
}
