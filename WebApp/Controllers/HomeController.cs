using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        //Uncomment this section if you want to see how SQL helper works, note, set Connection string in appsettings.json file
        //Create Database and run the script which is with the proejct or change the code as per your need.
        //Comment above Index Methid si that this code can be tested.

        ///// <summary>
        ///// To Check SQL Helper
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> Index()
        //{

        //    ProjectInfo objProject = new ProjectInfo();
        //    Random objRand = new Random();
        //    objProject.ProjectName = "Project_" + objRand.Next(0, 20).ToString();
        //    objProject.ProjectCode = "Project Code " + objRand.Next(0, 20).ToString();

        //    GenralCRUD objCRUID = new GenralCRUD();
        //    await objCRUID.AddProject(objProject);

        //    objProject.ProjectID = 1;
        //    objProject.ProjectName = "Project_" + objRand.Next(0, 20).ToString();
        //    objProject.ProjectCode = "Project Code " + objRand.Next(0, 20).ToString();

        //    await objCRUID.UpdateProject(objProject);
        //    objProject.ProjectID = 2;
        //    ProjectInfo objProjectget = await objCRUID.GetProject(objProject);

        //    return View();
        //}




        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
