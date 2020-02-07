namespace SilverlightStore.Views
{
    using SilverlightStore.ViewModels;

    public interface IView
    {
        ViewModel ViewModel
        {
            get;
            set;
        }
    }
}
