   M S B A M L   `   `   `  ����  hms#    repairhhp.Pages.HWVersionView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7  � RowDefinitions  5	   
   �� 5
      $��40 �6   �� 5      $
��300* �6   �� 5      $
��300* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $	��190��6   $	��215 �6.   �� 5      $	��190��6   �� 5      $��0 �6   .+�5   
   �� $��GATHERING DATA MACHINE��6   $��Center=�61   $
��Bold"�6L   $��40��6^   $��1��6j   $��Center=�6z   $	��215��6�   $��Center=�6�   $��Center=�6�   �� 5   
   $
��Line��6   $
��Left=�6   $��	-1,44,0,0q�6:   $��1��6M   $	��Top=�6Z   $��1��6r   $��23��6�   $��33��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ cmbLine��5      $
��Left=�6$   $��	34,44,0,0q�6?   $��1��6R   $	��Top=�6_   $	��172��6w   $��1��6�   $��26��6�    J� RenderTransformOrigin$ 0.463,0.462&�6�   �� 5   
   -   5         ��Name$ 	submitDec��5      $��SEARCH��6$   $��17,123,23,0q�65   $��1��6J   $	��Top=�6W   ���	#FF76A9F3$��25��6�   $��1��6�    � Cursor$
 Hand��6�   6�   �� 5   
   $��SEARCH CONDITION��6   $	��Top=�6+   $
��Bold"�6C   $��40��6U   $��1��6a   $
��Left=�6q   $	��215��6�   $��Center=�6�   $��Center=�6�   $��0,2,0,0q�6�   $��1��6�   �� 5   
   -   5         ��Name$ 
checkTimer��5      $��2��6'   $��
Refresh On��6;   $
��Left=�6P   $��9,85,0,0q�6k   $��1��6}   $	��Top=�6�    d� 	IsChecked$ FalseY�6�   $��18��6�   $��83��6�   �� 5   
   -   5        $ cmbTime��5      $��2��6$   $
��Left=�68   $��	97,82,0,0q�6S   $��1��6f   $	��Top=�6s   $��50��6�   $��26��6�   �� 5   
   $��Minutes��6   $
��Left=�6"   $��
147,84,0,0q�6=   $��1��6Q   $	��Top=�6^   $��1��6v   $��23��6�   $��58��6�   $��Center=�6�   �� 5   
   $��2��6   $#��Monitoring H/W Version RF Cal��6#   $��6,8,6,10q�6J   $	��1,2q�6\   ���	#CC7B8DB2$��2��6�   $��2��6�   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ dgRFCal��5      	   MinColumnWidth$		 100Q�6(   
   GridLinesVisibility$	
 All=�6=      
IsReadOnly . $��	0,-1,0,-2q�6i      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6|   $��15(�6�   5      �� 5   
   $��2��6   $��ALARM HW VERSION��6#   $��5,5,6,5q�6=   $	��1,2q�6N   ���	#CC7B8DB2$��2��6s   $��2��6�   .��5         -   5        $
 dgHW��5       $		 100Q�6%   $	
 All=�6:    . $��	0,-1,0,-2q�6f   $ * 6y   $��15(�6�   5!      5#      5$      