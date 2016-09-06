using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using Android.Content;

namespace MaydSchedulerApp
{
    public static class EmpListSerializer
    {
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