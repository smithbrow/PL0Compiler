using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL0CompilerLibrary
{
    public struct error
    {
        public int errNum;
        public int line;
    }
    public class ErrorHandle
    {
        /// <summary>
        /// 存放错误
        /// </summary>
        public Dictionary<int, string> errors = new Dictionary<int, string>();

        public List<error> errItem = new List<error>();

        private PL0Compiler compiler;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ErrorHandle(PL0Compiler C)
        {
            compiler = C;

            //添加错误
            errors.Add(1, "常数说明中的\"=\"写成\":=\"");
            errors.Add(2, "常数说明中的\"=\"后应是数字");
            errors.Add(3, "常数说明中标识符后应是\"=\"");
            errors.Add(4, "const, var, procedure 后应是标识符");
            errors.Add(5, "漏掉了\',\' 或\';\'");
            errors.Add(6, "过程说明后的符号不正确(应是语句开始符,或过程定义符");
            errors.Add(7, "应是语句开始符");
            errors.Add(8, "程序体内的语句部分的后跟符不正确");
            errors.Add(9, "程序结尾丢了句号\'.\'");
            errors.Add(10, "语句之间漏了\';\'");
            errors.Add(11, "标识符未说明");
            errors.Add(12, "赋值语句中, 赋值号左部标识符属性应是变量");
            errors.Add(13, "赋值语句左部标识符后应是赋值号\':=\'");
            errors.Add(14, "call 后应为标识符");
            errors.Add(15, "call 后标识符属性应为过程");
            errors.Add(16, "条件语句中丢了\'then\'");
            errors.Add(17, "丢了\'end\' 或\';\'");
            errors.Add(18, "while 型循环语句中丢了\'do\'");
            errors.Add(19, "语句后的符号不正确");
            errors.Add(20, "应为关系运算符");
            errors.Add(21, "表达式内标识符属性不能是过程");
            errors.Add(22, "表达式中漏掉右括号\')\'");
            errors.Add(23, "因子后的非法符号");
            errors.Add(24, "表达式的开始符不能是此符号");
            errors.Add(25, "repeat 型循环语句中没有until");
            errors.Add(26, "程序层次结构超过限制");
            errors.Add(30, "数位太长");
            errors.Add(31, "数越界");
            errors.Add(32, "read语句括号中的标识符不是变量");
            errors.Add(33, "语句漏掉左括号\'(\'");
            errors.Add(34, "语句漏掉右括号\')\'");
            errors.Add(35, "read语句缺乏变量");
            errors.Add(36, "程序体之外出现字符");
        }

        public void adderr(int errnum)
        {
            error item = new error();
            item.errNum = errnum;
            item.line = compiler.SA.LSymLine;
            this.errItem.Add(item);
        }
    }
}
