using System;
using System.ComponentModel.DataAnnotations;

namespace LexEnaTrs.Web
{
    
        public class MyRegularExpressionAttribute : RegularExpressionAttribute
        {
            public MyRegularExpressionAttribute(string pattern) : base(pattern) { }

            protected new int MatchTimeoutInMilliseconds { get; set; }
        }
   
}