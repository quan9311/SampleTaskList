using SimpleTaskList.Constants;
using SimpleTaskList.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskList.Model
{
    public class GlobalList
    {
        //Create a global task list to store task
        public static List<MyTask> TaskLists { get; set; } = new List<MyTask>()
        {
            new MyTask{
                TaskName = "TASK 1",
                DueDate = DateTime.Parse("2023-10-01"),
                Status = MyTaskStatus.Pending
            },new MyTask{
                TaskName = "TASK 2",
                DueDate = DateTime.Parse("2023-10-01"),
                Status = MyTaskStatus.Pending
            },new MyTask{
                TaskName = "TASK 3",
                DueDate = DateTime.Parse("2023-10-02"),
                Status = MyTaskStatus.Pending
            },new MyTask{
                TaskName = "TESTING",
                DueDate = DateTime.Parse("2023-10-03"),
                Status = MyTaskStatus.Pending
            }
        };
    }
}
