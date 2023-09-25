using SimpleTaskList.Helper;
using SimpleTaskList.Interface;
using SimpleTaskList.Model;
using SimpleTaskList.Model.Entities;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}
