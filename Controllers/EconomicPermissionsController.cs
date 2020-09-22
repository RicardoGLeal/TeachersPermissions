using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachersPermissions.Models;

namespace TeachersPermissions.Controllers
{
    public class EconomicPermissionsController : Controller
    {
        private readonly SchoolContext _context;
        public ICollection<Employee> Employees;
        public ICollection<Permission> Permissions;
        public ICollection<EconomicPermissions> eco;
        public ICollection<PermissionTypes> permissionTypes;


        public EconomicPermissionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: EconomicPermissions
        public async Task<IActionResult> Index(int? id)
        {
            if(id== null)
            {
            var schoolContext = _context.EconomicPermissions.Include(e => e.Permission);
            return View(await schoolContext.ToListAsync());
            }
            else
            {
                var schoolContext = _context.EconomicPermissions.Include(e => e.Permission).Where(n => n.PermissionId == id);
                return View(await schoolContext.ToListAsync());
            }
        }


        public async Task<IActionResult> Createto(int? id)
        {
            permissionTypes = _context.PermissionTypes.Where(c => c.PermissionTypeId == 1).ToList();
            Employees = _context.Employee.Where(c => c.EmployeeId == id).ToList();
            //Permissions = _context.Permission.Where(c => c.EmployeeId == id).ToList();

            ViewBag.Employee = Employees.First();
            ViewBag.RequestDate = DateTime.Now;

            //  Query Utilizado para saber si el Empleado ya utilizó su día de cumpleaños
            int count = _context.BirthdayPermissions.Count(t => t.Permission.EmployeeId == id && t.Permission.PermissionType == 2);
            ViewBag.BirthdayPermissionsCount = count;

            eco = _context.EconomicPermissions.Where(s => s.Permission.EmployeeId == id).ToList();
            ViewBag.EconomicPermissions = eco;


            ViewData["EmployeeId"] = new SelectList(Employees, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(permissionTypes, "PermissionTypeId", "PermissionTypeDesc");
            return View();
        }

        // GET: EconomicPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var economicPermissions = await _context.EconomicPermissions
                .Include(e => e.Permission)
                .FirstOrDefaultAsync(m => m.EconomicPermissionId == id);
            if (economicPermissions == null)
            {
                return NotFound();
            }

            return View(economicPermissions);
        }

        // GET: EconomicPermissions/Create
        public IActionResult Create()
        {
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId");
            return View();
        }

        // POST: EconomicPermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
     
        [HttpPost]
        [ValidateAntiForgeryToken]

        //public async Task<IActionResult> Create([Bind("PermissionId,PermissionType,EmployeeId,Autorize,requestDate,EconomicPermissionId,PermissionId,StartDate,FinalDate,NumberOfDays")] EconomicPermissions economicPermissions, Permission permission)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int numberOfDays;
        //        permission.RequestDate = DateTime.Now;
        //        numberOfDays = (int)(economicPermissions.FinalDate - economicPermissions.StartDate).Value.TotalDays + 1;
        //        economicPermissions.NumberOfDays = numberOfDays;
        //        permission.Autorize = 0;
        //        _context.Add(permission);
        //        await _context.SaveChangesAsync();

        //        economicPermissions.PermissionId = permission.PermissionId;
        //        _context.Add(economicPermissions);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", economicPermissions.PermissionId);
        //    return View(economicPermissions);
        //}


        public async Task<IActionResult> Createto(int? id,[Bind("PermissionId,PermissionType,EmployeeId,Autorize,requestDate,EconomicPermissionId,PermissionId,StartDate,FinalDate,NumberOfDays")] EconomicPermissions economicPermissions, Permission permission)
        {
            if (ModelState.IsValid)
            {
                int numberOfDays;
                permission.RequestDate = DateTime.Now;
                numberOfDays = (int)(economicPermissions.FinalDate - economicPermissions.StartDate).Value.TotalDays + 1;
                economicPermissions.NumberOfDays = numberOfDays;
                permission.Autorize = "En Espera";
                _context.Add(permission);
                await _context.SaveChangesAsync();

                economicPermissions.PermissionId = permission.PermissionId;
                _context.Add(economicPermissions);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("", "Permissions", new { id = permission.EmployeeId });
            }
            permissionTypes = _context.PermissionTypes.Where(c => c.PermissionTypeId == 1).ToList();
            Employees = _context.Employee.Where(c => c.EmployeeId == id).ToList();
            //Permissions = _context.Permission.Where(c => c.EmployeeId == id).ToList();

            ViewBag.Employee = Employees.First();
            ViewBag.RequestDate = DateTime.Now;

            //  Query Utilizado para saber si el Empleado ya utilizó su día de cumpleaños
            int count = _context.BirthdayPermissions.Count(t => t.Permission.EmployeeId == id && t.Permission.PermissionType == 2);
            ViewBag.BirthdayPermissionsCount = count;

            eco = _context.EconomicPermissions.Where(s => s.Permission.EmployeeId == id).ToList();
            ViewBag.EconomicPermissions = eco;


            ViewData["EmployeeId"] = new SelectList(Employees, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(permissionTypes, "PermissionTypeId", "PermissionTypeDesc");
            return View();
        }


        // GET: EconomicPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var economicPermissions = await _context.EconomicPermissions.FindAsync(id);
            if (economicPermissions == null)
            {
                return NotFound();
            }
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", economicPermissions.PermissionId);
            return View(economicPermissions);
        }

        // POST: EconomicPermissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EconomicPermissionId,PermissionId,StartDate,FinalDate,NumberOfDays")] EconomicPermissions economicPermissions)
        {
            if (id != economicPermissions.EconomicPermissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(economicPermissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EconomicPermissionsExists(economicPermissions.EconomicPermissionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", economicPermissions.PermissionId);
            return View(economicPermissions);
        }

        // GET: EconomicPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var economicPermissions = await _context.EconomicPermissions
                .Include(e => e.Permission)
                .FirstOrDefaultAsync(m => m.EconomicPermissionId == id);
            if (economicPermissions == null)
            {
                return NotFound();
            }

            return View(economicPermissions);
        }

        // POST: EconomicPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var economicPermissions = await _context.EconomicPermissions.FindAsync(id);
            _context.EconomicPermissions.Remove(economicPermissions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EconomicPermissionsExists(int id)
        {
            return _context.EconomicPermissions.Any(e => e.EconomicPermissionId == id);
        }
    }
}
