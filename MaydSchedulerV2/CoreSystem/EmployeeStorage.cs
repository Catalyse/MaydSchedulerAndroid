using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MaydSchedulerApp
{
    public static class EmployeeStorage
    {
        public static List<Employee> employeeList = new List<Employee>();

        public static void TestingModeLoad()
        {
            List<Employee> storage = new List<Employee>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));

            TextReader reader = new StringReader(TempData.EmpList);
            if (reader == null)
            { }
            //throw new EmpListNotFoundErr();
            storage = (List<Employee>)serializer.Deserialize(reader);
            reader.Close();

            employeeList = storage;
        }

        public static void Start()
        {
            if (MainActivity.testingMode)
            {
                TestingModeLoad();
            }
            else
            {
                List<Employee> temp = EmpListSerializer.DeserializeEmpList();
                if (temp != null)
                {
                    employeeList = temp;
                }
                else
                    Console.WriteLine("No employee list found");
            }
        }

        public static void AddEmployee(Employee toAdd)
        {
            employeeList.Add(toAdd);
            SortList();
        }

        public static void RemoveEmployee(int toRemove)
        {
            employeeList.RemoveAt(toRemove);
            SortList();
        }

        public static void OverWriteList(List<Employee> empList)
        {
            employeeList = empList;
            SortList();
        }

        public static Employee GetEmployee(string name)
        {
            try
            {
                for (int i = 0; i < employeeList.Count; i++)
                {
                    if (employeeList[i].empLastName == name)
                        return employeeList[i];
                }
                //throw new EmpNotFoundErr();
            }
            catch (Exception e)
            {
                //e.ThrowMsg();
                return null;
            }
            return null;
        }

        public static Employee GetEmployee(int empID)
        {
            try
            {
                for (int i = 0; i < employeeList.Count; i++)
                {
                    if (employeeList[i].empID == empID)
                        return employeeList[i];
                }
                //throw new EmpNotFoundErr();
            }
            catch (Exception e)
            {
                //e.ThrowMsg();
                return null;
            }
            return null;
        }

        private static void SortList()
        {
            //Starts false so if list is already in order do nothing
            bool sortAgain = false;
            Employee current;
            Employee next;
            do
            {
                if (employeeList.Count > 1)
                {
                    sortAgain = false;
                    for (int i = 0; i < employeeList.Count - 1; i++)
                    {
                        current = employeeList[i];
                        next = employeeList[i + 1];
                        if (current.empLastName.CompareTo(next.empLastName) > 0)
                        {
                            employeeList[i] = next;
                            employeeList[i + 1] = current;
                            sortAgain = true;
                        }
                    }
                }
            }
            while (sortAgain == true);
        }

        public static void ClearList()
        {
            employeeList.Clear();
            EmpListSerializer.DeleteEmpListFile();
        }

        public static void OnDestroy()
        {
            if (employeeList.Count > 0)
            {
                EmpListSerializer.SerializeEmpList(employeeList);
            }
            else
            { }
                Console.WriteLine("No employee list to serialize!");
        }
    }
}