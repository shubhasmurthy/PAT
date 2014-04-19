using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Allocation_Tool
{
    interface IPageInterface
    {
        void getOpenDemandRecords();
        void getApprovedDemandRecords();
        void getSavedDemandRecords();
        void getClosedDemandRecords();
        void showNewDemandWindow();
        void getOrderedDemandRecords();
        void AdminUtility();
        void getDeclinedDemandRecords();
        void LogOut();


    }
}
