using FacebookWrapper.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class PostCollection:IEnumerable<Post>
    {
        private readonly FacebookObjectCollection<Post> r_Posts;
        public PostCollection(FacebookObjectCollection<Post> i_posts)
        {
            r_Posts = i_posts;
        }
        public IEnumerator<Post> GetEnumerator()
        {
            return new PostIterator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PostIterator(this);
        }
        private class PostIterator : IEnumerator<Post>
        {
            private readonly PostCollection m_PostCollection;
            private int m_CurrentIdx = -1;
            private int m_Count = -1;
            public PostIterator(PostCollection i_PostCollection)
            {
                m_PostCollection = i_PostCollection;
                m_Count = m_PostCollection.r_Posts.Count;
            }
            public Post Current
            {
                get
                {
                    if(m_Count < m_CurrentIdx || m_CurrentIdx == -1)
                    {
                        throw new IndexOutOfRangeException("index out of range");
                    }
                    return m_PostCollection.r_Posts[m_CurrentIdx];
                }
            }
            object IEnumerator.Current => Current;
            public void Dispose()
            {
                Reset();
            }
            public bool MoveNext()
            {
                if (m_Count != m_PostCollection.r_Posts.Count)
                {
                    throw new Exception("Collection can not be changed during iteration!");
                }

                if (m_CurrentIdx >= m_Count)
                {
                    throw new Exception("Already reached the end of the collection");
                }

                return ++m_CurrentIdx < m_PostCollection.r_Posts.Count;
            }
            public void Reset()
            {
                m_CurrentIdx = -1;
            }
        }
    }
}
