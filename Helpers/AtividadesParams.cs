namespace agendamento_coordenacao.Helpers
{
    public class AtividadesParams
    {
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 20;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
    }
}