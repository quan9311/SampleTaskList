using BetterConsoles.Colors.Extensions;
using BetterConsoleTables;
using SimpleTaskList.Constants;
using SimpleTaskList.Helper;
using SimpleTaskList.Interface;
using SimpleTaskList.Model;
using SimpleTaskList.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskList.Service
{
    public class TaskListService : ITaskListService
    {
        public async void AddTask()
        {
            Console.WriteLine("Please Input Data");
            MyTask MyTask = new MyTask();
            var nameInput = InputHelper.TableInput<MyTaskValidation>("Task Name: ", "TaskName");
            if (nameInput.Success)
            {
                MyTask.TaskName = nameInput.Input;
            }
            else
            {
                return;
            }
            var dateInput = InputHelper.TableInput<MyTaskValidation>("Due Date (Exp: yyyy-MM-dd): ", "DueDate");
            if (dateInput.Success)
            {
                MyTask.DueDate = DateTime.Parse(dateInput.Input);
            }
            else
            {
                return;
            }

            Console.WriteLine("----------------------");
            Console.WriteLine("Input Data");
            Console.WriteLine("----------------------");
            Console.WriteLine($"Name: {MyTask.TaskName}");
            Console.WriteLine($"Due Date: {MyTask.DueDate.ToString("yyyy-MM-dd")}");
            //Show input data and confirmation to avoid wrong input
            bool confirmSave = await InputHelper.Confirmation("Are you sure to save these data?");
            if (confirmSave)
            {
                GlobalList.TaskLists.Add(MyTask);
                Console.WriteLine("Data Saved!");
                bool confirmReAdd = await InputHelper.Confirmation("Do you want add task again?");
                if (confirmReAdd)
                {
                    Console.Clear();
                    AddTask();
                }
            }
            else
            {
                bool confirmReAdd = await InputHelper.Confirmation("Do you want add task again?");
                if (confirmReAdd)
                {
                    Console.Clear();
                    AddTask();
                }
            }
        }

        public void MarkTask()
        {
            IEnumerable<MyTask> unmarkList = GlobalList.TaskLists.Where(x => x.Status.Equals(MyTaskStatus.Pending));
            if (unmarkList.Count() > 0)
            {
                Console.WriteLine(ToTable(unmarkList));
                Console.WriteLine("Please input no to mark task");
                Console.WriteLine("if want to mark multi task then input like 1, 2");
                Console.Write("Task No");
                var task_nos = Console.ReadLine();
                if (!string.IsNullOrEmpty(task_nos))
                {
                    string[] parts = task_nos.Replace(" ", "").Split(',');

                    var requestMarkList = unmarkList.Select((my_task, index) => new { MyTask = my_task, Index = (index + 1).ToString() })
                        .Where(x => parts.Contains(x.Index)).Select(x => x.MyTask);
                    foreach (MyTask data in requestMarkList)
                    {
                        data.Status = MyTaskStatus.Completed;
                    }
                }
                Console.WriteLine("Task marked");
            }
            else
            {
                Console.WriteLine("No task to mark!");
            }
        }

        public void ViewTask()
        {
            Console.WriteLine(ToTable(GlobalList.TaskLists));
        }

        public string ToTable(IEnumerable<MyTask> taskLists)
        {
            if (taskLists.Count() > 0)
            {
                var table = new Table("No", "Task", "Due Date", "Status");
                int i = 0;
                foreach (MyTask data in taskLists)
                {
                    string Status = "";
                    if (data.Status == MyTaskStatus.Pending)
                    {
                        Status = data.Status.ToString().ForegroundColor(Color.Red);
                    }
                    else
                    {
                        Status = data.Status.ToString().ForegroundColor(Color.Green);
                    }
                    table.AddRow((i += 1), data.TaskName, data.DueDate, Status);
                }
                return table.ToString();
            }
            else
            {
                return "Not record found!";
            }
        }
    }
}
