using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Momenton.CodingChallenge.Extensions;
using Momenton.CodingChallenge.Models;
using Newtonsoft.Json;

namespace Momenton.CodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawData = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Data/Employees.json");

            var employeeList = JsonConvert.DeserializeObject<List<Employee>>(rawData);

            //Get manager
            var manager = employeeList.FirstOrDefault(x => x.ManagerId == null);

            if (manager == null)
            {
                Console.WriteLine("No manager specified. Ending application...");
                Task.Delay(5000);
                Environment.Exit(0);
            }


            var employeeId = manager.Id;
            var companyLevel = 2;
            var employeePositionObjects = new Dictionary<int, EmployeePosition>();

            var managerPosition = new EmployeePosition { Name = manager.Name, CompanyLevel = 1, TableIndex = 0 };
            employeePositionObjects.TryAdd(employeeId, managerPosition);

            for (var v = 0; v < employeeList.Count; v++)
            {
                bool didUpdate = false;

                for (var i = 1; i < employeeList.Count; i++)
                {
                    var employeeName = employeeList[i].Name;
                    var employeeManagerId = employeeList[i].ManagerId;
                    var id = employeeList[i].Id;

                    if (employeeManagerId == null)
                    {
                        continue;
                    }

                    for (var x = 0; x < employeeList.Count; x++)
                    {

                        if (employeeManagerId == employeeId)
                        {
                            var employeeCompanyPosition = new EmployeePosition { Name = employeeName, TableIndex = i, CompanyLevel = companyLevel };
                            didUpdate = employeePositionObjects.TryAdd(id, employeeCompanyPosition);
                            break;
                        }
                    }

                }

                if (didUpdate)
                    companyLevel++;

                employeeId = employeeList[v].Id;
            }

            PrintTable(employeePositionObjects);
            Console.WriteLine("Hello World!");
        }

        private static void PrintTable(Dictionary<int, EmployeePosition> employeePositionObjects)
        {
            var employees = employeePositionObjects.Values.ToList().OrderBy(x => x.TableIndex);

            foreach (var employee in employees)
            {
                var row = string.Empty;

                row = row.AddCompanyLevelSpaces(employee.CompanyLevel);
                row += employee.Name;

                Console.WriteLine(row);
            }

            Console.ReadLine();
        }


    }
}
