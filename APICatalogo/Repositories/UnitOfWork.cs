using APICatalogo.Context;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository? _produtoRepo;

        private ICategoriaRepository? _categoriaRepo;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        // Propriedade pública que retorna a instância de IProdutoRepository
        public IProdutoRepository ProdutoRepository
        {
            get
            {
                // Verifica se _produtoRepo é nulo. Se for, cria uma nova instância de ProdutoRepository
                // utilizando o contexto _context. Caso contrário, retorna a instância já existente.
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }
        // Propriedade pública que retorna a instância de ICategoriaRepository
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                // Verifica se _categoriaRepo é nulo. Se for, cria uma nova instância de CategoriaRepository
                // utilizando o contexto _context. Caso contrário, retorna a instância já existente.
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
