/////////////////////////////////////////////////////////////////////////
// Client2.cs - CommService client sends and receives messages         //
// ver 2.1                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Used for performance measurement
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

    class WriteClient
    {
        string localUrl { get; set; } = "http://localhost:8082/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";
        
        string xmlLocation = ConfigurationManager.AppSettings["xmlLocation"];//"E:\\Syracuse University\\CSE 681 - SMA\\Assignments\\Proj4\\";
        string xmlName = "WriterLoad";//ConfigurationManager.AppSettings["writexmlName"];
        int numMsg = 100;

        //----< retrieve urls from the CommandLine if there are any >--------

        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = "http://localhost:" + args[0] + "/CommService"; 
        }

        private XDocument GetInputFromXml()
        {
            XDocument xmlDoc = XDocument.Load(xmlLocation + xmlName + ".xml");

            return xmlDoc;
        }

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
                sndr.SendWriterLatency(rcvr.totalExecutionTime / rcvr.count);
                if (++count == numMsg) break;
            }
        }

        static void Main(string[] args)
        {
            MessageMaker msgMaker = new MessageMaker();
            Console.Write("\nStarting Test Executive");
            Console.Write("\n=============================\n");
            Message msg = new Message();
            HiResTimer timer = new HiResTimer();
            Console.Title = "Write Client";
            WriteClient writeClient = new WriteClient();
            if (args.Length == 2)
                writeClient.numMsg = Convert.ToInt32(args[1]);
            writeClient.processCommandLine(args);
            string localPort = Util.urlPort(writeClient.localUrl);
            string localAddr = Util.urlAddress(writeClient.localUrl);
            Receiver rcvr = new Receiver(localPort, localAddr, timer);
            if (rcvr.StartService())
            {
                rcvr.doService(rcvr.defaultServiceAction());
            }
            Sender sndr = new Sender(writeClient.localUrl);  // Sender needs localUrl for start message
            XDocument xmlInput = writeClient.GetInputFromXml();
            var results = xmlInput.Descendants("element");
            writeClient.sendMessageToServer(results, timer, msgMaker, sndr, rcvr);
            Console.Write("\n  sender's url is {0}", writeClient.localUrl);
            Console.Write("\n  attempting to connect to {0}\n", writeClient.remoteUrl);
            //Wait until all messages from server are received.
            while (rcvr.count < writeClient.numMsg)
            {
                sndr.SendWriterLatency(rcvr.totalExecutionTime / rcvr.count);
            }
            msg.content = "done";
            msg.fromUrl = writeClient.localUrl;
            msg.toUrl = writeClient.remoteUrl;
            sndr.sendMessage(msg);
            Util.waitForUser();
            sndr.SendWriterLatency(rcvr.totalExecutionTime / rcvr.count);
            rcvr.shutDown();
            sndr.shutdown();
        }
    }
}

