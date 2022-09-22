using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class PedidoItemMap : ClassMap<PedidoItem>
    {
        public PedidoItemMap()
        {
            Table("USU_VE120IPD");

            Id(u => u.Sequencia, "SEQIPD");//.GeneratedBy.Assigned();

            Map(g => g.SituacaoItem, "SITIPD").Nullable();
            Map(g => g.CodigoProduto, "CODPRO").Nullable();
            Map(g => g.CodigoDerivacao, "CODDER").Nullable();
            Map(g => g.DescricaoProduto, "CPLIPD").Nullable();
            Map(g => g.CodigoDep, "CODDEP").Nullable();
            Map(g => g.UnidadeMedida, "UNIMED").Nullable();
            Map(g => g.DataEntrega, "DATENT").Nullable().Precision(4);
            Map(g => g.CodigoTRP, "CODTPR").Nullable();
            Map(g => g.PrecoBase, "PREBAS").Nullable();
            Map(g => g.QuantidadePedido, "QTDPED").Nullable();
            Map(g => g.QtdEstoque, "QTDEST").Nullable();
            
            Map(g => g.PrecoBruto, "PREBRU").Nullable();
            Map(g => g.PercentualDescontoUsuario, "USU_PERDSC").Nullable();
            Map(g => g.ValorDescontoUsuario, "USU_VLRDSC").Nullable();
            Map(g => g.PercentualAcrescimoUsuario, "USU_PERACR").Nullable();
            Map(g => g.ValorAcrescimoUsuario, "USU_VLRACR").Nullable();
            Map(g => g.PercentualOferta, "PEROFE");
            Map(g => g.PrecoUnitario, "PREUNI").Nullable();
            Map(g => g.ValorBruto, "VLRBRU").Nullable();
            Map(g => g.ValorLiquido, "VLRLIQ").Nullable();
            Map(g => g.ValorLiq, "VLRLIQ").Nullable();

            Map(g => g.ValorFrete, "VLRFRE").Nullable();
            Map(g => g.RateioFrete, "RATFRE").Nullable();

            Map(g => g.UsuarioDesconto, "USU_USUDSC").Nullable();
            Map(g => g.DataDesconto, "USU_DATDSC").Nullable();
            Map(g => g.HoraDesconto, "USU_HORDSC").Nullable();
            
            Map(u => u.Empresa, "CODEMP");
            Map(u => u.Filial, "CODFIL");

            References(u => u.Pedido, "NUMPED");
        }
    }
}
