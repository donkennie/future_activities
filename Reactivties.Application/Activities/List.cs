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
            public PagingParams Params { get; set; }
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
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername()})
                    .AsQueryable();



                return Result<PagedList<ActivityDTO>>.Success(
                     await PagedList<ActivityDTO>.CreateAsync(query, request.Params.PageNumber,
                        request.Params.PageSize));
            }
        }
    }
}
