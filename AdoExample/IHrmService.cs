namespace AdoExample
{
    internal interface IHrmService
    {
        public void ListEmployees();
        public void GetEmployee(int id);
        public void CreateEmployee(string firstName, string lastName, string email, string phoneNumber, DateTime? hireDate, int jobId, double salary, int managerId, int departmentId);        
        public void UpdateEmployee(int id, string? firstName, string? lastName, string? email, string phoneNumber, int? jobId, double? salary, int? managerId, int? departmentId);
        public void DeleteEmployee(int id);
    }
}
