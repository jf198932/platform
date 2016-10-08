using System.Collections.Generic;

namespace isriding.Web.Models.Authen
{
    public class RoleSelectedPermissionModel
    {
        public RoleSelectedPermissionModel()
        {
            this.HeaderPermissionList = new List<PermissionModel1>();
            this.ModulePermissionDataList = new List<ModulePermissionModel>();
        }

        public int RoleId { get; set; }
        public string OldModulePermission { get; set; }
        public string NewModulePermission { get; set; }

        public List<PermissionModel1> HeaderPermissionList { get; set; }
        public List<ModulePermissionModel> ModulePermissionDataList { get; set; }
    }

    public class ModulePermissionModel
    {
        public ModulePermissionModel()
        {
            this.PermissionDataList = new List<PermissionModel1>();
        }

        public int ModuleId { get; set; }

        public string LinkUrl { get; set; }

        public int? ParentId { get; set; }

        public string ModuleName { get; set; }

        public string Code { get; set; }

        public bool Selected { get; set; }

        public List<PermissionModel1> PermissionDataList { get; set; }
    }

    public class PermissionModel1
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public int OrderSort { get; set; }

        public bool Selected { get; set; }

        public bool Enabled { get; set; }

    }
}