using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimetableWPF.DTO
{
    public class MyTask
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Categories Category { get; set; }
        public string CategoryName { get; set; }
        public string Importance { get; set; }
        public bool IsChecked { get; set; }

        public static MyTask createTask(string name,  DateTime date, int importance, Guid id)
        {
            MyTask task = new MyTask();
            task.Id = Guid.NewGuid();
            task.Name = name;
            task.Date = date;
            task.CategoryId = id;
            switch (importance)
            {
                case 1:
                    task.Importance = "Very important";
                    break;
                case 2:
                    task.Importance = "Important";
                    break;
                case 3:
                    task.Importance = "Could be done later";
                    break;
                case 4:
                    task.Importance = "Not that important";
                    break;
            }
            task.IsChecked = false;

            return task;
        }
    }
}
