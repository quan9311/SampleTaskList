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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                Console.Write("Task No: ");
                var task_nos = Console.ReadLine();
                if (!string.IsNullOrEmpty(task_nos))
                {
                    string[] datas = task_nos.Replace(" ", "").Split(',');
                    List<int> ints = new List<int>();
                    foreach(var data in datas)
                    {
                        if (Int32.TryParse(data, out int i))
                        {
                            ints.Add(i);
                        }
                        else
                        {
                            Console.WriteLine($"{data} is not integer".ForegroundColor(Color.Red));
                        }
                    }

                    var requestMarkList = unmarkList.Select((my_task, index) => new { MyTask = my_task, Index = (index + 1) })
                        .Where(x => ints.Contains(x.Index)).Select(x => x.MyTask);
                    foreach (MyTask data in requestMarkList)
                    {
                        data.Status = MyTaskStatus.Completed;
                        Console.WriteLine($"Task {data.TaskName} has been mark as completed".ForegroundColor(Color.Green));
                    }
                }
            }
            else
            {
                Console.WriteLine("No task to mark!");
            }
        }

        public void ViewTask()
        {
            IEnumerable<MyTask> taskList = GlobalList.TaskLists;
            Console.WriteLine(ToTable(taskList));
            bool backToMenu = false;
            while (!backToMenu)
            {
                Console.WriteLine("1. Filter By");
                Console.WriteLine("2. Order By");
                Console.WriteLine("3. Reset");
                Console.WriteLine("4. Back To Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        taskList = FilterBy(taskList);
                        break;
                    case "2":
                        taskList = OrderBy(taskList);
                        break;
                    case "3":
                        taskList = GlobalList.TaskLists;
                        break;
                    case "4":
                        backToMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.".ForegroundColor(Color.Red));
                        break;
                }

            }
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
                    table.AddRow((i += 1), data.TaskName, data.DueDate.ToString("yyyy-MM-dd"), Status);
                }
                return table.ToString();
            }
            else
            {
                return "Not record found!";
            }
        }

        public IEnumerable<MyTask> FilterBy(IEnumerable<MyTask> taskLists)
        {
            Console.WriteLine("Filter By:");
            Console.WriteLine("1: Task Name");
            Console.WriteLine("2: Due Date");
            Console.WriteLine("3: Status");
            Console.Write("Choice:");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("Filter By Task Name:");
                string filterByName = Console.ReadLine();
                taskLists = taskLists.Where(x => x.TaskName.ToUpper().Contains(filterByName.ToUpper()));
            }else if (choice == "2")
            {
                Console.Write("Filter By Due Date (From):");
                string dateFrom = Console.ReadLine();
                Console.Write("Filter By Due Date (To):");
                string dateTo = Console.ReadLine();
                if (!string.IsNullOrEmpty(dateFrom))
                {
                    if (RegexHelper.IsValidDateFormat(dateFrom, "yyyy-MM-dd"))
                    {
                        taskLists = taskLists.Where(x => x.DueDate >= DateTime.Parse(dateFrom));
                    }
                }
                if (!string.IsNullOrEmpty(dateTo))
                {
                    if (RegexHelper.IsValidDateFormat(dateTo, "yyyy-MM-dd"))
                    {
                        taskLists = taskLists.Where(x => x.DueDate <= DateTime.Parse(dateTo));
                    }
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Filter By Status:");
                Console.WriteLine("1: Pending");
                Console.WriteLine("2: Completed");
                Console.Write("Choice:");
                string filterByStatus = Console.ReadLine();
                if (filterByStatus == "1")
                {
                    taskLists = taskLists.Where(x => x.Status.Equals(MyTaskStatus.Pending));
                }else if (filterByStatus == "2")
                {
                    taskLists = taskLists.Where(x => x.Status.Equals(MyTaskStatus.Completed));
                }
            }
            Console.WriteLine(ToTable(taskLists));
            return taskLists;
        }
        public IEnumerable<MyTask> OrderBy(IEnumerable<MyTask> taskLists)
        {
            Console.WriteLine("Order By:");
            Console.WriteLine("1: Task Name");
            Console.WriteLine("2: Due Date");
            Console.WriteLine("3: Status");
            Console.Write("Choice:");
            string orderChoice = Console.ReadLine();
            Console.WriteLine("Asc or Desc:");
            Console.WriteLine("1: Asc");
            Console.WriteLine("2: Desc");
            string ascOrDesc = Console.ReadLine();
            if (orderChoice == "1")
            {
                if (ascOrDesc == "1")
                {
                    taskLists = taskLists.OrderBy(x => x.TaskName);
                }
                else
                {
                    taskLists = taskLists.OrderByDescending(x => x.TaskName);
                }
            }else if (orderChoice == "2")
            {
                if (ascOrDesc == "1")
                {
                    taskLists = taskLists.OrderBy(x => x.DueDate);
                }
                else
                {
                    taskLists = taskLists.OrderByDescending(x => x.DueDate);
                }
            }
            else if (orderChoice == "3")
            {
                if (ascOrDesc == "1")
                {
                    taskLists = taskLists.OrderBy(x => x.Status);
                }
                else
                {
                    taskLists = taskLists.OrderByDescending(x => x.Status);
                }
            }
            Console.WriteLine(ToTable(taskLists));
            return taskLists;
        }

        public void DeleteTask()
        {
            Console.WriteLine(ToTable(GlobalList.TaskLists));
            Console.WriteLine("Please insert no to delete: ");
            Console.WriteLine("if want to delete multi task then input like 1, 2");
            Console.Write("Task No: ");
            var task_nos = Console.ReadLine();
            if (!string.IsNullOrEmpty(task_nos))
            {
                string[] datas = task_nos.Replace(" ", "").Split(',').OrderByDescending(item => item).ToArray();
                foreach (string data in datas)
                {
                    if (Int32.TryParse(data, out int i))
                    {
                        i -= 1;
                        try
                        {
                            Console.WriteLine($"Task {GlobalList.TaskLists.ElementAt(i).TaskName} has been deleted".ForegroundColor(Color.Green));
                            GlobalList.TaskLists.RemoveAt(i);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.WriteLine($"{data} is not existed".ForegroundColor(Color.Red));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{data} is not integer".ForegroundColor(Color.Red));
                    }
                }
            }
        }
    }
}
