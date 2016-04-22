///////////////////////////////////////////////////////////////
// PersistEngineTest.cs - DB Factory for Project #2             //
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
 * This package helps in testing Persist Engine
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
    class PersistEngineTest
    {
#if PersistEngine
        static void Main(string[] args)
        {
            DBEngine<string, DBElement<string, List<string>>> db = new DBEngine<string, DBElement<string, List<string>>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();

            "Demonstrating Requirement #2".title();
            DBElement<string, List<string>> elem = new DBElement<string, List<string>>();
            elem.name = "element";
            elem.descr = "test element";
            elem.timeStamp = DateTime.Now.AddDays(-4);
            elem.children.AddRange(new List<string> { "1", "2", "3 " });
            elem.payload = new List<string> { "elem's payload" };
            WriteLine("\n Item to be inserted.. \n");
            elem.showEnumerableElement();
            db.insert("1", elem);
            db.showEnumerableDB();
            WriteLine();

            WriteLine("\n Inserting second element into DB :");
            DBElement<string, List<string>> elem2 = new DBElement<string, List<string>>();
            elem2.name = "element2";
            elem2.descr = "test element2";
            elem2.timeStamp = DateTime.Now;
            elem2.children.AddRange(new List<string> { "1", "2", "3", "4" });
            elem2.payload = new List<string> { "elem's payload" };
            WriteLine("\nItem to be inserted.. \n");
            elem2.showElement();
            db.insert("2", elem2);
            WriteLine("\n\n DB after insertion:");
            db.showEnumerableDB();

            "Persisting the database to XML:".title('_');
            db.toXml();
            "Augumenting database from XML".title('_');
            db.restoreDatabase();
            db.showEnumerableDB();
        }
#endif
    }
}
