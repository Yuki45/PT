   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    hms-   'repairhhp.Pages.Popup.Popup_detail_rate   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5          � Title$  Detail History��5   	   $	��568��6    $	��800��6-      AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5   	    � 
ResizeMode$ NoResize=�5   	      ShowMaxRestoreButton .   � WindowStartupLocation$ CenterScreen=�5	   	   ���	#FFDDF2FF.��5
      � $��0,0,0,-2q�6    � ColumnDefinitions 5   
   �� 5      $	��14* �6   �� 5      $
��381* �6   �� 5      $
��399* �6    � RowDefinitions 5   
   �� 5      $	��250 �6   �� 5      $	��250 �6   �� 5      $��10 �6   .+�5   
   �� -   5         ��Name$ txtUN��5      $
��Left=�6!   $��23��6<   $��	0,196,0,0q�6H    �� TextWrapping$
 Wrap=�6[   $�� ��6o   $	��Top=�6w   $��49��6�   $y�Hidden=�6�   $��2��6�   �� 5   
   -   5        $ txtModel��5      $
��Left=�6$   $��23��6?   $��	0,196,0,0q�6K   $
 Wrap=�6^   $�� ��6r   $	��Top=�6z   $��49��6�   $y�Hidden=�6�   $��2��6�   �� 5   
   $��Detail Worst Defect��6   $��0q�60   $	��1,2q�6;   ���	#CC7B8DB2$��2��6`   .��5      &   System.Windows.Controls.DataGrid  -   5        	  Name$	 dgWorst��5      
   MinColumnWidth$	
 100Q�6(      GridLinesVisibility$	 All=�6=      
IsReadOnly . $��0,0,1,0q�6i      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6z   $��15(�6�   6�   5      �� 5   
   $��
Defect Qty��6   $��3,0,1,0q�6'   $	��1,2q�68   ���	#CC7B8DB2$��2��6]   .��5        -   5        $	 dgDefect��5      $	
 100Q�6)   $	 All=�6>    . $��0,0,1,0q�6j   $ * 6{   $��15(�6�   6�   5      �� 5   
   $��Detail Defect List��6   $��0,5,1,0q�6/   $	��1,2q�6@   ���	#CC7B8DB2$��1��6e   $��3��6r   .��5        -   5        $	 dgDetailPort��5      $	
 100Q�6-   $	 All=�6B    . $��0,0,1,0q�6n   $��15(�6�   5      �� 5    
   -   5         ��Name$ export2Excel��5       $��EXPORT TO EXCEL��6'   $��292,10,313,-24q�6A   $��2��6Y   $	��Top=�6f   ���	#FF76F38D$��25��6�   $��1��6�    � Cursor$
 Hand��6�   $��2��6�   5!      5"      