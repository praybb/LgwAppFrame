using System;
using System.Collections.Generic;
using System.Threading;

namespace LgwAppFrame.Code.HighConcurrencyTest
{
    //测试并发时间委托
    public delegate void ConcurrencyTest();
    /// <summary>
    /// 高并发测试类
    /// </summary>
    public static class HighConcurrencyTest
    {

        /// <summary>
        /// 测试高并发下单个委托的执行时间
        /// </summary>
        /// <param name="count">并发计数</param>
        /// <param name="cTest">委托</param>
        /// <param name="sizeThread">一个线程执行的估计时间，可以估计的大些，默认是6毫秒</param>
        /// <returns></returns>
        public static double StartTest(int count, ConcurrencyTest cTest, int sizeThread = 6)
        {
            #region 为保证之后取得的时间准确度，初始化
            ConcurrentLock cLock = new ConcurrentLock();
            ConcurrentLock autoLock = new ConcurrentLock();
            GC.Collect();
            GC.Collect();
            GC.Collect();
            GC.Collect();
            #endregion

            #region 测算1亿次if时间，并按比例计算出时间差
            int max = ((count + 1) * count) / 2;//if被操作的总次数
            double stime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
            for (int i = 0; i < 100000000; i++)
            {
                if (cLock.EndTime == -1)
                {
                }
            }
            double etime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
            double ifSpan = ((etime - stime) / 100000000d) * max;
            #endregion

            #region 清理内存使用，再初始化
            GC.Collect();
            GC.Collect();
            GC.Collect();
            GC.Collect();
            Thread.Sleep(3000);
            #endregion

            #region 执行，执行原理如下，因为线程开启的时间，各个线程之间存在时间差，现在采取一种办法，使其时间差只是一个if判断
            List<Thread> threads = new List<Thread>();
            int current = 0;
            for (int i = 0; i < count; i++)
            {
                var thread = new Thread(() =>
                {
                    //开始，挡路所有线程
                    lock (autoLock)
                    {
                        if (autoLock.StartTime == -1)
                        {
                            autoLock.StartTime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
                            Thread.Sleep(sizeThread * count);
                        }
                    }
                    //释放挡路的路牌
                    #region 委托执行
                    cTest();
                    #endregion
                    //结束使用原子计数器
                    Interlocked.Increment(ref current);
                    if (current >= count)
                    {
                        //此时所有线程都运行完该委托
                        autoLock.EndTime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.MTA);
                threads.Add(thread);
            }
            foreach (var t in threads)
            {
                t.Start();
            }
            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion

            return (double)(autoLock.EndTime - autoLock.StartTime - sizeThread * count - ifSpan) / (double)count;
        }
    }

    class ConcurrentLock
    {
        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public ConcurrentLock()
        {
            StartTime = -1;
            EndTime = -1;
        }

        public void Reset()
        {
            StartTime = -1;
            EndTime = -1;
        }
    }
}
