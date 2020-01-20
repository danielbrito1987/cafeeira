using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Tree
{
    public class Node
    {
        public Node()
        {
            NodesFilhos = new List<Node>();
            //children = new List<Node>();
        }

        public int Codigo { get; set; }

        public string Descricao { get; set; }

        //public string name { get; set; }

        public int? NodePai { get; set; }

        public bool Ativo { get; set; }

        public IList<Node> NodesFilhos { get; set; }

        //public IList<Node> children { get; set; }

        //public string CodigoEmpresa { get; set; }
    }
}
