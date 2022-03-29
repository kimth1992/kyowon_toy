﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TicketList()
        {

            return View();
        }


        public IActionResult BoardList(string search)
        {
            return View(BoardModel.GetList(search));
        }
        [Authorize]
        public IActionResult BoardWrite()
        {
            return View();
        }

        // 관리자만 사용 할 수 있다.
        //[Authorize(Roles ="admin")]
        [Authorize]
        public IActionResult BoardWrite_Input(string title, string contents)
        {
            var model = new BoardModel();

            model.Title = title;
            model.Contents = contents;
            model.User = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.UserName = User.Identity.Name;
            model.Insert();

            return Redirect("/home/boardlist");
        }
        public IActionResult BoardView(uint idx)
        {
            return View(BoardModel.Get(idx));
        }
        [Authorize]
        public IActionResult BoardEdit(uint idx, string type)
        {
            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (model.User != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            if (type == "U")
            {
                return View(model);
            }
            else if (type == "D")
            {
                model.Delete();
                return Redirect("/home/boardlist");

            }
            throw new Exception("잘못된 요청입니다.");
        }


        [Authorize]
        public IActionResult BoardEdit_Input(uint idx, string title, string contents)
        {
         // idx가 안넘어옴
          

            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (model.User != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            model.Title = title;
            model.Contents = contents;

            model.Update();

            return Redirect("/home/boardlist");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}