///////////////////////////////////////////////////////////////
// DBEngineTest.cs - Test DBEngine and DBExtensions          //
// Ver 1.0                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
// Author:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
// Author:      Nitish Bhaskar, CE, Syracuse University      //
//              (315) 420-8933, nibhaska@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package replaces DBEngine test stub to remove
 * circular package references.
 *
 * Now this testing depends on the class definitions in DBElement,
 * DBEngine, and the extension methods defined in DBExtensions.
 * We no longer need to define extension methods in DBEngine.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBEngineTest.cs,  DBElement.cs, DBEngine.cs,  
 *   DBExtensions.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 24 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using static System.Console;

namespace Project4Starter
{
    class Program
    {
        private void test1()
        {
            Write("\n --- Test DBElement<int,string> ---");
            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.payload = "a payload";
            Write(elem1.showElement<int, string>());
            WriteLine();

            DBElement<int, string> elem2 = new DBElement<int, string>("Darth Vader", "Evil Overlord");
            elem2.payload = "The Empire strikes back!";
            Write(elem2.showElement<int, string>());
            WriteLine();

            var elem3 = new DBElement<int, string>("Luke Skywalker", "Young HotShot");
            elem3.children.AddRange(new List<int> { 1, 5, 23 });
            elem3.payload = "X-Wing fighter in swamp - Oh oh!";
            Write(elem3.showElement<int, string>());
            WriteLine();

            Write("\n --- Test DBEngine<int,DBElement<int,string>> ---");

            int key = 0;
            Func<int> keyGen = () => { ++key; return key; };  // anonymous function to generate keys

            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            bool p1 = db.insert(keyGen(), elem1);
            bool p2 = db.insert(keyGen(), elem2);
            bool p3 = db.insert(keyGen(), elem3);
            if (p1 && p2 && p3)
                Write("\n  all inserts succeeded");
            else
                Write("\n  at least one insert failed");
            db.show<int, DBElement<int, string>, string>();
            WriteLine();           
        }
        
        private void test2()
        {
            Write("\n --- Test DBElement<string,List<string>> ---");
            DBElement<string, List<string>> newerelem1 = new DBElement<string, List<string>>();
            newerelem1.name = "newerelem1";
            newerelem1.descr = "better formatting";
            newerelem1.payload = new List<string> { "alpha", "beta", "gamma" };
            newerelem1.payload.Add("delta");
            newerelem1.payload.Add("epsilon");
            Write(newerelem1.showElement<string, List<string>, string>());
            WriteLine();

            DBElement<string, List<string>> newerelem2 = new DBElement<string, List<string>>();
            newerelem2.name = "newerelem2";
            newerelem2.descr = "better formatting";
            newerelem1.children.AddRange(new[] { "first", "second" });
            newerelem2.payload = new List<string> { "a", "b", "c" };
            newerelem2.payload.Add("d");
            newerelem2.payload.Add("e");
            Write(newerelem2.showElement<string, List<string>, string>());
            WriteLine();
            Write("\n --- Test DBEngine<string,DBElement<string,List<string>>> ---");

            int seed = 0;
            string skey = seed.ToString();
            Func<string> skeyGen = () =>
            {
                ++seed;
                skey = "string" + seed.ToString();
                skey = skey.GetHashCode().ToString();
                return skey;
            };
            int key = 0;
            Func<int> keyGen = () => { ++key; return key; };
            DBEngine<string, DBElement<string, List<string>>> newdb =
              new DBEngine<string, DBElement<string, List<string>>>();
            newdb.insert(skeyGen(), newerelem1);
            newdb.insert(skeyGen(), newerelem2);
            newdb.show<string, DBElement<string, List<string>>, List<string>, string>();
            WriteLine();
        }
#if DBEngineTest
        static void Main(string[] args)
        {
            "Testing DBEngine Package".title('=');
            WriteLine();
            Program dbEngineTest = new Program();
            dbEngineTest.test1();
            dbEngineTest.test2();
        }
#endif
    }
}
