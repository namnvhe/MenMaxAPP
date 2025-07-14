using MenMaxBackEnd.Models;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MenMaxBackEnd.Controllers
{
    public class ResponseEntity<T>
    {
        public T Value { get; set; }
        public HttpStatus StatusCode { get; set; }

        public ResponseEntity(T value, HttpStatus statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public ResponseEntity(HttpStatus statusCode)
        {
            StatusCode = statusCode;
        }

        public static implicit operator ActionResult<T>(ResponseEntity<T> response)
        {
            return new ObjectResult(response.Value)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }

    // ✅ Định nghĩa HttpStatus enum
    public enum HttpStatus
    {
        OK = 200,
        NotFound = 404,
        NotAcceptable = 406,
        InternalServerError = 500
    }

    [ApiController]
    [Route("")]
    public class UserController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(MenMaxContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        // ✅ GET /login - trả về UserDto
        [HttpGet("login")]
        public ResponseEntity<UserDto> Login(string id, string password)
        {
            Console.WriteLine(id);

            var userFind = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");

            if (userFind != null && !string.IsNullOrEmpty(userFind.Password))
            {
                string decodedValue = Encoding.UTF8.GetString(Convert.FromBase64String(userFind.Password));
                Console.WriteLine(userFind);

                if (password.Equals(decodedValue))
                {
                    userFind.Password = decodedValue;
                    var userDto = _mapper.Map<UserDto>(userFind);
                    return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var userDto = userFind != null ? _mapper.Map<UserDto>(userFind) : null;
                return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
            }
        }

        [HttpPost("signup")]
        [Consumes("application/x-www-form-urlencoded")]
        public ResponseEntity<UserDto> SignUp([FromForm] string username, [FromForm] string fullname, [FromForm] string email, [FromForm] string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == username && u.Role == "user");
            if (existingUser != null)
            {
                return new ResponseEntity<UserDto>(null, HttpStatus.OK);
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

                _context.Users.Add(newUser);
                _context.SaveChanges();

                // ✅ Debug: Kiểm tra User entity
                Console.WriteLine($"Saved User - ID: {newUser.Id}, Name: {newUser.UserName}, Email: {newUser.Email}");

                // ✅ Kiểm tra AutoMapper trước khi map
                if (_mapper == null)
                {
                    throw new InvalidOperationException("AutoMapper is not configured properly");
                }

                var userDto = _mapper.Map<UserDto>(newUser);

                // ✅ Debug: Kiểm tra UserDto
                Console.WriteLine($"Mapped DTO - ID: {userDto?.Id}, Name: {userDto?.UserName}, Email: {userDto?.Email}");

                if (userDto == null)
                {
                    throw new InvalidOperationException("AutoMapper returned null UserDto");
                }

                return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
            }
        }


        // ✅ POST /forgot - giữ nguyên trả về string
        [HttpPost("forgot")]
        public string ForgotPassword([FromForm] string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                Random random = new Random();
                int code = (int)Math.Floor(((random.NextDouble() * 899999) + 100000));

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

                    HttpContext.Session.SetString("code", code.ToString());
                    Console.WriteLine(code);

                    return code.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // ✅ POST /forgotnewpass - giữ nguyên trả về string
        [HttpPost("forgotnewpass")]
        [Consumes("application/x-www-form-urlencoded")]
        public string ForgotNewPass([FromForm] string id, [FromForm] string code, [FromForm] string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                user.Password = encodedValue;

                _context.Users.Update(user);
                _context.SaveChanges();

                return password;
            }
            else
            {
                return null;
            }
        }

        // ✅ POST /changepassword - giữ nguyên trả về string
        [HttpPost("changepassword")]
        [Consumes("application/x-www-form-urlencoded")]
        public ResponseEntity<string> ChangePassword( [FromForm] string id, [FromForm] string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                user.Password = encodedValue;

                _context.Users.Update(user);
                _context.SaveChanges();

                return new ResponseEntity<string>(password, HttpStatus.OK);
            }
            else
            {
                return new ResponseEntity<string>(HttpStatus.NotAcceptable);
            }
        }

        // ✅ POST /update - trả về UserDto
        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public ResponseEntity<UserDto> UpdateAvatar ( [FromForm] string id, IFormFile avatar, string fullname,
                                                   string email, string phoneNumber, string address)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user != null)
            {
                if (avatar != null)
                {
                    // ✅ Implement Cloudinary upload nếu cần
                    // var cloudinary = new Cloudinary(_configuration["Cloudinary:CloudinaryUrl"]);
                    // var uploadParams = new ImageUploadParams()
                    // {
                    //     File = new FileDescription(avatar.FileName, avatar.OpenReadStream()),
                    //     Folder = "FashionStore"
                    // };
                    // var uploadResult = cloudinary.Upload(uploadParams);
                    // user.Avatar = uploadResult.SecureUrl.ToString();
                }

                user.UserName = fullname;
                user.Email = email;
                user.PhoneNumber = phoneNumber;
                // user.Address = address;

                _context.Users.Update(user);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
                }

                var userDto = _mapper.Map<UserDto>(user);
                return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
            }
            else
            {
                return new ResponseEntity<UserDto>(HttpStatus.NotAcceptable);
            }
        }

        // ✅ POST /google - trả về UserDto
        [HttpPost("google")]
        [Consumes("application/x-www-form-urlencoded")]
        public UserDto LoginWithGoogle([FromForm] string id, [FromForm] string fullname, [FromForm] string email, [FromForm] string avatar)
        {
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
                   
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
