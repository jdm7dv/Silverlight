﻿#pragma checksum "C:\Silverlight4\Labs\SL4LOB_03_User_Registration\Source\Completed\Exercise 6\SlEventManager\Views\Home.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "35A952F71FB0BEA902495E0AE37792BF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SlEventManager {
    
    
    public partial class Home : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DomainDataSource eventDomainDataSource;
        
        internal System.Windows.Controls.DataGrid eventDataGrid;
        
        internal System.Windows.Controls.DataGridTextColumn eventTitleColumn;
        
        internal System.Windows.Controls.DataGridTemplateColumn eventStartDateColumn;
        
        internal System.Windows.Controls.DataGridTemplateColumn eventEndDateColumn;
        
        internal System.Windows.Controls.DataGridTextColumn eventVenueNameColumn;
        
        internal System.Windows.Controls.DataGridTextColumn eventDescriptionColumn;
        
        internal System.Windows.Controls.Button editCurrentButton;
        
        internal System.Windows.Controls.Button createNewEventButton;
        
        internal System.Windows.Controls.Button registerForEventButton;
        
        internal System.Windows.Controls.Button unregisterForEventButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlEventManager;component/Views/Home.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.eventDomainDataSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("eventDomainDataSource")));
            this.eventDataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("eventDataGrid")));
            this.eventTitleColumn = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("eventTitleColumn")));
            this.eventStartDateColumn = ((System.Windows.Controls.DataGridTemplateColumn)(this.FindName("eventStartDateColumn")));
            this.eventEndDateColumn = ((System.Windows.Controls.DataGridTemplateColumn)(this.FindName("eventEndDateColumn")));
            this.eventVenueNameColumn = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("eventVenueNameColumn")));
            this.eventDescriptionColumn = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("eventDescriptionColumn")));
            this.editCurrentButton = ((System.Windows.Controls.Button)(this.FindName("editCurrentButton")));
            this.createNewEventButton = ((System.Windows.Controls.Button)(this.FindName("createNewEventButton")));
            this.registerForEventButton = ((System.Windows.Controls.Button)(this.FindName("registerForEventButton")));
            this.unregisterForEventButton = ((System.Windows.Controls.Button)(this.FindName("unregisterForEventButton")));
        }
    }
}

