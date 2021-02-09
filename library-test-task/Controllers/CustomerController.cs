using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_test_task.Controllers
{
    public class CustomerController : Controller, IEntityController<CustomerViewModel>
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = customerViewModel.ToCustomer();
            var id = await _customerRepository.SaveAsync(customer);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] long id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return Ok(customer);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] long id)
        { 
            await _customerRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var customers = await _customerRepository.GetAll().ToListAsync();
            return Ok(customers);
        }
        
        [HttpGet]
        public async Task<IActionResult> ListNames()
        {
            var customers = await _customerRepository.GetAll()
                .Select(x => new {x.Id, x.LastName, x.FirstName, x.Patronymic}).ToListAsync();
            return Ok(customers);
        }
    }
}