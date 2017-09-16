using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PL0CompilerLibrary
{
    class symbol_reserved
    {
        public symbol sym;
        public string reserved;

        public symbol_reserved(symbol s, string r)
        {
            this.sym = s;
            this.reserved = r;
        }
    }
    public class SymbolAnalysis
    {
        private int ch;//当前字符
       
        public  string tname;//当前单词
        public  int tnumber;//当前数字
        public int LSymLine;//前一单词所在行号
        public  int lineNum; //行号

        public StreamReader sourceCode;
        private PL0Compiler compiler; //编译器
        private List<symbol_reserved> reservedSym = new List<symbol_reserved>();//保留字表 

        /// <summary>
        /// 构造函数
        /// </summary>
        public SymbolAnalysis(PL0Compiler C)
        {
            ch =(int)' ';
            LSymLine = 1;
            lineNum = 1;
            compiler = C;
            sourceCode = new StreamReader(compiler.source);

            //添加保留词
            reservedSym.Add(new symbol_reserved(symbol.beginSym, "begin"));
            reservedSym.Add(new symbol_reserved(symbol.endSym, "end"));
            reservedSym.Add(new symbol_reserved(symbol.constSym, "const"));
            reservedSym.Add(new symbol_reserved(symbol.varSym, "var"));
            reservedSym.Add(new symbol_reserved(symbol.procedureSym, "procedure"));
            reservedSym.Add(new symbol_reserved(symbol.oddSym, "odd"));
            reservedSym.Add(new symbol_reserved(symbol.ifSym, "if"));
            reservedSym.Add(new symbol_reserved(symbol.thenSym, "then"));
            reservedSym.Add(new symbol_reserved(symbol.elseSym, "else"));
            reservedSym.Add(new symbol_reserved(symbol.callSym, "call"));
            reservedSym.Add(new symbol_reserved(symbol.whileSym, "while"));
            reservedSym.Add(new symbol_reserved(symbol.doSym, "do"));
            reservedSym.Add(new symbol_reserved(symbol.repeatSym, "repeat"));
            reservedSym.Add(new symbol_reserved(symbol.untilSym, "until"));
            reservedSym.Add(new symbol_reserved(symbol.readSym, "read"));
            reservedSym.Add(new symbol_reserved(symbol.writeSym, "write"));
        }

        /// <summary>
        /// 检查是否为保留字
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        private int isReserved(string test)
        {
            for (int i = 0; i < reservedSym.Count; i++)
            {
                symbol_reserved sr = (symbol_reserved)(reservedSym[i]);
                if (sr.reserved == test)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 获取单词
        /// </summary>
        /// <returns></returns>
        public symbol getSymbol()
        {
           
            ErrorHandle error = compiler.EH;

            
            int maxVarLen = (int)constnum.MAX_VAR_LEN;
            symbol sym;
            int k,p,num;

            LSymLine = lineNum;
            while (ch == (int)' ' || ch == (int)'\r' || ch == (int)'\n'|| ch == (int)'\t')
            {
                if (ch == (int)'\n')
                {
                   
                    lineNum++;
                }
               
                ch = sourceCode.Read();
                
            }

            //读取标识符
            if ((ch >= (int)'a' && ch <= (int)'z') || (ch >= (int)'A' && ch <= (int)'Z'))
            {
                k = 0;
                List<char> name = new List<char>();
                do
                {
                    if (k < maxVarLen)
                        name.Add((char)ch);
                    k++;
                    
                    ch = sourceCode.Read();
                    
                } while ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'));

                tname = new string(name.ToArray());
                p = isReserved(tname);
                if (p >= 0)
                {
                    symbol_reserved sr = (symbol_reserved)(reservedSym[p]);
                    sym = sr.sym;
                }
                else
                    sym = symbol.ident;
            }

            //读取数字
            else if ((ch >= (int)'0' && ch <= (int)'9'))
            {
                k = 0;
                num = 0;
                sym = symbol.number;
                do
                {
                    num = num * 10 + ( ch - (int)'0');
                    k++;

                    ch = sourceCode.Read();
                    
                } while ((ch >= (int)'0' && ch <= (int)'9'));
                if (k > (int)constnum.MAX_INT_LEN)
                    error.adderr(30);
                if (num > (int)constnum.MAX_INT)
                {
                    error.adderr(31);
                    num = 0;
                }
                tnumber = num;
            }

            //读取其他
            else if (ch == (int)':')
            {
                ch = sourceCode.Read();
                
                if (ch == (int)'=')
                {
                    sym = symbol.becomes;
                    ch = sourceCode.Read();
                    
                }
                else
                    sym = symbol.nul;
            }
            else if (ch == (int)'>')
            {
                ch =sourceCode.Read();
                
                if (ch == (int)'=')
                {
                    sym = symbol.MoreThanE;
                    ch = sourceCode.Read();
                    
                }
                else
                    sym = symbol.MoreThan;
            }
            else if (ch == (int)'<')
            {
                ch = sourceCode.Read();
                
                if (ch == (int)'=')
                {
                    sym = symbol.LessThanE;
                    ch = sourceCode.Read();
                    
                }
                else
                    sym = symbol.LessThan;
            }

            else
            {
                switch (ch)
                {
                    case (int)'+': sym = symbol.plus;
                        break;
                    case '-': sym = symbol.minus;
                        break;
                    case (int)'*': sym = symbol.times;
                        break;
                    case (int)'/': sym = symbol.division;
                        break;
                    case (int)'(': sym = symbol.LParenthesis;
                        break;
                    case (int)')': sym = symbol.RParenthesis;
                        break;
                    case (int)';': sym = symbol.semicolon;
                        break;
                    case (int)',': sym = symbol.comma;
                        break;
                    case (int)'.': sym = symbol.period;
                        break;
                    case (int)'#': sym = symbol.inequality;
                        break;
                    case (int)'=': sym = symbol.equality;
                        break;
                    case -1: sym = symbol.endfile;
                        break;
                    default: sym = symbol.nul;
                        break;
                }
                ch = sourceCode.Read();
                
            }

            return sym;
        }
    }
}
