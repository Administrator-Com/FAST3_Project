﻿namespace FAST3_Repository
{
    /// <summary>
    /// 通用的Repository工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryFactory<T> where T : new()
    {
        /// <summary>
        /// 定义通用的Repository
        /// </summary>
        /// <returns></returns>
        public IRepository<T> Repository()
        {
            return new Repository<T>();
        }
    }
}
