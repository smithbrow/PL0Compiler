using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL0CompilerLibrary
{
    /// <summary>
    /// 操作码
    /// </summary>
    public enum oprCode
    {
        lit, lod, sto, cal, Int, jmp, jpc, opr
    }
    /// <summary>
    /// 指令格式
    /// </summary>
    public class instruction
    {
        public oprCode oprc;
        public int fa;
        public int la;
    }

    public class PCodeGenetate
    {
        public List<instruction> code = new List<instruction>();//存放目标代码
        public int cx;//存放代码的位置
        private int []runStack = new int[(int)constnum.MAX_RUN_STACK];//运行栈
        private int b;//栈基址

        /// <summary>
        /// 构造函数
        /// </summary>
        public PCodeGenetate()
        {
            cx = 0;
        }
        /// <summary>
        /// 添加目标代码
        /// </summary>
        /// <param name="op">操作码</param>
        /// <param name="f">第一参数</param>
        /// <param name="l">最后参数</param>
        public void Gen(oprCode op, int f, int l)
        {
            instruction ins = new instruction();
            ins.oprc = op;
            ins.fa = f;
            ins.la = l;
            code.Add(ins);
            cx++;
        }

        /// <summary>
        /// 获取与当前层差l层的基地址
        /// </summary>
        /// <param name="l">层次差</param>
        /// <returns>基地址</returns>
        private int Base(int l)
        {
            int b1 = b;
            while (l > 0)
            {
                b1 = runStack[b1];
                l--;
            }
            return b1;
        }

        /// <summary>
        /// 解释执行
        /// </summary>
        public void interpret()
        {
            int p, t;
            oprCode oc;
            int l, a;

            t=0;b=1;p=0;
            runStack[1]=0;
            runStack[2]=0;
            runStack[3]=0;

            do
            {
                oc = code[p].oprc;
                l = code[p].fa;
                a = code[p].la;
                p++;
                switch (oc)
                {
                    case oprCode.Int:
                        t = t + a;
                        break;
                    case oprCode.lit:
                        runStack[++t] = a;
                        break;
                    case oprCode.lod:
                        t++;
                        runStack[t] = runStack[Base(l) + a];
                        break;
                    case oprCode.sto:
                        runStack[Base(l) + a] = runStack[t];
                        t--;
                        break;
                    case oprCode.jmp:
                        p = a;
                        break;
                    case oprCode.jpc:
                        if (runStack[t] == 0)
                            p = a;
                        t--;
                        break;
                    case oprCode.cal:
                        runStack[t + 1] = Base(l);
                        runStack[t + 2] = b;
                        runStack[t + 3] = p;
                        b = t + 1;
                        p = a;
                        break;
                    case oprCode.opr:
                        int temp;
                        switch (a)
                        {
                            case 0:
                                t = b - 1;
                                p = runStack[t + 3];
                                b = runStack[t + 2];
                                break;
                            case 1:
                                runStack[t] = -runStack[t];
                                break;
                            case 2:
                                temp = runStack[t] + runStack[t - 1];
                                t--;
                                runStack[t] = temp;
                                break;
                            case 3:
                                temp = runStack[t - 1] - runStack[t];
                                t--;
                                runStack[t] = temp;
                                break;
                            case 4:
                                temp = runStack[t - 1] * runStack[t];
                                t--;
                                runStack[t] = temp;
                                break;
                            case 5:
                                temp = runStack[t - 1] / runStack[t];
                                t--;
                                runStack[t] = temp;
                                break;
                            case 6:
                                temp = runStack[t] % 2;
                                runStack[t] = temp;
                                break;
                            case 7:
                                break;
                            case 8:
                                temp = (runStack[t - 1] == runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 9:
                                temp = (runStack[t - 1] != runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 10:
                                temp = (runStack[t - 1] < runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 11:
                                temp = (runStack[t - 1] >= runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 12:
                                temp = (runStack[t - 1] > runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 13:
                                temp = (runStack[t - 1] <= runStack[t] ? 1 : 0);
                                t--;
                                runStack[t] = temp;
                                break;
                            case 14:
                                Console.Write(runStack[t]);
                                break;
                            case 15:
                                Console.Write("\n");
                                break;
                            case 16:
                                t++;
                                temp = Convert.ToInt32(Console.ReadLine());
                                runStack[t] = temp;
                                break;
                        }
                        break;
                }
            } while (p != 0);
        }

    }
}
