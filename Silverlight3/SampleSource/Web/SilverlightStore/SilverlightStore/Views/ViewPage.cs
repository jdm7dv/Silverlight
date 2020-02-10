﻿namespace SilverlightStore.Views
{
    using System.Windows.Controls;
    using SilverlightStore.ViewModels;

    public abstract class ViewPage : Page, IView
    {
        private ViewModel _viewModel;

        public ViewModel ViewModel
        {
            get
            {
                return this._viewModel;
            }
            set
            {
                this._viewModel = value;
                this.DataContext = value;
            }
        }
    }
}