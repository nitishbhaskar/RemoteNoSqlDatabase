///////////////////////////////////////////////////////////////
// ItemFactory.cs - DB Factory for Project #2                //
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
 * This package helps in creating an instance of DB Element.
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs, DBElement, UtilityExtensions
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

namespace Project4Starter
{
    public class ItemFactory<Key, Value>
    {
        public DBElement<Key, Value> Create()
        {
            return new DBElement<Key, Value>();
        }
    }

#if ITEM_FACTORY
    class TestItemFactory
    {
        static void Main(string[] args)
        {
            "Testing Item Factory".title();
            ItemFactory<int, string> itemFactoryInstance = new ItemFactory<int, string>();
            var newElement = itemFactoryInstance.Create();            
            Console.WriteLine("\nThe new element created using Item factory of {0}\n is of type {1}",itemFactoryInstance.GetType(), newElement.GetType());

            ItemFactory<string, List<string>> itemFactoryInstance1 = new ItemFactory<string, List<string>>();
            var newElement1 = itemFactoryInstance1.Create();
            Console.WriteLine("\n\nThe new element created using Item factory of\n {0} is of type {1}", itemFactoryInstance1.GetType(), newElement1.GetType());
        }
    }
#endif
}
