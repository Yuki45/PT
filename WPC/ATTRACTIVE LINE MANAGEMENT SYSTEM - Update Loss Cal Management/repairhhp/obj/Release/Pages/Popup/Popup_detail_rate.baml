   M S B A M L   `   `   `  ���� U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   hms-   'repairhhp.Pages.Popup.Popup_detail_rate   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     -     � Title$  Detail History��$	��568��$	��800��   AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �    � 
ResizeMode$ NoResize=�   ShowMaxRestoreButton .   � WindowStartupLocation$ CenterScreen=����	#FFDDF2FF.��� $��0,0,0,-2q� � ColumnDefinitions �� $	��14* ��� $
��381* ��� $
��399* � � RowDefinitions �� $	��250 ��� $	��250 ��� $��10 �.+��� -    ��Name$ txtUN��$
��Left=�$��23��$��	0,196,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$��49��$y�Hidden=�$��2���� -   $ txtModel��$
��Left=�$��23��$��	0,196,0,0q�$
 Wrap=�$�� ��$	��Top=�$��49��$y�Hidden=�$��2���� $��Detail Worst Defect��$��0q�$	��1,2q����	#CC7B8DB2$��2��.��&   System.Windows.Controls.DataGrid  -   	  Name$	 dgWorst��
   MinColumnWidth$	
 100Q�   GridLinesVisibility$	 All=�   
IsReadOnly . $��0,0,1,0q�   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * $��15(��� $��
Defect Qty��$��3,0,1,0q�$	��1,2q����	#CC7B8DB2$��2��.��  -   $	 dgDefect��$	
 100Q�$	 All=� . $��0,0,1,0q�$ * $��15(��� $��Detail Defect List��$��0,5,1,0q�$	��1,2q����	#CC7B8DB2$��1��$��3��.��  -   $	 dgDetailPort��$	
 100Q�$	 All=� . $��0,0,1,0q�$��15(��� -    ��Name$ export2Excel��$��EXPORT TO EXCEL��$��292,10,313,-24q�$��2��$	��Top=����	#FF76F38D$��25��$��1�� � Cursor$
 Hand��$��2��