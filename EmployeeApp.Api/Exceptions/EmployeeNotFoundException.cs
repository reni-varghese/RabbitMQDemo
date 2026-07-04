namespace EmployeeApp.Api.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message) :base(message)
        {
            
        }
    }
}
