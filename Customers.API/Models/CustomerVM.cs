using System.ComponentModel.DataAnnotations;

namespace Customers.API.Models
{
    public class CustomerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public Department Department { get; set; }
        public Genre Genre { get; set; }
        public DateTime Created { get; set; }
    }
}
