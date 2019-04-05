using System;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;

namespace Laboratory5_TaskManager.Model
{
    internal class MyProcess: IEquatable<MyProcess>
    {
        internal MyProcess(Process process)
        {
            Process = process;
            Counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            Id = process.Id;
            Name = process.ProcessName;
            UserName = GetProcessOwner(Id);
            Title = process.MainWindowTitle;
            try
            {
                StartTimeD = Process.StartTime;
                StartTime = StartTimeD.ToString();

            }
            catch (Exception)
            {
                StartTime = "";
            }
            try
            {
                FilePath = Process.MainModule.FileName;

            }
            catch (Exception)
            {
                FilePath = "";
            }
            RefreshMetaDate();
        }
        private PerformanceCounter Counter { get; set; }
       
        private string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                try
                {
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {

                        return argList[0];
                    }
                } catch (ManagementException)
                { }
            }

            return "";
        }

        internal async void RefreshMetaDate()
        {
            await Task.Run(() =>
            {
                Active = Process.Responding;

                try
                {
                    RamCapacity = new PerformanceCounter("Process", "Working Set", Name, true).NextValue() / 1024;
                    Threads = Process.Threads.Count;
                }
                catch (Exception)
                {
                    RamCapacity = 0;

                }

              Ram = Math.Round(RamCapacity*1024 / Environment.WorkingSet, 3);
            
            try
            {
                Cpu = Math.Round(Counter.NextValue() / Environment.ProcessorCount, 3);
            }
            catch (Exception)
            {
                Cpu = 0;
            }

            });
           
            
            
        }
        internal Process Process { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Cpu { get; set; }
       
        public float RamCapacity { get; set; }
        public double Ram { get; set; }
        public int Threads { get; set; }

        public string UserName { get; set; }
        public string FilePath { get; set; }
        public string StartTime { get; set; }
        internal DateTime StartTimeD { get; set; }

        public string Title { get; set; }

        public bool Equals(MyProcess other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return  Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyProcess) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Process != null ? Process.GetHashCode() : 0) * 397) ^ Id;
            }
        }
    }
}
