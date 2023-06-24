using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/Educations")]
    public class EducationController : GeneralController<Education>
    {
        public EducationController(IEducationRepository repository) : base(repository) { }
    }
}
