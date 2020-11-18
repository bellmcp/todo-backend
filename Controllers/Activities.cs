using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// ใช้เช็ค token
using Microsoft.AspNetCore.Authorization;
// ใช้ติดต่อ database
using ToDo_Backend.Models;

namespace ToDo_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(ILogger<ActivitiesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles="admin,user")]
        public IActionResult Get()
        {
            var db = new ToDoDbContext();
            var activities = db.Activity.Select(s => s);
            return Ok(activities);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles="admin,user")]
        public IActionResult Get(uint id)
        {
            var db = new ToDoDbContext();
            var a = db.Activity.Find(id);
            return Ok(a);
        }

        [HttpPost]
        [Authorize(Roles="admin,user")]
        public IActionResult Post([FromBody] Activity a)
        {
            var db = new ToDoDbContext();
            db.Activity.Add(a);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles="admin,user")]
        public IActionResult Put(uint id, [FromBody] Activity a) {
            var db = new ToDoDbContext();
            a.Id = id;
            db.Update(a);
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles="admin,user")]
        public IActionResult Delete(uint id) {
            var db = new ToDoDbContext();
            var a = db.Activity.Find(id);
            db.Activity.Remove(a);
            db.SaveChanges();
            return Ok();
        }
    }
}