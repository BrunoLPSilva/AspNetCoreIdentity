using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityProject.Context;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Authorization;

namespace IdentityProject.Controllers
{
    // [Authorize(Roles = "Admin")]
    [Authorize]
    public class AlunosController : Controller
    {
        private readonly AppDbContext _context;

        public AlunosController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Alunos
        public async Task<IActionResult> Index()
        {
            var alunos = await _context.Alunos.ToListAsync();
            return View(alunos);
        }

        [Authorize(Roles = "User, Admin, Gerente")]
        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        
        // GET: Alunos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alunos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoId,Nome,Email,Idade,Curso")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        [Authorize(Roles = "Admin, Gerente")]
        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlunoId,Nome,Email,Idade,Curso")] Aluno aluno)
        {
            if (id != aluno.AlunoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.AlunoId))
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
            return View(aluno);
        }

       // [Authorize(Roles = "Admin")]

        // GET: Alunos/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.AlunoId == id);
        }
    }
}
