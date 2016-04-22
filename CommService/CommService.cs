/////////////////////////////////////////////////////////////////////////
// CommService.cs - Implementation of WCF message-passing service      //
// ver 1.1                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Additions to the C# Console Wizard code:
 * - added reference to System.ServiceModel
 * - added using System.ServiceModel
 * - added reference to Project4Starter.ICommService
 * - copied BlockingQueue.cs into project folder
 * - Added BlockingQueue.cs to project
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.2 : 18 Nov 2015
 * - Added operation contracts that is used for transporting performance metrics 
 * ver 1.1 : 24 Oct 2015
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
using System.ServiceModel;

namespace Project4Starter
{
    using SWTools;
    using Util = Utilities;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CommService : ICommService
    {
        // static rcvrQueue is shared by all instances of this class

        private static SWTools.BlockingQueue<Message> rcvrQueue =
          new SWTools.BlockingQueue<Message>();

        private static BlockingQueue<double> reader = new BlockingQueue<double>();
        private static BlockingQueue<double> writer = new BlockingQueue<double>();
        private static BlockingQueue<double> server = new BlockingQueue<double>();

        private static double averageReaderLatency;
        private static double averageWriterLatency;
        private static double serverThroughput;
        //----< called by clients, will only block briefly >-----------------

        public void sendMessage(Message msg)
        {
            if (Util.verbose)
                Console.Write("\n  this is CommService.sendMessage");
            rcvrQueue.enQ(msg);
        }
        //----< called by server, blocks caller while empty >----------------
        /*
         * Note: this is NOT a service method - see interface definition
         */
        public Message getMessage()
        {
            if (Util.verbose)
                Console.Write("\n  this is CommService.getMessage");
            return rcvrQueue.deQ();
        }

        /// <summary>
        /// Sends the reader latency.
        /// </summary>
        /// <param name="time">The time.</param>
        public void sendReaderLatency(double time)
        {
            averageReaderLatency = time;
            reader.enQ(time);
        }

        /// <summary>
        /// Sends the writer latency.
        /// </summary>
        /// <param name="time">The time.</param>
        public void sendWriterLatency(double time)
        {
            averageWriterLatency = time;
            writer.enQ(time);
        }

        /// <summary>
        /// Sends the throughput details.
        /// </summary>
        /// <param name="time">The time.</param>
        public void sendThroughputDetails(double time)
        {
            serverThroughput = time;
            server.enQ(time);
        }

        //Sends performance details to WPF
        /// <summary>
        /// Gets the performance details.
        /// </summary>
        /// <returns></returns>
        public PerformanceDetails getPerformanceDetails()
        {
            return new PerformanceDetails
            {
                readerLatency = averageReaderLatency,
                writerLatency = averageWriterLatency,
                serverThroughput = serverThroughput
            };
        }
    }
}
