using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.Comparers
{
    public class PostcardComparer : IEqualityComparer<Postcard>
    {
        public bool Equals(Postcard x, Postcard y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Postcard obj)
        {
            return obj.Id;
        }
    }
}