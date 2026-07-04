namespace EmployeeApp.Api.Events
{
    public class EmployeeEvent
    {
        public string EventType { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
