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
    public class ManagersController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public ManagersController(EmployeeContext context)
        {
            _context = context;
            //_context.Managers.RemoveRange(_context.Managers);
            //_context.SaveChanges();

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
                _context.Managers.Add(new Manager { EmployeeId = shubham.Id, ManagerId= vighnesh.Id });
                _context.Managers.Add(new Manager { EmployeeId = rishab.Id, ManagerId = omkar.Id });
                _context.Managers.Add(new Manager { EmployeeId = raunak.Id, ManagerId = bhanu.Id });
                _context.SaveChanges();
            }
        }

        // GET: api/manager
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllManagers()
        {
            var managerIds = _context.Managers.Select(m => m.ManagerId).Distinct().ToList();
            return (await _context.Employees.ToListAsync())
                .Where(e=> managerIds.Contains(e.Id))
                .ToList();
        }
        // GET: api/manager/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetManager(int id)
        {
            var managerIds = _context.Managers.Select(m => m.ManagerId).Distinct().ToList();
            if (!managerIds.Contains(id))
            {
                return NotFound("Manager Not Found");

            }
             return Ok(await _context.Employees.FindAsync(id));

        }

        // GET: api/manager/id/employee
        [HttpGet("{id}/employee")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesUnderManager(int id)
        {
            var managerIds = _context.Managers.Select(m => m.ManagerId).Distinct().ToList();
            var employeeIds = _context.Managers.Where(m => m.ManagerId == id).
                Select(m => m.EmployeeId).ToList();
            if (!managerIds.Contains(id))
            {
                return NotFound("Manager Not Found");

            }
            return (await _context.Employees.ToListAsync())
               .Where(e => employeeIds.Contains(e.Id))
               .ToList();
        }

        [HttpPost]
        public IActionResult AddManager([FromBody] Manager manager)
        {
            try
            {
                var employeeIds = _context.Managers.Select(m => m.EmployeeId).Distinct().ToList();
                if(!employeeIds.Contains(manager.EmployeeId))
                 {
                    return BadRequest("400 Bad Request..!!! Employee Not Found..");
                }
                _context.Managers.Add(manager);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("400 Bad Request ...."+e.ToString());
            }

        }

    }
}
