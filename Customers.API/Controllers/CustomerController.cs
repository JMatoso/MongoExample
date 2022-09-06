using AutoMapper;
using Customers.API.Entities;
using Customers.API.Extensions;
using Customers.API.Helpers;
using Customers.API.Models;
using Customers.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers
{
    /// <summary>
    /// Customers.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/customer")]
    [Produces("application/json")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMockEntity<Customer> _customers;

        public CustomerController(IMockEntity<Customer> customers, IMapper mapper)
        {
            _mapper = mapper;
            _customers = customers;
        }

        /// <summary>
        /// Get customers.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerVM>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerVM>>> GetCustomers()
        {
            return Ok(_mapper.Map<IEnumerable<CustomerVM>>(await _customers.GetAsync()));
        }

        /// <summary>
        /// Get customer.
        /// </summary>
        /// <param name="id">Customer id.</param>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(CustomerVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerVM>> GetCustomer(Guid id)
        {
            if(Guid.Empty == id)
            {
                return BadRequest(ActionReporterProvider.Set("Invalid id.", StatusCodes.Status400BadRequest));
            }

            var customer = await _customers.GetAsync(c => c.Id == id);

            return customer is null ? NotFound(ActionReporterProvider.Set("Customer not found.", StatusCodes.Status404NotFound)) : Ok(_mapper.Map<CustomerVM>(customer)); 
        }

        /// <summary>
        /// Add customer.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerVM), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]CustomerRequest newCustomer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = _mapper.Map<Customer>(newCustomer);

            await _customers.CreateAsync(customer);

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, _mapper.Map<CustomerVM>(customer));
        }

        /// <summary>
        /// Update customer.
        /// </summary>
        /// <param name="id">Customer id.</param>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, CustomerRequest updatedCustomer)
        {
            if (Guid.Empty == id)
            {
                return BadRequest(ActionReporterProvider.Set("Invalid id.", StatusCodes.Status400BadRequest));
            }

            if (!ModelState.IsValid) return BadRequest();

            var customer = await _customers.GetAsync(c => c.Id == id);

            if (customer is null)
            {
                return NotFound(ActionReporterProvider.Set("Customer not found.", StatusCodes.Status404NotFound));
            }

            customer.Name = updatedCustomer.Name;
            customer.Genre = updatedCustomer.Genre;
            customer.Salary = updatedCustomer.Salary;
            customer.Department = updatedCustomer.Department;

            await _customers.UpdateAsync(c => c.Id == id, customer);

            return NoContent();
        }

        /// <summary>
        /// Delete customer.
        /// </summary>
        /// <param name="id">Customer id.</param>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ActionReporter), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest(ActionReporterProvider.Set("Invalid id.", StatusCodes.Status400BadRequest));
            }

            var book = await _customers.GetAsync(c => c.Id == id);

            if (book is null)
            {
                return NotFound(ActionReporterProvider.Set("Customer not found.", StatusCodes.Status404NotFound));
            }

            await _customers.RemoveAsync(c => c.Id == id);

            return NoContent();
        }
    }
}
