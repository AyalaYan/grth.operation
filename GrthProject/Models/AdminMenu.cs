using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    /// <summary>
    /// Admin Menu at the Side Menu
    /// </summary>
    public class AdminMenu
    {
        /// <summary>
        /// initial instance of List and Functions of Menus
        /// </summary>
        public static AdminMenuData AdminMenuData = new AdminMenuData();

        private string _controller = "Admin";
        public int ID { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }
        public string Action { get; set; }
        public int ParentID { get; set; }
        public bool isSingel { get; set; } = false;
    }

    /// <summary>
    ///  List and Functions of Menus
    /// </summary>
    public class AdminMenuData
    {
        #region Properties
        private IEnumerable<AdminMenu> _listAdminMenu;
        //initial List of Menu
        public List<AdminMenu> ListAdminMenu
        {
            get
            {
                if (_listAdminMenu == null)
                    _listAdminMenu = getList();
                return _listAdminMenu.ToList();
            }
        }

        //Default Menu to show at home page
        public int DefaultMenuID;
        //Default Parent Menu to show at home page
        public int DefaultParentID;
        //Curren Unique Id
        private int ID = 0;
        //Curret Parent Id
        private int ParentId;
        #endregion

        #region Private Methods
        /// <summary>
        /// Increase Id Seed 1
        /// </summary>
        /// <returns>Curren Unique id</returns>
        private int IncreaseId(bool IsDefaultMenu = false)
        {
            int CurrentID = ++ID;
            if (IsDefaultMenu)
                DefaultMenuID = CurrentID;
            return CurrentID;
        }

        /// <summary>
        /// Increase Id Seed 1 and Parent
        /// </summary>
        /// <returns></returns>
        private int IncreaseParent(bool IsDefaultMenu = false)
        {
            ParentId = IncreaseId();
            if (IsDefaultMenu)
                DefaultParentID = ParentId;
            return ParentId;
        }
        private List<AdminMenu> getList()
        {
            return new List<AdminMenu>
            {
                    new AdminMenu(){ ID = IncreaseId(true), Order=1,  Name="Employees" ,Action="Employee",ParentID =0,isSingel=true},
                    new AdminMenu(){ ID = IncreaseId(), Order=2,  Name="Customers",Action="Customer",ParentID =0,isSingel=true},
                    new AdminMenu(){ ID = IncreaseId(), Order=2,  Name="Projects",Action="Project",ParentID =0,isSingel=true},
                    new AdminMenu(){ ID = IncreaseParent(), Order=2,Name="Admin",ParentID=0},
                    new AdminMenu(){ ID = IncreaseId(), Order=1,  Name="Job Names",Action="Job",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=2,  Name="Project Type Names",Action="ProjectType",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=3,  Name="Technology Names",Action="Technology",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=4,  Name="Locations",Action="Places",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=5,  Name="Company",Action="Company",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=6,  Name="Users",Action="User",ParentID =ParentId},
                    new AdminMenu(){ ID = IncreaseId(), Order=6,  Name="Roles",Action="Role",ParentID =ParentId},

            };
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// return One Menu By predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public AdminMenu Find(Func<AdminMenu, bool> predicate)
        {
            return ListAdminMenu.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get List of All Childs of Parent ID
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns>List Of childs</returns>
        public List<AdminMenu> getAllChilds(int ParentID)
        {
            return ListAdminMenu.Where(p => p.ParentID == ParentID).ToList();
        }

        /// <summary>
        /// get List of Level to the root of Menu tree
        /// </summary>
        /// <param name="ID">id of the bottom Level</param>
        /// <returns></returns>
        public List<AdminMenu> GetLevel(int ID)
        {
            List<AdminMenu> Levels = new List<AdminMenu>();
            AdminMenu AdminMenuLevel = ListAdminMenu.FirstOrDefault(p => p.ID == ID);
            if (AdminMenuLevel != null)
            {
                if (AdminMenuLevel.ParentID != 0)
                    Levels.AddRange(GetLevel(AdminMenuLevel.ParentID));
                Levels.Add(AdminMenuLevel);
            }
            return Levels;
        }

        /// <summary>
        /// get List of Level to the root of Menu tree
        /// </summary>
        /// <param name="actionName">actionName of the bottom Level</param>
        /// <param name="controllerName">controllerName of the bottom Level</param>
        /// <returns></returns>
        public List<AdminMenu> GetLevel(string actionName, string controllerName)
        {
            List<AdminMenu> Levels = new List<AdminMenu>();
            AdminMenu AdminMenuLevel = ListAdminMenu.FirstOrDefault(p => p.Action == actionName && p.Controller == controllerName);
            if (AdminMenuLevel != null && AdminMenuLevel.ParentID != 0)
                Levels.AddRange(GetLevel(AdminMenuLevel.ParentID));
            Levels.Add(AdminMenuLevel);
            return Levels;
        }
        #endregion

    }

}