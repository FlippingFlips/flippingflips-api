using System;
using System.ComponentModel.DataAnnotations;

namespace FF.Shared.Attributes
{
    public class YearRangeAttribute : RangeAttribute
    {
        public YearRangeAttribute()
          : base(typeof(int), 1910.ToString(), DateTime.Now.Year.ToString() ){ }
    }
}
