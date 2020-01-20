using adminlte.Models;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class PedidoItem
    {
        public virtual int Sequencia { get; set; }

        public virtual string DescricaoProduto { get; set; }

        public virtual string CodigoProduto { get; set; }
        
        public virtual string DataEntrega { get; set; }

        public virtual int SituacaoItem { get; set; }

        public virtual string CodigoDerivacao { get; set; }

        public virtual string CodigoDep { get; set; }

        public virtual string UnidadeMedida { get; set; }

        public virtual string CodigoTRP { get; set; }

        public virtual double PrecoBase { get; set; }

        public virtual double QuantidadePedido { get; set; }
        
        public virtual double PrecoBruto { get; set; }

        public virtual double PercentualDescontoUsuario { get; set; }

        public virtual double ValorDescontoUsuario { get; set; }

        public virtual double PercentualAcrescimoUsuario { get; set; }

        public virtual double ValorAcrescimoUsuario { get; set; }

        public virtual double PrecoUnitario { get; set; }

        public virtual double ValorBruto { get; set; }

        double _ValorLiquido;
        public virtual double ValorLiquido
        {
            get
            {
                return _ValorLiquido;
            }
            set
            {
                _ValorLiquido = value;
            }
        }

        //private double Arredondar(double _ValorLiquido)
        //{
        //    decimal valorLiquido = (decimal)Math.Round(_ValorLiquido, 5, MidpointRounding.AwayFromZero);
        //    valorLiquido = Math.Round(valorLiquido, 2, MidpointRounding.AwayFromZero);
        //    //valorLiquido = Convert.ToDecimal(String.Format("{0:0.##}", _ValorLiquido));

        //    return (double)valorLiquido;
        //}

        public virtual double ValorLiq { get; set; }

        public virtual int Filial { get; set; }

        public virtual Pedido Pedido { get; set; }

        public virtual int Empresa { get; set; }


        //novos atributos para controle do desconto do item
        public virtual string DataDesconto { get; set; }

        public virtual string HoraDesconto { get; set; }

        public virtual string UsuarioDesconto { get; set; }
        
        public virtual double QtdEstoque { get; set; }

        //public virtual string SomDes { get; set; }

        //public virtual string DesAut { get; set; }

        //public virtual string PerDes { get; set; }

        public virtual double ValorBase { get { return (QuantidadePedido * PrecoBase); } set { } }

        public virtual string CodigoAgrupamento { get; set; }

        public virtual string Agrupamento { get; set; }

        public virtual string codTabela { get; set; }

        public virtual List<Derivacao> ListaDerivacao { get; set; }

        public virtual List<ProdutoDeposito> ListaDepositos { get; set; }

        public virtual string DerivacaoSelecionada { get; set; }

        public virtual string DepositoSelecionado { get; set; }

        public virtual double SalEst { get; set; }

        public virtual string EstDep { get; set; }

        public virtual string Observacao { get; set; }
        
        public virtual double ValorFrete { get; set; }

        public virtual bool RateioFrete { get; set; }

        public virtual string TemDerivacao { get; set; }
    }
}
