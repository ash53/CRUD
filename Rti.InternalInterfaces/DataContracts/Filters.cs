using System.Runtime.Serialization;
using System;
using System.Collections;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class Filters
    {
        [DataMember]
        public string statusAll = "Show all items";
        [DataMember]
        public static string statusActive = "Show only active items";
        [DataMember]
        public string statusNotActive = "Show all not active items";
        [DataMember]
        public static string suspendedYes = "Show all cash items";
        [DataMember]
        public string suspendedNo = "Do not show suspended cash";
        [DataMember]
        public string suspendedOnly = "Show only suspended cash";
        [DataMember]
        public static string pendedYes = "Show all items";
        [DataMember]
        public string pendedYesApproved = "Show all items - including approved matches";
        [DataMember]
        public string pendedNo = "Show rejected and unmatched items";
        [DataMember]
        public string pendedOnly = "Show only pended matches";

        private string practiceValue = string.Empty;
	    private string extPayorValue = string.Empty;
	    private ArrayList docDetailValue = new ArrayList();
	    private ArrayList pmtMethodValue = new ArrayList();
	    private double checkAmountMinValue = -1;
	    private double checkAmountMaxValue = -1;
	    private DateTime dateMinValue = DateTime.MinValue;
	    private DateTime dateMaxValue = DateTime.MinValue;
	    private string statusValue = statusActive;
	    private string postingSuspendsValue = suspendedYes;
	    private string matchStatusValue = pendedYes;
	    private string serviceTypeValue = string.Empty;

        [DataMember]
        public string Practice
        {
		    get { return practiceValue; }
		    set { practiceValue = value; }
	    }

        [DataMember]
        public string ExtPayor
        {
		    get { return extPayorValue; }
		    set { extPayorValue = value; }
	    }

        [DataMember]
        public ArrayList DocDetail
        {
		    get { return docDetailValue; }
		    set { docDetailValue = value; }
	    }

        [DataMember]
        public ArrayList PmtMethod
        {
		    get { return pmtMethodValue; }
		    set { pmtMethodValue = value; }
	    }

        [DataMember]
        public double CheckAmountMin
        {
		    get { return checkAmountMinValue; }
		    set { checkAmountMinValue = value; }
	    }

        [DataMember]
        public double CheckAmountMax
        {
		    get { return checkAmountMaxValue; }
		    set { checkAmountMaxValue = value; }
	    }

        [DataMember]
        public DateTime DateMin
        {
		    get { return dateMinValue; }
		    set { dateMinValue = value; }
	    }

        [DataMember]
        public DateTime DateMax
        {
		    get { return dateMaxValue; }
		    set { dateMaxValue = value; }
	    }

        [DataMember]
        public string Status
        {
		    get { return statusValue; }
		    set { statusValue = value; }
	    }

        [DataMember]
        public string PostingSuspends
        {
		    get { return postingSuspendsValue; }
		    set { postingSuspendsValue = value; }
	    }

        [DataMember]
        public string MatchStatus
        {
		    get { return matchStatusValue; }
		    set { matchStatusValue = value; }
	    }

        [DataMember]
        public string ServiceType
        {
		    //LL - for facility Billing project T63894
		    get { return serviceTypeValue; }
		    set { serviceTypeValue = value; }
	    }
    }
}