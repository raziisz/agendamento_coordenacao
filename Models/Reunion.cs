using System;

namespace backend.Models
{
    public class Reunion : Schedule
    {
        public DateTime DateReunion { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }

        public Reunion()
        {
            
        }
        
    }
}