using Laboratory5_TaskManager.Model;
using Laboratory5_TaskManager.Tools.Managers;
using Laboratory5_TaskManager.Tools.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Laboratory5_TaskManager.ViewsModel
{
    class ProcessViewModel:BaseViewModel
    {
        private List<MyProcess> _myProcesses;
        private ICommand _sortingCommand;
        private bool _reverseOrder;
       
        public bool ReverseOrder
        {
            get => _reverseOrder;
            set
            {
                _reverseOrder = value;
                OnPropertyChanged();
            }
        }
        public ICommand SortingCommand =>
            _sortingCommand ?? (_sortingCommand =
                new RelayCommand<object>(SortingCommandImplementation));

       
        public List<MyProcess> MyProcessList {
            get =>_myProcesses;
            set {
                _myProcesses = value;
                OnPropertyChanged();
            }
        }
        private ICommand _selectCommand;
        private ICommand _stopProcessCommand;

        public ICommand StopProcessCommand
        {
            get
            {
                return _stopProcessCommand ?? (_stopProcessCommand = new RelayCommand<object>(StopProcessCommandImplementation));
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand =>
            _openCommand ?? (_openCommand = new RelayCommand<object>(OpenCommandImplementation));

        private void OpenCommandImplementation(object obj)
        {
            if(SelectedProcess == null) return;
            
            int i = SelectedProcess.FilePath.LastIndexOf("\\", StringComparison.Ordinal);
            if (i == -1)
                return;
            Process.Start(SelectedProcess.FilePath.Substring(0, i));

        }

        public ICommand SelectionCommand => _selectCommand ?? (_selectCommand = new RelayCommand<object>(SelectCommandImplementation));
        public MyProcess SelectedProcess { get; set; }
        private void SelectCommandImplementation(object obj)
        {
            StationManager.SelectedProcess = SelectedProcess;
            StationManager.ViewsNavigatable[1].Refresher();
            NavigationManager.Instance.Navigate(ViewType.Modules);
        }

        private int _type = 0;

        public int SelectedSortingType
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private Thread _workingThread;
        private Thread _workingThreadRefreshing;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        internal ProcessViewModel()
        {
           
         
        MyProcessList = new List<MyProcess>();
            foreach (Process process in Process.GetProcesses())
            {
                MyProcessList.Add(item: new MyProcess(process));
            }
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
          
        }

        private void StartWorkingThread()
        {
           _workingThread = new Thread(WorkingThreadProcess);
           _workingThreadRefreshing = new Thread(RefreshingProcess);
           _workingThreadRefreshing.Start();
           _workingThread.Start();
            
        }

        private void RefreshingProcess()
        {
           
            while (!_token.IsCancellationRequested)
            {
                

                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(200);
                    if (_token.IsCancellationRequested)
                        break;
                }
                UpdateMetaDate();
                if (_token.IsCancellationRequested)
                    break;
               
              
            }
        }

        private void UpdateMetaDate()
        {
            
            int count = MyProcessList.Count;
            for(int i =  count-1; i>=0; i--)
            {
                 MyProcessList[i].RefreshMetaDate();
            }
            MyProcessList = new List<MyProcess>(MyProcessList);

        }
        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
           _workingThreadRefreshing.Join(2000);
            _workingThreadRefreshing.Abort();
          _workingThreadRefreshing = null;
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }
        private void WorkingThreadProcess()
        {
           
            while (!_token.IsCancellationRequested)
            {
                
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(500);
                    if (_token.IsCancellationRequested)
                        break;
                }
                if (_token.IsCancellationRequested)
                    break;
                
                WorkingProcess();
            }
        }
     

        private void WorkingProcess()
        {
          
            List<MyProcess> existingProcesses = new List<MyProcess>(MyProcessList);
            List<MyProcess> newProcesses = new List<MyProcess>();

            foreach (Process proc in Process.GetProcesses())
            {
                newProcesses.Add(new MyProcess(proc));
            }

            foreach (var proc in existingProcesses)
            {
                if (!newProcesses.Contains(proc))
                {
                    MyProcessList.Remove(proc);
               
                }
            }

         
            foreach (var proc in newProcesses)
            {
                if (!MyProcessList.Contains(proc))
                
                    MyProcessList.Add(proc);
                   

            }

             MyProcessList = new List<MyProcess>(MyProcessList);
        }
        private void SortingCommandImplementation(object obj)
        {
            List<MyProcess> orderedEnumerable;
            switch (SelectedSortingType)
            {
                case 0:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Id))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Id));
                    break;
                case 1:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Name))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Name));
                    break;
                case 2:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Active))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Active));
                    break;
                case 3:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Cpu))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Cpu));
                    break;
                case 4:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.RamCapacity))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.RamCapacity));
                    break;
                case 5:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Ram))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Ram));
                    break;
                case 6:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Threads))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Threads));
                    break;
                case 7:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.UserName))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.UserName));
                    break;
                case 8:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.FilePath))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.FilePath));
                    break;
                case 9:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.StartTimeD))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.StartTimeD));
                    break;
                case 10:
                    orderedEnumerable = !_reverseOrder
                        ? new List<MyProcess>(MyProcessList.OrderBy(process => process.Title))
                        : new List<MyProcess>(MyProcessList.OrderByDescending(process => process.Title));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(null);
            }

            MyProcessList = orderedEnumerable;
        }
        private void StopProcessCommandImplementation(object obj)
        {


            try
            {
                SelectedProcess.Process.Kill();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

     
       
        public override void Refresher()
        {
            
        }
    }
}
