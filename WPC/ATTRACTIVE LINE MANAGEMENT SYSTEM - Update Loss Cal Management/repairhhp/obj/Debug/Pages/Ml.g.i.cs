﻿#pragma checksum "..\..\..\Pages\Ml.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B79614DE093D5DC07F31237801841067"
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
using System.Windows.Controls.DataVisualization.Charting;
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
    /// MlosscalView
    /// </summary>
    public partial class MlosscalView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridFiles;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbModel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submitDec;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button export2Excel;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkTimer;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbTime;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdAll;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdNG;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdOK;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbItem;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUpload;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Pages\Ml.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelStock;
        
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
            System.Uri resourceLocater = new System.Uri("/hms;component/pages/ml.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Ml.xaml"
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
            
            #line 7 "..\..\..\Pages\Ml.xaml"
            ((repairhhp.Pages.MlosscalView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UCLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DataGridFiles = ((System.Windows.Controls.DataGrid)(target));
            
            #line 26 "..\..\..\Pages\Ml.xaml"
            this.DataGridFiles.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DataGridFiles_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\Pages\Ml.xaml"
            this.DataGridFiles.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.DataGridFiles_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbModel = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.submitDec = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\Pages\Ml.xaml"
            this.submitDec.Click += new System.Windows.RoutedEventHandler(this.submitDec_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.export2Excel = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\Pages\Ml.xaml"
            this.export2Excel.Click += new System.Windows.RoutedEventHandler(this.export2Excel_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.checkTimer = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.cmbTime = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.rdAll = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 9:
            this.rdNG = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 10:
            this.rdOK = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 11:
            this.cmbItem = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 12:
            this.btnUpload = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Pages\Ml.xaml"
            this.btnUpload.Click += new System.Windows.RoutedEventHandler(this.btnUpload_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.labelStock = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

