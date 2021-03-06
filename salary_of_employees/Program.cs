using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace salary_of_employees
{
    public class Employee
    {
        public string Name { get; set; }
        public string departament { get; set; }
        public int Salary { get; set; }
        public bool Director { get; set; }

        public Employee(string name, string depart, int salary, bool director = false)
        {
            Name = name;
            departament = depart;
            Salary = salary;
            Director = director;
        }

        public static void ReadFile()
        {
            var file = new StreamReader("file.txt");

            while (!file.EndOfStream)
            {
                ReadString(file.ReadLine().Split(";"));
            }
        }

        public static void ReadString(string[] array)
        {
            if (array.Length > 3)
                Departament.FindDepartament(array[1]).AddPerson(new Employee(array[0], array[1], Convert.ToInt32(array[2]), Convert.ToBoolean(array[3])));
            else
                Departament.FindDepartament(array[1]).AddPerson(new Employee(array[0], array[1], Convert.ToInt32(array[2])));

        }

        //public static string GetJSONs()
        //{
        //    var sb = new StringBuilder();

        //    foreach (var dep in Departament.GetAllDepartments())
        //    {
        //        foreach (var men in dep.Employees)
        //        {
        //            sb.Append(JsonSerializer.Serialize(men) + "\n");
        //        }
        //        foreach (var men in dep.Directors)
        //        {
        //            sb.Append(JsonSerializer.Serialize(men) + "\n");
        //        }
        //    }

        //    return sb.ToString();
        //}
    }

    public class Departament
    {
        public string Name { get; private set; }
        public List<Employee> Directors { get; private set; } = new List<Employee>();
        public List<Employee> Employees { get; private set; } = new List<Employee>();
        private static readonly Dictionary<string, Departament> Departaments = new Dictionary<string, Departament>();

        public Departament(string name) => Name = name;

        public void AddPerson(Employee men)
        {
            if (men.Director)
                Directors.Add(men);
            else
                Employees.Add(men);
        }

        public static Departament FindDepartament(string name)
        {
            if (!Departaments.ContainsKey(name))
                Departaments.Add(name, new Departament(name));

            return Departaments[name];
        }

        public static Departament HighestPaidDirector()
        {
            var salary = 0;
            Departament departament = null;
            foreach (var dep in Departaments.Values)
            {
                foreach (var men in dep.Directors)
                {
                    if (men.Salary >= salary)
                    {
                        salary = men.Salary;
                        departament = dep;
                    }
                }
            }
            return departament;
        }

        public static Departament[] GetAllDepartments() => Departaments.Values.ToArray();

        public bool CheckDirectors() => (Directors.Count > 0) && (Directors.Count < 3);

        public static bool CheckDirectorsInAllDepart() => Departaments.Values.All(dep => dep.CheckDirectors());

        public int CalculateSalary() => (int)Employees.Select(men => men.Salary).Average();

        //public int CalculateSalary()
        //{
        //    int salary = 0;
        //    int count = 0;
        //    foreach (var men in Employees)
        //    {
        //        salary += men.Salary;
        //        count += 1;
        //    }
        //    return salary / count;
        //}
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee.ReadFile();

            if (Departament.CheckDirectorsInAllDepart())
            {
                foreach (var e in Departament.GetAllDepartments())
                {
                    Console.WriteLine($"departament: {e.Name}  average salary: {e.CalculateSalary()}");
                }

                Console.WriteLine($"отдел с самым высокооплачиваемым руководителем: {Departament.HighestPaidDirector().Name}");
            }
            else
                throw new Exception("error");


            //Console.WriteLine(JsonSerializer.Serialize<Employee>(new Employee( "Иванов Иван Иванович", "Цех 1", 25000 )));
        }
    }
}
