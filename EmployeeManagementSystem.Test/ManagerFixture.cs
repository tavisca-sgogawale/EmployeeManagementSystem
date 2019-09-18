using EmployeeManagementSystem.Controllers;
using EmployeeManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Test
{
    public class ManagerFixture
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
        public async Task DisplayAllManager()
        {
            var context = CreateContextForSQLite();
            var testManagerDb = GetTestManager(context);
            var controller = new ManagersController(testManagerDb);
            var managerIds = testManagerDb.Managers.Select(m => m.ManagerId).Distinct().ToList();
            var expected= (await testManagerDb.Employees.ToListAsync())
                             .Where(e => managerIds.Contains(e.Id))
                         .ToList();
            var result = await controller.GetAllManagers();
            Assert.Equal(expected, result.Value);
        }


        private EmployeeContext GetTestManager(EmployeeContext _context)
        {
           // _context = context;
            _context.Managers.RemoveRange(_context.Managers);
            _context.SaveChanges();
            _context.Employees.RemoveRange(_context.Employees);
            _context.SaveChanges();
            if (_context.Employees.Count() == 0)
            {
                _context.Employees.Add(new Employee { Name = "Shubham", Age = 22, Id = 1118, Salary = 50000 });
                _context.Employees.Add(new Employee { Name = "Vighnesh", Age = 21, Id = 1119, Salary = 150000 });
                _context.Employees.Add(new Employee { Name = "Omkar", Age = 23, Id = 1114, Salary = 76000 });
                _context.Employees.Add(new Employee { Name = "Bhanu", Age = 26, Id = 1115, Salary = 80000 });
                _context.Employees.Add(new Employee { Name = "Ronak", Age = 21, Id = 1116, Salary = 56500 });
                _context.Employees.Add(new Employee { Name = "Rishabh", Age = 29, Id = 1117, Salary = 20000 });

                _context.SaveChanges();
            }
            if (_context.Managers.Count() == 0)
            {

                // Create a new Employee if collection is empty,
                // which means you can't delete all.
                var shubham = _context.Employees.Where(e => e.Name == "Shubham").First();
                var omkar = _context.Employees.Where(e => e.Name == "Omkar").First();
                var vighnesh = _context.Employees.Where(e => e.Name == "Vighnesh").First();
                var bhanu = _context.Employees.Where(e => e.Name == "Bhanu").First();
                var rishab = _context.Employees.Where(e => e.Name == "Rishabh").First();
                var raunak = _context.Employees.Where(e => e.Name == "Ronak").First();
                _context.Managers.Add(new Manager { EmployeeId = shubham.Id, ManagerId = vighnesh.Id });
                _context.Managers.Add(new Manager { EmployeeId = rishab.Id, ManagerId = omkar.Id });
                _context.Managers.Add(new Manager { EmployeeId = raunak.Id, ManagerId = bhanu.Id });
                _context.SaveChanges();
            }
            return _context;
        }
    }
}
