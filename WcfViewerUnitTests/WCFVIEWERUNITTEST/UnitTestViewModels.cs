using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfDocViewer;
using WpfDocViewer.ViewModel;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;



namespace WcfViewerUnitTest
{
    [TestClass]
    public class UnitTestViewModels
    {
        [TestMethod]
        public void TestChannelFactory()
        {
        }
        [TestMethod]
        public void TestLogin()
        {
            LoginViewModel loginIDM = new LoginViewModel();
            loginIDM.DomainName = "EMSC";
            loginIDM.Username = "marklane";

            string password = "password";
            loginIDM.Password = password;
            Assert.IsFalse(loginIDM.LoginApproved());
        }
        [TestMethod] 
        public void TestSubmitButton()
        {


        }
        [TestMethod]
        public void TestSearchPostDoc()
        {
            string searchBy = "Docno";
            string searchValue = "111797005840086";

            SearchAccounts searchAccount = new SearchAccounts();
            searchAccount.SearchType = searchBy;
            searchAccount.SearchValue = searchValue;
            
           // App.Current.FindResource("SearchType").ToString(), App.Current.FindResource("SearchValue").ToString()

            searchAccount.OnSearch();
           
            Assert.IsTrue(searchAccount.SearchResults != null);
            Assert.IsTrue(searchAccount.IDMResults != "");

        }
        [TestMethod]
        public void TestSearchPostBD()
        {

        }
    }
}
