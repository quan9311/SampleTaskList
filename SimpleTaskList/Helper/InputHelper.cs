using SimpleTaskList.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskList.Helper
{
    public static class InputHelper
    {
        //Table input validation for custom validation purpose 
        public static InputResponse TableInput<T>(string message, string Column)
        {
            try
            {
                while (true)
                {
                    Type type = typeof(T);
                    object instance = Activator.CreateInstance(type);
                    Expression body = Expression.Constant(instance, type);
                    MethodInfo method = type.GetMethod("Validate" + Column);
                    Console.Write(message);
                    string input = Console.ReadLine();
                    Expression arg = Expression.Constant(input, typeof(string));
                    Expression methodCall = Expression.Call(body, method, arg);
                    Func<(bool, string)> validateMethod = Expression.Lambda<Func<(bool, string)>>(methodCall).Compile();
                    (bool Success, string ErrorMessage) = validateMethod();
                    if (Success)
                    {
                        return new InputResponse()
                        {
                            Success = Success,
                            Input = input
                        };
                    }
                    else
                    {
                        Console.WriteLine(ErrorMessage);
                        Console.WriteLine("Please input again!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method not found");
                throw;
            }
        }

        //function for confirmation
        public static async Task<bool> Confirmation(string message)
        {
            bool confirm = false;
            while (true)
            {
                Console.WriteLine(message + " (y/n)");
                string YesNo = Console.ReadLine() ?? "";
                if (YesNo.ToUpper() == "Y" || YesNo.ToUpper() == "N")
                {
                    if (YesNo.ToUpper() == "Y")
                    {
                        confirm = true;
                    }
                    else
                    {
                        confirm = false;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter either Y or N");
                }
            }
            return confirm;
        }
    }
}
