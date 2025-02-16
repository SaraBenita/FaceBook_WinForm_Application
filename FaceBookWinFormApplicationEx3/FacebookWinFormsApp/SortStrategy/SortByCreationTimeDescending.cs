using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class SortByCreationTimeDescending : ISortPhotosStrategy
    {
      
        List<Photo> ISortPhotosStrategy.Sort(Album i_Album)
        {
            return i_Album.Photos.AsEnumerable().OrderByDescending(photo => photo.CreatedTime).ToList();
        }
    }
}
