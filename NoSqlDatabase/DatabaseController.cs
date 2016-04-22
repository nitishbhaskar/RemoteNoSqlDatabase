///////////////////////////////////////////////////////////////
// DBEngine.cs - define noSQL database                       //
// Ver 1.3                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell 7000, Core-i7, Windows 10               //
// Author:      Nitish Bhaskar, CE, Syracuse University      //
//              (315) 420-8933, nibhaska@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements DBEngine<Key, Value> where Value
 * is the DBElement<key, Data> type.
 *
 * This class acts as a mediator between the server and the noSQL database.
 * All requests that server receives are redirected here for query processing.
 */
/*
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 18 Nov 15
 * - first release
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project4Starter
{
    public class DatabaseController
    {
        private DBEngine<string, DBElement<string, List<string>>> db;
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseController"/> class.
        /// </summary>
        public DatabaseController()
        {
            this.db = new DBEngine<string, DBElement<string, List<string>>>();
        }

        /// <summary>
        /// Parses the XML.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private DBElement<string, List<string>> ParseXml(String input)
        {
            XElement element = XElement.Parse(input);
            DBElement<String, List<String>> dbElement = new DBElement<string, List<string>>();
            dbElement.dbOperation = element.Descendants("dbOperation").FirstOrDefault().Value;
            if (dbElement.dbOperation == "Add" || dbElement.dbOperation == "Edit")
            {
                dbElement.name = element.Descendants("name").FirstOrDefault().Value;
                dbElement.descr = element.Descendants("description").FirstOrDefault().Value;
                dbElement.payload = element.Descendants("payload").Select(q => { return q.Value; }).ToList<string>();
                dbElement.children = element.Descendants("children").Descendants("child").Select(q => { return q.Value; }).ToList<string>();
                dbElement.key = element.Descendants("key").FirstOrDefault().Value;
            }
            if (dbElement.dbOperation == "Delete" || dbElement.dbOperation == "SearchChild" || dbElement.dbOperation == "Search")
            {
                dbElement.key = element.Descendants("key").FirstOrDefault().Value;
            }
            return dbElement;
        }

        /// <summary>
        /// Queries the database.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public String QueryDatabase(String input)
        {
            String result = String.Empty;
            var dbElement = ParseXml(input);
            switch (dbElement.dbOperation)
            {
                case "Add":
                    db.insert(dbElement.key, dbElement);
                    result = "Key " + dbElement.key + " inserted successfully!";
                    break;
                case "Edit":
                    db.replaceKeyInstance(dbElement.key, dbElement);
                    result = "Key " + dbElement.key + "'s value modified successfully!";
                    break;
                case "Delete":
                    if (db.delete(dbElement.key))
                        result = "Key " + dbElement.key + " deleted successfully!";
                    else
                        result = "Key " + dbElement.key + " not found!!";
                    break;
                case "SearchChild":
                    var children = db.searchChildren(dbElement.key);
                    result = "Children are " + string.Join(", ", children.ToArray());
                    break;
                case "Search":
                    var value = db.searchValue(dbElement.key);
                    if (value != null)
                    {
                        result = "Key " + dbElement.key + " found!!!!!\n\n";
                        result += value.showEnumerableElement();
                    }
                    else
                        result = "Key " + dbElement.key + " not found!!";
                    break;
                case "Persist":
                    db.toXml();
                    result = "Database persisted successfully";
                    break;
                case "Restore":
                    db.restoreDatabase();
                    result = "Database successfully restored!!";
                    break;
                default: break;
            }
            string dbView = db.showEnumerableDB();
            "\nDB Contents:\n".title();
            result += dbView;
            return result;
        }

#if DatabaseController
        public void Main()
        {
            XElement root = new XElement("noSqlDb");
            XDocument xmlDoc = new XDocument();
            xmlDoc.Add(root);
            XElement element = new XElement("element");
            root.Add(element);
            XElement dbOperation = new XElement("dbOperation", "Search");
            element.Add(dbOperation);
            XElement keyValue = new XElement("key", "NewYork");
            element.Add(keyValue);
            DatabaseController dbController = new DatabaseController();
            var result = dbController.QueryDatabase(xmlDoc.ToString());
            Console.WriteLine(result);
        }
#endif
    }
}
