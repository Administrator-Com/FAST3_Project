namespace FAST3_BaseLib
{
    /// <summary>
    /// 用于各个子端口的规范化接口
    /// </summary>
    public interface IThreadMission
    {
        /// <summary> 
        /// 用于线程监听器的标准方法
        /// </summary>
        /// <param name="pro">名称</param>
        void StartTask(string pro);

        /// <summary>
        /// 获取相对应的参数列表
        /// </summary>
        /// <returns>参数列表</returns>
        object[] GetParam();
    }
}
