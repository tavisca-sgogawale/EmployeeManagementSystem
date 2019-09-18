using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;

            //_context.Employees.RemoveRange(_context.Employees);
            //_context.SaveChanges();

            if (_context.Employees.Count() == 0)
            {
                // Create a new Employee if collection is empty,
                // which means you can't delete all.
                _context.Employees.Add(new Employee { Name = "Shubham", Age = 22, Id=1118, Salary = 50000 });
                _context.Employees.Add(new Employee { Name = "Vighnesh", Age = 21, Id = 1119, Salary = 150000 });
                _context.Employees.Add(new Employee { Name = "Omkar", Age = 23, Id = 1114, Salary = 76000 });
                _context.Employees.Add(new Employee { Name = "Bhanu", Age = 26, Id = 1115,  Salary = 80000 });
                _context.Employees.Add(new Employee { Name = "Ronak", Age = 21, Id = 1116, Salary = 56500 });
                _context.Employees.Add(new Employee { Name = "Rishabh", Age = 29, Id = 1117, Salary = 20000 });

                _context.SaveChanges();
            }
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        // GET: api/employees/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return NotFound();
                }

                return employee;
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("400 Bad Request ....");
            }
            
        }

    }
}