/* =============================================================================*\
*
* Filename: OutputData.cs
* Description: 
*
* Version: 1.0
* Created: 9/15/2017 16:01:48(UTC+8:00)
* Compiler: Visual Studio 2017
* 
* Author: zsh2401
* Company: I am free man
*
\* =============================================================================*/
namespace AutumnBox.Basic.Executer
{
    using AutumnBox.Basic.Function.Event;
    using AutumnBox.Basic.Util;
    using AutumnBox.Support.CstmDebug;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class OutputData : IPrintable
    {
        /// <summary>
        /// 所有的输出
        /// </summary>
        public List<string> LineAll { get; internal set; } = new List<string>();
        /// <summary>
        /// 所有的标准输出
        /// </summary>
        public List<string> LineOut { get; internal set; } = new List<string>();
        /// <summary>
        /// 所有的标准错误
        /// </summary>
        public List<string> LineError { get; internal set; } = new List<string>();
        /// <summary>
        /// 所有的输出
        /// </summary>
        public StringBuilder All { get; internal set; } = new StringBuilder();
        /// <summary>
        /// 所有的标准输出
        /// </summary>
        public StringBuilder Out { get; internal set; } = new StringBuilder();
        /// <summary>
        /// 所有的标准错误
        /// </summary>
        public StringBuilder Error { get; internal set; } = new StringBuilder();
        /// <summary>
        /// 是否已被关闭
        /// </summary>
        private bool _IsClosed = false;
        /// <summary>
        /// 判断输出中是否包含某段字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public bool Contains(string str, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return All.ToString().ToLower().Contains(str.ToLower());
            }
            else
            {
                return All.ToString().Contains(str);
            }
        }
        /// <summary>
        /// 添加输出信息
        /// </summary>
        /// <param name="outData"></param>
        public void OutAdd(string outData)
        {
            try
            {
                if (outData == null) return;
                if (_IsClosed) return;
                All.Append(outData + System.Environment.NewLine);
                LineAll.Add(outData);//<----就是这里出现的异常!!!
                LineOut.Add(outData);
                Out.Append(outData + System.Environment.NewLine);
            }
            catch (IndexOutOfRangeException)
            {
                //2017 11 21 01:00的一次调试中
                //我在显示关闭其它助手的提示后,关闭了360手机助手并点击了"我已关闭其它助手"
                //然后出现了RateBox,半秒后便出现了这个奇怪的IndexOutOfRangeException??
                //在StackOverFlow上搜寻后,有人说这是一个奇怪的BUG
                //既然如此...下次就抓住这个BUG吧...
            }
        }
        /// <summary>
        /// 添加错误输出信息
        /// </summary>
        /// <param name="errorData"></param>
        public void ErrorAdd(string errorData)
        {
            try
            {
                if (errorData == null) return;
                if (_IsClosed) return;
                All.Append(errorData + Environment.NewLine);
                LineAll.Add(errorData);
                LineError.Add(errorData);
                Error.Append(errorData + Environment.NewLine);
            }
            catch (IndexOutOfRangeException)
            {
                //2017 11 21 01:24 出现跟上面一样的异常,奇怪!
            }
        }
        /// <summary>
        /// 添加另一个OutputData对象的内容
        /// </summary>
        /// <param name="output"></param>
        public void Append(OutputData output)
        {
            if (_IsClosed) return;
            LineAll.AddRange(output.LineAll);
            LineOut.AddRange(output.LineOut);
            LineError.AddRange(output.LineError);
            All.Append(output.ToString());
            Out.Append(output.Out.ToString());
            Error.Append(output.Error.ToString());
        }
        /// <summary>
        /// 清空内容
        /// </summary>
        public void Clear()
        {
            Out = new StringBuilder();
            Error = new StringBuilder();
            All = new StringBuilder();
            LineAll.Clear();
            LineOut.Clear();
            LineError.Clear();
        }
        /// <summary>
        /// 停止添加内容,执行后将不可再添加内容
        /// </summary>
        public void StopAdding()
        {
            _IsClosed = true;
        }
        /// <summary>
        /// 添加需要被监听的输出产生者
        /// </summary>
        /// <param name="sender"></param>
        public void AddOutSender(IOutSender sender)
        {
            sender.OutputReceived += (s, e) =>
            {
                if (!e.IsError)
                {
                    OutAdd(e.Text);
                }
                else
                {
                    ErrorAdd(e.Text);
                }
            };
        }
        /// <summary>
        /// 获取完整的输出数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return All.ToString();
        }

        public void PrintOnLog(bool printOnRelease = false)
        {
            if (printOnRelease)
            {
                Logger.T($"PrintOnLog(): {ToString()}");
            }
            else
            {
                Logger.D($"PrintOnLog(): {ToString()}");
            }
        }

        public void PrintOnConsole()
        {
            Console.WriteLine($"PrintOnConsole(): {ToString()}");
        }
    }
}
