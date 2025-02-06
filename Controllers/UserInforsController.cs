using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeLeapChallengeAPI_06022025.Data.Class;
using CodeLeapChallengeAPI_06022025.Data.Context;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    public class UserInforsController : Controller
    {
        private readonly CodeDBContext _context;

        public UserInforsController(CodeDBContext context)
        {
            _context = context;
        }

        // GET: UserInfors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: UserInfors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (userInfor == null)
            {
                return NotFound();
            }

            return View(userInfor);
        }

        // GET: UserInfors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInfor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfor);
        }

        // GET: UserInfors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users.FindAsync(id);
            if (userInfor == null)
            {
                return NotFound();
            }
            return View(userInfor);
        }

        // POST: UserInfors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            if (id != userInfor.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInforExists(userInfor.UserName))
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
            return View(userInfor);
        }

        // GET: UserInfors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (userInfor == null)
            {
                return NotFound();
            }

            return View(userInfor);
        }

        // POST: UserInfors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userInfor = await _context.Users.FindAsync(id);
            if (userInfor != null)
            {
                _context.Users.Remove(userInfor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInforExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }
    }
}
