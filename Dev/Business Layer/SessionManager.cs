using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using System.Collections.Specialized;

namespace Platform_Allocation_Tool.Business_Layer
{
    public sealed class SessionManager : IHttpSessionState
    {
        #region Private Members
        private static SessionManager instance = null;
        private static readonly object threadlock = new object();
        #endregion
        #region Instance Method
        public static SessionManager Instance
        {
            get
            {
                lock (threadlock)
                {
                    if (Object.Equals(instance, null))
                    {
                        instance = new SessionManager();
                    }
                    return instance;
                }
            }
        }
        #endregion
        #region Methods
        public void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }
        public void Add(string name, object value)
        {
            HttpContext.Current.Session[GUID_KEY + name] = value;
        }
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
        public void CopyTo(Array array, int index)
        {
            HttpContext.Current.Session.CopyTo(array, index);
        }
        public IEnumerator GetEnumerator()
        {
            return HttpContext.Current.Session.GetEnumerator();
        }
        public void Remove(string name)
        {
            HttpContext.Current.Session.Remove(GUID_KEY + name);
        }
        public void RemoveAll()
        {
            this.Clear();
        }
        public void RemoveAt(int index)
        {
            HttpContext.Current.Session.RemoveAt(index);
        }
        private string GUID_KEY
        {
            get
            {
                string GUID = HttpContext.Current.Request.QueryString["SESS"];
                if (string.IsNullOrEmpty(GUID))
                {
                    GUID = Guid.NewGuid().ToString().Replace("-", "");
                    UriBuilder baseUri = new UriBuilder(HttpContext.Current.Request.Url);
                    string queryToAppend = string.Format("SESS={0}", GUID);

                    if (baseUri.Query != null && baseUri.Query.Length > 1)
                        baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
                    else
                        baseUri.Query = queryToAppend;

                    HttpContext.Current.Response.Redirect(baseUri.ToString(), true);
                }
                return GUID;
            }
        }
        #endregion
        #region Properties
        public int CodePage
        {
            get
            {
                return HttpContext.Current.Session.CodePage;
            }
            set
            {
                HttpContext.Current.Session.CodePage = value;
            }
        }
        public HttpCookieMode CookieMode
        {
            get
            {
                return HttpContext.Current.Session.CookieMode;
            }
        }
        public int Count
        {
            get
            {
                return HttpContext.Current.Session.Count;
            }
        }
        public bool IsCookieless
        {
            get
            {
                return HttpContext.Current.Session.IsCookieless;
            }
        }
        public bool IsNewSession
        {
            get
            {
                return HttpContext.Current.Session.IsNewSession;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return HttpContext.Current.Session.IsReadOnly;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return HttpContext.Current.Session.IsSynchronized;
            }
        }
        public object this[int index]
        {
            get
            {
                return HttpContext.Current.Session[index];
            }
            set
            {
                HttpContext.Current.Session[index] = value;
            }
        }
        public object this[string name]
        {
            get
            {
                return HttpContext.Current.Session[GUID_KEY + name];
            }
            set
            {
                HttpContext.Current.Session[GUID_KEY + name] = value;
            }
        }
        public NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return HttpContext.Current.Session.Keys;
            }
        }
        public int LCID
        {
            get
            {
                return HttpContext.Current.Session.LCID;
            }
            set
            {
                HttpContext.Current.Session.LCID = value;
            }
        }
        public SessionStateMode Mode
        {
            get
            {
                return HttpContext.Current.Session.Mode;
            }
        }
        public string SessionID
        {
            get
            {
                return HttpContext.Current.Session.SessionID;
            }
        }
        public HttpStaticObjectsCollection StaticObjects
        {
            get
            {
                return HttpContext.Current.Session.StaticObjects;
            }
        }
        public object SyncRoot
        {
            get
            {
                return HttpContext.Current.Session.SyncRoot;
            }
        }
        public int Timeout
        {
            get
            {
                return HttpContext.Current.Session.Timeout;
            }
            set
            {
                HttpContext.Current.Session.Timeout = value;
            }
        }
        #endregion
    }
}