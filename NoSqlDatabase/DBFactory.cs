///////////////////////////////////////////////////////////////
// DBFactory.cs - DB Factory for Project #2                  //
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
 * This package helps in creating immutable database
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs
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
using System.Collections.Generic;
using System.Linq;
using System;

namespace Project4Starter
{
    public class DBFactory<Key, Value>
    {
        #region Private instance fields
        private Dictionary<Key, Value> dbStore;
        #endregion

        public DBFactory(Dictionary<Key, Value> dbElements)
        {
            dbStore = new Dictionary<Key, Value>();
            foreach (dynamic elem in dbElements)
            {
                insertIntoImmutableDb(elem.Key, elem.Value);
            }
        }

        private void insertIntoImmutableDb(Key key, Value val)
        {
            dbStore[key] = val;
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

        public IEnumerable<Key> Keys()
        {
            return dbStore.Keys;
        }

    }

#if (TEST_DBFACTORY)
    class TestDBFactory
    {
        static void Main(string[] args)
        {
            
        }
    }
#endif
}
