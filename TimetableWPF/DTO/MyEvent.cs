using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableWPF.DTO
{
    public class MyEvent
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public Color Color { get; set; }

        public static MyEvent createMyEvent(string name, DateTime date, Color color)
        {
            MyEvent myEvent = new MyEvent();
            myEvent.Id = Guid.NewGuid();
            myEvent.Name = name;
            myEvent.Date = date;
            myEvent.Color = color;

            return myEvent;
        }
    }
}
