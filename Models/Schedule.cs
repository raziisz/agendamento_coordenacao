namespace agendamento_coordenacao.Models
{
    public abstract class Schedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Reschedule { get; set; }
        public string Local { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Schedule()
        {
            
        }
    }
}