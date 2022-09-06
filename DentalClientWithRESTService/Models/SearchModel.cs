using System.Collections.Generic;

namespace DentalClientWithRESTService.Models
{
    public class SearchModel
    {
        private static Dictionary<string, string> _viewToHttpMapper = new Dictionary<string, string>()
        {
            {"Imię", "Name" },
            {"Nazwisko", "Surname" },
            {"E-mail", "Email" },
        };

        public string SearchPhrase { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public string SortByNormalized => _viewToHttpMapper[SortBy.Substring(SortBy.IndexOf(" ") + 1)];
        public string SortDirectionNormalized => SortDirection.Substring(SortBy.IndexOf(" ") + 1);
    }
}
