using APICatalogo.Context;
using APICatalogo.Models;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    //NO REPOSITORY DEVE SEMPRE SE MANTER A LOGICA DE ACESSO A DADOS.
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }
        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
        
    }
}
