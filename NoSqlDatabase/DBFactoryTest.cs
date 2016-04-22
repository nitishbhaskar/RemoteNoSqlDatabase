///////////////////////////////////////////////////////////////
// DBFactoryTest.cs - DB Factory for Project #2                  //
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
 * This package helps in testing creation immutable database in DBFactory
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs, DBElement, DBEngine, DBExtensions, Display, ItemEditor
 *   ItemFactory, QueryEngine, UtilityExtensions
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
using static System.Console;

namespace Project4Starter
{
    class DBFactoryTest
    {
#if DBFactoryTest
        static void Main(string[] args)
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();

            "Demonstrating Requirement #2".title();
            DBElement<int, string> elem = itemFactory.Create();
            elem.name = "element";
            elem.descr = "test element";
            elem.timeStamp = DateTime.Now.AddDays(-4);
            elem.children.AddRange(new List<int> { 1, 2, 3 });
            elem.payload = "elem's payload";
            WriteLine("\n Item to be inserted.. \n");
            elem.showElement();
            db.insert(1, elem);
            db.showDB();
            WriteLine("\n Inserting second element into DB :");
            DBElement<int, string> elem2 = new DBElement<int, string>();
            elem2.name = "element2";
            elem2.descr = "test element2";
            elem2.timeStamp = DateTime.Now;
            elem2.children.AddRange(new List<int> { 1, 2, 3, 4 });
            elem2.payload = "elem2's payload";
            WriteLine("\nItem to be inserted.. \n");
            elem2.showElement();
            db.insert(2, elem2);
            WriteLine("\n\n DB after insertion:");
            db.showDB();            
            WriteLine("\n\n DB after insertion:");
            db.showDB();            
            var dict = new Dictionary<int, DBElement<int, string>>();
            var keys = db.searchForTimeStamp(DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-3));
            Console.WriteLine("\nRunning a query on database..\n");
            Console.WriteLine("\n\nThe result are the following keys:\n\n");
            foreach (var item in keys)
            {
                dynamic result = db.searchValue(item);
                Console.Write("{0}, ", item);
                dict.Add(item, result);
            }
            DBFactory<int, DBElement<int, string>> dbFactory = new DBFactory<int, DBElement<int, string>>(dict);
            "Immutable database:".title('.');
            Console.WriteLine("\n\nImmutable database is of type DBFactory, which is immutable (Found in DBFactory.cs).");
        }
#endif
    }
}
