﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class TarjetasCredito
    {
        public int Id { get; set; }  
        public string IdentificadorUnico { get; set; } 
        public string NumeroTarjeta { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal DeudaActual { get; set; }

        public int IdProductoFinanciero { get; set; }  
        public ProductosFinancieros ProductoFinanciero { get; set; }
    }
}