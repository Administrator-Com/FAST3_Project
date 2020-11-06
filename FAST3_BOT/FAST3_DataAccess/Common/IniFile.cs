using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FAST3_DataAccess
{
    public class IniFile
    {
        ///  <summary>
        ///  ini文件名称(带路径)
        ///  </summary>
        public string filePath;

        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        ///  <summary>
        ///  类的构造函数
        ///  </summary>
        ///  <param  name="iNIPath">INI文件名</param>  
        public IniFile(string iNIPath)
        {
            filePath = iNIPath;
        }

        ///  <summary>
        ///  写INI文件
        ///  </summary>
        ///  <param  name="Section">Section</param>
        ///  <param  name="Key">Key</param>
        ///  <param  name="value">value</param>
        public void WriteInivalue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }

        ///  <summary>
        ///  读取INI文件指定部分
        ///  </summary>
        ///  <param  name="Section">Section</param>
        ///  <param  name="Key">Key</param>
        ///  <returns>String</returns>  
        public string ReadInivalue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileString(section, key, key, temp, 1024, filePath);
            return temp.ToString();
        }
    }
}
