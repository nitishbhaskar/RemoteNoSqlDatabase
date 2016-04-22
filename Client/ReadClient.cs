/////////////////////////////////////////////////////////////////////////
// Client1.cs - CommService client sends and receives messages         //
// ver 2.1                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Used for performance testing.
 *
 * Additions to C# Console Wizard generated code:
 * - Added using System.Threading
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - in this incantation the client has Sender and now has Receiver to
 *   retrieve Server echo-back messages.
 * - If you provide command line arguments they should be ordered as:
 *   remotePort, remoteAddress, localPort, localAddress
 */
/*
 * Maintenance History:
 * --------------------
  * ver 2.2 : 18 Nov 2015
 * - Added new methods for reading input from XML and sending it to server.
 * ver 2.1 : 29 Oct 2015
 * - fixed bug in processCommandLine(...)
 * - added rcvr.shutdown() and sndr.shutDown() 
 * ver 2.0 : 20 Oct 2015
 * - replaced almost all functionality with a Sender instance
 * - added Receiver to retrieve Server echo messages.
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.Configuration;

namespace Project4Starter
{
    using Util = Utilities;

    ///////////////////////////////////////////////////////////////////////
    // Client class sends and receives messages in this version
    // - commandline format: /L http://localhost:8085/CommService 
    //                       /R http://localhost:8080/CommService
    //   Either one or both may be ommitted

    class ReadClient
    {
        string localUrl { get; set; } = "http://localhost:8081/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";

        string xmlLocation = ConfigurationManager.AppSettings["xmlLocation"];

        string xmlName = "ReaderLoad";

        int numMsg = 100;

        //----< retrieve urls from the CommandLine if there are any >--------

        /// <summary>
        /// Processes the command line.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void processCommandLine(string[] args)
        {
            localUrl = "http://localhost:" + args[0] + "/CommService";
        }

        /// <summary>
        /// Gets the input from XML.
        /// </summary>
        /// <returns></returns>
        private XDocument GetInputFromXml()
        {
            XDocument xmlDoc = XDocument.Load(xmlLocation + xmlName + ".xml");

            return xmlDoc;
        }

        /// <summary>
        /// Sends the message to server.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="timer">The timer.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="msgMaker">The MSG maker.</param>
        /// <param name="sndr">The SNDR.</param>
        /// <param name="rcvr">The RCVR.</param>
        private void sendMessageToServer(IEnumerable<XElement> results, HiResTimer timer,
            MessageMaker msgMaker, Sender sndr, Receiver rcvr)
        {
            timer.Start();
            int count = 1;
            foreach (var eachInput in results)
            {
                Message msg = new Message();
                msg = msgMaker.makeMessage(this.localUrl, this.remoteUrl, eachInput);
                if (!sndr.Connect(msg.toUrl))
                {
                    Console.Write("\n  could not connect in {0} attempts", sndr.MaxConnectAttempts);
                    sndr.shutdown();
                    rcvr.shutDown();
                    return;
                }
                "Message sent".endofTest();
                "Content".title();
                Console.Write("\n\n{0}\n", msg.content);
                "End of message".endofTest();
                while (true)
                {
                    if (sndr.sendMessage(msg))
                    {
                        Thread.Sleep(200);
                        break;
                    }
                }
                if (!Double.IsNaN(rcvr.totalExecutionTime / rcvr.count))
                    sndr.SendReaderLatency(rcvr.totalExecutionTime / rcvr.count);
                if (++count == numMsg) break;
            }
        }

        static void Main(string[] args)
        {
            HiResTimer timer = new HiResTimer();
            Message msg = new Message();
            MessageMaker msgMaker = new MessageMaker();
            Console.Write("\nStarting Test Executive");
            Console.Write("\n=============================\n");
            Console.Title = "Read Client";
            ReadClient readClient = new ReadClient();
            if (args.Length == 2)
                readClient.numMsg = Convert.ToInt32(args[1]);
            readClient.processCommandLine(args);
            string localPort = Util.urlPort(readClient.localUrl);
            string localAddr = Util.urlAddress(readClient.localUrl);
            Receiver rcvr = new Receiver(localPort, localAddr, timer);
            if (rcvr.StartService())
            {
                rcvr.doService(rcvr.defaultServiceAction());
            }
            Sender sndr = new Sender(readClient.localUrl);  // Sender needs localUrl for start message
            XDocument xmlInput = readClient.GetInputFromXml();
            var results = xmlInput.Descendants("element");
            Console.Write("\n  sender's url is {0}", readClient.localUrl);
            Console.Write("\n  attempting to connect to {0}\n", readClient.remoteUrl);
            readClient.sendMessageToServer(results, timer, msgMaker, sndr, rcvr);
            //Wait until all messages from server are received.
            while (rcvr.count < readClient.numMsg)
            {
                sndr.SendReaderLatency(rcvr.totalExecutionTime / rcvr.count);
            }
            msg.content = "done";
            msg.fromUrl = readClient.localUrl;
            msg.toUrl = readClient.remoteUrl;
            sndr.sendMessage(msg);
            Util.waitForUser();
            sndr.SendReaderLatency(rcvr.totalExecutionTime / rcvr.count);
            rcvr.shutDown();
            sndr.shutdown();
        }
    }
}

