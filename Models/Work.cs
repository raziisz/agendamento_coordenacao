using System;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Models
{
    public class Work : Schedule
    {
        public DateTime? DateWork { get; set; }

        public Work()
        {
            
        }
        
        public Work(AgendaDto agenda)
        {
            this.Local = agenda.Local;
            this.Reschedule = agenda.Reschedule;
            this.Title = agenda.Title;
            this.Description = agenda.Description;
            this.UserId = agenda.UserId;
            this.DateWork = agenda.DateWork;
        }
    }
}