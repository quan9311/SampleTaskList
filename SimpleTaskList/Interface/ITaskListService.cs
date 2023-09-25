using SimpleTaskList.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskList.Interface
{
    public interface ITaskListService
    {
        void ViewTask();
        void AddTask();
        void MarkTask();
        void DeleteTask();
        string ToTable(IEnumerable<MyTask> t);
    }
}
