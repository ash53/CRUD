using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Rti;
using WpfDocViewer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rti.InternalInterfaces.ServiceProxies;
using System.Data;
using System.Windows;

namespace WpfDocViewer.ViewModel
{
	public class MatchingViewModel : INotifyPropertyChanged
	{
        private string matchMessage = "";
        int selectedRow = 0;
     

		public MatchingViewModel()
		{
            MatchingChecksCollection = new ObservableCollection<MatchingChecks>();
			BindData();
            this.ReleaseMatch = new RelayCommand(OnRelease);
            this.RemoveSelectedItem = new RelayCommand(OnRemoveItem);
            this.OpenViewer = new RelayCommand(OnView);
		}

        private ObservableCollection<MatchingChecks> matchingChecksCollection;
        public ObservableCollection<MatchingChecks> MatchingChecksCollection
		{
            get { return matchingChecksCollection; }
			set
			{
                matchingChecksCollection = value;
                RaisePropertyChanged("MatchingChecksCollection");
			}
		}



   

		private void BindData()
		{
          /*  MatchingChecksCollection.Add(new MatchingChecks("12345", "CK", 42.32, 25.36) { Description = "Check {0} Matched. " });
            MatchingChecksCollection.Add(new MatchingChecks("12345", "RM", 39.45, 22.30) { Description = "Remit not Matched." });
            MatchingChecksCollection.Add(new MatchingChecks("12345", "RM", 40.13, 26.75) { Description = "Remit not Matched." });
            MatchingChecksCollection.Add(new MatchingChecks("67789", "CK", 40.40, 23.23) { Description = "Pending Match" });
            MatchingChecksCollection.Add(new MatchingChecks("67789", "RM", 40.69, 23.10) { Description = "Pending Match" });
            MatchingChecksCollection.Add(new MatchingChecks("224566", "CK", 37.15, 20.06) { Description = "Check Not Matched. " });
            MatchingChecksCollection.Add(new MatchingChecks("224566", "RM", 43.05, 28.08) { Description = "Remit Not Matched. " });
		
           */
        }

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
        
		public void RaisePropertyChanged(string propertyName)
		{
			if (null != PropertyChanged)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

        public string MatchMessage
        {
            get { return matchMessage; }
            set
            {
                matchMessage = value;
                RaisePropertyChanged("MatchMessage");
            }
        }
        public int SelectedRow
        {
            get { return selectedRow; }
            set
            {
                selectedRow = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand ReleaseMatch
        {
            get;
            set;
        }

        public RelayCommand PendMatch
        {
            get;
            set;
        }
        public RelayCommand DeleteMatch
        {
            get;
            set;
        }
        public RelayCommand RemoveSelectedItem
        {
            get;
            set;
        }
        public RelayCommand OpenViewer
        {
            get;
            set;
        }
		#endregion
        /// <summary>
        /// used to Release a Match.
        /// </summary>
        public void OnRelease()
        { 
                    /*match_create:
                    <iQInterface name="match_create" version="1" rpc="iqrpc/cashremit/match_create.p">
                      <in  seq="1" name="inuserid"         type="character"/>
                      <in  seq="2" name="intmp" type="table">
                        <field seq="1"  name="key"                 type="integer"/>
                        <field seq="2"  name="srcfileflag"         type="character"/>
                      </in>
                      <out seq="1" name="outParams" type="character"/>
                      <out seq="2" name="outResult" type="logical"/>
                    </iQInterface>

                    match_approval:
                    <iQInterface name="match_approval" version="1" rpc="iqrpc/cashremit/match_approval.p">
                      <in  seq="1" name="inuserid"         type="character"/>
                      <in  seq="2" name="inmatchid"        type="integer"/>
                      <out seq="1" name="outParams"        type="character"/>
                      <out seq="2" name="outResult"        type="logical"/>
                    </iQInterface>
                    */
            //update CK's pardocno
            ResourceDictionary resources = App.Current.Resources;
            //Mark Lane a little linq to get me a proper pardocno.
            try
            {
                string parentDocno = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "CK" || data.Doctype == "CE" select (string)data.ParDocno).FirstOrDefault().ToString();
                resources["ParentDocno"] = parentDocno;
            }
            catch(Exception ex)
            {
                MessageBox.Show("You must have at least one check in cart to match.");
                return;
            }
            
            if (ValidateMatch() != true)
            {
                return;
            }

            MatchMessage = "Create and Approve Match";
            DataTable createMatch = new DataTable("intmp");
           
            createMatch.Columns.Add("key", System.Type.GetType("System.Int32"));
            createMatch.Columns.Add("srcfileflag", System.Type.GetType("System.String"));
            foreach(MatchingChecks match in Model.MatchingChecksCollection.MatchingChecksObservableCollection)
            {
                DataRow dr;
                dr = createMatch.NewRow();
                createMatch.Rows.Add(dr);
                dr["key"] = match.Key;
                dr["srcfileflag"] = match.SrcFileFlag;
            }


            try
            {
                using (var eiqProxy = new EagleIqGatewayClient(Constants.EagleIqGatewayServiceURL, Constants.EagleIqGatewayServiceENDPOINT_NAME))
                {
                    // if (eiqProxy.IsAlive())
                    // {
                    Rti.InternalInterfaces.DataContracts.RpcInMessage RpcInMessage = new Rti.InternalInterfaces.DataContracts.RpcInMessage();

                    var results = eiqProxy.CreateMatch(new Rti.InternalInterfaces.DataContracts.RpcInMessage() { Context = "", UserName = Environment.UserName, Workstation = Environment.MachineName }, "mpi", createMatch);
                    if (results != null)
                    {
                        if(results.IsSuccess == true)
                        {
                            int matchid = Convert.ToInt32(results.OutMessage);
                            var returnApprove = eiqProxy.ApproveMatch(new Rti.InternalInterfaces.DataContracts.RpcInMessage() { Context = "", UserName = Environment.UserName, Workstation = Environment.MachineName }, "longrpc", matchid);
                            if (returnApprove.IsSuccess == true)
                            {
                                MatchMessage = "Success on Match! " + returnApprove.OutMessage.ToString();
                                MessageBox.Show("Success on Match! " + returnApprove.OutMessage.ToString());
                                Model.MatchingChecksCollection.MatchingChecksObservableCollection.Clear();
                                Model.MatchingChecksCollection.CartCount = 0;
                            }
                            else
                            {
                                MessageBox.Show("Approve Match failed.  " + returnApprove.OutMessage);
                                MatchMessage = "Failed to Approve Match " + returnApprove.OutMessage.ToString();

                            }
                        }
                        else
                        {
                            MessageBox.Show("Create Match failed.  " + results.OutMessage);
                            MatchMessage = "Create Match failed : " + results.OutMessage;

                        }
                    }
                }
             
            }
            catch(Exception ex)
            {
                MatchMessage = "Failed : " + ex.Message.ToString();
                MessageBox.Show("Failed on EIQ Create/Approve Match: " + ex.Message.ToString());
            }

          //  eiqServiceClient.

        }
        /// <summary>
        /// Mark Lane
        /// validate the match
        ///      a. "Please select at least one cash item to match.";
        ///               Loop through the items in matching grid
        /// If no cash2post item  then exit
        /// b."The cash items are [Different $ Amount] more than the [remit/cash] items. Do you still want to match?"
        ///      c. If the practice for a remit is different from the one for a cash, and docdet for remit is not "ER",
        /// then  "You are not allowed to match a paper remit to a cash with different practice." Exit matching
        /// d. check if doctype = RM docdet = ER to allow comingled match
        /// then check the Cash CK to make sure its not already matched
        /// then check the Remits to make sure they are not already matched
        /// all for when their practice does not match the checks practice
        /// 
        
        /// </summary>
        /// <returns></returns>
        public bool ValidateMatch()
        {
            if (CheckCashEquality() == false)
            {
                return false;
            }
            if (ComingledAllowed() == false)
            {
                return false;
            }
            if (CheckAllowedComingledDetails() == false)
            {
                return false;
            }
            return true;
        }
        private bool ComingledAllowed()
        {
            var ValidComingle = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "RM" && data.Docdetail != "ER" select (string)data.Practice);
            string ChecksPractice = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "CK" || data.Doctype == "CE" select (string)data.Practice).FirstOrDefault();
              
            foreach (string practice in ValidComingle)
            {
                if (practice != ChecksPractice)
                {
                    MessageBox.Show("You are not allowed to match a paper remit to a cash with different practice.", "WARNING");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// d. below is what Liang gave to me to put in for the logic.
        /// </summary>
        /// 
        /*      d. If the practice for a remit is different from the one for a cash, and docdet for remit is "ER",
                      then 
                               Prompt the user with "The remit practice is different from cash practice. Do you still want to match them?"
                     If "yes" then 
                          Loop throgh all the cash items
                                    If ComingleStat.Equals("Y") Or ComingleStat.Equals("W") Then
                                            MessageBox.Show("The check has a Comingle Status " + ComingleStat + ". Can't create this match. ")
                                            Exit matching
                                    End If
                                     'if check is posted
                       string result = eagleIqGateway.ProcessR2P(MachineName,
                            new UpdR2PInputParams()
                            {
                                UserId = UserId + ",checkposted"
                                 ,
                                InKey = cash2post.key
                                 ,
                                InDocDet = ""
                                 ,
                                InPrCheckDt = DateTime.Now
                                 ,
                                InCheckNum = ""
                                 ,
                                InExtPayCd = ""
                                 ,
                                InPractice = ""
                                 ,
                                InDivision = ""
                                 ,
                                InProvId = ""
                            });                           
                                      If result.StartWith("False") Then
                                              Show "The check " + CheckNumber) + " has been posted. Can't create this match. "
                                              Exit matching
                                      End If
                           End Loop
                           Loop through all the remit items with practice different from cash2post.practice
                     string result1 = eagleIqGateway.ProcessR2P(MachineName,
                            new UpdR2PInputParams()
                            {
                                UserId = UserId + ", changePracOnly "
                                 ,
                                InKey = remit2post.key
                                 ,
                                InDocDet = ""
                                 ,
                                InPrCheckDt = DateTime.Now
                                 ,
                                InCheckNum = ""
                                 ,
                                InExtPayCd = ""
                                 ,
                                InPractice = cash2post.practice
                                 ,
                                InDivision = ""
                                 ,
                                InProvId = ""
                            });                           
                                        If result1.StartWith("False") Then
                                              Show "Failed to update remit(Docno:" + remit.DocNumber) + "). Can't create this match. "
                                              Exit matching
                                      End If
                              End Loop
                      else  // "No" from prompt
                              Exit matching
             **/
        /// <returns></returns>
        private bool CheckAllowedComingledDetails()
        {
            //it has the ER RM.
            var ValidComingle = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "RM" && data.Docdetail == "ER" select data);
           if(ValidComingle != null)
            {
                foreach (var item in ValidComingle)
                {
                    
                    var CheckAllComingledStatus = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "CK" select data);

                    foreach (var cominglestatus in CheckAllComingledStatus)
                    {
                        if (item.Practice != cominglestatus.Practice)
                        {
                            if (cominglestatus.ComingledStatus == "W" || cominglestatus.ComingledStatus == "Y")
                            {
                                MessageBox.Show("You can not match where comingled status = " + cominglestatus + " .", "WARNING");
                                return false;
                            }
                            if(CheckRemitsRemit2Post(item.Key.ToString(), item.Practice) == false)
                            {
                                return false;
                            }
                            if(CheckCashRemit2Post(cominglestatus.Key.ToString()) == false)
                            {
                                return false;
                            }
                            
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheckCashRemit2Post(string key)
        {
            using (var eiqProxy = new EagleIqGatewayClient(Constants.EagleIqGatewayServiceURL, Constants.EagleIqGatewayServiceENDPOINT_NAME))
            {
                // if (eiqProxy.IsAlive())
                // {
                Rti.InternalInterfaces.DataContracts.RpcInMessage RpcInMessage = new Rti.InternalInterfaces.DataContracts.RpcInMessage();

                string results = eiqProxy.ProcessR2P(Environment.MachineName, new Rti.InternalInterfaces.DataContracts.UpdR2PInputParams()
                {
                    UserId = Environment.UserName + ",checkposted",
                    InKey = key,
                    InDocDet = "",
                    InPrCheckDt = DateTime.Now,
                    InCheckNum = "",
                    InExtPayCd = "",
                    InPractice = "",
                    InDivision = "",
                    InProvId = ""
                });

                if (results != null)
                {
                    if (results.StartsWith("False"))
                    {
                       MessageBox.Show( "The check has been posted. Can't create this match. ", "WARNING");
                        return false;
                    }
                }
               
            }
                return true;
        }
        /// <summary>
        /// Author Mark Lane
        /// Loop through all the remit items with practice different from cash2post.practice
        /// </summary>
        /// <param name="key"></param>
        /// <param name="practice"></param>
        /// <returns></returns>
        private bool CheckRemitsRemit2Post(string key, string practice)
        {
            using (var eiqProxy = new EagleIqGatewayClient(Constants.EagleIqGatewayServiceURL, Constants.EagleIqGatewayServiceENDPOINT_NAME))
            {
                // if (eiqProxy.IsAlive())
                // {
                Rti.InternalInterfaces.DataContracts.RpcInMessage RpcInMessage = new Rti.InternalInterfaces.DataContracts.RpcInMessage();

                string results = eiqProxy.ProcessR2P(Environment.MachineName, new Rti.InternalInterfaces.DataContracts.UpdR2PInputParams()
                {
                    UserId = Environment.UserName + ", changePracOnly ",
                    InKey = key,
                    InDocDet = "",
                    InPrCheckDt = DateTime.Now,
                    InCheckNum = "",
                    InExtPayCd = "",
                    InPractice = practice,
                    InDivision = "",
                    InProvId = ""
                });

                if (results != null)
                {
                    if (results.StartsWith("False"))
                    {
                        MessageBox.Show("Failed to update remit(Docno). Can't create this match. ", "WARNING");
                        return false;
                    }
                }

            }
            return true;
        }
        /// <summary>
        /// check the cash amounts
        /// </summary>
        /// <returns></returns>
        private bool CheckCashEquality()
        {
            //List<int> nums = new List<int>{1,2,3,4,5};
            /*
            nums.Aggregate(0, (x,y) => x + y); // sums up the numbers, starting with 0 => 15
            nums.Aggregate(0, (x,y) => x * y); // multiplies the numbers, starting with 0 => 0, because anything multiplied by 0 is 0
            nums.Aggregate(1, (x,y) => x * y); // multiplies the numbers, starting with 1 => 120
             * */
            int TallyCashAmounts = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype == "CK" || data.Doctype == "CE" select (int)data.CheckAmount).Aggregate(0, (x, y) => x + y);
            int TallyRemitAmounts = (from data in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where data.Doctype != "CK" && data.Doctype != "CE" select (int)data.CheckAmount).Aggregate(0, (x, y) => x + y);

            int Total = TallyCashAmounts - TallyRemitAmounts;
            if(Total != 0)
            {
               if( MessageBox.Show("The Check and Remit Amounts do not match by $" + Total + " Proceed with Match?", "WARNING", MessageBoxButton.YesNo) == MessageBoxResult.No)
               {
                   return false;
               }
            }

            return true;
        }
        //remove the MatchCheck model item from collection
        private void OnRemoveItem()
        {
            try
            {


                if (SelectedRow >= 0)
                {

                    Model.MatchingChecksCollection.MatchingChecksObservableCollection.RemoveAt(SelectedRow);
                    Model.MatchingChecksCollection.CartCount = Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count();

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Author Mark Lane
        /// Search Control should contain its own viewer.
        /// This viewer needs to export from the searches viewer.
        /// Or should it not have a viewer?
        /// could have search raise data to MainWindow to handle all viewer
        /// functions?
        /// all the big work here.
        /// </summary>
        private void OnView()
        {
            try
            {
                if (SelectedRow > -1)
                {
                    string docno = Model.MatchingChecksCollection.MatchingChecksObservableCollection[SelectedRow].Docno;
                    View.DocumentViewer doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                    doc.Visibility = System.Windows.Visibility.Visible;
                    doc.Show();

                    doc.GetParentDocnoData(docno);
                    doc.ShowDocno();
                }


            }
            catch (Exception ex)
            {
                //handle
            }
        }
    }
}
