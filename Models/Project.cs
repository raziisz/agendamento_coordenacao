using System;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Models
{
    public class Project : Schedule
    {
        public DateTime? StartProject { get; set; }
        public DateTime? EndProject { get; set; }

        public Project()
        {
            
        }
        public Project(AgendaDto agenda)
        {
            this.Local = agenda.Local;
            this.Reschedule = agenda.Reschedule;
            this.Title = agenda.Title;
            this.Description = agenda.Description;
            this.UserId = agenda.UserId;
            this.StartProject = agenda.StartProject;
            this.EndProject = agenda.EndProject;
        }
    }
}