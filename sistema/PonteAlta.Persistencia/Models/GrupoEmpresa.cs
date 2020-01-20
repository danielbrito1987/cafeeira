using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class GrupoEmpresa
    {
        public virtual int Codigo { get; set; }

        public virtual string Nome { get; set; }

        public virtual DateTime DataDemissao { get; set; }

        public virtual int CodigoBloqueio { get; set; }

        public virtual string DescricaoBloqueio { get; set; }

        public virtual string TituloVencido { get; set; }

        public virtual int Empresa { get; set; }

        public virtual string DescBloqueio
        {
            get
            {
                return (DescricaoBloqueio != null) ? DescricaoBloqueio.ToString() : "Não";
            }
        }
        public virtual string Bloqueado
        {
            get
            {
                return (CodigoBloqueio > 0) ? "Sim" : "Não";
            }
        }

        public virtual string DescTituloVencido
        {
            get
            {
                return TituloVencido.ToString() == "S" ? "Sim" : "Não";
            }
        }
    }
}
