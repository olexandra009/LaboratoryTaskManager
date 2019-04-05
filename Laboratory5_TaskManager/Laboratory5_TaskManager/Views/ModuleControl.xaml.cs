using Laboratory5_TaskManager.Tools.Navigation;
using Laboratory5_TaskManager.ViewsModel;

namespace Laboratory5_TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для ModuleControl.xaml
    /// </summary>
    public partial class ModuleControl : INavigatable
    {
        private BaseViewModel _model;
        public ModuleControl()
        {
            InitializeComponent();
            _model = new ModuleViewModel();
            DataContext = _model;

        }

        public void Refresher()
        {
            _model.Refresher();
        }
    }
}
