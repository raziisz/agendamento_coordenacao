using System;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Models
{
    public class Reunion : Schedule
    {
        public DateTime? DateReunion { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }

        public Reunion()
        {
            
        }
        public Reunion(AgendaDto agenda)
        {
            this.Local = agenda.Local;
            this.Reschedule = agenda.Reschedule;
            this.Title = agenda.Title;
            this.Description = agenda.Description;
            this.HourEnd = agenda.HourEnd;
            this.HourStart = agenda.HourStart;
            this.DateReunion = agenda.DateReunion;
            this.UserId = agenda.UserId;
        }
        
    }
}