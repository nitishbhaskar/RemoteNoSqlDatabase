/////////////////////////////////////////////////////////////////////////
// Server.cs - CommService server                                      //
// ver 2.2                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Additions to C# Console Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - This server now receives and then sends back received messages.
 */
/*
 * Plans:
 * - Add message decoding and NoSqlDb calls in performanceServiceAction.
 * - Provide requirements testing in requirementsServiceAction, perhaps
 *   used in a console client application separate from Performance 
 *   Testing GUI.
 */
/*
 * Maintenance History:
 * --------------------
  * ver 2.4 : 18 Nov 2015
 * - Added performance measurement, which measures server throughput and also linked the no SQL database
 * ver 2.3 : 29 Oct 2015
 * - added handling of special messages: 
 *   "connection start message", "done", "closeServer"
 * ver 2.2 : 25 Oct 2015
 * - minor changes to display
 * ver 2.1 : 24 Oct 2015
 * - added Sender so Server can echo back messages it receives
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 2.0 : 20 Oct 2015
 * - Defined Receiver and used that to replace almost all of the
 *   original Server's functionality.
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4Starter
{
    using Util = Utilities;

    class Server
    {
        string address { get; set; } = "localhost";
        string port { get; set; } = "8080";

        //----< quick way to grab ports and addresses from commandline >-----

        public void ProcessCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                port = args[0];
            }
            if (args.Length > 1)
            {
                address = args[1];
            }
        }

        /// <summary>
        /// Displays the received MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void displayReceivedMsg(Message msg)
        {
            "Received message".endofTest();
            "Sender: ".title();
            Console.Write("\n{0}\n", msg.fromUrl);
            "Content: ".title();
            Console.Write("\n\n{0}\n", msg.content);
            "End of message".endofTest();
        }

        /// <summary>
        /// Displays the sent MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="result">The result.</param>
        private void displaySentMsg(Message msg, string result)
        {
            "Response message".endofTest();
            "Server Response:".title();
            Console.Write("\n\n{0}", result);
            "End of message".endofTest();
        }

        /// <summary>
        /// Calculates the throughput.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="totalTime">The total time.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private double calculateThroughput(HiResTimer timer, ref double totalTime, int count)
        {
            totalTime += timer.ElapsedTimeSpan.TotalSeconds;
            double avgTime = totalTime / count;
            Console.Title = "Server: Avg process time =" + avgTime + " secs ; Throughput =" + 1 / avgTime + " req/sec Total Responses =" + count;
            return 1 / avgTime;
        }
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            DatabaseController dbController = new DatabaseController();
            Util.verbose = false;
            Server srvr = new Server();
            srvr.ProcessCommandLine(args);
            double totalTime = 0;
            int count = 0;
            Console.Title = "Server";
            Console.Write(String.Format("\n  Starting CommService server listening on port {0}", srvr.port));
            Console.Write("\n ====================================================\n");
            Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));
            Receiver rcvr = new Receiver(srvr.port, srvr.address);
            Action serviceAction = () =>
            {
                Message msg = null;
                while (true)
                {
                    HiResTimer timer = new HiResTimer();
                    timer.Start();
                    msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                    srvr.displayReceivedMsg(msg);
                    if (msg.content == "connection start message")
                        continue; // don't send back start message
                    if (msg.content == "done")
                    {
                        Console.Write("\n  client {0} has finished\n", msg.fromUrl);
                        continue;
                    }
                    if (msg.content == "closeServer")
                    {
                        Console.Write("received closeServer");
                        break;
                    }
                    if (msg.fromUrl == "http://localhost:8080/CommService")
                        continue;
                    var result = dbController.QueryDatabase(msg.content);
                    msg.content = "received " + msg.content + " from " + msg.fromUrl;
                    Util.swapUrls(ref msg);
                    Message testMsg = new Message { content = result, fromUrl = msg.fromUrl, toUrl = msg.toUrl };
                    srvr.displaySentMsg(msg, result);
                    sndr.sendMessage(testMsg);
                    timer.Stop();                    
                    sndr.SendServerThroughput(srvr.calculateThroughput(timer, ref totalTime,++count));
                }
            };
            if (rcvr.StartService())
                rcvr.doService(serviceAction); // so the call doesn't block.
            Util.waitForUser();
        }
    }
}
