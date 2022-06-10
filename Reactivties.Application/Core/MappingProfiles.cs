using AutoMapper;
using Reactivities.Domain;
using Reactivties.Application.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactivties.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Activity, Activity>();


            CreateMap<Activity, ActivityDTO>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
                    .FirstOrDefault(x => x.IsHost).AppUser.UserName)); //Mapping from the root of Username with hostUsername. 

            CreateMap<ActivityAttendee, Profiles.Profile>()
               .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName)) //The helpful of this is to specify or select out of the list of them.
               .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
               .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));



        }
    }
}
