using System;

namespace isriding.Web.Models.Authen
{
    public class RoleModulePermissionModel : IEquatable<RoleModulePermissionModel>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? PermissionId { get; set; }

        public bool Equals(RoleModulePermissionModel rmpm)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(rmpm, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, rmpm)) return true;

            //Check whether the objects properties are equal. 
            return RoleId.Equals(rmpm.RoleId) && ModuleId.Equals(rmpm.ModuleId) && PermissionId.Equals(rmpm.PermissionId);
        }

        public override int GetHashCode()
        {
            //Get hash code for the RoleId field if it is not null. 
            int hashRoleId = RoleId.GetHashCode();

            //Get hash code for the ModuleId field. 
            int hashModuleId = ModuleId.GetHashCode();

            //Get hash code for the PermissionId field. 
            int hashPermissionId = PermissionId == null ? 0 : ModuleId.GetHashCode();

            //Calculate the hash code for the entire object. 
            return hashRoleId ^ hashModuleId ^ hashPermissionId;
        }
    }
}