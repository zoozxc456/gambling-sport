using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gambling_sport.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gambling_sport.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly Member.MemberClient _memberClient;
        public RegisterController(ILogger<RegisterController> logger, Member.MemberClient memberClient)
        {
            _logger = logger;
            _memberClient = memberClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var requestModel = new RegisterRequest
            {
                Account = viewModel.Account,
                Password = viewModel.Password,
                Username = viewModel.Username,
                Email = viewModel.Email
            };

            var response = await _memberClient.RegisterAsync(requestModel);

            if (response.Message == "Create New Member Success")
            {
                return RedirectToAction("Index", "Login");
            }

            return View("Index");
        }

    }
}

