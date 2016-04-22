///////////////////////////////////////////////////////////////
// QueryEngineTest.cs - DB Factory for Project #2             //
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
 * This package helps in testing Query Engine
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
    class QueryEngineTest
    {
            private void test1()
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbString = new DBEngine<string, DBElement<string, List<string>>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();
            //------------Search for value---------------------
            "Search for a value".title('.');
            WriteLine("\n\n Elements in DB :");
            db.showDB();
            int key = 2;
            WriteLine("\n\nSearching for value of Key {0}\n\n", key);
            var value = db.searchValue<int, string>(key);
            if (value != null)
            {
                value.showElement();
            }
            else
            {
                WriteLine("\nKey not found..!\n\n");
            }
            //------------Search for value---------------------

            //------------Search for children---------------------
            "Search for children:".title('.');
            WriteLine("\n\n Elements in DB :");
            db.showDB();
            key = 3;
            WriteLine("\n\nSearching for Children of Key {0}\n", key);
            dynamic children = db.searchChildren(key);
            foreach (var child in children)
            {
                Write("{0}, ", child);
            }
            //------------Search for children---------------------
        }

        private void test2()
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbString = new DBEngine<string, DBElement<string, List<string>>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();
            //------------Search for Keys with pattern---------------------
            "Search for keys with a pattern:".title('.');
            WriteLine("\n\n Elements in DB :");
            DBElement<string, List<string>> elemString = new DBElement<string, List<string>>();
            elemString.name = "element";
            elemString.descr = "test element";
            elemString.timeStamp = DateTime.Now;
            elemString.children.AddRange(new List<string> { "Martin", "Nitish", "Michelle" });
            elemString.payload = new List<string> { "elem's payload", "sample payload" };
            dbString.insert("Name", elemString);

            DBElement<string, List<string>> elemString1 = new DBElement<string, List<string>>();
            elemString1.name = "element";
            elemString1.descr = "test element";
            elemString1.timeStamp = DateTime.Now;
            elemString1.children.AddRange(new List<string> { "Syracuse", "Boston", "Mysore" });
            elemString1.payload = new List<string> { "elem's payload", "sample payload" };
            dbString.insert("Cities", elemString1);
            dbString.showEnumerableDB();
            string keyPattern = "^C";
            WriteLine("\n\nSearching for Keys with pattern: {0}\n", keyPattern);
            dynamic keys = dbString.searchForKeyPattern(keyPattern);
            WriteLine("\n\nMatched keys : \n");
            foreach (var item in keys)
            {
                Write("{0}, ", item);
            }
            //------------Search for Keys with pattern---------------------

            //------------Search for specified string in metadata---------------------
            "Search for specified string in metadata:".title('.');
            WriteLine("\n\n Elements in DB :");
            db.showDB();
            string searchString = "element3";
            WriteLine("\n\n\nSearch string : {0}", searchString);
            WriteLine("\n\nSearching for specified string in metadata: \n\n", searchString);
            var matchedKeys = db.searchMetadata(searchString);
            foreach (var matchedKey in matchedKeys) Write("{0}, ", matchedKey);
            //------------Search for specified string in metadata---------------------
        }
        
        private void test3()
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbString = new DBEngine<string, DBElement<string, List<string>>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();
            //---------------Search for data with specified time interval---------------
            "Search for data with specified time interval:".title('.');
            WriteLine("\n\n Elements in DB :");
            var elem = new DBElement<int, string>();
            elem.name = "old element";
            elem.descr = "old test element";
            elem.timeStamp = DateTime.Now.AddDays(-3);
            elem.children.AddRange(new List<int> { 85, 27, 65 });
            elem.payload = "old elem's payload";

            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.name = "old2 element";
            elem1.descr = "old2 test element";
            elem1.timeStamp = DateTime.Now.AddDays(-2);
            elem1.children.AddRange(new List<int> { 11, 12, 13 });
            elem1.payload = "old2 elem's payload";

            WriteLine("\n\n Search for Key which is created between {0} and {1}:\n\n", DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(-1).Date);
            db.insert(99, elem);
            db.insert(50, elem1);
            db.showDB();
            WriteLine();
            var resultkeys = db.searchForTimeStamp(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1));
            WriteLine("\n\nThe result is:\n\n");
            foreach (var item in resultkeys)
            {
                Write("{0}, ", item);
            }
            //---------------Search for data with specified time interval---------------
        }

#if QueryEngineTest
        static void Main(string[] args)
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbString = new DBEngine<string, DBElement<string, List<string>>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();

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
            WriteLine();

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

            //Inserting element 3
            WriteLine("\n Inserting third element into DB :");
            DBElement<int, string> elem3 = new DBElement<int, string>();
            elem3.name = "element3";
            elem3.descr = "test element3";
            elem3.timeStamp = DateTime.Now.AddDays(-5);
            elem3.children.AddRange(new List<int> { 5, 8, 10 });
            elem3.payload = "elem3's payload";
            WriteLine("\nItem to be inserted.. \n");
            elem3.showElement();
            db.insert(3, elem3);
            WriteLine("\n\n DB after insertion:");
            db.showDB();
            QueryEngineTest engineTest = new QueryEngineTest();
            engineTest.test1();
            engineTest.test2();
            engineTest.test3();
        }
#endif
    }
}
