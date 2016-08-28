using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using Android.Content;

namespace MaydSchedulerApp
{
    public static class FileManager
    {
        public static List<Employee> TestingModeLoad()
        {
            List<Employee> storage = new List<Employee>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));

            TextReader reader = new StringReader(TempData.EmpList);
            if (reader == null)
            { }
            //throw new EmpListNotFoundErr();
            storage = (List<Employee>)serializer.Deserialize(reader);
            reader.Close();

            return storage;
        }

        //FILE SERIALIZATION ========================================================================================
        public static bool CheckIfFileExists(string fileName)
        {
            Context context = CoreSystem.currentActivity;
            string[] fileList = context.FileList();
            var results = Array.FindAll(fileList, s => s.Equals(fileName));
            if (results != null)
                return true;
            fileName = fileName + ".xml";
            var results1 = Array.FindAll(fileList, s => s.Equals(fileName));
            if (results1 != null)
                return true;
            else
                return false;
        }

        public static void SerializeFile<T>(T objectToSerialize, string fileName)
        {
            Context context = CoreSystem.currentActivity;
            fileName = fileName + ".xml";
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());

                Stream stream = context.OpenFileOutput(fileName, FileCreationMode.Private);

                serializer.Serialize(stream, objectToSerialize);
                stream.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Serialization Error || CoreSystem || SerializeFile<T>");
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.InnerException);
            }
        }

        public static T DeserializeFile<T>(string fileName)
        {
            Context context = CoreSystem.currentActivity;
            fileName = fileName + ".xml";
            T returnObject;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                Stream reader = context.OpenFileInput(fileName);
                returnObject = (T)serializer.Deserialize(reader);
                reader.Close();

                return returnObject;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("FileNotFound Exception! (Or something else this is a catch-all error)");
                System.Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static void SerializeCoreSave()
        {
            try
            {
                Context context = CoreSystem.currentActivity;
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(CoreSaveType));

                Stream stream = context.OpenFileOutput("CoreSave.xml", FileCreationMode.Private);

                serializer.Serialize(stream, CoreSystem.coreSave);
                stream.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Serialization Error || CoreSystem || SerializeFile<T>");
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.InnerException);
            }
        }

        /// <summary>
        /// This is specifically meant for the CoreSave since the file can be quite large
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void DeserializeCoreSave(string fileName)
        {
            Context context = CoreSystem.currentActivity;
            CoreSaveType returnObject;
            try
            {
                string directory = Directory.GetCurrentDirectory();
                XmlSerializer serializer = new XmlSerializer(typeof(CoreSaveType));

                Stream reader = context.OpenFileInput("CoreSave.xml");
                if (reader == null)
                { }
                    //throw new EmpListNotFoundErr();
                returnObject = (CoreSaveType)serializer.Deserialize(reader);
                reader.Close();

                CoreSystem.CoreSaveLoaded(returnObject);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Deserialization Error! (Or the file is missing, or something else)");
                System.Console.WriteLine(ex.Message);
            }
        }
        //FILE SERIALIZATION ========================================================================================
    }
}