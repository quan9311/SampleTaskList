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
        public static List<MyTask> TaskLists { get; set; } = new List<MyTask>();
    }
}
