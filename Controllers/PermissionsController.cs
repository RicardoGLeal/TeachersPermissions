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
    public class PermissionsController : Controller
    {
        private SchoolContext _context;
        public ICollection<Employee> Employees;
        public ICollection<Permission> Permissions;
        public ICollection<EconomicPermissions> eco;
        
        
        public PermissionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Permissions
        //public async Task<IActionResult> Index()
        //{
        //    var schoolContext = _context.Permission.Include(p => p.Employee).Include(p => p.PermissionTypeNavigation);
        //    return View(await schoolContext.ToListAsync());
        //}

        public async Task<IActionResult>Index(int? id)
        {
            if (id == null || id == 9999)
            {
                if(id==9999)
                    ViewBag.id = id;
                    
                var schoolContext = _context.Permission.Include(p => p.Employee).Include(p => p.PermissionTypeNavigation);
                return View(await schoolContext.ToListAsync());
            }
            else
            {
                ViewBag.id = id;
                //ViewBag.name
                var schoolContext = _context.Permission.Include(p => p.Employee).Include(p => p.PermissionTypeNavigation).Where(c => c.EmployeeId == id);
                return View(await schoolContext.ToListAsync());
            }
        }


        // GET: Permissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission
                .Include(p => p.Employee)
                .Include(p => p.PermissionTypeNavigation)
                .FirstOrDefaultAsync(m => m.PermissionId == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // GET: Permissions/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(_context.PermissionTypes, "PermissionTypeId", "PermissionTypeId");
            return View();
        }

        public async Task<IActionResult> Createto(int? id)
        {
            Employees = _context.Employee.Where(c => c.EmployeeId == id).ToList();
            //Permissions = _context.Permission.Where(c => c.EmployeeId == id).ToList();

            ViewBag.Employee = Employees.First();
            ViewBag.RequestDate = DateTime.Now;

            //  Query Utilizado para saber si el Empleado ya utilizó su día de cumpleaños
            int count = _context.BirthdayPermissions.Count(t => t.Permission.EmployeeId == id && t.Permission.PermissionType == 2);
            ViewBag.BirthdayPermissionsCount = count;
            //Query Utilizado para saber cuantos días económicos ha tomado el empleado.
            
            eco = _context.EconomicPermissions.Where(s => s.Permission.EmployeeId == id).ToList();
            ViewBag.EconomicPermissions = eco;
            ViewData["EmployeeId"] = new SelectList(Employees, "EmployeeId", "EmployeeId");
            ViewData["PermissionType"] = new SelectList(_context.PermissionTypes, "PermissionTypeId", "PermissionTypeDesc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermissionId,PermissionType,EmployeeId,Autorize,requestDate")] Permission permission, BirthdayPermissions birthdayPermissions, HoursPermissions hoursPermissions, EconomicPermissions economicPermissions)
        {
            if (ModelState.IsValid)
            {
               // permission.EmployeeId = Int32.Parse((Request.Form["employeeid"].ToString()));
                permission.RequestDate = DateTime.Now;
                permission.Autorize = "En Espera";
                int permissiontype = Int32.Parse((Request.Form["permissiontype"].ToString()));
                int numberOfDays;
                _context.Add(permission);
                await _context.SaveChangesAsync();
                switch (permissiontype)
                {
                    case 1:
                        economicPermissions.PermissionId = permission.PermissionId;
                        economicPermissions.StartDate = Convert.ToDateTime(Request.Form["startdate"].ToString());
                        economicPermissions.FinalDate = Convert.ToDateTime(Request.Form["finaldate"].ToString());
                        numberOfDays = (int)(economicPermissions.FinalDate - economicPermissions.StartDate).Value.TotalDays+1;
                        economicPermissions.NumberOfDays = numberOfDays;
                        _context.Add(economicPermissions);
                        await _context.SaveChangesAsync();
                        break;
                    case 2:                    
                        birthdayPermissions.PermissionId = permission.PermissionId;
                        birthdayPermissions.GrantedDayDate = Convert.ToDateTime(Request.Form["birthdayDayPicker"].ToString());
                        _context.Add(birthdayPermissions);
                        await _context.SaveChangesAsync();
                        break;
                    case 3:
                        hoursPermissions.PermissionId = permission.PermissionId;
                        hoursPermissions.HoursRange = Request.Form["hoursRange"].ToString();
                        hoursPermissions.Reason = Request.Form["hours_reason"].ToString();
                        _context.Add(hoursPermissions);
                        await _context.SaveChangesAsync();
                        break;
                    default:
                        break;
                }    
                if (permission.PermissionType == 1)
                    return RedirectToAction("Index", "", new {id = permission.PermissionId });
                else if (permission.PermissionType == 2)
                    return RedirectToAction("Index", "BirthdayPermissions");
            }
           /* ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", permission.EmployeeId);
            ViewData["PermissionType"] = new SelectList(_context.PermissionTypes, "PermissionTypeId", "PermissionTypeId", permission.PermissionType);*/
            return View(permission);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", permission.EmployeeId);
            ViewData["PermissionType"] = new SelectList(_context.PermissionTypes, "PermissionTypeId", "PermissionTypeId", permission.PermissionType);
            return View(permission);
        }
        public async Task<IActionResult> Validate(int? id)
        {
            var permission = _context.Permission.Where(n => n.PermissionId == id).FirstOrDefault();
            permission.Autorize = "Aprobado";
            _context.Update(permission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Permissions", new { id = 9999 });
        }
        public async Task<IActionResult> Denegate(int? id)
        {
            var permission = _context.Permission.Where(n => n.PermissionId == id).FirstOrDefault();
            permission.Autorize = "Rechazado";
            _context.Update(permission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Permissions", new { id = 9999 });
        }

            
        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermissionId,PermissionType,EmployeeId,Autorize,requestDate")] Permission permission)
        {
            if (id != permission.PermissionId)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionExists(permission.PermissionId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", permission.EmployeeId);
            ViewData["PermissionType"] = new SelectList(_context.PermissionTypes, "PermissionTypeId", "PermissionTypeId", permission.PermissionType);
            return View(permission);
        }

        // GET: Permissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission
                .Include(p => p.Employee)
                .Include(p => p.PermissionTypeNavigation)
                .FirstOrDefaultAsync(m => m.PermissionId == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permission.FindAsync(id);
            _context.Permission.Remove(permission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionExists(int id)
        {
            return _context.Permission.Any(e => e.PermissionId == id);
        }
    }

    public class async
    {
    }
}
