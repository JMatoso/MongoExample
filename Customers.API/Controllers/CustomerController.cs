using AutoMapper;
using Customers.API.Entities;
using Customers.API.Models;
using Customers.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/customer")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MockEntity _customers;

        public CustomerController(MockEntity customers, IMapper mapper)
        {
            _customers = customers;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerVM>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerVM>>> GetCustomers()
        {
            return Ok(_mapper.Map<IEnumerable<CustomerVM>>(await _customers.GetAsync()));
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(CustomerVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerVM>> GetCustomer(Guid id)
        {
            if(Guid.Empty == id)
            {
                return BadRequest("Invalid id.");
            }

            var customer = await _customers.GetAsync(id);

            return customer is null ? NotFound("Customer not found.") : Ok(_mapper.Map<CustomerVM>(customer)); 
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerVM), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]CustomerRequest newCustomer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Fill all required fields.");
            }

            var customer = _mapper.Map<Customer>(newCustomer);

            await _customers.CreateAsync(customer);

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, _mapper.Map<CustomerVM>(customer));
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, CustomerRequest updatedCustomer)
        {
            if (Guid.Empty == id) return BadRequest("Invalid id.");

            if (!ModelState.IsValid) return BadRequest("Fill all required fields.");

            var customer = await _customers.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.Department = updatedCustomer.Department;
            customer.Genre = updatedCustomer.Genre;
            customer.Salary = updatedCustomer.Salary;

            await _customers.UpdateAsync(id, customer);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Invalid id.");
            }

            var book = await _customers.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _customers.RemoveAsync(id);

            return NoContent();
        }
    }
}
