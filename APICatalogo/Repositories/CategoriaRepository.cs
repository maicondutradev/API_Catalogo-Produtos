using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    //NO REPOSITORY DEVE SEMPRE SE MANTER A LOGICA DE ACESSO A DADOS.
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context) 
        {
        }
    }
}
