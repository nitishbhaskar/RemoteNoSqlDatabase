///////////////////////////////////////////////////////////////
// ItemEditor.cs - DB Factory for Project #2                 //
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
 * This package helps in modifying the database items
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBFactory.cs, DBEngine, DBElement
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

namespace Project4Starter
{
    public static class ItemEditor
    {
        public static bool addRelationship<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key, List<Key> children)
        {
            DBElement<Key, Value> elem;
            if (dbEngine.containsKey(key))
            {
                dbEngine.getValue(key, out elem);
                elem.children.AddRange(children);
                return true;
            }
            return false;
        }

        public static bool removeRelationship<Key,Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key, List<Key> children)
        {
            DBElement<Key, Value> elem;
            if (dbEngine.containsKey(key))
            {
                dbEngine.getValue(key, out elem);
                foreach (var child in children)
                {
                    if (elem.children.Contains(child))
                    {
                        elem.children.Remove(child);
                    }
                }
                return true;
            }
            return false;
        }

        public static bool modifyMetadata<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key, string name, string description)
        {
            DBElement<Key, Value> elem;
            if (dbEngine.containsKey(key))
            {
                dbEngine.getValue(key, out elem);
                elem.name = name;
                elem.descr = description;
                return true;
            }
            return false;
        }

        public static bool replaceKeyInstance<Key, Value>(this DBEngine<Key, DBElement<Key, Value>> dbEngine, Key key, DBElement<Key, Value> val)
        {
            if (dbEngine.containsKey(key))
            {
                dbEngine.saveToDb(key,val);
                return true;
            }
            return false;
        }
    }

#if (ITEM_EDITOR_TEST)
    class ItemEditorTest
    {
        static void Main(string[] args)
        {
        }
    }
#endif
}
