/////////////////////////////////////////////////////////////////////////
// ICommService.cs - Contract for WCF message-passing service          //
// ver 1.1                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Additions to C# Console Wizard generated code:
 * - Added reference to System.ServiceModel
 * - Added using System.ServiceModel
 * - Added reference to System.Runtime.Serialization
 * - Added using System.Runtime.Serialization
 */
/*
 * Maintenance History:
 * --------------------
  * ver 1.2 : 18 Nov 2015
 * - Added operation contracts that is used for transporting performance metrics 
 * ver 1.1 : 29 Oct 2015
 * - added comment in data contract
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Project4Starter
{
    [ServiceContract(Namespace = "Project4Starter")]
    public interface ICommService
    {
        [OperationContract(IsOneWay = true)]
        void sendMessage(Message msg);

        [OperationContract(IsOneWay = true)]
        void sendReaderLatency(double time);

        [OperationContract(IsOneWay = true)]
        void sendWriterLatency(double time);

        [OperationContract(IsOneWay = true)]
        void sendThroughputDetails(double time);

        [OperationContract]
        PerformanceDetails getPerformanceDetails();
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string fromUrl { get; set; }
        [DataMember]
        public string toUrl { get; set; }
        [DataMember]
        public string content { get; set; }  // will hold XML defining message information
    }

    [DataContract]
    public class PerformanceDetails
    {
        [DataMember]
        public double readerLatency { get; set; }
        [DataMember]
        public double writerLatency { get; set; }
        [DataMember]
        public double serverThroughput { get; set; }  // will hold XML defining message information
    }
}
