using System;
using System.Collections;
using System.Data;
using System.Management;
using System.ServiceProcess;

namespace Rti.AdministrationServer
{
    //Updated By Shariq
    public class RACServiceController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        System.ServiceProcess.ServiceController svc;

        //Updated By Shariq
        public bool StopService(string workstation, string username, string serviceName)
        {
            Log.Debug(string.Format("Enter StopService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

            bool status;

            try
            {
                svc = new System.ServiceProcess.ServiceController();

                svc.ServiceName = serviceName; //Receive service name from client
                string svcStatus = svc.Status.ToString(); //Retriving the service status

                if (svcStatus == "Running")
                {
                    svc.Stop();
                    svc.WaitForStatus(ServiceControllerStatus.Stopped); //Waiting for service to stop
                    status = true;
                }
                else
                {
                    status = false;
                }

                Log.Debug(string.Format("Exit StopService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

                return status;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                Log.Debug(string.Format("Exit StopService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

                return false;
            }

        }



        //Updated By Shariq
        public bool StartService(string workstation, string username, string serviceName)
        {
            Log.Debug(string.Format("Enter StartService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

            bool status;

            try
            {
                svc = new System.ServiceProcess.ServiceController();

                svc.ServiceName = serviceName; //Receive service name from client
                string svcStatus = svc.Status.ToString(); //Retriving the service status

                if (svcStatus == "Stopped")
                {
                    svc.Start();
                    svc.WaitForStatus(ServiceControllerStatus.Running); //Waiting for service to start
                    status = true;
                }
                else
                {
                    status = false;
                }

                Log.Debug(string.Format("Exit StartService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

                return status;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                Log.Debug(string.Format("Exit StartService() Workstation: [{0}], Username: [{1}], Servicename: [{2}]",
                workstation, username, serviceName));

                return false;
            }

        }


        //Updated By Shariq
        public DataTable ListServicesStatus(string workstation, string username)
        {
            Log.Debug(string.Format("Enter ListServicesStatus() Workstation: [{0}], Username: [{1}]", workstation, username));

            ArrayList serviceList = new ArrayList();
            ArrayList statusList = new ArrayList();

            DataTable dataTable = new DataTable("servicestatus");
            dataTable.Columns.Add("Service Name", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Operation", typeof(string));

            //Retriving the services which are Log on as "emsc\" i.e. (emsc\username)
            System.Management.SelectQuery query = new System.Management.SelectQuery(string.Format(
                "select name, state from Win32_Service where startname LIKE'%emsc%'"));

            using (ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                foreach (ManagementObject service in searcher.Get())
                {
                    serviceList.Add((service["Name"]));
                    statusList.Add(service["State"]);
                }

            }

            //Populating datatable with service
            for (int i = 0; i < serviceList.Count; i++)
            {
                dataTable.Rows.Add(serviceList[i], statusList[i]);
            }


            Log.Debug(string.Format("Exit ListServicesStatus() Rows:[{0}]", dataTable.Rows.Count));

            return dataTable;
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            Log.DebugFormat("Enter ServiceInfo() Workstation: [{0}], Username: [{1}]", workstation, username);
            Log.Debug("Exit ServiceInfo()");

            return Rti.ServiceInfo.GetServiceInfo();
        }

    }
}
