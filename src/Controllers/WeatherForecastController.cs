using EfSamples.Model;
using EfSamples.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EfSamples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository _repository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_repository.GetAllUsers());
        }

        [HttpGet]
        public ActionResult<User> Get(int id)
        {
            return Ok(_repository.GetUser(id));
        }

        [HttpPost]
        public ActionResult<User> Post(User user)
        {
            return Ok(_repository.AddUser(user));
        }
        [HttpPut]
        public ActionResult<User> Put(User user)
        {
            return Ok(_repository.UpdateUser(user));
        }
        [HttpDelete]
        public ActionResult Delete(User user)
        {
            _repository.RemoveUser(user);
            return Ok();
        }
    }
}
