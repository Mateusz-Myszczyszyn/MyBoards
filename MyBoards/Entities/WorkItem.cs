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
    public class Epic : WorkItem
    {
        //Epic
        public DateTime? StartDate { get; set; }
        // [Precision(3)]
        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        //Issue
        public decimal Efford { get; set; }
    }

    public class Task : WorkItem
    {
        //Task
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }
    }
    public abstract class WorkItem
    {
        //[Key] this is a pointer to primary key to the property below.
        public int Id { get; set; }

        public string Area { get; set; }

        public string IterationPath { get; set; }

        public int Priority { get; set; }
        
        public  List<Comment> Comments { get; set; } = new List<Comment>();

        public  User Author { get; set; }

        public Guid AuthorId { get; set; }

        public  List<Tag> Tags { get; set; }

        public  WorkItemState State { get; set; }

        public int StateId { get; set; }


    }
}
