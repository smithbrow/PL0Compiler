using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PL0CompilerLibrary
{
    public class PL0Compiler
    {

        //源程序文件
        public FileStream source {get;private set;}

        //词法分析器
        public SymbolAnalysis SA {get;private set;}

        //语法，语义分析器
        public GrammarAndSemanticAna GSA {get;private set;}

        //错误处理
        public ErrorHandle EH {get;private set;}

        //符号表管理
        public SymbolTable ST {get;private set;}

        //目标代码生成
        public PCodeGenetate PCG {get;private set;}

        /// <summary>
        /// 构造函数
        /// </summary>
        public PL0Compiler(string path)
        {
            try
            {
                source = new FileStream(path, FileMode.Open);
                SA = new SymbolAnalysis(this);
                ST = new SymbolTable(this);
                EH = new ErrorHandle(this);
                GSA = new GrammarAndSemanticAna(this);
                PCG = new PCodeGenetate();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                source = null;
                SA = null;
                ST = null;
                EH = null;
                GSA = null;
                PCG = null;
            }
            finally
            {
            }
           
        }

        /// <summary>
        /// 编译
        /// </summary>
        public int compile()
        {
            if (source != null && SA != null && ST != null && EH != null && GSA != null && PCG != null)
            {
                GSA.analysis();
                return 1;
            }
            return 0;
        }

        public string displayErr()
        {
            int num = this.EH.errItem.Count;
            string errors = "";
            errors += ("共有错误"+num.ToString()+"个");
            if (num == 0)
                errors += "。\r\n";
            else if (num > 0)
            {
                errors += "：\r\n\r\n";
                for (int i = 0; i < num; i++)
                {
                    int line = this.EH.errItem[i].line;
                    int errId = this.EH.errItem[i].errNum;
                    string errmsg = this.EH.errors[errId];
                    errors += ((i+1).ToString() + ". 错误在第" + line.ToString() + "行： "
                        + "\t" + errmsg+"\r\n\r\n");
                }
            }
            return errors;
        }

        public string listCode()
        {
            int codeNum = this.PCG.code.Count;
            string codes = "";
            if (codeNum > 0)
            {
                instruction ins;
                for (int i = 0; i < codeNum; i++)
                {
                    ins = this.PCG.code[i];
                    codes += ("    " + ins.oprc.ToString() + "     \t" + ins.fa.ToString() + "\t" + ins.la.ToString() + "\r\n\r\n");
                }
            }
            return codes;
        }

        public void Interpret()
        {
            this.PCG.interpret();
        }
    }
}
