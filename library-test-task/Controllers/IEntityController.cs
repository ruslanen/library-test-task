using System.Collections;
using System.Threading.Tasks;
using library_test_task.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace library_test_task.Controllers
{
    public interface IEntityController<T>
    {
        Task<IActionResult> Add(T viewModel);
        
        Task<IActionResult> Get(long id);

        Task<IActionResult> Delete(long id);

        Task<IActionResult> List();
    }
}