using Laboratory5_TaskManager.Tools.Navigation;
using Laboratory5_TaskManager.ViewsModel;

namespace Laboratory5_TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для ProcessControl.xaml
    /// </summary>
    public partial class ProcessControl : INavigatable
    {
        private BaseViewModel _model;
        public ProcessControl()
        {
            InitializeComponent();
            _model = new ProcessViewModel();
            DataContext = _model;
        }

        public void Refresher()
        {
            _model.Refresher();
        }
    }
}
