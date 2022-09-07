namespace RESTDentalService.Models
{
    public class CreateOperationDTO
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Date { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
    }
}
