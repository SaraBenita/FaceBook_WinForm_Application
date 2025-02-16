using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class SortByCreationTimeAscending : ISortPhotosStrategy
    {
        public List<Photo> Sort(Album i_Album)
        {
            return i_Album.Photos.AsEnumerable().OrderBy(photo => photo.CreatedTime).ToList();
        }
    }
}
