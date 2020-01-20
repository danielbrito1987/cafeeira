using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class PermissaoUsuario
    {
        public virtual int CODUSU { get; set; }

        public virtual string NOMUSU { get; set; }

        public virtual string INTNET { get; set; }

        public virtual int FILATI { get; set; }

        public virtual string SIGFIL { get; set; }

        public virtual int EMPATI { get; set; }

        public virtual string NOMCOM { get; set; }

        public virtual string VENPDS { get; set; }

        public virtual string VENLPV { get; set; }

        public virtual string VENLPF { get; set; }
    }
}
