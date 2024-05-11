using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gambling_sport.Models;
using gambling_sport;
using Grpc.Net.Client;

namespace gambling_sport.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly ISession _session;
    private readonly IHostEnvironment _environment;
    private readonly Member.MemberClient _memberClient;

    public LoginController(ILogger<LoginController> logger,
        IHttpContextAccessor httpContextAccessor,
        IHostEnvironment environment,
        Member.MemberClient member)
    {
        _logger = logger;
        _session = httpContextAccessor.HttpContext.Session;
        _environment = environment;
        _memberClient = member;
    }


    public IActionResult Index()
    {
        var role = _session.GetString("role") ?? "";
        return _RedirectPageByRole(role);
    }

    [HttpPost]
    public async Task<IActionResult> Login(string account, string password)
    {
        //if (_environment.IsDevelopment())
        //{
        //    account = "a";
        //    password = "a";
        //}
        try
        {
            var result = await _memberClient.LoginAsync(new LoginRequest { Account = account, Password = password });
            _session.SetString("token", result.Token);
            _session.SetString("id", result.Id);
            _session.SetString("username", result.Username);
            _session.SetString("role", result.Role);

            return _RedirectPageByRole(result.Role);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            throw;
        }     
    }

    public IActionResult Logout()
    {
        _session.Clear();
        return RedirectToAction("Index");
    }

    private IActionResult _RedirectPageByRole(string role)
    {
        // Action Filter

        if (role == "Admin")
        {
            return RedirectToAction("Index", "Admin");
        }
        else if (role == "User")
        {
            return RedirectToAction("Index", "Lobby");
        }

        return View();
    }
}

