///////////////////////////////////////////////////////////////
// DBEngine.cs - define noSQL database                       //
// Ver 1.3                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
// Source:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
// Author:      Nitish Bhaskar, CE, Syracuse University      //
//              (315) 420-8933, nibhaska@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements DBEngine<Key, Value> where Value
 * is the DBElement<key, Data> type.
 *
 * This class is a starter for the DBEngine package you need to create.
 * It doesn't implement many of the requirements for the db, e.g.,
 * It doesn't remove elements, doesn't persist to XML, doesn't retrieve
 * elements from an XML file, and it doesn't provide hook methods
 * for scheduled persistance.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs, and
 *                 UtilityExtensions.cs only if you enable the test stub
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.3 : 07 Oct 15
 * - added saveToDb and containsKey methods
 * ver 1.2 : 24 Sep 15
 * - removed extensions methods and tests in test stub
 * - testing is now done in DBEngineTest.cs to avoid circular references
 * ver 1.1 : 15 Sep 15
 * - fixed a casting bug in one of the extension methods
 * ver 1.0 : 08 Sep 15
 * - first release
 *
 */

using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Project4Starter
{
    public class DBEngine<Key, Value>
    {
        #region private instance fields

        private Dictionary<Key, Value> dbStore;

        #endregion

        public DBEngine()
        {
            dbStore = new Dictionary<Key, Value>();
        }

        public bool insert(Key key, Value val)
        {
            if (dbStore.Keys.Contains(key))
                return false;
            dbStore[key] = val;
            return true;
        }

        public bool saveToDb(Key key, Value val)
        {
            if (dbStore.Keys.Contains(key))
            {
                dbStore[key] = val;
                return true;
            }
            return false;
        }

        public bool getValue(Key key, out Value val)
        {
            if (dbStore.Keys.Contains(key))
            {
                val = dbStore[key];
                return true;
            }
            val = default(Value);
            return false;
        }

        public bool containsKey(Key key)
        {
            if (dbStore.Keys.Contains(key))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Key> Keys()
        {
            return dbStore.Keys;
        }

        public bool delete(Key key)
        {
            if (dbStore.Keys.Contains(key))
            {
                dbStore.Remove(key);
                return true;
            }
            return false;
        }
    }
#if (TEST_DBENGINE)

    class TestDBEngine
    {
        static void Main(string[] args)
        {
            "Testing DBEngine Package".title('=');
            WriteLine();

            Write("\n  All testing of DBEngine class moved to DBEngineTest package.");
            Write("\n  This allow use of DBExtensions package without circular dependencies.");

            Write("\n\n");
        }
    }
#endif

}
