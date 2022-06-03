using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace IngressoMVC.Controllers
{
    public class AtoresController : Controller
    {
        private IngressoDbContext _context;

        public AtoresController(IngressoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Atores);
        }
                
        public IActionResult Detalhes(int id)
        {
            return View(_context.Atores.Find(id));
        }

        public IActionResult Criar()
        {
            return View();
        }
                

        [HttpPost]
        public IActionResult Criar(PostAtorDTO atorDto)
        {
            //validar os dados
            if(!ModelState.IsValid || !atorDto.FotoPerfilURL.EndsWith(".jpg"))
            {
                return View(atorDto);
            }

            //instanciar novo ator
            Ator ator = new Ator(atorDto.Nome, atorDto.Bio, atorDto.FotoPerfilURL);

            //gravar esse ator no banco de dados
            _context.Atores.Add(ator);

            //salvar as mudanças
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Atualizar(int id)
        {
            //buscar o ator no banco
            //passar o ator na view
            return View();
        }
         
        public IActionResult Deletar(int id)
        {
            //buscar o ator no banco
            //passar o ator na view
            return View();
        }

    }
}
