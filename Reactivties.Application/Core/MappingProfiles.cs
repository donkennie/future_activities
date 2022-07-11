using AutoMapper;
using Reactivities.Domain;
using Reactivties.Application.Activities;
using Reactivties.Application.Comments;
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
            string currentUsername = null; 

            CreateMap<Activity, Activity>();


            CreateMap<Activity, ActivityDTO>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
                    .FirstOrDefault(x => x.IsHost).AppUser.UserName)); //Mapping from the root of Username with hostUsername. 

            CreateMap<ActivityAttendee, AttendeeDTO>()
               .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName)) //The helpful of this is to specify or select out of the list of them.
               .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
               .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
               .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
               .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
               .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
               .ForMember(d => d.Following,
                    o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername)));

            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers))
                .ForMember(d => d.Following,
                    o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)));

            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
            



        }
    }
}
