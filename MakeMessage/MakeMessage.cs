/////////////////////////////////////////////////////////////////////////
// MessageMaker.cs - Construct ICommService Messages                   //
// ver 1.0                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Purpose:
 *----------
 * This is a placeholder for application specific message construction
 *
 * Additions to C# Console Wizard generated code:
 * - references to ICommService and Utilities
 */
/*
 * Maintenance History:
 * --------------------
  * ver 1.2 : 18 Nov 2015
 * - Added new methods for processing XML as string.
 * ver 1.0 : 29 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project4Starter
{
    public class MessageMaker
    {
        public static int msgCount { get; set; } = 0;
        /// <summary>
        /// Makes the message.
        /// </summary>
        /// <param name="fromUrl">From URL.</param>
        /// <param name="toUrl">To URL.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public Message makeMessage(string fromUrl, string toUrl, XElement input)
        {
            Message msg = new Message();
            msg.fromUrl = fromUrl;
            msg.toUrl = toUrl;
            msg.content = input.ToString();//String.Format("\n  message #{0}", ++msgCount);
            return msg;
        }

        /// <summary>
        /// To the XML string.
        /// </summary>
        /// <param name="dbEngine">The database engine.</param>
        /// <returns></returns>
        private String ToXmlString(DBEngine<string, DBElement<string, List<String>>> dbEngine)
        {
            dynamic dict = dbEngine.Keys();
            XDocument xmlDoc = new XDocument();
            XElement root = new XElement("noSqlDb");
            xmlDoc.Add(root);
            return xmlDoc.ToString();
        }

#if (TEST_MESSAGEMAKER)
        static void Main(string[] args)
        {
            MessageMaker mm = new MessageMaker();
            XElement keyValue = new XElement("key", 2);
            var  message = mm.makeMessage("localhost:8090", "localhost:8080", keyValue);
            Console.WriteLine(message.content);
        }
#endif
    }
}
