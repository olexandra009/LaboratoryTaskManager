using Laboratory5_TaskManager.Model;
using Laboratory5_TaskManager.Tools.Navigation;
using System;
using System.Windows;

namespace Laboratory5_TaskManager.Tools.Managers
{
    internal static class StationManager
    {
        public static event Action StopThreads;
        public static MyProcess SelectedProcess { get; set; }
        private static INavigatable[] _viewsNavigatables;

        internal static INavigatable[] ViewsNavigatable
        {
            get => _viewsNavigatables;
        }
        internal static void Initialize(int views)
        {
            _viewsNavigatables = new INavigatable[views];
        }

        internal static void CloseApp()
        {
            MessageBox.Show("ShutDown");
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}