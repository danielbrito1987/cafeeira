using adminlte.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Models
{
    public class Pedido
    {
        public virtual int Codigo { get; set; }

        public virtual int SituacaoPedido { get; set; }

        public virtual string DescSituacaoPedido { get; set; }

        public virtual DateTime DataEmissao { get; set; }

        public virtual int HoraEmissao { get; set; }
        
        public virtual string PodeAlterar { get; set; }

        public virtual string CodigoTransportadora { get; set; }

        public virtual string NomeTransportadora { get; set; }

        public virtual string NomePlaca { get; set; }

        public virtual string CIFFOB { get; set; }

        public virtual double ValorFrete { get; set; }

        public virtual string ObsPedido { get; set; }

        public virtual string ObsEntrega { get; set; }
        
        public virtual int CodRepresentante { get; set; }

        public virtual string NomeRepresentante { get; set; }

        public virtual int CodFilial { get; set; }

        public virtual string CodCondicaoPagamento { get; set; }

        public virtual string DescCondicaoPagamento { get; set; }

        public virtual int CodFormaPagamento { get; set; }

        public virtual string DescFormaPagamento { get; set; }

        public virtual int CodTransacao { get; set; }

        public virtual string DescTransacao { get; set; }

        public virtual int CodEmpresa { get; set; }

        public virtual int CodGrupoEmpresa { get; set; }

        public virtual int CodCliente { get; set; }

        public virtual string NomeCliente { get; set; }
        
        public virtual double ValorBruto { get; set; }

        public virtual double ValorDesconto { get; set; }

        public virtual double ValorAcrescimo { get; set; }

        public virtual double ValorLiquido { get; set; }

        public virtual string ObsAprovacao { get; set; }

        public virtual string BloqueioPedido { get; set; }
        
        public virtual DateTime DataIniJuros { get; set; }

        public virtual double TaxaJuros { get; set; }

        public virtual double AcrFin { get; set; }
    }
}

/* Situação do pedido
 * Lista: 
 * 1 = Aberto total,
 * 2 = Aberto parcial, 
 * 3 = Suspenso,
 * 4 = Liquidado,
 * 5 = Cancelado, 
 * 6 = Aguardando integração WMAS,
 * 7 = Em transmissão, 
 * 8 = Preparação análise ou NF, 
 * 9 = Não fechado --> Está em modo de edição. O recálculo é feito somente neste status.
 * 9 = Não fechado*/
