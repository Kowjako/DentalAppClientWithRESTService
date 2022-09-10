namespace DentalClientWithRESTService.Models
{
    public class OperationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Doctor { get; set; }
        public decimal Cost { get; set; }
        public string Term { get; set; }
    }
}
