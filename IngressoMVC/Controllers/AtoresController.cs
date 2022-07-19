using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.RequestDTO;
using IngressoMVC.Models.ViewModels.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IngressoMVC.Controllers
{
    public class AtoresController : Controller
    {
        private IngressoDbContext _context;

        public AtoresController(IngressoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Atores);

        public IActionResult Detalhes(int id) => View(AtorFilmes(id));

        public GetAtorDto AtorFilmes(int id)
        {
            #region Método 1 - Recupera a entidade, Instancia o objeto passando os dados 
            var ator = _context.Atores.Include(b => b.AtoresFilmes).ThenInclude(f => f.Filme).FirstOrDefault(at => at.Id == id);

            GetAtorDto result = new GetAtorDto()
            {
                Nome = ator.Nome,
                Bio = ator.Bio,
                FotoPerfilURL = ator.FotoPerfilURL,
                FilmeFotoURL = ator.AtoresFilmes.Select(af => af.Filme.ImageURL).ToList(),
                TituloFilmes = ator.AtoresFilmes.Select(af => af.Filme.Titulo).ToList()
            };
            #endregion

            #region Método 2 - Recupera o objeto passando os dados para o objeto instanciado
            //var result = _context.Atores.Where(ator => ator.Id == id)
            //    .Select(at => new GetAtorDto()
            //    {
            //        Bio = at.Bio,
            //        FotoPerfilURL = at.FotoPerfilURL,
            //        Nome = at.Nome,
            //        TituloFilmes = at.AtoresFilmes.Select(fm => fm.Filme.Titulo).ToList(),
            //        FilmeFotoURL = at.AtoresFilmes.Select(fm => fm.Filme.ImageURL).ToList()
            //    }).FirstOrDefault();
            #endregion

            #region Método 3 - Sql Pura            
            // _context.Atores.FromSqlRaw("select * from Atores inner join AtoresFilmes on Atores.id = AtoresFilmes.AtorId inner join Filmes on Filmes.Id = AtoresFilmes.FilmeId where Atores.Id = @id ");
            #endregion
            return result;
        }

        public IActionResult Criar() => View();

        [HttpPost]
        public IActionResult Criar(PostAtorDTO atorDto)
        {
            //validar os dados
            if (!ModelState.IsValid)
                return View(atorDto);

            //instanciar novo ator
            Ator ator = new Ator(atorDto.Nome, atorDto.Bio, atorDto.FotoPerfilURL);

            //gravar esse ator no banco de dados
            _context.Atores.Add(ator);

            //salvar as mudanças
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Atualizar(int? id)
        {
            if (id == null)
                return NotFound();

            //buscar o ator no banco
            var result = _context.Atores.FirstOrDefault(a => a.Id == id);

            if (result == null)
                return View();

            //passar o ator na view
            return View(result);
        }

        [HttpPost]
        public IActionResult Atualizar(int id, PostAtorDTO atorDto)
        {
            var ator = _context.Atores.FirstOrDefault(a => a.Id == id);

            if (!ModelState.IsValid)
                return View(ator);

            ator.AtualizarDados(atorDto.Nome, atorDto.Bio, atorDto.FotoPerfilURL);

            _context.Update(ator);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Deletar(int id)
        {
            var result = _context.Atores.FirstOrDefault(a => a.Id == id);

            if (result == null) return View();

            return View(result);
        }

        [HttpPost, ActionName("Deletar")]
        public IActionResult ConfirmarDeletar(int id)
        {
            var result = _context.Atores.FirstOrDefault(a => a.Id == id);
            _context.Atores.Remove(result);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
