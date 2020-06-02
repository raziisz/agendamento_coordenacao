using System;

namespace backend.Models
{
    public class Project : Schedule
    {
        public DateTime StartProject { get; set; }
        public DateTime EndProject { get; set; }

        public Project()
        {
            
        }
    }
}