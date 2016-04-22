///////////////////////////////////////////////////////////////
// ItemEditorTest.cs - DB Factory for Project #2             //
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
 * This package helps in testing Item Editor
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
    class ItemEditorTest
    {
        private void test()
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();
            int key = 0;
            //-----------Modify Metadata--------------------------
            "Modify metadata".title('.');
            WriteLine("\n Elements in DB :");
            db.showDB();
            key = 2;
            string name = "Nitish";
            string description = "MS in CE";
            WriteLine("\n\n\n New Details for metadata of key {0} :", key);
            WriteLine("\n Name : {0}", name);
            WriteLine("\n Description : {0}", description);
            WriteLine();
            db.modifyMetadata(key, name, description);
            WriteLine("\n\n DB after the change :");
            db.showDB();
            WriteLine();
            "".demarcation();
            //-----------Modify Metadata--------------------------

            //-------------------Replace Value instance-----------------------
            "Replace value instance".title('.');
            WriteLine("\n\n Elements in DB :");
            DBElement<int, string> elem = itemFactory.Create();
            db.showDB();
            key = 2;
            //DBElement<int, string> elem = new DBElement<int, string>();
            elem.name = "new element";
            elem.descr = "new test element";
            elem.timeStamp = DateTime.Now;
            elem.children.AddRange(new List<int> { 7, 8, 9 });
            elem.payload = "new elem's payload";
            WriteLine("\n\n New instance for replacement for key {0}:\n", key);
            elem.showElement();
            WriteLine("\n\n Replace instance of a key :");
            db.replaceKeyInstance(key, elem);
            WriteLine("\n\n DB after the change :");
            db.showDB();
            WriteLine();
            "".demarcation();
            //-------------------Replace Value instance-----------------------
        }
#if ItemEditorTest
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
            "Adding relationship :".title('.');
            WriteLine("\n Elements in DB :");
            db.showDB();
            int key = 2;
            var children = new List<int> { 5, 6 };
            WriteLine("\n Add children for key {0} : ", key);
            foreach (var child in children)
                Write("{0}, ", child);
            WriteLine();
            db.addRelationship(key, children);
            WriteLine("\n\n DB after the change :");
            db.showDB();
            WriteLine();
            "".demarcation();
            ItemEditorTest editorTest = new ItemEditorTest();
            editorTest.test();
        }
#endif
    }
}
