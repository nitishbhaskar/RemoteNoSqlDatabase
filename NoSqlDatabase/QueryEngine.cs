///////////////////////////////////////////////////////////////
// QueryEngine.cs - DB Factory for Project #2                //
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
 * This package helps in querying the database.
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs, DBElement, DBEngine
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
using System.Text.RegularExpressions;

namespace Project4Starter
{
    public static class QueryEngine
    {
        public static DBElement<Key, Value> searchValue<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key)
        {
            //ToDo: Check whether key exist
            DBElement<Key, Value> val;
            dbEngine.getValue(key, out val);
            return val;
        }

        public static List<Key> searchChildren<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key)
        {
            List<Key> children = new List<Key>();
            DBElement<Key, Value> val;

            if (dbEngine.containsKey(key))
            {
                dbEngine.getValue(key, out val);
                foreach (var child in val.children)
                {
                    children.Add(child);
                }
                return children;
            }
            return children;
        }

        public static List<Key> searchForKeyPattern<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, string keyPattern)
        {

            List<Key> matchedKeys = new List<Key>();
            dynamic keys = dbEngine.Keys();
            DBElement<Key, Value> val;

            foreach (var key in keys)
            {
                dbEngine.getValue(key, out val);

                if ((Regex.IsMatch(key, keyPattern)))
                {
                    matchedKeys.Add(key);
                }
            }
            return matchedKeys;
        }

        public static List<Key> searchMetadata<Key,Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, string searchString)
        {
            List<Key> matchedKeys = new List<Key>();
            dynamic keys = dbEngine.Keys();
            DBElement<Key, Value> val;

            foreach (var key in keys)
            {
                dbEngine.getValue(key, out val);

                if (Regex.IsMatch(val.name, searchString) || Regex.IsMatch(val.descr, searchString))
                {
                    matchedKeys.Add(key);
                }
            }
            return matchedKeys;
        }

        public static List<Key> searchForTimeStamp<Key,Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, DateTime from, DateTime? to)
        {
            List<Key> matchedKeys = new List<Key>();
            dynamic keys = dbEngine.Keys();
            DBElement<Key, Value> val;

            if (!to.HasValue)
            {
                to = DateTime.Now;
            }

            foreach (var key in keys)
            {
                dbEngine.getValue(key, out val);

                if (val.timeStamp >= from && val.timeStamp <= to)
                {
                    matchedKeys.Add(key);
                }
            }
            return matchedKeys;
        }

        public static List<string> searchForKeysInCategory(this DBEngine<string, DBElement<string, List<string>>> dbEngine, List<string> category)
        {
            List<string> matchedKeys = new List<string>();
            dynamic keys = dbEngine.Keys();
            
            foreach (var cat in category)
            {
                //dbEngine.getValue(cat, out val);
                foreach(var key in keys)
                {
                    if(category.Contains(key))
                    {
                        matchedKeys.Add(key);
                    }
                }
            }
            return matchedKeys;
        }
    }
#if (QUERY_ENGINE_TEST)
    class QueryEngineTest
    {
        static void Main(string[] args)
        {
        }
    }
#endif
}

