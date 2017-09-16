using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PL0CompilerLibrary
{
    public enum symbol
    {
        //保留字
        beginSym, endSym, constSym,
        varSym, procedureSym, oddSym,
        ifSym, thenSym, elseSym,
        callSym, whileSym, doSym,
        repeatSym, untilSym, readSym,writeSym,
        //分隔符
        comma, semicolon, period,
        //运算符
        plus, minus, times,becomes,
        division, LParenthesis, RParenthesis,
        LessThan, MoreThan, inequality,LessThanE,MoreThanE,equality,

        //变量
        ident,

        //数值
        number,

        //常数
        endfile = -1,nul=0

    }
    public enum constnum
    {
        MAX_VAR_LEN = 10, MAX_LEVEL = 3, MAX_INT = 4096, MAX_INT_LEN = 9,
        MAX_RUN_STACK=500
    }

    public class SymbolSet
    {
        /// <summary>
        /// 保存保留字集合
        /// </summary>
        public List<symbol> SymSet = new List<symbol>(); 
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public SymbolSet()
        {
           
        }

        public bool isInSet(symbol sym)
        {
            bool isIn = SymSet.Contains(sym);
            return isIn;
        }

        public void AddRange(List<symbol> ss)
        {
            for (int i = 0; i < ss.Count; i++)
            {
                if (!SymSet.Contains(ss[i]))
                    SymSet.Add(ss[i]);
            }
        }

        public void Add(symbol s)
        {
            if (!SymSet.Contains(s))
                SymSet.Add(s);
        }

        public void Clear()
        {
            SymSet.Clear();
        }
    }
}
