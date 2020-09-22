using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeachersPermissions.Models;

namespace TeachersPermissions.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {


            return View();
        }
        public IActionResult PromedioAño(int? id)
        {
            if (id == 1)
            {
                int employeenum = _context.Employee.Count();
                double permissionsNum = _context.Permission.Count();
                var lstModel = new List<SimpleReportViewModel>();
                double quant, porcent;
                int res;
                double promedio = 0;
                foreach (var item in _context.Employee)
                {
                    quant = _context.Permission.Where(n => n.EmployeeId == item.EmployeeId).Count();
                    // porcent = quant / permissionsNum * 100;
                    res = (int)quant;
                    promedio += quant;
                    lstModel.Add(new SimpleReportViewModel { DimensionOne = item.EmployeeId.ToString() + " " + item.FirstName + " " + item.FirstLastName, Quantity = res });
                }
                promedio = promedio / employeenum;
                ViewBag.id = 1;
                ViewBag.Key = "Promedio de Permisos por Profesor al Año: ";
                ViewBag.Value = promedio;
                ViewBag.Total = permissionsNum;

                return View(lstModel);
            }
            else
               if (id == 2)
            {
                var lstModel = new List<SimpleReportViewModel>();
                string month;
                DateTime StartDate = new DateTime(DateTime.Now.Year, 01, 01);
                DateTime FinalDate = new DateTime(DateTime.Now.Year, 01, 31);
                for (int i = 0; i < 12; i++)
                {
                    int asp = _context.Permission.Where(n => n.RequestDate >= StartDate && n.RequestDate <= FinalDate).Count();
                    month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartDate.Month);
                    lstModel.Add(new SimpleReportViewModel { DimensionOne = month, Quantity = asp });
                    StartDate = StartDate.AddMonths(1);
                    FinalDate = FinalDate.AddMonths(1);
                }
                return View(lstModel);
            }
            else
                return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {

            if (ModelState.IsValid)
            {
                int typedID = Int32.Parse(Request.Form["typedID"].ToString());
                if (typedID == 9999)
                {
                    return RedirectToAction("Index", "Permissions", new { id = typedID });
                }
                else
                {
                    foreach (var item in _context.Employee)
                    {
                        if (item.EmployeeId == typedID)
                            return RedirectToAction("Index", "Permissions", new { id = typedID });
                    }

                }
            }
            return RedirectToAction(nameof(Index));
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
