﻿using EventManagementsystem.Models;
using EventManagementSystem.IService;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unipluss.Sign.ExternalContract.Entities;

namespace EventManagementsystem.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _loginDetailsService;
        private readonly IEventService _eventDetailsService;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, ILoginService loginDetailsService, IEventService eventDetailsService, IConfiguration configuration)
        {
            _logger = logger;
            _loginDetailsService = loginDetailsService;
            _eventDetailsService = eventDetailsService;
            _configuration = configuration;
        }
        /// <summary>
        /// Main Home index page
        /// </summary>
        /// <param name="type">1 or 0</param>
        /// <returns>public event details list</returns>
        public IActionResult Index(string type)
        {
            if (type == null)
            {
                type = "1";
            }
            List <EventDetails> evenList = new List<EventDetails> ();
            evenList =_eventDetailsService.GetEventDetailsList(1,Convert.ToInt32(type),0);
            return View(evenList);
        }

        public IActionResult Details(string id)
         {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Signup()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult RegisterUser(UserDetails user)
        {
            user.Password = EncodePasswordToBase64(user.Password);
            _loginDetailsService.SaveUserDetails(user);
            var list = _loginDetailsService.GetLoginDetailByEmail(user.Email, user.Password);
            var token = GenerateJSONWebToken(list);
            var id = list.UserId;
            var response = new
            {
                Id = id,
                Token = token
            };
            HttpContext.Session.SetString("token", token.ToString());
            HttpContext.Session.SetString("UserId", list.UserId.ToString());
            HttpContext.Session.SetString("user_name", list.FirstName.ToString() + " " + list.LastName.ToString());
            HttpContext.Session.SetString("Email", list.Email.ToString());
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailValidate(string email)
        {
            try
            {
                var list = _loginDetailsService.GetEmailValidate(email);
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLoginDetailByEmail(string email, string password)
        {
            try
            {
                var list = _loginDetailsService.GetLoginDetailByEmail(email, EncodePasswordToBase64(password));
                if (list != null)
                {
                    var tonen = GenerateJSONWebToken(list);
                    var id = list.UserId;
                    var response = new
                    {
                        Id = id,
                        Token = tonen
                    };
                    HttpContext.Session.SetString("token", tonen.ToString());
                    HttpContext.Session.SetString("UserId", list.UserId.ToString());
                    HttpContext.Session.SetString("user_name", list.FirstName.ToString()+" "+ list.LastName.ToString());
                    HttpContext.Session.SetString("Email", list.Email.ToString());
                    return Ok(response);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEventComments(string id)
        {
            try
            {
                List<Comments> evenList = new List<Comments>();
                evenList = _eventDetailsService.GetEventComments(id);
                return PartialView("Views/Home/EventComments.cshtml", evenList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEventComments(string id ,string comment)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (userId == null)
                {
                    userId = "0";
                }
                _eventDetailsService.AddEventComments(id, comment, Convert.ToInt32(userId));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult GetEventById(string id)
        {
            EventDetails evenData = new EventDetails();
            evenData = _eventDetailsService.GetEventDetailById(Convert.ToInt32(id));
            return Ok(evenData);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private string GenerateJSONWebToken(UserDetails list)
        {
            var securityKey = Encoding.ASCII.GetBytes("ThisismySecretKeyusingforgeneratetoken");
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("key", "key"),
                    new Claim("value", "Value"),
                    new Claim("DateOfJoing", DateTime.Now.ToString()),
                    new Claim(ClaimTypes.Email, list.Email),
                    new Claim(ClaimTypes.GivenName, list.FirstName+" "+list.LastName)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> UserModel()
        {
            return Ok(new UserDetails());
        }
        // this function Convert to Encript your password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}