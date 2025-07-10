using MenMaxBackEnd.Models;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly IConfiguration _configuration;

        public UserController(MenMaxContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("login")]
        public ActionResult<User> Login(string id, string password)
        {
            Console.WriteLine(id);

            // Tìm user trực tiếp từ database
            var userFind = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");

            if (userFind != null && !string.IsNullOrEmpty(userFind.Password))
            {
                string decodedValue = Encoding.UTF8.GetString(Convert.FromBase64String(userFind.Password));
                Console.WriteLine(userFind);

                if (password.Equals(decodedValue))
                {
                    userFind.Password = decodedValue;
                    return Ok(userFind);
                }
                else
                {
                    return BadRequest("Invalid password");
                }
            }
            else
            {
                return Ok(userFind);
            }
        }

        [HttpPost("signup")]
        public ActionResult<User> SignUp([FromForm] string username, [FromForm] string fullname,
                                       [FromForm] string email, [FromForm] string password)
        {
            // Kiểm tra user đã tồn tại
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == username && u.Role == "user");
            if (existingUser != null)
            {
                return Ok(null);
            }
            else
            {
                string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                string avatar = "https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png";

                var newUser = new User
                {
                    Id = username,
                    LoginType = "default",
                    Role = "user",
                    Password = encodedValue,
                    UserName = fullname,
                    Avatar = avatar,
                    Email = email,
                    PhoneNumber = null
                };

                // Lưu user mới vào database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                Console.WriteLine(newUser);
                return Ok(newUser);
            }
        }

        [HttpPost("forgot")]
        public ActionResult<string> ForgotPassword([FromForm] string id)
        {
            // Tìm user từ database
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                Random random = new Random();
                int code = random.Next(100000, 999999);

                // Gửi email trực tiếp
                try
                {
                    var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                    {
                        Port = int.Parse(_configuration["Email:Port"]),
                        Credentials = new System.Net.NetworkCredential(
                            _configuration["Email:Username"],
                            _configuration["Email:Password"]
                        ),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("duonglthe170176@fpt.edu.vn"),
                        Subject = "Forgot Password",
                        Body = $"Your code is: {code}",
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(user.Email);

                    smtpClient.Send(mailMessage);

                    // Lưu code vào session
                    HttpContext.Session.SetString("code", code.ToString());
                    Console.WriteLine(code);

                    return Ok(JsonSerializer.Serialize(code.ToString()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    return StatusCode(500, "Error sending email");
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("forgotnewpass")]
        public ActionResult<string> ForgotNewPass([FromForm] string id, [FromForm] string code,
                                                [FromForm] string password)
        {
            // Tìm user từ database
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                user.Password = encodedValue;

                // Cập nhật user trong database
                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok(password);
            }
            else
            {
                return StatusCode(406);
            }
        }

        [HttpPost("changepassword")]
        public ActionResult<string> ChangePassword([FromForm] string id, [FromForm] string password)
        {
            // Tìm user từ database
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                user.Password = encodedValue;

                // Cập nhật user trong database
                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok(password);
            }
            else
            {
                return StatusCode(406);
            }
        }

       /* [HttpPost("update")]
        public ActionResult<User> UpdateAvatar([FromForm] string id, [FromForm] IFormFile? avatar,
                                             [FromForm] string fullname, [FromForm] string email,
                                             [FromForm] string phoneNumber)
        {
            // Tìm user từ database
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                if (avatar != null)
                {
                    // Upload file lên Cloudinary trực tiếp
                    var cloudinary = new Cloudinary(_configuration["Cloudinary:CloudinaryUrl"]);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(avatar.FileName, avatar.OpenReadStream()),
                        Folder = "FashionStore"
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    user.Avatar = uploadResult.SecureUrl.ToString();
                }

                user.UserName = fullname;
                user.Email = email;
                user.PhoneNumber = phoneNumber;

                // Cập nhật user trong database
                _context.Users.Update(user);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
                }

                return Ok(user);
            }
            else
            {
                return StatusCode(406);
            }
        }*/

        [HttpPost("google")]
        public ActionResult<User> LoginWithGoogle([FromForm] string id, [FromForm] string fullname,
                                                [FromForm] string email, [FromForm] string avatar)
        {
            // Tìm user từ database
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user == null)
            {
                user = new User
                {
                    Id = id,
                    LoginType = "google",
                    Role = "user",
                    Password = null,
                    UserName = fullname,
                    Avatar = avatar,
                    Email = email,
                    PhoneNumber = null
                };

                // Lưu user mới vào database
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return Ok(user);
        }
    }
}
