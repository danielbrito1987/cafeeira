using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Models
{
    public class Cliente
    {
        public virtual int Codigo { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Apelido { get; set; }
        
        public virtual string Situacao { get; set; }

        public virtual string CPF { get; set; }

        public virtual string InscricaoEstadual { get; set; }

        public virtual string Endereco { get; set; }

        //public virtual Grupo Grupo { get; set; }

        //private int codigoGrupo;

        //private string descGrupo;

        public virtual int CodGrupo { get; set; }

        //public virtual int CodGrupo
        //{
        //    get
        //    {
        //        return Grupo != null ? Grupo.Codigo : codigoGrupo;
        //    }

        //    set
        //    {
        //        codigoGrupo = value;
        //    }
        //}

        //public virtual string DescGrupo
        //{
        //    get
        //    {
        //        return Grupo != null ? Grupo.Nome : descGrupo;
        //    }

        //    set
        //    {
        //        descGrupo = value;
        //    }
        //}
    }
}