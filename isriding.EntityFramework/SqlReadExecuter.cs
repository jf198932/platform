﻿using System.Linq;
using Abp.Dependency;
using Abp.EntityFramework;
using isriding.EntityFramework;

namespace isriding
{
    public class SqlReadExecuter : ISqlReadExecuter, ITransientDependency
    {
        private readonly IDbContextProvider<ReadonlyisridingDbContext> _dbContextProvider;
        public SqlReadExecuter(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) { _dbContextProvider = dbContextProvider; }

        /////<summary>        
        ///// 执行给定的命令        
        ///// </summary>        
        ///// <param name="sql">命令字符串</param>        
        ///// <param name="parameters">要应用于命令字符串的参数</param>        
        ///// <returns>执行命令后由数据库返回的结果</returns>        
        //public int Execute(string sql, params object[] parameters)
        //{
        //    return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql, parameters);
        //}

        /////<summary>        
        ///// 执行给定的命令        
        ///// </summary>        
        ///// <param name="sql">命令字符串</param>        
        ///// <param name="parameters">要应用于命令字符串的参数</param>        
        ///// <returns>执行命令后由数据库返回的结果</returns>        
        //public Task<int> ExecuteAsync(string sql, params object[] parameters)
        //{
        //    return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommandAsync(sql, parameters);
        //}

        /// <summary>        
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。        
        /// </summary>        
        /// <typeparam name="T">查询所返回对象的类型</typeparam>        
        /// <param name="sql">SQL 查询字符串</param>        
        /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>        
        /// <returns></returns>        
        public IQueryable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.SqlQuery<T>(sql, parameters).AsQueryable<T>();
        }
    }
}