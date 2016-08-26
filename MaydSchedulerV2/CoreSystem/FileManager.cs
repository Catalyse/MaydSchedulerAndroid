using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

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
            fileName = fileName + ".xml";
            try
            {
                StreamReader reader = new StreamReader(fileName);
                reader.Close();
                return true;
            }
            catch//filenotfound
            {
                Console.WriteLine(fileName + " was not found.");
                return false;
            }
        }

        public static void SerializeFile<T>(T objectToSerialize, string fileName)
        {
            string directory = Directory.GetCurrentDirectory();
            fileName = directory +  "\\" + fileName + ".xml";
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, objectToSerialize);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
                if (fileName != "CoreSaveFile")
                    CoreSystem.savedFileList.Add(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Serialization Error || CoreSystem || SerializeFile<T>");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
        }

        public static T DeserializeFile<T>(string fileName)
        {
            string directory = Directory.GetCurrentDirectory();
            fileName = directory +  "\\" +fileName + ".xml";
            T returnObject;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                StreamReader reader = new StreamReader(fileName);
                if (reader == null)
                { }
                    //throw new EmpListNotFoundErr();
                returnObject = (T)serializer.Deserialize(reader);
                reader.Close();

                return returnObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FileNotFound Exception! (Or something else this is a catch-all error)");
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static void SerializeCoreSave()
        {
            Thread serializeProcess = new Thread(new ThreadStart(ThreadedSerialize));
            serializeProcess.Start();
        }

        private static void ThreadedSerialize()
        {
            try
            {
                string directory = Directory.GetCurrentDirectory();
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(CoreSaveType));
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, CoreSystem.coreSave);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(directory +  "\\" +"CoreSave.xml");
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Serialization Error || CoreSystem || SerializeFile<T>");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
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
            Thread deserializeProcess = new Thread(new ThreadStart(ThreadedDeserialize));
            deserializeProcess.Start();
        }

        private static void ThreadedDeserialize()
        {
            CoreSaveType returnObject;
            try
            {
                string directory = Directory.GetCurrentDirectory();
                XmlSerializer serializer = new XmlSerializer(typeof(CoreSaveType));

                StreamReader reader = new StreamReader(directory +  "\\" +"CoreSave.xml");
                if (reader == null)
                { }
                    //throw new EmpListNotFoundErr();
                returnObject = (CoreSaveType)serializer.Deserialize(reader);
                reader.Close();

                CoreSystem.CoreSaveLoaded(returnObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization Error! (Or the file is missing, or something else)");
                Console.WriteLine(ex.Message);
            }
        }
        //FILE SERIALIZATION ========================================================================================
    }
}