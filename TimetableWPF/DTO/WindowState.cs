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
