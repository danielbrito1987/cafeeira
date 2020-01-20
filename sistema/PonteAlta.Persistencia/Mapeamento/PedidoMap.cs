using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Table("USU_VE120PED");

            Id(u => u.Codigo, "NUMPED");//.GeneratedBy.Assigned();

            Map(u => u.CodEmpresa, "CODEMP");
            Map(u => u.CodFilial, "CODFIL");

            Map(g => g.SituacaoPedido, "SITPED").Nullable();
            Map(g => g.DescSituacaoPedido, "SITDES");

            Map(g => g.DataEmissao, "DATEMI").Nullable().Precision(4);
            Map(g => g.HoraEmissao, "HOREMI").Nullable();

            Map(u => u.CodGrupoEmpresa, "CODGRE");
            Map(g => g.CodCliente, "CODCLI");
            Map(g => g.NomeCliente, "NOMCLI");

            Map(g => g.CodTransacao, "TNSPRO");
            Map(g => g.DescTransacao, "DESTNS");

            Map(u => u.CodFormaPagamento, "CODFPG");
            Map(u => u.DescFormaPagamento, "DESFPG");

            Map(u => u.CodCondicaoPagamento, "CODCPG");
            Map(u => u.DescCondicaoPagamento, "DESCPG");

            Map(g => g.CodRepresentante, "CODREP");
            Map(g => g.NomeRepresentante, "NOMREP");

            Map(g => g.CodigoTransportadora, "CODTRA").Nullable();
            Map(g => g.NomeTransportadora, "NOMTRA").Nullable();
            Map(g => g.NomePlaca, "PLAVEI").Nullable();
            Map(g => g.CIFFOB, "CIFFOB").Nullable();
            Map(g => g.ValorFrete, "VLRFRE").Nullable();
                        
            Map(g => g.ObsPedido, "OBSPED").Nullable();
            Map(g => g.ObsEntrega, "OBSENT").Nullable();
            Map(g => g.ObsAprovacao, "OBSMOT").Nullable();

            Map(g => g.ValorBruto, "VLRBPR").Nullable();
            Map(g => g.ValorDesconto, "VLRDSC").Nullable();
            Map(g => g.ValorAcrescimo, "VLRACR").Nullable();
            Map(g => g.ValorLiquido, "VLRLIQ").Nullable();

            Map(g => g.PodeAlterar, "PODALT").Nullable();
            Map(g => g.BloqueioPedido, "PEDBLO").Nullable();

            Map(g => g.DataIniJuros, "INIJUR");
            Map(g => g.TaxaJuros, "TXAJUR");
            Map(g => g.AcrFin, "ACRFIN");            
        }
    }
}
