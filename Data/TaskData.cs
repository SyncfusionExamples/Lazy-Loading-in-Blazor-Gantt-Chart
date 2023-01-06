using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static WebAPI.Controller.DefaultController;


namespace WebAPI.Data
{
    public class TaskData
    {
        public static List<TaskData> tree = new List<TaskData>();

        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("TaskName")]
        public string TaskName { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("Duration")]
        public string Duration { get; set; }

        [JsonPropertyName("Progress")]
        public int Progress { get; set; }

        [JsonPropertyName("ParentId")]
        public int? ParentId { get; set; }

        [JsonPropertyName("Predecessor")]
        public string Predecessor { get; set; }

        [JsonPropertyName("isParent")]
        public bool? isParent { get; set; }
        public TaskData() { }
        public static List<TaskData> GetTree()
        {
            if (tree.Count == 0)
            {
                Random rand = new Random();
                var x = 0;
                int duration = 0;
                DateTime startDate = new DateTime(2000, 1, 3, 08, 00, 00);
                for (var i = 1; i <= 50; i++)
                {
                    startDate = startDate.AddDays(i == 1 ? 0 : 7);
                    DateTime childStartDate = startDate;
                    TaskData Parent = new TaskData()
                    {
                        ID = ++x,
                        TaskName = "Task " + x,
                        StartDate = startDate,
                        EndDate = startDate.AddDays(26),
                        Duration = "20",
                        Progress = rand.Next(100),
                        Predecessor = null,
                        isParent = true,
                        ParentId = null
                    };
                    tree.Add(Parent);
                    for (var j = 1; j <= 4; j++)
                    {
                        childStartDate = childStartDate.AddDays(j == 1 ? 0 : duration + 2);
                        duration = 5;
                        tree.Add(new TaskData()
                        {
                            ID = ++x,
                            TaskName = "Task " + x,
                            StartDate = childStartDate,
                            EndDate = childStartDate.AddDays(5),
                            Duration = duration.ToString(),
                            Progress = rand.Next(100),
                            ParentId = Parent.ID,
                            Predecessor = j > 1 ? (x - 1) + "FS" : "",
                            isParent = false
                        });
                    }
                }
            }
            return tree;
        }
    }
}