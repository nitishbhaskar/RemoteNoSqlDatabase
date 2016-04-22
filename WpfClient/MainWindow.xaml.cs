/////////////////////////////////////////////////////////////////////////
// MainWindows.xaml.cs - CommService GUI Client                        //
// ver 2.0                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University             //
//              (315) 443-3948, jfawcett@twcny.rr.com                   //
// Author:      Nitish Bhaskar, CE, Syracuse University                 //
//              (315) 420-8933, nibhaska@syr.edu                        //
/////////////////////////////////////////////////////////////////////////
/*
 * Displays performance metrics on click of start button
 *
 * Additions to C# WPF Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, MakeMessage, Utilities
 * - Added using Project4Starter
 *
 * Note:
 * - This client receives and sends messages.
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
  * ver 2.1 : 18 Nov 2015
 * - changed Xaml to display only the performance information such as Latency and throughput measurement.
 * ver 2.0 : 29 Oct 2015
 * - changed Xaml to achieve more fluid design
 *   by embedding controls in grid columns as well as rows
 * - added derived sender, overridding notification methods
 *   to put notifications in status textbox
 * - added use of MessageMaker in send_click
 * ver 1.0 : 25 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Project4Starter;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        static bool firstConnect = true;
        static Receiver rcvr = null;
        static wpfSender sndr = null;
        string localAddress = "localhost";
        string localPort = "8089";
        string remoteAddress = "localhost";
        string remotePort = "8080";

        /////////////////////////////////////////////////////////////////////
        // nested class wpfSender used to override Sender message handling
        // - routes messages to status textbox
        public class wpfSender : Sender
        {
            TextBox lStat_ = null;  // reference to UIs local status textbox
            System.Windows.Threading.Dispatcher dispatcher_ = null;

            public wpfSender(TextBox lStat, System.Windows.Threading.Dispatcher dispatcher)
            {
                dispatcher_ = dispatcher;  // use to send results action to main UI thread
                lStat_ = lStat;
            }
            public override void sendMsgNotify(string msg)
            {
                Action act = () => { lStat_.Text = msg; };
                dispatcher_.Invoke(act);

            }
            public override void sendExceptionNotify(Exception ex, string msg = "")
            {
                Action act = () => { lStat_.Text = ex.Message; };
                dispatcher_.Invoke(act);
            }
            public override void sendAttemptNotify(int attemptNumber)
            {
                Action act = null;
                act = () => { lStat_.Text = String.Format("attempt to send #{0}", attemptNumber); };
                dispatcher_.Invoke(act);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            rAddr.Text = remoteAddress;
            rPort.Text = remotePort;
            Title = "Prototype WPF Client";
        }

        public void getPerformanceDetails()
        {
            while (true)
            {
                displayPerformanceDetails(sndr.GetPerformanceDetails());
                Thread.Sleep(2000);
            }
        }
        //----< trim off leading and trailing white space >------------------

        string trim(string msg)
        {
            StringBuilder sb = new StringBuilder(msg);
            for (int i = 0; i < sb.Length; ++i)
                if (sb[i] == '\n')
                    sb.Remove(i, 1);
            return sb.ToString().Trim();
        }
        //----< indirectly used by child receive thread to post results >----

        public void postRcvMsg(string content)
        {
            TextBlock item = new TextBlock();
            item.Text = trim(content);
            item.FontSize = 16;
            rcvmsgs.Items.Insert(0, item);
        }
        //----< used by main thread >----------------------------------------

        public void postSndMsg(string content)
        {
            TextBlock item = new TextBlock();
            item.Text = trim(content);
            item.FontSize = 16;
        }
        //----< get Receiver and Sender running >----------------------------

        public void displayPerformanceDetails(PerformanceDetails performanceDetails)
        {
            TextBlock item = new TextBlock();
            rcvmsgs.Items.Clear();
            StringBuilder displayMessage = new StringBuilder();
            displayMessage.AppendLine("Reader Latency = "+performanceDetails.readerLatency + " seconds");
            displayMessage.AppendLine("Writer Latency = " + performanceDetails.writerLatency+ " seconds");
            displayMessage.AppendLine("Server Throughput = "+performanceDetails.serverThroughput+ " requests/second");
            item.Text = displayMessage.ToString();
            item.FontSize = 16;
            rcvmsgs.Items.Insert(0, item);
        }

        void setupChannel()
        {
            rcvr = new Receiver(localPort, localAddress);
            Action serviceAction = () =>
            {
                try
                {
                    while (true)
                    {
                        Action act = () =>
                        {
                            displayPerformanceDetails(sndr.GetPerformanceDetails());
                        };
                        Dispatcher.Invoke(act, System.Windows.Threading.DispatcherPriority.Background);
                    }
                }
                catch (Exception ex)
                {
                    Action act = () => { rStat.Text = ex.Message; };
                    Dispatcher.Invoke(act);
                }
            };
            if (rcvr.StartService())
            {
                rcvr.doService(serviceAction);
            }

            sndr = new wpfSender(rStat, this.Dispatcher);
        }
        //----< set up channel after entering ports and addresses >----------

        private void start_Click(object sender, RoutedEventArgs e)
        {
            remoteAddress = rAddr.Text;
            remotePort = rPort.Text;

            if (firstConnect)
            {
                firstConnect = false;
                if (rcvr != null)
                    rcvr.shutDown();
                setupChannel();
            }
            rStat.Text = "connect setup";
            connect.IsEnabled = false;
        }
        
        private void rcvmsgs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
