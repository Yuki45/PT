﻿#pragma checksum "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8420D435975FA97B591201B3E1695227"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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


namespace repairhhp.Pages.Popup {
    
    
    /// <summary>
    /// Popup_gen
    /// </summary>
    public partial class Popup_gen : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid UserGrid;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearch;
        
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
            System.Uri resourceLocater = new System.Uri("/repairhhp;component/pages/popup/popup_gen%20-%20copy.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
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
            
            #line 9 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
            ((repairhhp.Pages.Popup.Popup_gen)(target)).Loaded += new System.Windows.RoutedEventHandler(this.popupLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.UserGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 15 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
            this.UserGrid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.rowDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\..\Pages\Popup\Popup_gen - Copy.xaml"
            this.txtSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.searchChange);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

