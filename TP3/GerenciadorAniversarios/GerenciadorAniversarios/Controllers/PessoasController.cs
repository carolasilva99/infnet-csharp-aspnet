using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorAniversarios.Data;
using GerenciadorAniversarios.Models;

namespace GerenciadorAniversarios.Controllers
{
    public class PessoasController : Controller
    {
        private readonly GerenciadorAniversariosContext _context;

        public PessoasController(GerenciadorAniversariosContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        public async Task<IActionResult> Index(string ordenacao, string pesquisa)
        {
            ViewBag.OrdenacaoNome = string.IsNullOrEmpty(ordenacao) ? "nome_desc" : "";
            ViewBag.OrdenacaoDias = string.IsNullOrEmpty(ordenacao) ? "dias_asc" : "";

            var pessoas = await _context.Pessoa.ToListAsync();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                pessoas = pessoas.Where(pessoa => pessoa.NomeContem(pesquisa)).ToList();
            }

            switch (ordenacao)
            {
                case "nome_desc":
                    pessoas = pessoas.OrderByDescending(pessoa => pessoa.Nome).ToList();
                    break;
                case "dias_asc":
                    pessoas = pessoas.OrderBy(pessoa => pessoa.DiasParaProximoAniversario()).ToList();
                    break;
                case "dias_desc":
                    pessoas = pessoas.OrderByDescending(pessoa => pessoa.DiasParaProximoAniversario()).ToList();
                    break;
                default:
                    pessoas = pessoas.OrderBy(pessoa => pessoa.Nome).ToList();
                    break;
            }

            return View(pessoas);
        }

        // GET: Pessoas/Detalhes/5
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Adicionar
        public IActionResult Adicionar()
        {
            return View();
        }

        // POST: Pessoas/Adicionar
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar([Bind("Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Editar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoaExiste(pessoa.Id))
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
            return View(pessoa);
        }

        // GET: Pessoas/Excluir/5
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'GerenciadorAniversariosContext.Pessoa'  is null.");
            }
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa != null)
            {
                _context.Pessoa.Remove(pessoa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PessoaExiste(int id)
        {
          return _context.Pessoa.Any(e => e.Id == id);
        }
    }
}
