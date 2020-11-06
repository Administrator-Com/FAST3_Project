namespace FAST3_BaseLib
{
    /// <summary>
    /// Entity对象处理，检验
    /// </summary>
    public static class OkaEntityClass
    {
        public static string CheckIsNull(object value,string name)
        {
            string result = "";
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                result = "参数异常：[" + name + "]为必要字段,不能传入Null或者空字符串!";
            }
            return result;
        }
    }
}
