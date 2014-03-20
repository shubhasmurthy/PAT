using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform_Allocation_Tool.Data_Layer;
namespace Platform_Allocation_Tool.Business_Layer
{
    public struct CSCRole
    {
        private Byte role;
        private String roleName;


        #region Properties

        public Byte Role
        {
            get { return role; }
            set { role = value; }
        }

        public String RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        #endregion
    }
    public class User
    {
        #region Attributes
        private String id;
        private String Wwid;
        private String name;
        private String firstName;
        private String lastName;
        private String email;
        private bool active;
        private bool isAdmin;
        private String role;

        #endregion


        #region Properties

        public String ID
        {
            get { return id; }
            set { id = value; }
        }

        public String WWID
        {
            get { return Wwid; }
            set { Wwid = value; }
        }

        public String Name
        {
            get { return firstName + " " + lastName; }
        }

        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public Boolean Active
        {
            get { return active; }
            set { active = value; }
        }

        public String Role
        {
            get { return role; }
            set { role = value; }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
            
        }
        #endregion

        #region Constructors

        public User(String uname, String pwd)
        {
            if (ConnectionData.AuthenticateUser(uname, pwd))
            {
                this.id = uname;
                ConnectionData.GetSelectedUser(this);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        #endregion

        public static String GetName(String id)
        {
            return ConnectionData.GetSelectedUserName(id); 
        }
    }
}