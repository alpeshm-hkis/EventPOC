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

        public IActionResult Index()
        {
            List<EventDetails> evenList =_eventDetailsService.GetEventDetailsList(1,1,0);
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
            _loginDetailsService.SaveUserDetails(user);
            return View();
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
                var list = _loginDetailsService.GetLoginDetailByEmail(email, password);
                if (list != null)
                {
                    var tonen = GenerateJSONWebToken(email, password);
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
                _eventDetailsService.AddEventComments(id, comment);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private string GenerateJSONWebToken(string email, string password)
        {
            var securityKey = Encoding.ASCII.GetBytes("ThisismySecretKeyusingforgeneratetoken");
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("key", "key"),
                    new Claim("value", "Value"),
                    new Claim("DateOfJoing", DateTime.Now.ToString())
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
    }
}