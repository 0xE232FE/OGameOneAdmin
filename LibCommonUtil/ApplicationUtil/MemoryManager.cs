using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace LibCommonUtil
{
    public class MemoryManager
    {

        /***********************************************************************************************************/


        #region ------ Private Static Variables ------


        private static MemoryManager _memoryManager;

        private static long _lastReduceWorkingSetTicks = DateTime.Now.Ticks;
        private static long _reduceWorkingSetInterval;

        private static long _lastForceGCCollectTicks = DateTime.Now.Ticks;
        private static long _forceGCCollectInterval;

        private static bool _bAutoForceGCCollect = false;
        private static bool _bAutoReduceWorkingSet = false;
        private static bool _bFlushMemory = false;

        #endregion ------ Private Static Variables ------


        /***********************************************************************************************************/


        #region ------ Constructor ------


        private MemoryManager(bool bAutoForceGCCollect, bool bAutoReduceWorkingSet)
        {
            AssemblySettings settings = new AssemblySettings();

            try
            {
                _forceGCCollectInterval = new TimeSpan(0, Int32.Parse(settings["ForceGCCollectInterval"].Trim()), 0).Ticks;
            }
            catch
            {
                _forceGCCollectInterval = new TimeSpan(0, 30, 0).Ticks;
            }

            try
            {
                _reduceWorkingSetInterval = new TimeSpan(0, 0, Int32.Parse(settings["ReduceWorkingSetInterval"].Trim())).Ticks;
            }
            catch
            {
                _reduceWorkingSetInterval = new TimeSpan(0, 0, 5).Ticks;
            }

            _bAutoReduceWorkingSet = bAutoReduceWorkingSet;
            _bAutoForceGCCollect = bAutoForceGCCollect;
            _bFlushMemory = false;
            Application.Idle += new EventHandler(ReduceMemoryUsage);
        }


        private MemoryManager(bool bAutoForceGCCollect, bool bAutoReduceWorkingSet, long forceGCCollectInterval, long reduceWorkingSetInterval)
        {
            _forceGCCollectInterval = forceGCCollectInterval;
            _reduceWorkingSetInterval = reduceWorkingSetInterval;
            _bAutoForceGCCollect = bAutoForceGCCollect;
            _bAutoReduceWorkingSet = bAutoReduceWorkingSet;
            _bFlushMemory = false;
            Application.Idle += new EventHandler(ReduceMemoryUsage);
        }


        #endregion ------ Constructor ------


        #region ------ Destructor ------


        ~MemoryManager()
        {
            Application.Idle -= new EventHandler(ReduceMemoryUsage);
        }


        #endregion ------ Destructor ------


        /***********************************************************************************************************/


        #region ------ Private Static Events ------


        private static void ReduceMemoryUsage(object sender, EventArgs e)
        {
            try
            {
                long ticks = DateTime.Now.Ticks;

                if (_bFlushMemory)
                {
                    _bFlushMemory = false;
                    _lastForceGCCollectTicks = ticks;
                    _lastReduceWorkingSetTicks = ticks;
                    ForceGCCollect();
                    ReduceWorkingSetSize();
                }
                else
                {
                    if (_bAutoForceGCCollect && (ticks - _lastForceGCCollectTicks) > _forceGCCollectInterval)
                    {
                        _lastForceGCCollectTicks = ticks;
                        ForceGCCollect();
                    }

                    if (_bAutoReduceWorkingSet && (ticks - _lastReduceWorkingSetTicks) > _reduceWorkingSetInterval)
                    {
                        _lastReduceWorkingSetTicks = ticks;
                        ReduceWorkingSetSize();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //using (StreamWriter sw = File.AppendText(@"MemoryManagerUtil.txt"))
                    //{
                    //    sw.WriteLine("Reduced memory usage failed at: " + DateTime.Now.ToLongTimeString());
                    //    sw.WriteLine(ex.Message + " | " + ex.StackTrace);
                    //}
                }
                catch
                { }
            }
        }


        #endregion ------ Private Static Events ------


        /***********************************************************************************************************/


        #region ------ Private Static Methods ------


        [DllImport("kernel32", EntryPoint = "SetProcessWorkingSetSize")]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);


        private static void ReduceWorkingSetSize()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                SetProcessWorkingSetSize(process.Handle, -1, -1);
            }
        }


        private static void ForceGCCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        #endregion ------ Private Static Methods ------


        #region ------ Public Static Methods ------


        public static void AttachApp(bool bAutoForceGCCollect, bool bAutoReduceWorkingSet)
        {
            try
            {
                if (_memoryManager != null)
                    DetachApp();

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    _memoryManager = new MemoryManager(bAutoForceGCCollect, bAutoReduceWorkingSet);
                }
            }
            catch
            {
            }
        }


        public static void AttachApp(bool bAutoForceGCCollect, bool bAutoReduceWorkingSet, long forceGCCollectInterval, long reduceWorkingSetInterval)
        {
            try
            {
                if (_memoryManager != null)
                    DetachApp();

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    _memoryManager = new MemoryManager(bAutoForceGCCollect, bAutoReduceWorkingSet, forceGCCollectInterval, reduceWorkingSetInterval);
                }
            }
            catch
            {
            }
        }


        public static void DetachApp()
        {
            if (_memoryManager != null)
            {
                Application.Idle -= new EventHandler(ReduceMemoryUsage);
                _memoryManager = null;
            }
        }


        public static void FlushMemory()
        {
            if (_memoryManager == null)
            {
                AttachApp(false, false);
            }

            if (_memoryManager != null)
                _bFlushMemory = true;
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/

    }
}
