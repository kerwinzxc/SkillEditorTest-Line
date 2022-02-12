/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Timer\TimerMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-2      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;

    public sealed class TimerMgr : Singleton<TimerMgr>
    {
        private LinkedList<Timer> mTimerAddList = new LinkedList<Timer>();
        private LinkedList<Timer> mTimerUpdateList = new LinkedList<Timer>();
        private LinkedList<Timer> mTimerHandleList = new LinkedList<Timer>();

        private LinkedList<Ticker> mTickerAddList = new LinkedList<Ticker>();
        private LinkedList<Ticker> mTickerUpdateList = new LinkedList<Ticker>();
        private LinkedList<Ticker> mTickerHandlerList = new LinkedList<Ticker>();

        public override void Init()
        {
            
        }

        public override void Destroy()
        {
            using (var itr = mTimerAddList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            using (var itr = mTimerUpdateList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }

            mTimerAddList.Clear();
            mTimerUpdateList.Clear();
        }

        public Timer AddTimer(float duration, bool repeat, int repeatCount, Timer.TimerAction handler, params object[] param)
        {
            Timer timer = new Timer(duration, repeat, repeatCount, handler, param);
            mTimerAddList.AddLast(timer);

            return timer;
        }

        public Ticker AddTicker(float duration, Ticker.TickerUpdateAction updateHandler, Ticker.TickerEndAction endHandler)
        {
            Ticker ticker = new Ticker(duration, updateHandler, endHandler);
            mTickerAddList.AddLast(ticker);

            return ticker;
        }

        public void Update(float fTick)
        {
            // update timer
            using (var itr = mTimerAddList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mTimerUpdateList.AddLast(itr.Current);
                }
            }
            mTimerAddList.Clear();

            using (var itr = mTimerUpdateList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Update(fTick))
                    {
                        mTimerHandleList.AddLast(itr.Current);
                    }
                }
            }

            using (var itr = mTimerHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                    mTimerUpdateList.Remove(itr.Current);
                }
            }
            mTimerHandleList.Clear();

            // update ticker
            using (var itr = mTickerAddList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mTickerUpdateList.AddLast(itr.Current);
                }
            }
            mTickerAddList.Clear();

            using (var itr = mTickerUpdateList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Update(fTick))
                    {
                        mTickerHandlerList.AddLast(itr.Current);
                    }
                }
            }

            using (var itr = mTickerHandlerList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                    mTickerUpdateList.Remove(itr.Current);
                }
            }
            mTickerHandlerList.Clear();
        }
    }
}
