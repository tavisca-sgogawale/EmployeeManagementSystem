using EmployeeManagementSystem.Controllers;
using EmployeeManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.Test
{
    public class EmployeeFixture
    {
        public EmployeeContext CreateContextForSQLite()
        {
            //var connection = new SqliteConnection("Data Source=ems.db");
            //connection.Open();
            
            var option = new DbContextOptionsBuilder<EmployeeContext>().UseSqlite("Data Source=ems.db").Options;

            var context = new EmployeeContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        

        [Fact]
        public async Task DisplayAllEmployee()
        {
            var context = CreateContextForSQLite();
            var getEmployees = await GetTestEmployees(context);
            var controller = new EmployeesController(getEmployees);
            var result = await controller.GetEmployees();
            Assert.Equal( await getEmployees.Employees.ToListAsync(), result.Value);
        }

        [Fact]
        public async Task TestForDisplayEmployeeById()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            var result = await controller.GetEmployeeById(1118);
            Assert.Equal(await testEmployeeContext.Employees.FindAsync(1118),result.Value);
        }


        [Fact]
        public async Task TestForDisplayEmployeeByIdFailure()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            var result = await controller.GetEmployeeById(1);
            Assert.Equal(await testEmployeeContext.Employees.FindAsync(1), result.Value);
        }



        [Fact]
        public async Task TestForAddEmployee()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            Employee employee = new Employee { Name = "Akshay", Age = 22, Id = 1111, Salary = 50000 };
            var result = controller.AddEmployee(employee);
            Assert.Equal(await testEmployeeContext.Employees.FindAsync(1111), employee);
        }

        [Fact]
        public async Task TestForAddEmployeeFailureByWrongData()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            Employee employee = new Employee { Name = "Aksha" , Id = 1111, Salary = 50000 };
            var result = controller.AddEmployee(employee);
            Assert.Equal(await testEmployeeContext.Employees.FindAsync(1111), employee);

        }

        [Fact]
        public async Task TestForAddEmployeeFailureBy()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            Employee employee = new Employee { Name = "Aksha", Id = 1114, Salary = 50000 };
            controller.AddEmployee(employee);
            Assert.NotEqual(await testEmployeeContext.Employees.FindAsync(1114), employee);

        }
        [Fact]
        public async Task TestForDisplayEmployeeByIdFailureByPassingWrongData()
        {
            var context = CreateContextForSQLite();
            var testEmployeeContext = await GetTestEmployees(context);
            var controller = new EmployeesController(testEmployeeContext);
            var result = await controller.GetEmployeeById(0);
            Assert.Equal(await testEmployeeContext.Employees.FindAsync(0), result.Value);
        }

        private async Task<EmployeeContext> GetTestEmployees(EmployeeContext context)
        {
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();
            if (context.Employees.Count() == 0)
            {
                context.Employees.Add(new Employee { Name = "Shubham", Age = 22, Id = 1118, Salary = 50000 });
                context.Employees.Add(new Employee { Name = "Vighnesh", Age = 21, Id = 1119, Salary = 150000 });
                context.Employees.Add(new Employee { Name = "Omkar", Age = 23, Id = 1114, Salary = 76000 });
                context.Employees.Add(new Employee { Name = "Bhanu", Age = 26, Id = 1115, Salary = 80000 });
                context.Employees.Add(new Employee { Name = "Ronak", Age = 21, Id = 1116, Salary = 56500 });
                context.Employees.Add(new Employee { Name = "Rishabh", Age = 29, Id = 1117, Salary = 20000 });

                context.SaveChanges();
            }
            return  context;
        }
    }
}
