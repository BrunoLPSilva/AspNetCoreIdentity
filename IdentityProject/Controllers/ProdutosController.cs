using IdentityProject.Context;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityProject.Context;
using IdentityProject.Entities;

namespace MvcWebIdentity.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return _context.Produtos != null ?
                    View(await _context.Produtos.ToListAsync()) :
                    Problem("Entity set 'AppDbContext.Produtos' é null.");
    }

    [Authorize(Policy = "TempoCadastroMinimo")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(m => m.ProdutoId == id);
        if (produto == null)
        {
            return NotFound();
        }
        return View(produto);
    }

    [Authorize(Policy = "TempoCadastroMinimo")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdutoId,Nome,Preco")] Produto produto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    [Authorize(Policy = "TempoCadastroMinimo")]
   
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }
        return View(produto); // Certifique-se de passar um único Produto
    }



    // POST: Produtos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,Nome,Preco")] Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto.ProdutoId))
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
        return View(produto); // Certifique-se de passar um único Produto
    }


    [Authorize(Policy = "TempoCadastroMinimo")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(m => m.ProdutoId == id);

        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    // POST: Produtos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Produtos == null)
        {
            return Problem("Entity set 'AppDbContext.Produtos'  is null.");
        }
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(int id)
    {
        return (_context.Produtos?.Any(e => e.ProdutoId == id)).GetValueOrDefault();
    }
}

