using System;
using System.Data.Entity.Migrations;
using isriding.Entities.Authen;
using isriding.Helper;

namespace isriding.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<isriding.EntityFramework.isridingDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "isriding";
        }

        protected override void Seed(isriding.EntityFramework.isridingDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...

            //系统管理员
            context.BackUsers.AddOrUpdate(
                u => u.Id,
                new BackUser
                {
                    Id = 1,
                    LoginName = "admin",
                    LoginPwd = DESProvider.EncryptString("123456"),
                    FullName = "admin",
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();

            //角色
            context.Roles.AddOrUpdate(
                r => r.Id,
                new Role
                {
                    Id = 1,
                    Name = "系统管理员",
                    Description = "开发人员、系统配置人员使用",
                    OrderSort = 1,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                },
                new Role
                {
                    Id = 2,
                    Name = "网站管理员",
                    Description = "网站内容管理人员",
                    OrderSort = 2,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();

            //用户-角色
            context.UserRoles.AddOrUpdate(
                ur => ur.Id,
                new UserRole { UserId = 1, RoleId = 1 });
            context.SaveChanges();
            //模块
            context.Modules.AddOrUpdate(m => m.Id,
                new Module
                {
                    Id = 1,
                    ParentId = null,
                    Name = "权限管理",
                    LinkUrl = null,
                    Area = null,
                    Controller = null,
                    Action = null,
                    Icon = "icon-settings",
                    Code = "20",
                    OrderSort = 1,
                    Description = null,
                    IsMenu = true,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                },
                new Module
                {
                    Id = 2,
                    ParentId = 1,
                    Name = "角色管理",
                    LinkUrl = "Role/Index",
                    Area = "",
                    Controller = "Role",
                    Action = "Index",
                    Icon = "",
                    Code = "2001",
                    OrderSort = 2,
                    Description = null,
                    IsMenu = true,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                },
                new Module
                {
                    Id = 3,
                    ParentId = 1,
                    Name = "用户管理",
                    LinkUrl = "BackUser/Index",
                    Area = "",
                    Controller = "User",
                    Action = "Index",
                    Icon = "",
                    Code = "2002",
                    OrderSort = 3,
                    Description = null,
                    IsMenu = true,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                },
                new Module
                {
                    Id = 4,
                    ParentId = 1,
                    Name = "模块管理",
                    LinkUrl = "Module/Index",
                    Area = "",
                    Controller = "Module",
                    Action = "Index",
                    Icon = "",
                    Code = "2003",
                    OrderSort = 4,
                    Description = null,
                    IsMenu = true,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                },
                new Module
                {
                    Id = 5,
                    ParentId = 1,
                    Name = "权限管理",
                    LinkUrl = "Permission/Index",
                    Area = "",
                    Controller = "Permission",
                    Action = "Index",
                    Icon = "",
                    Code = "2004",
                    OrderSort = 5,
                    Description = null,
                    IsMenu = true,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "admin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();
            //权限
            context.Permissions.AddOrUpdate(p => p.Id,
                new Permission
                {
                    Id = 1,
                    Code = "Index",
                    Name = "浏览",
                    OrderSort = 1,
                    Icon = null,
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 2,
                    Code = "Create",
                    Name = "新增",
                    OrderSort = 2,
                    Icon = "icon-plus",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 3,
                    Code = "Edit",
                    Name = "编辑",
                    OrderSort = 3,
                    Icon = "icon-pencil",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 4,
                    Code = "Delete",
                    Name = "删除",
                    OrderSort = 4,
                    Icon = "icon-remove",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 5,
                    Code = "SetButton",
                    Name = "设置按钮",
                    OrderSort = 5,
                    Icon = "icon-legal",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 6,
                    Code = "SetPermission",
                    Name = "设置权限",
                    OrderSort = 6,
                    Icon = "icon-sitemap",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 7,
                    Code = "ChangePwd",
                    Name = "修改密码",
                    OrderSort = 7,
                    Icon = "icon-key",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new Permission
                {
                    Id = 8,
                    Code = "DeleteAll",
                    Name = "删除全部",
                    OrderSort = 8,
                    Icon = "icon-trash",
                    Description = null,
                    Enabled = true,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();
            //模块-权限
            context.ModulePermissions.AddOrUpdate(mp => mp.Id,
                new ModulePermission
                {
                    Id = 1,
                    ModuleId = 2,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 2,
                    ModuleId = 2,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 3,
                    ModuleId = 2,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 4,
                    ModuleId = 2,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 5,
                    ModuleId = 2,
                    PermissionId = 6,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new ModulePermission
                {
                    Id = 6,
                    ModuleId = 3,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 7,
                    ModuleId = 3,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 8,
                    ModuleId = 3,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 9,
                    ModuleId = 3,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 10,
                    ModuleId = 3,
                    PermissionId = 7,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new ModulePermission
                {
                    Id = 11,
                    ModuleId = 4,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 12,
                    ModuleId = 4,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 13,
                    ModuleId = 4,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 14,
                    ModuleId = 4,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 15,
                    ModuleId = 4,
                    PermissionId = 5,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new ModulePermission
                {
                    Id = 16,
                    ModuleId = 5,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 17,
                    ModuleId = 5,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 18,
                    ModuleId = 5,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new ModulePermission
                {
                    Id = 19,
                    ModuleId = 5,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();
            //角色-模块-权限
            context.RoleModulePermissions.AddOrUpdate(
                rmp => rmp.Id,
                new RoleModulePermission
                {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = null,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new RoleModulePermission
                {
                    Id = 2,
                    RoleId = 1,
                    ModuleId = 2,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 3,
                    RoleId = 1,
                    ModuleId = 2,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 4,
                    RoleId = 1,
                    ModuleId = 2,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 5,
                    RoleId = 1,
                    ModuleId = 2,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 6,
                    RoleId = 1,
                    ModuleId = 2,
                    PermissionId = 6,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new RoleModulePermission
                {
                    Id = 7,
                    RoleId = 1,
                    ModuleId = 3,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 8,
                    RoleId = 1,
                    ModuleId = 3,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 9,
                    RoleId = 1,
                    ModuleId = 3,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 10,
                    RoleId = 1,
                    ModuleId = 3,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 11,
                    RoleId = 1,
                    ModuleId = 3,
                    PermissionId = 7,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new RoleModulePermission
                {
                    Id = 12,
                    RoleId = 1,
                    ModuleId = 4,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 13,
                    RoleId = 1,
                    ModuleId = 4,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 14,
                    RoleId = 1,
                    ModuleId = 4,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 15,
                    RoleId = 1,
                    ModuleId = 4,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 16,
                    RoleId = 1,
                    ModuleId = 4,
                    PermissionId = 5,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },

                new RoleModulePermission
                {
                    Id = 17,
                    RoleId = 1,
                    ModuleId = 5,
                    PermissionId = 1,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 18,
                    RoleId = 1,
                    ModuleId = 5,
                    PermissionId = 2,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 19,
                    RoleId = 1,
                    ModuleId = 5,
                    PermissionId = 3,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                },
                new RoleModulePermission
                {
                    Id = 20,
                    RoleId = 1,
                    ModuleId = 5,
                    PermissionId = 4,
                    CreateBy = "admin",
                    CreateId = 1,
                    CreateTime = DateTime.Now,
                    ModifyId = 1,
                    ModifyBy = "amdin",
                    ModifyTime = DateTime.Now
                });
            context.SaveChanges();
        }
    }
}
