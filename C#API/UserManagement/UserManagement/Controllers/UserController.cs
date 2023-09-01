using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;

        public UserController(ApiContext context)
        {
            _context = context;
        }

        // Create a User
        [HttpPost]
        public IActionResult Create(UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("User object is null");
            }

            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                if (_context.Users.Any(u => u.Username == userModel.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return BadRequest(ModelState);
                }

                var user = new UserModel
                {
                    Username = userModel.Username,
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    Notes = userModel.Notes
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }

            return BadRequest(ModelState);
        }

        // Edit an existing User by Username
        [HttpPut("{username}")]
        public IActionResult Edit(string username, UserModel userModel)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Username == username);

            if (userInDb == null)
            {
                return NotFound();
            }

            // Check if the updated username already exists
            if (_context.Users.Any(u => u.Username == userModel.Username && u.Id != userInDb.Id))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return BadRequest(ModelState);
            }

            userInDb.Username = userModel.Username;
            userInDb.Name = userModel.Name;
            userInDb.Email = userModel.Email;
            userInDb.Password = userModel.Password;
            userInDb.Notes = userModel.Notes;

            _context.SaveChanges();

            return Ok(userInDb);
        }


        // Get all Users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // Get User by Username
        [HttpGet("{username}")]
        public IActionResult GetByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Delete User by Username
        [HttpDelete("{username}")]
        public IActionResult DeleteByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok($"User with username '{user.Username}' deleted.");
        }

    }
}
