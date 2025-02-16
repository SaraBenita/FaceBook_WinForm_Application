using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal interface ISortPhotosStrategy
    {
        List<Photo> Sort(Album i_Album);
    }
}
