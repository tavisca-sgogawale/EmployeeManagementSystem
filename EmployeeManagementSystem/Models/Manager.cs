using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Manager
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee NormalEmployee { get; set; }

        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Employee _Manager { get; set; }

    }
}
