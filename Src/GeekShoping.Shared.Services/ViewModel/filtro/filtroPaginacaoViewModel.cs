namespace GeekShoping.Shared.Services.ViewModel.filtro
{
    public class filtroPaginacaoViewModel
    {
        private int _paginaAtual;
        private int _quantidadePorPagina;

        public int PaginaAtual
        {
            get { return _paginaAtual; }
            set { _paginaAtual = value.Equals(0) ? 1 : value; }
        }
        public int QuantidadePorPagina
        {
            get { return _quantidadePorPagina; }
            set { _quantidadePorPagina = value.Equals(0) ? 50 : value; }
        }
    }
}
