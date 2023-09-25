using SimpleTaskList.Helper;
using SimpleTaskList.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskList
{
    public class MyTaskValidation
    {
        public (bool, string) ValidateTaskName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return (false, "Task name can't be blank");
            }
            else
            {
                if (GlobalList.TaskLists.Where(x => x.TaskName.ToUpper().Equals(Name.ToUpper())).Count() > 0)
                {
                    return (false, "Task name has been used!");
                }
            }

            return (true, "");
        }

        public (bool, string) ValidateDueDate(string Date)
        {
            if (string.IsNullOrEmpty(Date))
            {
                return (false, "Due Date can't be blank");
            }
            else
            {
                bool CheckDate = RegexHelper.IsValidDateFormat(Date, "yyyy-MM-dd");
                if (!CheckDate)
                {
                    return (false, "The input is not a valid date. (Exp: YYYY-MM-DD)");
                }
                else
                {
                    if (DateTime.Parse(Date) < DateTime.Today)
                    {
                        return (false, "Not allow back date");
                    }
                }
            }
            return (true, "");
        }
    }
}
