using SimpleTaskList.Constants;

namespace SimpleTaskList.Model.Entities
{
    public class MyTask
    {
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        //using MyTaskStatus as variable name because TaskStatus is existing in System.Threading.Tasks
        // Set default value as Pending
        public MyTaskStatus Status { get; set; } = MyTaskStatus.Pending;
    }
}
