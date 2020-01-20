using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class ProdutoDeposito
    {
        public virtual int Empresa { get; set; }

        public virtual int Filial { get; set; }

        public virtual string Produto { get; set; }

        public virtual string Derivacao { get; set; }

        public virtual string Deposito { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as ProdutoDeposito;
            if (t == null)
                return false;
            if (Empresa == t.Empresa &&
                Filial == t.Empresa &&
                Derivacao == t.Derivacao &&
                Deposito == t.Deposito &&
                Produto == t.Produto)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (string.Concat(Empresa, Filial, Derivacao, Deposito, Produto)).GetHashCode();
        }
    }
}
