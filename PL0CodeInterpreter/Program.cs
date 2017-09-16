using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PL0CompilerLibrary;


namespace PL0CodeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            interpreter inter = new interpreter(args[0]);
            
            inter.interpret();

            Console.WriteLine("请按任意键退出...");
            Console.ReadKey();
            
        }
    }

    class interpreter
    {
        private const int MAX_RUN_STACK = 500;
        private int[] runStack = new int[MAX_RUN_STACK];//运行栈
        private int b;//栈基址
        private List<instruction> code = new List<instruction>();

        public interpreter(string file)
        {
            if (file != "")
            {
                try
                {
                    string pCode = File.ReadAllText(file);
                    string[] pCodes = pCode.Split('\n');

                    
                    for (int i = 0; i < pCodes.Length - 1; i++)
                    {
                        string cl = pCodes[i].Remove(pCodes[i].Length - 1);
                        string[] ops = cl.Split(' ');
                        instruction ins = new instruction();
                        ins.oprc = (oprCode)Convert.ToInt32(ops[0]);
                        ins.fa = Convert.ToInt32(ops[1]);
                        ins.la = Convert.ToInt32(ops[2]);
                        code.Add(ins);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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

            t = 0; b = 1; p = 0;
            runStack[1] = 0;
            runStack[2] = 0;
            runStack[3] = 0;

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
