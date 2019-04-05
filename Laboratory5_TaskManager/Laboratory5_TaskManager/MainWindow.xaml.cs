
using System.ComponentModel;
using System.Windows.Controls;
using Laboratory5_TaskManager.Tools.Managers;
using Laboratory5_TaskManager.Tools.Navigation;
using Laboratory5_TaskManager.Views;
using Laboratory5_TaskManager.ViewsModel;

namespace Laboratory5_TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IContentOwner
    {
        public MainWindow()
        {
            InitializeComponent();
            StationManager.Initialize(2);
            var owner = new InitializationNavigationModel(this);
            NavigationManager.Instance.Initialize(owner);
            var process = new ProcessControl();
            var model = new ModuleControl();
            owner.ViewsDictionary.Add(ViewType.Processes, process);
            owner.ViewsDictionary.Add(ViewType.Modules, model);
            StationManager.ViewsNavigatable[0] = process;
            StationManager.ViewsNavigatable[1] = model;
            NavigationManager.Instance.Navigate(ViewType.Processes);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.CloseApp();
        }

        public ContentControl ContentControl => _contentControl;
    }
}
