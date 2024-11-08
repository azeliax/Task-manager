using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableWPF.DTO;

namespace TimetableWPF
{
    public class WindowStateJson
    {
        public List<MyTask> tasks { get; set; } = new List<MyTask>();
        public List<Categories> categories { get; set; } = new List<Categories>();
        public List<MyEvent> events { get; set; } = new List<MyEvent>();

        public void Save()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            File.WriteAllText("tasks.json", json);


            //using (var context = new TimetableDbContext())
            //{
            //    foreach (var category in categories)
            //    {
            //        var existingCategory = context.Categories
            //            .FirstOrDefault(c => c.CategoryId == category.CategoryId);

            //        if (existingCategory == null)
            //        {
            //            context.Categories.Add(category);
            //        }
            //        else
            //        {
            //            existingCategory.CategoryName = category.CategoryName;
            //            existingCategory.CategoryColorHex = category.CategoryColorHex;
            //        }
            //    }


            //    foreach (var task in tasks)
            //    {
            //        var existingTask = context.MyTasks
            //            .FirstOrDefault(t => t.Id == task.Id);

            //        if (existingTask == null)
            //        {
            //            var category = context.Categories.FirstOrDefault(c => c.CategoryId == task.CategoryId);
            //            if (category != null)
            //            {
            //                task.CategoryName = category.CategoryName;
            //            }

            //            context.MyTasks.Add(task);
            //        }
            //        else
            //        {
            //            existingTask.Name = task.Name;
            //            existingTask.Date = task.Date;
            //            existingTask.CategoryId = task.CategoryId;
            //            existingTask.Importance = task.Importance;
            //            var category = context.Categories.FirstOrDefault(c => c.CategoryId == task.CategoryId);
            //            if (category != null)
            //            {
            //                existingTask.CategoryName = category.CategoryName;
            //            }
            //            existingTask.IsChecked = task.IsChecked;
            //        }
            //    }

            //    context.SaveChanges();
            //}
        }

        public static WindowStateJson Load()
        {
            if (File.Exists("tasks.json"))
            {
                var json = File.ReadAllText("tasks.json");
                var deserialize = Newtonsoft.Json.JsonConvert.DeserializeObject<WindowStateJson>(json);
                foreach (MyTask myTask in deserialize.tasks) 
                {
                    myTask.CategoryName = deserialize.categories.FirstOrDefault(x => x.CategoryId == myTask.CategoryId)?.CategoryName;
                }
                return deserialize;
            }
            return new WindowStateJson();
        }
    }
}
