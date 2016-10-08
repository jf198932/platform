using System.Collections.Generic;

namespace isriding.Web.Models.Authen
{
    /// <summary>
	/// 选中菜单
	/// </summary>
	public class RoleSelectedModuleModel
    {
        public RoleSelectedModuleModel()
        {
            this.ModuleDataList = new List<ModuleModel1>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string OldModulePermission { get; set; }

        public List<ModuleModel1> ModuleDataList { get; set; }
    }


    public class ModuleModel1
    {
        public int ModuleId { get; set; }
        public int? ParentId { get; set; }
        public string ModuleName { get; set; }
        public string Code { get; set; }
        public bool Selected { get; set; }
    }
}