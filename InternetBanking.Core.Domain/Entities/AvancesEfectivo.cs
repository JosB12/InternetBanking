﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class AvancesEfectivo
    {
        public int Id { get; set; }  
        public decimal Monto { get; set; }
        public decimal Interes { get; set; }

        public int IdTarjetaCredito { get; set; }  
        public TarjetasCredito TarjetaCredito { get; set; }

        public int IdCuentaDestino { get; set; }  
        public CuentasAhorro CuentaDestino { get; set; }
    }
}