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
    public class HoursPermissionsController : Controller
    {
        private readonly SchoolContext _context;
        public ICollection<Employee> Employees;
        public ICollection<Permission> Permissions;
        public ICollection<HoursPermissions> hours;
        public ICollection<PermissionTypes> permissionTypes;

        public HoursPermissionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: HoursPermissions
        public async Task<IActionResult> Index(int? id)
        {
            if(id==null)
            {
            var schoolContext = _context.HoursPermissions.Include(h => h.Permission);
            return View(await schoolContext.ToListAsync());
            }
            else
            {
                var schoolContext = _context.HoursPermissions.Include(h => h.Permission).Where(n => n.PermissionId == id);
                return View(await schoolContext.ToListAsync());
            }
        }

        public async Task<IActionResult> Createto(int? id)
        {
            permissionTypes = _context.PermissionTypes.Where(c => c.PermissionTypeId == 3).ToList();
            Employees = _context.Employee.Where(c => c.EmployeeId == id).ToList();
            Permissions = _context.Permission.Where(c => c.EmployeeId == id).ToList();


             DateTime inicioQuincena, finQuincena; 
            if(DateTime.Now.Day > 0 && DateTime.Now.Day < 15)
            {
                inicioQuincena = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                finQuincena = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
            }
            else
            {
                inicioQuincena = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
                finQuincena = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
            }
            ViewBag.inicioQuincena = inicioQuincena;
            ViewBag.finQuincena = finQuincena;
            //Numero de Permisos en la Quincena
            var filteredData = _context.HoursPermissions.Where(t => t.Day >= inicioQuincena && t.Day < finQuincena && t.Permission.PermissionType==3 && t.Permission.EmployeeId == id);
            var QuincenaHoursPermissions = filteredData;
            ViewBag.QuincenaCount = QuincenaHoursPermissions.Count();


            DateTime a;
            a = DateTime.Now.AddMonths(1);
            hours = _context.HoursPermissions.Where(s => s.Permission.EmployeeId == id).ToList();

            ViewBag.Employee = Employees.First();
            ViewBag.RequestDate = DateTime.Now;

            ViewData["EmployeeId"] = new SelectList(Employees, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(permissionTypes, "PermissionTypeId", "PermissionTypeDesc");
            return View();
        }

        // GET: HoursPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoursPermissions = await _context.HoursPermissions
                .Include(h => h.Permission)
                .FirstOrDefaultAsync(m => m.HoursPermissionsId == id);
            if (hoursPermissions == null)
            {
                return NotFound();
            }

            return View(hoursPermissions);
        }

        // GET: HoursPermissions/Create
        public IActionResult Create()
        {
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId");
            return View();
        }

        // POST: HoursPermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoursPermissionsId,HoursRange,PermissionId,Reason,Day")] HoursPermissions hoursPermissions, Permission permission)
        {
            if (ModelState.IsValid)
            {

                permission.RequestDate = DateTime.Now;
                permission.Autorize = "En Espera";
                _context.Add(permission);
                await _context.SaveChangesAsync();

                hoursPermissions.PermissionId = permission.PermissionId;
                hoursPermissions.HoursRange = Request.Form["hoursRange"].ToString();
                hoursPermissions.Reason = Request.Form["hours_reason"].ToString();
                hoursPermissions.Day = Convert.ToDateTime(Request.Form["date"].ToString());

                _context.Add(hoursPermissions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Permissions", new { id = permission.EmployeeId });

            }
            return RedirectToAction("Createto", "Permissions", new { id = permission.EmployeeId });
        }
    

    // GET: HoursPermissions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hoursPermissions = await _context.HoursPermissions.FindAsync(id);
        if (hoursPermissions == null)
        {
            return NotFound();
        }
        ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", hoursPermissions.PermissionId);
        return View(hoursPermissions);
    }

    // POST: HoursPermissions/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("HoursPermissionsId,HoursRange,PermissionId,Reason,Day")] HoursPermissions hoursPermissions)
    {
        if (id != hoursPermissions.HoursPermissionsId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hoursPermissions);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoursPermissionsExists(hoursPermissions.HoursPermissionsId))
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
        ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", hoursPermissions.PermissionId);
        return View(hoursPermissions);
    }

    // GET: HoursPermissions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hoursPermissions = await _context.HoursPermissions
            .Include(h => h.Permission)
            .FirstOrDefaultAsync(m => m.HoursPermissionsId == id);
        if (hoursPermissions == null)
        {
            return NotFound();
        }

        return View(hoursPermissions);
    }

    // POST: HoursPermissions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hoursPermissions = await _context.HoursPermissions.FindAsync(id);
        _context.HoursPermissions.Remove(hoursPermissions);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HoursPermissionsExists(int id)
    {
        return _context.HoursPermissions.Any(e => e.HoursPermissionsId == id);
    }
}
}
