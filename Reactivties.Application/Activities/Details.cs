using MediatR;
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
    public class Details
    {

        public class Query : IRequest<Result<Activity>>
        {

            public Guid Id { get; set; }

        }


        public class Handler : IRequestHandler<Query, Result<Activity>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity= await _context.Activities.FindAsync(request.Id);

                return  Result<Activity>.Success(activity);
            }
        }

    }
}
