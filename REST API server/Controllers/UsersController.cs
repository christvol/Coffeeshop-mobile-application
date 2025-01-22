using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API_SERVER.Classes;
using Strings = REST_API_SERVER.Classes.CommonLocal.Strings;

namespace REST_API_SERVER.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CoffeeShopContext _context;

    public UsersController(CoffeeShopContext context)
    {
        this._context = context;
    }

    private bool UserExists(int id)
    {
        return this._context.Set<Users>().Any(e => e.Id == id);
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
    {
        return await this._context.Set<Users>().ToListAsync();
    }

    // GET: api/Users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Users>> GetUserById(int id)
    {
        var user = await this._context.Set<Users>().FindAsync(id);

        if (user == null)
        {
            return this.NotFound(new
            {
                Message = Strings.UsersController.UserNotFound
            });
        }

        return user;
    }

    // GET: api/Users/phone/{phoneNumber}
    [HttpGet("phone/{phoneNumber}")]
    public async Task<ActionResult<Users>> GetUserByPhoneNumber(string phoneNumber)
    {
        var cleanedPhoneNumber = StringHelper.CleanPhoneNumber(phoneNumber);

        var user = await this._context.Set<Users>()
            .FirstOrDefaultAsync(u => u.PhoneNumber == cleanedPhoneNumber);

        if (user == null)
        {
            return this.NotFound(new
            {
                Message = Strings.UsersController.UserPhoneNotFound
            });
        }

        return user;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<Users>> CreateUser([FromBody] UserRequestDto userDto)
    {
        var cleanedPhoneNumber = StringHelper.CleanPhoneNumber(userDto.PhoneNumber);

        var existingUser = await this._context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == cleanedPhoneNumber);

        if (existingUser != null)
        {
            return this.Ok(existingUser);
        }

        var user = new Users
        {
            IdUserType = userDto.IdUserType,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            BirthDate = userDto.BirthDate,
            Email = userDto.Email,
            PhoneNumber = cleanedPhoneNumber,
            CreationDate = DateTime.UtcNow
        };

        this._context.Users.Add(user);
        await this._context.SaveChangesAsync();

        return this.CreatedAtAction(nameof(GetUserById), new
        {
            id = user.Id
        }, user);
    }

    // PUT: api/Users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto userDto)
    {
        if (id != userDto.Id)
        {
            return this.BadRequest(new
            {
                Message = Strings.UsersController.UserIdMismatch
            });
        }

        var existingUser = await this._context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return this.NotFound(new
            {
                Message = Strings.UsersController.UserNotFound
            });
        }

        existingUser.IdUserType = userDto.IdUserType;
        existingUser.FirstName = userDto.FirstName;
        existingUser.LastName = userDto.LastName;
        existingUser.BirthDate = userDto.BirthDate;
        existingUser.Email = userDto.Email;
        existingUser.PhoneNumber = userDto.PhoneNumber;

        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return this.StatusCode(500, new
            {
                Message = Strings.UsersController.ConcurrencyConflict
            });
        }

        return this.NoContent();
    }

    // DELETE: api/Users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await this._context.Users.FindAsync(id);

        if (user == null)
        {
            return this.NotFound(new
            {
                Message = Strings.UsersController.UserNotFound
            });
        }

        this._context.Users.Remove(user);
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }

    // GET: api/Users/{id}/UserType
    [HttpGet("{id}/UserType")]
    public async Task<ActionResult<UserTypeDto>> GetUserType(int id)
    {
        var user = await this._context.Set<Users>()
            .Include(u => u.IdUserTypeNavigation)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null || user.IdUserTypeNavigation == null)
        {
            return this.NotFound(new
            {
                Message = Strings.UsersController.UserNotFound
            });
        }

        var userTypeDto = new UserTypeDto
        {
            Id = user.IdUserTypeNavigation.Id,
            Title = user.IdUserTypeNavigation.Title
        };

        return this.Ok(userTypeDto);
    }
}
