using Laboratory5_TaskManager.Views;
using System;

namespace Laboratory5_TaskManager.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {

            switch (viewType)
            {

                case ViewType.Processes:
                    ViewsDictionary.Add(viewType, new ProcessControl());
                    break;
             case ViewType.Modules:
                    ViewsDictionary.Add(viewType, new ModuleControl());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
