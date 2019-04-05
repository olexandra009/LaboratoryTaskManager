using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Laboratory5_TaskManager.Tools.Managers;
using Laboratory5_TaskManager.Tools.Navigation;

namespace Laboratory5_TaskManager.ViewsModel
{
    class ModuleViewModel : BaseViewModel
    {
        private ProcessModuleCollection _processModuleCollection;
        private ProcessThreadCollection _processThreadCollection;
        private string _process;
        public string Process
        {
            get => _process;
            set
            {
                _process = value;
                OnPropertyChanged();
            }

        }
        public ProcessThreadCollection ProcessThreadCollection
        {
            get => _processThreadCollection;
            set
            {
               _processThreadCollection = value;
                OnPropertyChanged();
            }
        }
        public ProcessModuleCollection ProcessModuleCollection { get=> _processModuleCollection;
            set
            {
                _processModuleCollection = value;
                OnPropertyChanged();
            } }
        private ICommand _backCommand;
        public ICommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand<object>(BackCommandImplementation));

        private void BackCommandImplementation(object obj)
        {
           NavigationManager.Instance.Navigate(ViewType.Processes);
        }

        internal ModuleViewModel()
        {
         
        }


        public override void Refresher()
        {
            ProcessModuleCollection = null;
            ProcessThreadCollection = null;
            
            try
            {
                Process = "ID: " + StationManager.SelectedProcess.Id + " Name: " + StationManager.SelectedProcess.Name;
                ProcessModuleCollection = StationManager.SelectedProcess.Process.Modules;
                ProcessThreadCollection = StationManager.SelectedProcess.Process.Threads;

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Select nothing");
                
            }
            catch(Exception)
             {
                MessageBox.Show("Error");
                

            }


        }
    }
}