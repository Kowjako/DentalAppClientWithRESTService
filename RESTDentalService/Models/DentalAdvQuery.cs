using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Models
{
    public enum SortType
    {
        Asc, 
        Desc
    }

    public class DentalAdvQuery
    {
        public string SortBy { get; set; }
        public SortType SortDirection { get; set; }
        public string SearchPhrase { get; set; }
    }
}
