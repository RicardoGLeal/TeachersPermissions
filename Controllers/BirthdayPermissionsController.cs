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
    public class BirthdayPermissionsController : Controller
    {
        private readonly SchoolContext _context;
        public ICollection<PermissionTypes> permissionTypes;
        public ICollection<Employee> Employees;


        public BirthdayPermissionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: BirthdayPermissions
        public async Task<IActionResult> Index(int? id)
        {
            if (id==null)
            {
            var schoolContext = _context.BirthdayPermissions.Include(b => b.Permission);
            return View(await schoolContext.ToListAsync());
            }
            else
            {
                var schoolContext = _context.BirthdayPermissions.Include(b => b.Permission).Where(b => b.PermissionId == id);
                return View(await schoolContext.ToListAsync());
            }
        }

        public async Task<IActionResult> Createto(int? id)
        {
            permissionTypes = _context.PermissionTypes.Where(c => c.PermissionTypeId == 2).ToList();
            Employees = _context.Employee.Where(c => c.EmployeeId == id).ToList();
            //Permissions = _context.Permission.Where(c => c.EmployeeId == id).ToList();

            ViewBag.Employee = Employees.First();
            ViewBag.RequestDate = DateTime.Now;

            int count = _context.BirthdayPermissions.Count(t => t.Permission.EmployeeId == id && t.Permission.PermissionType == 2);
            ViewBag.BirthdayPermissionsCount = count;

            ViewData["EmployeeId"] = new SelectList(Employees, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(permissionTypes, "PermissionTypeId", "PermissionTypeDesc");
            return View();
        }



        // GET: BirthdayPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthdayPermissions = await _context.BirthdayPermissions
                .Include(b => b.Permission)
                .FirstOrDefaultAsync(m => m.BirthdayPermissionId == id);
            if (birthdayPermissions == null)
            {
                return NotFound();
            }

            return View(birthdayPermissions);
        }

        // GET: BirthdayPermissions/Create
        public IActionResult Create()
        {
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId");
            return View();
        }

        // POST: BirthdayPermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createto([Bind("PermissionId,PermissionType,EmployeeId,Autorize,requestDate,BirthdayPermissionId,PermissionId,GrantedDayDate")] BirthdayPermissions birthdayPermissions, Permission permission)
        {
            if (ModelState.IsValid)
            {
                permission.RequestDate = DateTime.Now;
                permission.Autorize = "En Espera";
                _context.Add(permission);
                await _context.SaveChangesAsync();

                birthdayPermissions.PermissionId = permission.PermissionId;
                _context.Add(birthdayPermissions);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("", "Permissions", new { id = permission.EmployeeId });
            }
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", birthdayPermissions.PermissionId);
            return View(birthdayPermissions);
        }

        // GET: BirthdayPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthdayPermissions = await _context.BirthdayPermissions.FindAsync(id);
            if (birthdayPermissions == null)
            {
                return NotFound();
            }
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", birthdayPermissions.PermissionId);
            return View(birthdayPermissions);
        }

        // POST: BirthdayPermissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BirthdayPermissionId,PermissionId,GrantedDayDate")] BirthdayPermissions birthdayPermissions)
        {
            if (id != birthdayPermissions.BirthdayPermissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(birthdayPermissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirthdayPermissionsExists(birthdayPermissions.BirthdayPermissionId))
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
            ViewData["PermissionId"] = new SelectList(_context.Permission, "PermissionId", "PermissionId", birthdayPermissions.PermissionId);
            return View(birthdayPermissions);
        }

        // GET: BirthdayPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthdayPermissions = await _context.BirthdayPermissions
                .Include(b => b.Permission)
                .FirstOrDefaultAsync(m => m.BirthdayPermissionId == id);
            if (birthdayPermissions == null)
            {
                return NotFound();
            }

            return View(birthdayPermissions);
        }

        // POST: BirthdayPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birthdayPermissions = await _context.BirthdayPermissions.FindAsync(id);
            _context.BirthdayPermissions.Remove(birthdayPermissions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirthdayPermissionsExists(int id)
        {
            return _context.BirthdayPermissions.Any(e => e.BirthdayPermissionId == id);
        }
    }
}
