namespace Laboratory5_TaskManager.Tools.Navigation
{
    internal enum ViewType
    {
        Processes,
        Modules
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
