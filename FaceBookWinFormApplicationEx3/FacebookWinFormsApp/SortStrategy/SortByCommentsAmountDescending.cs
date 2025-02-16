using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class SortByCommentsAmountDescending : ISortPhotosStrategy
    {
       
        List<Photo> ISortPhotosStrategy.Sort(Album i_Album)
        {
            return i_Album.Photos.AsEnumerable().OrderBy(photo => photo.Comments.Count).ToList();
        }
    }
}
