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
            {"Numer unikalny", "UniqueNumber" },
            {"Adres", "Location" },
            {"Nazwa", "Name" },
        };

        public string SearchPhrase { get; set; } = string.Empty;
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public string SortByNormalized => SortBy == null ? string.Empty
                                                         : _viewToHttpMapper[SortBy.Substring(SortBy.IndexOf(" ") + 1)];
        public string SortDirectionNormalized => SortDirection == null ? string.Empty
                                                                       : SortDirection.Substring(SortBy.IndexOf(" ") + 1);
    }
}
