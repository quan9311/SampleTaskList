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
            throw new NotImplementedException();
        }

        public void ViewTask()
        {
            Console.Write(GetAllAsTable());
        }

        public string GetAllAsTable()
        {
            var tl = GlobalList.TaskLists;
            if (GlobalList.TaskLists.Count > 0)
            {
                var table = new Table("No", "Task", "Due Date", "Status");
                int i = 0;
                foreach (MyTask data in tl)
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
