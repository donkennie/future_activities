using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;
using Reactivties.Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactivties.Application.Activities
{
    public class List
    {


        public class Query : IRequest<Result<List<Activity>>>
        {

        }


        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {

            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }


            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Activity>>.Success( await _context.Activities.ToListAsync(cancellationToken));
            }
        }
    }
}
