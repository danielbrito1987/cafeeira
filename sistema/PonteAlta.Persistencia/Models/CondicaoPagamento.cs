using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Models
{
    public class CondicaoPagamento
    {
        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Abreviacao { get; set; }

        public virtual int TipoParcela { get; set; } //3 parcelas diferentes, 1 ou 2 sao parcelas 

        public virtual int QuantidadeParcelas { get; set; } //quantidade de parcelas a serem trabalhadas
        
        public virtual string TipoEspecial { get; set; } // "S" = TSIM e "N" = NÃO
        
        public virtual string ManutencaoParcela { get; set; }

        public virtual decimal TaxaAcrescimoFinanceiro { get; set; }

        public virtual decimal TaxaJuros { get; set; }

        public virtual string TipoJuros { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }

        public virtual int Empresa { get; set; }

    }
}