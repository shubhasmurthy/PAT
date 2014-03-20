using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Platform_Allocation_Tool.Data_Layer;

namespace Platform_Allocation_Tool.Business_Layer
{
    [Serializable]
    public class TechnicalDocumentation
    {
        #region
        private String tDocName;
        private String tDocAddress;
        #endregion

        #region Properties
        public String TDocName
        {
            get { return tDocName; }
            set { tDocName = value; }
        }
        public String TDocAddress
        {
            get { return tDocAddress; }
            set { tDocAddress = value; }
        }
        #endregion

        #region Constructors

        public TechnicalDocumentation()
        {

        }

        public TechnicalDocumentation(String name, String link)
        {
            this.tDocName = name;
            this.tDocAddress = link;
        }

        #endregion
    }

    [Serializable]
    public class TeamBoard
    {
        #region Attributes
        private String sku;
        private String boardType;
        private String teamName;
        private Int16 numberOfBoards;

        #endregion

        #region Properties
        public String SKU
        {
            get { return sku; }
            set { sku = value; }
        }

        public String BoardType
        {
            get { return boardType; }
            set { boardType = value; }
        }

        public String TeamName
        {
            get { return teamName; }
            set { teamName = value; }
        }

        public Int16 NumberOfBoards
        {
            get { return numberOfBoards; }
            set { numberOfBoards = value; }
        }
        #endregion

        #region Constructors
          
        public TeamBoard()
        {
            this.sku = String.Empty;
            this.boardType = String.Empty;
            this.teamName = String.Empty;
            this.numberOfBoards = 0;
        }
        public TeamBoard(String sku, String bType, String tName, Int16 noOfBoards = 0)
        {
            this.sku = sku;
            this.boardType = bType;
            this.teamName = tName;
            this.numberOfBoards = noOfBoards;
        }
        
        #endregion
        
    }
    public class Demand
    {
        #region Attributes
        private Int16 demandId;
        private String demandName;
        private String status;
        private String programName;
        private String platformName;
        private DateTime createdDate;
        private DateTime closeDate;
        private List<TeamBoard> boards;
        private List<TechnicalDocumentation> technicalDocumentation;
        //private 

        #endregion

        #region Properties
        public Int16 DemandId
        {
            get { return demandId; }
            set { demandId = value; }
        }

        public String DemandName
        {
            get { return demandName; }
            set { demandName = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }

        public String ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }

        public String PlatformName
        {
            get { return platformName; }
            set { platformName = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public DateTime CloseDate
        {
            get { return closeDate; }
            set { closeDate = value; }
        }

        public List<TeamBoard> Boards
        {
            get { return boards; }
            set { boards = value; }
        }


        public List<TechnicalDocumentation> TechnicalDocumentation
        {
            get { return technicalDocumentation; }
            set { technicalDocumentation = value; }
        }
        #endregion

        #region Constructors
        public Demand()
        {
        }

        public Demand(Int16 demandId)
        {
            this.DemandId = demandId;
            ConnectionData.GetDemandByID(this);
            
        }
        public Demand(String name, String prgName, String pfName, DateTime date, List<TeamBoard> bList, List<TechnicalDocumentation> tDoc)
        {
            this.demandName = name;
            this.status = "Open";
            this.programName = prgName;
            this.platformName = pfName;
            this.createdDate = DateTime.Now;
            this.closeDate = date;
            this.boards = bList;
            this.technicalDocumentation = tDoc;
        }
        #endregion

        #region ADO Methods
            
        public static DataTable ListAll(User u)
        {
            return ConnectionData.ListAllDemands(u);
        }

        public static DataTable ListOpenDemands(User u)
        {
            return ConnectionData.ListAllDemands(u);
        }

        public static DataTable ListApprovedDemands(User u)
        {
            return ConnectionData.ListApprovedDemands(u);
        }

        #endregion

    }
}