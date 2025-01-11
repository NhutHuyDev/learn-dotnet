using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AdoExample.Tests
{
    public class ProgramTests
    {
        private SqlConnection _connection;
        private HrmService _hrmService;

        public ProgramTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration configuration = builder.Build();

            _connection = new SqlConnection(configuration.GetConnectionString("SimpleHRMDB"));
            _hrmService = new HrmService(_connection, null);
        }

        [Fact]
        public void ConnectionTest()
        {
            _connection.Open();
            var state = _connection.State;

            Assert.Equal(System.Data.ConnectionState.Open, state);

            _connection.Close();
        }

        [Fact]
        public void ListEmployeesTest()
        {
            _connection.Open();
            _hrmService.ListEmployees();
            _connection.Close();
        }

        [Fact]
        public void GetEmployeeTest()
        {
            _connection.Open();
            _hrmService.GetEmployee(102);
            _connection.Close();
        }

        [Fact]
        public void CreateEmployeeTest()
        {
            _connection.Open();
            var trans = _connection.BeginTransaction();
            _hrmService.SetTransaction(trans);
            _hrmService.CreateEmployee("Nguyen", "Van A", "nguyenvana@gmail.com", "036.659.8888", null, 6, 40000, 100, 10);
            trans.Commit();
            _connection.Close();
        }

        [Fact]

        public void UpdateEmployeeTest()
        {
            _connection.Open();
            var trans = _connection.BeginTransaction();
            _hrmService.SetTransaction(trans);
            _hrmService.UpdateEmployee(207, null, null, null, "036.659.7777", null, null, null, null);
            trans.Commit();
            _connection.Close();
        }

        [Fact]
        public void DeleteEmployeeTest()
        {
            _connection.Open();
            var trans = this._connection.BeginTransaction();
            _hrmService.SetTransaction(trans);
            _hrmService.DeleteEmployee(209);
            trans.Commit();
            _connection.Close();
        }
    }
}

