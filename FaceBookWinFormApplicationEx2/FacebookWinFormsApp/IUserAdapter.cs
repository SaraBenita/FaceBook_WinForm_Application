using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal interface IUserAdapter
    {
        FacebookObjectCollection<Group> GroupsLoded { get; }
        FacebookObjectCollection<Post> PostsLoded { get; }
        FacebookObjectCollection<Album> AlbumsLoded { get; }
        FacebookObjectCollection<Page> PagesLoded { get; }
        string UserName { get; }
        string ProfilePictureUrl { get; }
        string Email { get; }
        string Birthday { get; }
        string Gender { get; }
    }
}
