   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    hms1   +repairhhp.Pages.Popup.Popup_Upload_Loss_Cal   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5          � Title$   Upload FINAL Loss Cal LOGS��5   	   $	��250��6,   $	��800��69      AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5   	    � 
ResizeMode$ NoResize=�5   	      ShowMaxRestoreButton .   � WindowStartupLocation$ CenterScreen=�5	   	   ���	#FFDDF2FF.��5
      �  � ColumnDefinitions 5   
   �� 5      $
��442* �6   �� 5      $	��43* �6   �� 5      $
��212* �6   �� 5      $	��97* �6    � RowDefinitions 5   
   �� 5      $	��250 �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      $��	0,32,0,28q�6%      GridLinesVisibility$	 All=�68   	   
IsReadOnly	 . $��4��6d   .@�5      �� 5      �� 5   
   -   5        
 ��Name$
 txtUN��5      $
��Left=�6!   $��23��6<   $��	0,196,0,0q�6H    �� TextWrapping$
 Wrap=�6[   $�� ��6o   $	��Top=�6w   $��49��6�   $y�Hidden=�6�   �� 5   
   -   5        $
 txtModel��5      $
��Left=�6$   $��23��6?   $��	0,196,0,0q�6K   $
 Wrap=�6^   $�� ��6r   $	��Top=�6z   $��49��6�   $y�Hidden=�6�   �� 5   
   $��Source Folder��6   $
��Left=�6(   $��13,4,0,0q�6C   $	��Top=�6U   �� 5   
   -   5        $
 	txtFolder��5       �� 
IsReadOnly .  $
��Left=�68   $��23��6S   $��90,3,0,0q�6_   $
 Wrap=�6q   $�� ��6�   $	��Top=�6�   $	��434��6�   $��3��6�   �� 5   
   -   5         ��Name$ 	submitDec��5      $��Upload��6$   $��
178,4,10,0q�65   $	��Top=�6I   ���	#FF76A9F3$��25��6x    � Cursor$
 Hand��6�   $��2��6�   $��2��6�   6�   �� 5   
   -   5        $ Source��5      $��Source��6!   $��	52,4,39,0q�62   $	��Top=�6E   ���	#FF76A9F3$��25��6t   $
 Hand��6�   $��2��6�   6�   5      5      