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
 * This package is the entry points which demonstrates that Proj 2 requirements can
 * be tested in Project 4. 
 * It also initiates the readers and writers for performance testing
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.0 : 18 Nov 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Configuration;
using System.Reflection;


namespace Project4Starter
{
    using Util = Utilities;
    class TestExecutive
    {
        string localUrl { get; set; } = "http://localhost:8050/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";
        string xmlLocation = ConfigurationManager.AppSettings["xmlLocation"];//"E:\\Syracuse University\\CSE 681 - SMA\\Assignments\\Proj4\\";
        string xmlName = ConfigurationManager.AppSettings["xmlName"];
        string readerLocation = ConfigurationManager.AppSettings["readerLocation"];
        string writerLocation = ConfigurationManager.AppSettings["writerLocation"];
        int numReaderMessages = Convert.ToInt32(ConfigurationManager.AppSettings["readerMessages"]);
        int numWriterMessages = Convert.ToInt32(ConfigurationManager.AppSettings["writerMessages"]);
        int readers = Convert.ToInt32(ConfigurationManager.AppSettings["Readers"]);
        int writers = Convert.ToInt32(ConfigurationManager.AppSettings["Writers"]);

        //----< retrieve urls from the CommandLine if there are any >--------

        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);

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
                sndr.SendReaderLatency(rcvr.totalExecutionTime / rcvr.count);
                sndr.SendWriterLatency(rcvr.totalExecutionTime / rcvr.count);
            }
        }

        private void LaunchReadersAndWriters()
        {
            
            var readerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderPort"]);            
            var writerPort = Convert.ToInt32(ConfigurationManager.AppSettings["WriterPort"]);
            var location = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);//AppDomain.CurrentDomain.BaseDirectory;
            
            for (var i = 0; i < writers; i++)
            {
                System.Diagnostics.Process.Start(location+writerLocation, " "+writerPort++ + " "+numReaderMessages);
            }

            for (var i = 0; i < readers; i++)
            {
                System.Diagnostics.Process.Start(location + readerLocation, " " + readerPort++ + " "+numWriterMessages);
            }
        }

        static void Main(string[] args)
        {
            TestExecutive test = new TestExecutive();
            if(args.Count() == 4)
            {
                test.readers = Convert.ToInt32(args[0]);
                test.writers = Convert.ToInt32(args[1]);
                test.numReaderMessages = Convert.ToInt32(args[2]);
                test.numWriterMessages = Convert.ToInt32(args[3]);
            }
            MessageMaker msgMaker = new MessageMaker();
            HiResTimer timer = new HiResTimer();
            Message msg = new Message();
            Console.Write("\nStarting Test Executive");
            Console.Write("\n=============================\n");
            Console.Title = "Test Executive";
            string localPort = Util.urlPort(test.localUrl);
            string localAddr = Util.urlAddress(test.localUrl);
            Receiver rcvr = new Receiver(localPort, localAddr, timer);
            if (rcvr.StartService())
            {
                rcvr.doService(rcvr.defaultServiceAction());
            }            
            Sender sndr = new Sender(test.localUrl);  // Sender needs localUrl for start message
            XDocument xmlInput = test.GetInputFromXml();
            var results = xmlInput.Descendants("element");
            Console.Write("\n  sender's url is {0}", test.localUrl);
            Console.Write("\n  attempting to connect to {0}\n", test.remoteUrl);
            timer.Start();
            test.sendMessageToServer(results, timer, msgMaker, sndr, rcvr);
            msg.content = "done";
            msg.fromUrl = test.localUrl;
            msg.toUrl = test.remoteUrl;
            sndr.sendMessage(msg);
            Console.WriteLine("\n\nPress any key to start performance testing...");
            Util.waitForUser();
            test.LaunchReadersAndWriters();
            rcvr.shutDown();
            sndr.shutdown();
        }
    }
}
