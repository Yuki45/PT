﻿#pragma checksum "..\..\..\Pages\RackDashboardView - Copy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A231C360A27494002ADC19BB2F95E36B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace repairhhp.Pages {
    
    
    /// <summary>
    /// RackDashboardView
    /// </summary>
    public partial class RackDashboardView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ItemSearch;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Search;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataStock;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbStorage;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbZone;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbBin;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox readyQty;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox warningQty;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Pages\RackDashboardView - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox dangerQty;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/hms;component/pages/rackdashboardview%20-%20copy.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            ((repairhhp.Pages.RackDashboardView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UCLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ItemSearch = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.Search = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            this.Search.Click += new System.Windows.RoutedEventHandler(this.Search_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DataStock = ((System.Windows.Controls.DataGrid)(target));
            
            #line 22 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            this.DataStock.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.DataStock_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cmbStorage = ((System.Windows.Controls.ComboBox)(target));
            
            #line 23 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            this.cmbStorage.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbStorage_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cmbZone = ((System.Windows.Controls.ComboBox)(target));
            
            #line 25 "..\..\..\Pages\RackDashboardView - Copy.xaml"
            this.cmbZone.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbZone_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.cmbBin = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.readyQty = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.warningQty = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.dangerQty = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
