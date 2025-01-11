using System.Text;
using Microsoft.Data.SqlClient;

namespace AdoExample
{
    public class HrmService(SqlConnection conn, SqlTransaction? trans) : IHrmService
    {
        private readonly SqlConnection Conn = conn;
        private SqlTransaction? Trans = trans;

        public void SetTransaction(SqlTransaction trans)
        {
            this.Trans = trans;
        }

        public void CreateEmployee(string firstName, string lastName, string email, string phoneNumber, DateTime? hireDate, int jobId, double salary, int managerId, int departmentId)
        {
            var cmd = new SqlCommand(@"
                    INSERT INTO employees(
                        first_name, 
                        last_name, 
                        email, 
                        phone_number, 
                        job_id,                         
                        hire_date,
                        salary, 
                        department_id, 
                        manager_id
                    ) VALUES (
                        @first_name, 
                        @last_name, 
                        @email, 
                        @phone_number, 
                        @job_id,           
                        @hire_date,
                        @salary,
                        @department_id, 
                        @manager_id
                    );", this.Conn, this.Trans);

            cmd.Parameters.Add(new SqlParameter("first_name", System.Data.SqlDbType.VarChar, 20)).Value = firstName;
            cmd.Parameters.Add(new SqlParameter("last_name", System.Data.SqlDbType.VarChar, 25)).Value = lastName;
            cmd.Parameters.Add(new SqlParameter("email", System.Data.SqlDbType.VarChar, 100)).Value = email;
            cmd.Parameters.Add(new SqlParameter("phone_number", System.Data.SqlDbType.VarChar, 20)).Value = phoneNumber;
            cmd.Parameters.Add(new SqlParameter("job_id", System.Data.SqlDbType.Int)).Value = jobId;
            cmd.Parameters.Add(new SqlParameter("hire_date", System.Data.SqlDbType.Date)).Value = hireDate ?? DateTime.Now; ;
            cmd.Parameters.Add(new SqlParameter("salary", System.Data.SqlDbType.Int)).Value = salary;
            cmd.Parameters.Add(new SqlParameter("department_id", System.Data.SqlDbType.Int)).Value = departmentId;
            cmd.Parameters.Add(new SqlParameter("manager_id", System.Data.SqlDbType.Int)).Value = managerId;

            cmd.ExecuteNonQuery();
        }

        public void DeleteEmployee(int id)
        {
            var cmd = new SqlCommand("DELETE FROM employees WHERE employee_id = @employee_id", this.Conn, this.Trans);
            cmd.Parameters.Add(new SqlParameter("employee_id", System.Data.SqlDbType.Int)).Value = id;
            
            cmd.ExecuteNonQuery();
        }

        public void GetEmployee(int id)
        {
            var cmd = new SqlCommand(@"
                    SELECT 
                        e.employee_id, 
                        e.first_name, 
                        e.last_name, 
                        e.email, 
                        e.phone_number, 
                        j.job_title,
                        d.department_name,
                        e.manager_id,
                        ma.first_name,
                        ma.last_name
                    FROM employees e
                    LEFT JOIN jobs j ON e.job_id = j.job_id
                    LEFT JOIN departments d ON e.department_id = d.department_id                    
                    LEFT JOIN employees ma ON e.manager_id = ma.employee_id
                    WHERE e.employee_id = @id", this.Conn);

            cmd.Parameters.AddWithValue("@id", id); 

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine($"ID: {reader.GetInt32(0)}");
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine($"Fullname           : {reader.GetString(1)} {reader.GetString(2)}");
                Console.WriteLine($"Email              : {reader.GetString(3)}");
                Console.WriteLine($"Phone Number       : {(reader.IsDBNull(4) ? "N/A" : reader.GetString(4))}");
                Console.WriteLine($"Job Title          : {(reader.IsDBNull(5) ? "N/A" : reader.GetString(5))}");
                Console.WriteLine($"Department Name    : {(reader.IsDBNull(6) ? "N/A" : reader.GetString(6))}");
                Console.WriteLine($"Manager ID         : {(reader.IsDBNull(7) ? "N/A" : reader.GetInt32(7))}");
                Console.WriteLine($"Dependents Fullname: {(reader.IsDBNull(7) ? "N/A" : $"{reader.GetString(8)} {reader.GetString(9)}")}");
                Console.WriteLine(); 
            }
            else
            {
                Console.WriteLine("Not found");
            }

        }

    public void ListEmployees()
        {
            var cmd = new SqlCommand(@"
                    SELECT 
                        e.employee_id, 
                        e.first_name, 
                        e.last_name, 
                        e.email, 
                        e.phone_number, 
                        j.job_title,
                        d.department_name
                    FROM employees e
                    LEFT JOIN jobs j
                        ON e.job_id = j.job_id
                    LEFT JOIN departments d
                        ON e.department_id = d.department_id", this.Conn);

            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"{"ID",-8} {"Fullname",-35} {"Email",-35} {"Phone Number",-20} {"Job Title",-33} {"Department Name",-25}");
            Console.WriteLine(new string('-', 160)); 

            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0),-8} " +
                                  $"{(reader.IsDBNull(1) ? "N/A" : reader.GetString(1)) + " " + reader.GetString(2),-35} " +
                                  $"{reader.GetString(3),-35} " +
                                  $"{(reader.IsDBNull(4) ? "N/A" : reader.GetString(4)),-20} " +
                                  $"{(reader.IsDBNull(5) ? "N/A" : reader.GetString(5)),-33} " +
                                  $"{(reader.IsDBNull(6) ? "N/A" : reader.GetString(6)),-25}");
            }

        }

        public void UpdateEmployee(int id, string? firstName, string? lastName, string? email, string? phoneNumber, int? jobId, double? salary, int? managerId, int? departmentId)
        {
            var updates = new Dictionary<string, object?> {
                { "first_name", firstName },
                { "last_name", lastName },
                { "email", email },
                { "phone_number", phoneNumber },
                { "job_id", jobId },
                { "salary", salary },
                { "department_id", departmentId },
                { "manager_id", managerId }
            };

            var query = "UPDATE employees SET ";
            var parameters = new List<SqlParameter>();

            foreach (var field in updates)
            {
                if (field.Value != null)
                {
                    query += $"{field.Key} = @{field.Key}, ";
                    parameters.Add(new SqlParameter($"@{field.Key}", field.Value));
                }
            }

            query = query.TrimEnd(',', ' ') + " WHERE employee_id = @employee_id";
            parameters.Add(new SqlParameter("@employee_id", id));

            var cmd = new SqlCommand(query, this.Conn, this.Trans);
            cmd.Parameters.AddRange([.. parameters]);
           cmd.ExecuteNonQuery();
        }
    }
}
