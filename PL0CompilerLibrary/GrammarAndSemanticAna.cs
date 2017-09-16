using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL0CompilerLibrary
{
    public class GrammarAndSemanticAna
    {
        private PL0Compiler compiler; //编译器
        private int level;//当前层次
        private int[] dx = new int[((int)constnum.MAX_LEVEL) + 1]; //层次的空间
        private symbol sym;//当前单词
         
        private SymbolSet declarebegs = new SymbolSet();//声明部分开始符号集
        private SymbolSet statementbegs = new SymbolSet();//语句开始符号集
        private SymbolSet factorbegs = new SymbolSet();//因子开始符号集

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="C"></param>
        public GrammarAndSemanticAna(PL0Compiler C)
        {
            compiler = C;

            //添加声明部分开始符号
            declarebegs.Add(symbol.constSym);
            declarebegs.Add(symbol.varSym);
            declarebegs.Add(symbol.procedureSym);
            
            //添加语句开始符号
            statementbegs.Add(symbol.beginSym);
            statementbegs.Add(symbol.callSym);
            statementbegs.Add(symbol.whileSym);
            statementbegs.Add(symbol.ifSym);
            statementbegs.Add(symbol.repeatSym);

            //添加因子开始符号
            factorbegs.Add(symbol.ident);
            factorbegs.Add(symbol.number);
            factorbegs.Add(symbol.LParenthesis);

        }

        /// <summary>
        /// 获取单词
        /// </summary>
        public void getsym()
        {
            sym = compiler.SA.getSymbol();
        }

        /// <summary>
        /// 语法、语义分析
        /// </summary>
        public void analysis()
        {
            level = -1;

            getsym();
            SymbolSet blockBESym = new SymbolSet();
            blockBESym.AddRange(declarebegs.SymSet);
            blockBESym.AddRange(statementbegs.SymSet);
            blockBESym.Add(symbol.period);
            program(blockBESym);
        }

        /// <summary>
        /// 错误探测
        /// </summary>
        /// <param name="sBeg">开始符号集</param>
        /// <param name="sEnd">后继符号集</param>
        /// <param name="errorNum">错误编号</param>
        public void ErrorTest(SymbolSet sBeg, SymbolSet sEnd, int errorNum)
        {
            if (!sBeg.isInSet(sym) && sym != symbol.endfile)
            {
            
                compiler.EH.adderr(errorNum);

                sBeg.AddRange(sEnd.SymSet);

                while (!sBeg.isInSet(sym) && sym != symbol.endfile)
                    getsym();
            }
        }

        /// <summary>
        /// 获取当前层次
        /// </summary>
        /// <returns></returns>
        public int getLevel()
        {
            return this.level;
        }

        /// <summary>
        /// 获取变量地址
        /// </summary>
        /// <returns></returns>
        public int getAddress()
        {
            return dx[level];
        }

        /// <summary>
        /// 程序模块
        /// </summary>
        /// <param name="progSym"></param>
        private void program(SymbolSet progSym)
        {
            ErrorHandle err = compiler.EH;
            block(progSym);
            if (sym == symbol.period)
            {
                while (sym != symbol.endfile)
                {
                    getsym();
                    if ((int)sym >=0)
                        err.adderr(36);
                }
            }
            else
                err.adderr(9);
            compiler.source.Close();

        }
        /// <summary>
        /// 分程序模块
        /// </summary>
        /// <param name="bSym"></param>
        private void block(SymbolSet bSym)
        {
            level++;

            int txThis, cxThis;
            SymbolSet s = new SymbolSet();

            ErrorHandle err = compiler.EH;
            SymbolTable table = compiler.ST;
            PCodeGenetate pCode = compiler.PCG;
            
            dx[level] = 3;
            txThis = table.tableIndex();
            table.SymTable[txThis].address = pCode.cx;
            pCode.Gen(oprCode.jmp, 0, 0);
            if (level > (int)constnum.MAX_LEVEL)
                err.adderr(26);

            do
            {
                if (sym == symbol.constSym)
                {
                    getsym();
                    do
                    {
                        constDeclare();
                        while (sym == symbol.comma)
                        {
                            getsym();
                            constDeclare();
                        }

                        if (sym == symbol.semicolon)
                            getsym();
                        else
                            err.adderr(5);

                    }while(sym == symbol.ident);
                }
                if (sym == symbol.varSym)
                {
                    getsym();
                    do
                    {
                        varDeclare();
                        while (sym == symbol.comma)
                        {
                            getsym();
                            varDeclare();
                        }

                        if (sym == symbol.semicolon)
                            getsym();
                        else
                            err.adderr(5);
                    }while(sym == symbol.ident);
                }
                while (sym == symbol.procedureSym)
                {
                    getsym();
                    if (sym == symbol.ident)
                    {
                        table.Enter(ObjectK.procedure);
                        getsym();
                    }
                    else
                        err.adderr(4);
                    if (sym == symbol.semicolon)
                        getsym();
                    else
                        err.adderr(5);
                    s.AddRange(bSym.SymSet);
                    s.Add(symbol.semicolon);
                    block(s);
                    if (sym == symbol.semicolon)
                    {
                        getsym();
                        s.Clear();
                        s.AddRange(statementbegs.SymSet);
                        s.Add(symbol.ident);
                        s.Add(symbol.procedureSym);
                        ErrorTest(s, bSym, 6);
                    }
                    else
                        err.adderr(5);
                }
                s.Clear();
                s.AddRange(statementbegs.SymSet);
                s.Add(symbol.ident);
                ErrorTest(s, declarebegs, 7);

            } while (declarebegs.isInSet(sym));

            int ad = table.SymTable[txThis].address;
            (pCode.code[ad]).la = pCode.cx;
            table.SymTable[txThis].address = pCode.cx;
            table.SymTable[txThis].size = dx[level];

            cxThis = pCode.cx;
            pCode.Gen(oprCode.Int, 0, dx[level]);

            s.Clear();
            s.AddRange(bSym.SymSet);
            s.Add(symbol.semicolon);
            s.Add(symbol.endSym);
            statement(s);
            pCode.Gen(oprCode.opr, 0, 0);
            ErrorTest(s, new SymbolSet(), 8);

            level--;
        }

        /// <summary>
        /// 常数声明模块
        /// </summary>
        private void constDeclare()
        {
            SymbolTable table = compiler.ST;
            ErrorHandle err = compiler.EH;

            if (sym == symbol.ident)
            {
                getsym();
                if (sym == symbol.equality || sym == symbol.becomes)
                {
                    if (sym == symbol.becomes)
                        err.adderr(1);
                    getsym();
                    if (sym == symbol.number)
                    {
                        table.Enter(ObjectK.constant);
                        getsym();
                    }
                    else
                        err.adderr(2);
                }
                else
                    err.adderr(3);
            }
            else
                err.adderr(4);
        }

        /// <summary>
        /// 变量声明模块
        /// </summary>
        private void varDeclare()
        {
            SymbolTable table = compiler.ST;
            ErrorHandle err = compiler.EH;

            if (sym == symbol.ident)
            {
                table.Enter(ObjectK.variable);
                dx[level]++;
                getsym();
            }
            else
                err.adderr(4);
        }

        /// <summary>
        /// 语句模块
        /// </summary>
        /// <param name="stateSym">开始与后继符号集</param>
        private void statement(SymbolSet stateSym)
        {
            SymbolTable table = compiler.ST;
            ErrorHandle err = compiler.EH;
            PCodeGenetate pCode = compiler.PCG;

            SymbolSet s = new SymbolSet(), s1=new SymbolSet();
            int i, cx1, cx2;

            switch (sym)
            {
                case symbol.ident:
                    i = table.getPosition(compiler.SA.tname);
                    if (i == 0)
                        err.adderr(11);
                    else if (table.SymTable[i].kind != ObjectK.variable)
                    {
                        err.adderr(12);
                        i=0;
                    }

                    getsym();
                    if (sym == symbol.becomes)
                    {
                        getsym();
                    }
                    else
                        err.adderr(13);
                    expression(stateSym);
                    if (i > 0)
                    {
                        int fa = level - table.SymTable[i].level;
                        int la = table.SymTable[i].address;
                        pCode.Gen(oprCode.sto, fa, la);
                    }
                    break;
                case symbol.readSym:
                    getsym();
                    if (sym != symbol.LParenthesis)
                        err.adderr(33);
                    else
                    {
                        do
                        {
                            getsym();
                            if (sym == symbol.ident)
                                i = table.getPosition(compiler.SA.tname);
                            else
                                i = 0;
                            if (i == 0 || table.SymTable[i].kind != ObjectK.variable)
                            {
                                err.adderr(32);
                                getsym();
                            }
                            else if (i == 0 && (sym == symbol.RParenthesis || sym == symbol.semicolon))
                                err.adderr(35);
                            else
                            {
                                pCode.Gen(oprCode.opr, 0, 16);
                                int fa, la;
                                fa = level - table.SymTable[i].level;
                                la = table.SymTable[i].address;
                                pCode.Gen(oprCode.sto, fa, la);
                                getsym();
                            }
                            
     
                        }while(sym == symbol.comma);
                    }
                    if (sym != symbol.RParenthesis)
                    {
                        err.adderr(34);
                        while (!stateSym.isInSet(sym))
                            getsym();
                    }
                    else
                        getsym();
                    break;
                case symbol.writeSym:
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s.Add(symbol.RParenthesis);
                    s.Add(symbol.comma);
                    getsym();
                    if (sym == symbol.LParenthesis)
                    {
                        do
                        {
                            getsym();
                            expression(s);
                            pCode.Gen(oprCode.opr, 0, 14);

                        } while (sym == symbol.comma);
                        if (sym != symbol.RParenthesis)
                            err.adderr(34);
                        else
                            getsym();
                    }
                    pCode.Gen(oprCode.opr, 0, 15);
                    break;
                case symbol.callSym:
                    getsym();
                    if (sym != symbol.ident)
                    {
                        err.adderr(14);
                    }
                    else
                    {
                        i = table.getPosition(compiler.SA.tname);
                        if (i == 0)
                        {
                            err.adderr(11);
                        }
                        else if (table.SymTable[i].kind == ObjectK.procedure)
                        {
                            int fa = level - table.SymTable[i].level;
                            int la = table.SymTable[i].address;
                            pCode.Gen(oprCode.cal, fa, la);
                        }
                        else
                            err.adderr(15);
                        getsym();
                    }
                    break;
                case symbol.whileSym:
                    cx1 = pCode.cx;
                    getsym();
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s.Add(symbol.doSym);
                    condition(s);
                    cx2 = pCode.cx;
                    pCode.Gen(oprCode.jpc, 0, 0);
                    if (sym == symbol.doSym)
                        getsym();
                    else
                        err.adderr(18);
                    statement(stateSym);
                    pCode.Gen(oprCode.jmp, 0, cx1);
                    pCode.code[cx2].la = pCode.cx;
                    break;
                case symbol.repeatSym:
                    getsym();
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s.Add(symbol.untilSym);
                    cx1 = pCode.cx;
                    statement(s);
                    s1.Clear();
                    s1.AddRange(statementbegs.SymSet);
                    s1.Add(symbol.semicolon);
                    while (s1.isInSet(sym))
                    {
                        if (sym == symbol.semicolon)
                            getsym();
                        else
                            err.adderr(10);
                        statement(s);
                    }
                    if (sym == symbol.untilSym)
                    {
                        getsym();
                        condition(stateSym);
                        pCode.Gen(oprCode.jpc, 0, cx1);
                    }
                    else
                        err.adderr(25);
                    break;
                case symbol.ifSym:
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s.Add(symbol.thenSym);
                    s.Add(symbol.doSym);
                    getsym();
                    condition(s);
                    if (sym == symbol.thenSym)
                        getsym();
                    else
                        err.adderr(16);
                    cx1 = pCode.cx;
                    pCode.Gen(oprCode.jpc, 0, 0);
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s.Add(symbol.elseSym);
                    statement(s);
                    if (sym == symbol.elseSym)
                    {
                        getsym();
                        cx2 = pCode.cx;
                        pCode.Gen(oprCode.jmp, 0, 0);
                        statement(stateSym);
                        pCode.code[cx1].la = cx2 + 1;
                        pCode.code[cx2].la = pCode.cx;
                    }
                    else
                        pCode.code[cx1].la = pCode.cx;
                    break;
                case symbol.beginSym:
                    s.Clear();
                    s.AddRange(stateSym.SymSet);
                    s1.Clear();
                    s1.AddRange(statementbegs.SymSet);
                    s1.Add(symbol.semicolon);
                    getsym();
                    statement(s);
                    while (s1.isInSet(sym))
                    {
                        if (sym == symbol.semicolon)
                            getsym();
                        else
                            err.adderr(10);
                        statement(s);
                    }
                    if (sym == symbol.endSym)
                        getsym();
                    else
                        err.adderr(17);
                    break;
            }

            ErrorTest(stateSym, new SymbolSet(), 19);
            
        }

        /// <summary>
        /// 表达式模块
        /// </summary>
        /// <param name="expressSym">开始与后继符号集</param>
        private void expression(SymbolSet expressSym)
        {
            PCodeGenetate pCode = compiler.PCG;

            symbol addsym;
            SymbolSet s = new SymbolSet();
            s.AddRange(expressSym.SymSet);
            s.Add(symbol.plus);
            s.Add(symbol.minus);

            if (sym == symbol.plus || sym == symbol.minus)
            {
                addsym = sym;
                getsym();
                term(s);
                if (addsym == symbol.minus)
                    pCode.Gen(oprCode.opr, 0, 1);
            }
            else
            {
                term(s);
            }
            while (sym == symbol.plus || sym == symbol.minus)
            {
                addsym = sym;
                getsym();
                term(s);
                if (addsym == symbol.plus)
                    pCode.Gen(oprCode.opr, 0, 2);
                else
                    pCode.Gen(oprCode.opr, 0, 3);
            }
        }

        /// <summary>
        /// 条件模块
        /// </summary>
        /// <param name="condiSym">开始与后继符号集</param>
        private void condition(SymbolSet condiSym)
        {
            ErrorHandle err = compiler.EH;
            PCodeGenetate pCode = compiler.PCG;

            symbol relsym;
            SymbolSet s = new SymbolSet(), s1=new SymbolSet();
            s.Add(symbol.equality);
            s.Add(symbol.inequality);
            s.Add(symbol.LessThan);
            s.Add(symbol.LessThanE);
            s.Add(symbol.MoreThan);
            s.Add(symbol.MoreThanE);

            if (sym == symbol.oddSym)
            {
                getsym();
                expression(condiSym);
                pCode.Gen(oprCode.opr, 0, 6);
            }
            else
            {
                s1.AddRange(s.SymSet);
                s1.AddRange(condiSym.SymSet);
                expression(s1);
                if (!s.isInSet(sym))
                    err.adderr(20);
                else
                {
                    relsym = sym;
                    getsym();
                    expression(condiSym);
                    switch (relsym)
                    {
                        case symbol.equality:
                            pCode.Gen(oprCode.opr, 0, 8);
                            break;
                        case symbol.inequality:
                            pCode.Gen(oprCode.opr, 0, 9);
                            break;
                        case symbol.LessThan:
                            pCode.Gen(oprCode.opr, 0, 10);
                            break;
                        case symbol.LessThanE:
                            pCode.Gen(oprCode.opr, 0, 13);
                            break;
                        case symbol.MoreThan:
                            pCode.Gen(oprCode.opr, 0, 11);
                            break;
                        case symbol.MoreThanE:
                            pCode.Gen(oprCode.opr, 0, 12);
                            break;       
                    }

                }
            }
        }

        /// <summary>
        /// 项模块
        /// </summary>
        /// <param name="termSym">开始与后继符号集</param>
        private void term(SymbolSet termSym)
        {
            PCodeGenetate pCode = compiler.PCG;

            symbol mulsym;
            SymbolSet s = new SymbolSet();
            s.AddRange(termSym.SymSet);
            s.Add(symbol.times);
            s.Add(symbol.division);

            factor(s);
            while (sym == symbol.times || sym == symbol.division)
            {
                mulsym = sym;
                getsym();
                factor(s);
                if (mulsym == symbol.times)
                    pCode.Gen(oprCode.opr, 0, 4);
                else
                    pCode.Gen(oprCode.opr, 0, 5);
            }
        }

        /// <summary>
        /// 因子模块
        /// </summary>
        /// <param name="factSym">开始与后继符号集</param>
        private void factor(SymbolSet factSym)
        {
            int i;
            ErrorHandle err = compiler.EH;
            SymbolTable table = compiler.ST;
            PCodeGenetate pCode = compiler.PCG;

            SymbolSet sfb = new SymbolSet();
            sfb.AddRange(factorbegs.SymSet);
            
            ErrorTest(sfb, factSym, 24);
            while (factorbegs.isInSet(sym))
            {
                switch (sym)
                {
                    case symbol.ident:
                        i = table.getPosition(compiler.SA.tname);
                        if (i == 0)
                            err.adderr(11);
                        else
                        {
                            SymTabItem STI = table.SymTable[i];

                            switch (STI.kind)
                            {
                                case ObjectK.constant:
                                    pCode.Gen(oprCode.lit, 0, STI.value);
                                    break;
                                case ObjectK.variable:
                                    int fa = level-STI.level;
                                    int la = STI.address;
                                    pCode.Gen(oprCode.lod, fa, la);
                                    break;
                                case ObjectK.procedure:
                                    err.adderr(21);
                                    break;
                            }
                        }
                        getsym();
                        break;
                    case symbol.number:
                        pCode.Gen(oprCode.lit, 0, compiler.SA.tnumber);
                        getsym();
                        break;
                    case symbol.LParenthesis:
                        getsym();
                   
                        factSym.Add(symbol.RParenthesis);
                        expression(factSym);

                        if (sym == symbol.RParenthesis)
                            getsym();
                        else
                            err.adderr(22);
                        break;
                }
                ErrorTest(factSym, factorbegs, 23);
            }
        }
    }
}
