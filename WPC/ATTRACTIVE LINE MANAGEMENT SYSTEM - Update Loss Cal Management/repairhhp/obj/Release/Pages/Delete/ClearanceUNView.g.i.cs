﻿#pragma checksum "..\..\..\..\Pages\Delete\ClearanceUNView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4973F729BFBADA2641357B7F53A12D82"
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
    /// ClearanceUNView
    /// </summary>
    public partial class ClearanceUNView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchGrid;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridFiles;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button clearGI;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submitDec;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelStock;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUN;
        
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
            System.Uri resourceLocater = new System.Uri("/hms;component/pages/delete/clearanceunview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
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
            
            #line 7 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            ((repairhhp.Pages.ClearanceUNView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UCLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.searchGrid = ((System.Windows.Controls.TextBox)(target));
            
            #line 32 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            this.searchGrid.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.searchChange);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.refreshClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DataGridFiles = ((System.Windows.Controls.DataGrid)(target));
            
            #line 34 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            this.DataGridFiles.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.rowDoubleClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.clearGI = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            this.clearGI.Click += new System.Windows.RoutedEventHandler(this.clear_click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.submitDec = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\..\Pages\Delete\ClearanceUNView.xaml"
            this.submitDec.Click += new System.Windows.RoutedEventHandler(this.submitDec_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.labelStock = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.txtUN = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

