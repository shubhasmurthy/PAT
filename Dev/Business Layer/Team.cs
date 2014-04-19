using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform_Allocation_Tool.Data_Layer;

namespace Platform_Allocation_Tool.Business_Layer
{
    public class Team
    {
        #region Attributes
        private String name;
        private String mgrId;
        private String repId;
        private String mgrEmailId;
        private String repEmailId;

        #endregion

        #region Properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String MgrId
        {
            get { return mgrId; }
            set { mgrId = value; }
        }

        public String RepId
        {
            get { return repId; }
            set { repId = value; }
        }

        public String MgrEmailId
        {
            get { return mgrEmailId; }
            set { mgrEmailId = value; }
        }

        public String RepEmailId
        {
            get { return repEmailId; }
            set { repEmailId = value; }
        }

        #endregion

        #region Constructors
        public Team(String teamName)
        {

            this.name = teamName;
            ConnectionData.GetSelectedTeam(this);
            
        }

        #endregion
    }
}