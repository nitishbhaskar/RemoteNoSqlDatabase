///////////////////////////////////////////////////////////////
// Persistengine.cs - DB Factory for Project #2              //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    HP DV7t Quad, Core-i7, Windows 10            //
// Author:      Nitish Bhaskar, CE, Syracuse University      //
//              (315) 420-8933, nibhaska@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package helps in persisting the database to an XML file.
 * It also helps in augumenting the XML into the database.
 * It also supports timely persistance of database to XML.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs, DBElement, DBEngine, TimerDemo, UtilitiesExtensions
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 07 Oct 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;

namespace Project4Starter
{
    public static class PersistEngine
    {
        /*private static string xmlLocation = "E:\\Syracuse University\\CSE 681 - SMA\\Assignments\\Proj4\\";//ConfigurationManager.AppSettings["xmlLocation"];
        private static string xmlName = "PersistedFile";//ConfigurationManager.AppSettings["xmlName"];   
        private static string restoreXmlName = "RestoreFile";
        //private static string xmlAugument = ConfigurationManager.AppSettings["xmlAugument"];
        //private static string projectStructure = ConfigurationManager.AppSettings["projectStructure"];*/
        private static string xmlLocation = ConfigurationManager.AppSettings["xmlLocation"];
        private static string xmlName = ConfigurationManager.AppSettings["xmlPersist"];
        private static string restoreXmlName = ConfigurationManager.AppSettings["xmlRestore"];

        public static void toXml(this DBEngine<string, DBElement<string, List<string>>> dbEngine)
        {
            dynamic dict = dbEngine.Keys();
            DBElement<string, List<string>> result;
            XDocument xmlDoc = new XDocument();
            XElement root = new XElement("noSqlDb");
            xmlDoc.Add(root);

            foreach (var key in dict)
            {
                dbEngine.getValue(key, out result);
                XElement element = new XElement("element");
                root.Add(element);
                XElement keyValue = new XElement("key", key);
                element.Add(keyValue);
                XElement value = new XElement("value");
                element.Add(value);
                XElement children = new XElement("children");
                value.Add(children);
                foreach (var item in result.children)
                {
                    XElement child = new XElement("child", item);
                    children.Add(child);
                }
                XElement description = new XElement("description", result.descr);
                XElement name = new XElement("name");
                name.SetValue(result.name);
                XElement timestamp = new XElement("timestamp", result.timeStamp);
                XElement payLoad = new XElement("payload");
                value.Add(description);
                value.Add(name);
                value.Add(timestamp);
                value.Add(payLoad);
                foreach(var item in result.payload)
                {
                    XElement load = new XElement("load", item);
                    payLoad.Add(load);
                }
            }
            saveXml(xmlDoc);
        }

        private static void saveXml(XDocument document)
        {
            Console.WriteLine("");
            "Persisted XML".title('_');
            Console.WriteLine("\n\nPersisted Location: {0}\n\n", xmlLocation + xmlName + ".xml");
            document.Save(xmlLocation + xmlName + ".xml");
        }

        public static void restoreDatabase(this DBEngine<string, DBElement<string, List<string>>> dbEngine)
        {
            XDocument xmlDoc = XDocument.Load(xmlLocation + restoreXmlName + ".xml");
            "XML to be augumented".title('_');
            Console.WriteLine("{0}", xmlDoc.ToString());

            var results = xmlDoc.Descendants("element").Select(p => new DBElement<string, List<string>>
            {
                name = p.Descendants("name").FirstOrDefault().Value,
                descr = p.Descendants("description").FirstOrDefault().Value,
                payload = p.Descendants("payload").Select(q => { return q.Value; }).ToList(),
                children = p.Descendants("children").Descendants("child").Select(q => { return q.Value; }).ToList(),
                key = p.Descendants("key").FirstOrDefault().Value,
                dbOperation = p.Descendants("dbOperation").FirstOrDefault().Value
            });

            var keys = dbEngine.Keys();
            foreach (var result in results)
            {
                //ToDo: Check if already present in DB
                dbEngine.insert(result.key, result);
                //toDo: Update the persisted file with new DB contents
            }
            //Update Xml with new DB
            toXml(dbEngine);
        }
    }

#if (PERSIST_ENGINE_TEST)
    public class TestPersistEngine
    {
        static void Main(string[] args)
        {
        }
    }
#endif
}
