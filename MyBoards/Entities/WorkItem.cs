using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities
{
    public class WorkItem
    {
        //[Key] this is a pointer to primary key to the property below.
        public int Id { get; set; }
        public string State { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }
        //Epic
        public DateTime? StartDate { get; set; }
       // [Precision(3)]
        public DateTime? EndDate { get; set; }
        //Issue
        public decimal Efford { get; set; }
        //Task
        public string Activity { get; set; }
        public decimal RemaininWork { get; set; }
        public string Type { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public User Author { get; set; }

        public Guid AuthorId { get; set; }
        public List<Tag> Tags { get; set; }
        
    }
}
