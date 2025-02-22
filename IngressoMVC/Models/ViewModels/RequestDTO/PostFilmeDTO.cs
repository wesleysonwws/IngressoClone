﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Models.ViewModels.RequestDTO
{
    public class PostFilmeDTO
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImageURL { get; set; }

        #region relacionamentos
        public string NomeCinema { get; set; }

        public int ProdutorId { get; set; }

        public List<int> AtoresId { get; set; }
        public List<string> Categorias { get; set; } //Por Id
        #endregion
    }
}
