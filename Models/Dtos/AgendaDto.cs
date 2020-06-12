using System;

namespace agendamento_coordenacao.Models.Dtos
{
    public class AgendaDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Reschedule { get; set; }
        public string Local { get; set; }
        public string Tipo { get; set; }
        //Reunião
        public DateTime? DateReunion { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        //Projeto
        public DateTime? StartProject { get; set; }
        public DateTime? EndProject { get; set; }
        //Tarefa
        public DateTime? DateWork { get; set; }

        public AgendaDto()
        {
            
        }
    }
}