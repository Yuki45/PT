   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5       hms%   repairhhp.Pages.MtrackingWSView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f9Custom,http://metro.mahapps.com/winfx/xaml/controls  6�   -   5        ��5      �� 5	   
     �� EndPoint$  0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5
      �  � Color$ 	#FFE8F5FD��6    � Offset$ 0Q�6-   � 5      $ 	#FFDDF2FF��6   $ 1Q�6-   5      .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7 � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $
��500* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $	��190��6   $	��215 �6.   �� 5      $	��190��6   $
��367* �6.   �� 5      $��0 �6   .+�5   
   �� $��HISTORY WEIGHT SCALE��6   $	��Top=�6/   $
��Bold"�6G   $��40��6Y   $��1��6e   $	��215��6u   $��Center=�6�   $��Center=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGridFiles��5      $��2��6*      MinColumnWidth$	 100Q�6:      GridLinesVisibility$	 All=�6O   	   
IsReadOnly	 . $��1��6{   $��2��6�   6�   �� 5   
   -   5        
 ��Name$
 	submitDec��5      $��SEARCH��6$   $��	35,68,5,0q�65   $��1��6H   $	��Top=�6U   ���	#FF76A9F3$��25��6�   $��1��6�    � Cursor$
 Hand��6�   6�   �� 5   
   -   5        $
 export2Excel��5      $��EXPORT TO EXCEL RATE��6'   $��
35,105,5,0q�6F   $��1��6Z   $	��Top=�6g   ���	#FF76F38D$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5   
   $��SN NO��6   $
��Left=�6    $��0,34,0,0q�6;   $��1��6M   $	��Top=�6Z   $��1��6r   $��23��6�   $��48��6�   $��Center=�6�   �� 5   
   $��2��6   $��SEARCH CONDITION��6$   $��Center=�6?   $
��Left=�6c   $��1��6~   $	��Top=�6�   $	��215��6�   $��30��6�   +  %MahApps.Metro.Controls.ControlsHelper   ContentCharacterCasing$ Upper=�6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ 
labelStock��5      $��-��6$   $��1��60   $
��Left=�6@   $��	214,8,0,0q�6[   $	��Top=�6n   ���	#FFF1DCC6$��26��6�   $��2��6�   $	��168��6�   �� 5    
   -   5         ��Name$ txt_un��5       $��2��6"   $
��Left=�66   $��23��6Q   $��	48,34,0,0q�6]   $��1��6p    �� TextWrapping$
 Wrap=�6}   $�� ��6�   $	��Top=�6�   $	��157��6�   5"      5#      