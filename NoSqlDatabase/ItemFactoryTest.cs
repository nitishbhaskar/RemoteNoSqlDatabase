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
 * This package helps in testing Item Factory
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
    class ItemFactoryTest
    {
#if ItemFactoryTest
        static void Main(string[] args)
        {
            ItemFactory<int, string> itemFactory = new ItemFactory<int, string>();

            DBElement<int, string> elem = itemFactory.Create();

            WriteLine("\n DB Element of type {0} is created.\n",elem.GetType());
        }
#endif
    }
}
