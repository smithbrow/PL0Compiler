using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL0CompilerLibrary
{
    public class SymbolTable
    {
        /// <summary>
        /// 字符表
        /// </summary>
        public List<SymTabItem> SymTable = new List<SymTabItem>();

        private PL0Compiler compiler;
        private int lev;
        private int[] tx = new int[((int)constnum.MAX_LEVEL) + 1];

        /// <summary>
        /// 构造函数
        /// </summary>
        public SymbolTable(PL0Compiler C)
        {
            this.lev = 0;
            
            this.tx[0] = 0;
            SymTabItem nul = new SymTabItem();
            nul.name = "";
            SymTable.Add(nul);

            this.compiler = C;
        }

        /// <summary>
        /// 获取层次入口
        /// </summary>
        /// <returns></returns>
        public int tableIndex()
        {
            return tx[lev];
        }

        /// <summary>
        /// 通过标识符名称获取其位置
        /// </summary>
        /// <param name="name">标识符名称</param>
        /// <returns>位置</returns>
        public int getPosition(string name)
        {
            int i = tx[lev];
            while (!SymTable[i].name.Equals(name)&& i>0)
                i--;
            return i;
        }

        /// <summary>
        /// 字符表登录函数
        /// </summary>
        /// <param name="K">类型</param>
        public void Enter(ObjectK K)
        {
            GrammarAndSemanticAna GS = compiler.GSA;

            if (GS.getLevel() > lev)
            {
                lev++;
                tx[lev] = tx[lev - 1];
            }
            else if (GS.getLevel() < lev)
                lev--;

            tx[lev]++;

            SymTabItem item = new SymTabItem();
            item.kind = K;
            item.name = compiler.SA.tname;
            switch (K)
            {
                case ObjectK.constant:
                    item.value = compiler.SA.tnumber;
                    SymTable.Add(item);
                    break;
                case ObjectK.variable:
                    item.level = compiler.GSA.getLevel();
                    item.address = compiler.GSA.getAddress();
                    SymTable.Add(item);
                    break;
                case ObjectK.procedure:
                    item.level = compiler.GSA.getLevel();
                    SymTable.Add(item);
                    break;
            }
        }

    }
}
