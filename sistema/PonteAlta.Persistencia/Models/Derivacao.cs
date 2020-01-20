using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class Derivacao
    {
        public virtual int CodEmpresa { get; set; }

        public virtual string CodProduto { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Nome { get; set; }

        public virtual string DESCPL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var t = obj as Derivacao;

            if (t == null)
                return false;

            if (CodEmpresa == t.CodEmpresa &&
                CodProduto == t.CodProduto &&
                Codigo == t.Codigo)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return string.Concat(CodEmpresa, CodProduto, Codigo).GetHashCode();
        }
    }
}
