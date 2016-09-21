using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

using Android.Content;

namespace MaydSchedulerApp
{
    public static class EmployeeStorage
    {
        public static List<Employee> employeeList = new List<Employee>();
        public static bool loaded = false;

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
            List<Employee> temp = DeserializeEmpList();
            if (temp != null)
            {
                employeeList = temp;
                loaded = true;
            }
            else
                Console.WriteLine("No employee list found");
        }

        public static void AddEmployee(Employee toAdd)
        {
            if (employeeList.Count < 1)
                loaded = true;
            employeeList.Add(toAdd);
            SortList();
            OnSaveList();
        }

        public static void RemoveEmployee(int toRemove)
        {
            employeeList.RemoveAt(toRemove);
            SortList();
            OnSaveList();
        }

        public static void OverWriteList(List<Employee> empList)
        {
            employeeList = empList;
            SortList();
        }

        public static bool CheckIfEmployeesInPosition(int pos)
        {
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (employeeList[i].position == pos)
                    return true;
            }    
            return false;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + ex.StackTrace);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + ex.StackTrace);
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
            DeleteEmpListFile();
        }

        public static void OnSaveList()
        {
            if (employeeList.Count > 0)
            {
                SerializeEmpList(employeeList);
            }
            else
            {
                Console.WriteLine("No employee list to serialize!");
            }
        }

        public static string fileName = "empList.xml";

        public static void SerializeEmpList(List<Employee> list)
        {
            if (list == null) { return; }

            try
            {
                Context context = MainActivity.currentActivity;
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(list.GetType());
                Stream stream = context.OpenFileOutput(fileName, FileCreationMode.Private);

                serializer.Serialize(stream, list);
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
        }


        public static List<Employee> DeserializeEmpList()
        {
            try
            {
                Context context = MainActivity.currentActivity;
                List<Employee> storage = new List<Employee>();

                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));

                Stream reader = context.OpenFileInput(fileName);
                if (reader == null)
                { }
                //throw new EmpListNotFoundErr();
                storage = (List<Employee>)serializer.Deserialize(reader);
                reader.Close();

                return storage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void DeleteEmpListFile()
        {
            File.Delete(fileName);
        }
    }
}