   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5       hms"   repairhhp.Pages.MlosscalView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f9Custom,http://metro.mahapps.com/winfx/xaml/controls  6�   -   5        ��5      �� 5	   
     �� EndPoint$  0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5
      �  � Color$ 	#FFE8F5FD��6    � Offset$ 0Q�6-   � 5      $ 	#FFDDF2FF��6   $ 1Q�6-   5      .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7 � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $
��400* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $	��190��6   $	��215 �6.   �� 5      $	��190��6   $
��367* �6.   �� 5      $��0 �6   .+�5   
   �� $��STANDART LOSS ANALYS��6   $	��Top=�6/   $
��Bold"�6G   $��40��6Y   $��1��6e   $	��215��6u   $��Center=�6�   $��Center=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGridFiles��5      $��2��6*      MinColumnWidth$	 100Q�6:      GridLinesVisibility$	 All=�6O   	   
IsReadOnly	 . $��1��6{   $��2��6�   6�   �� 5   
   $��Model��6   $
��Left=�6    $��0,47,0,0q�6;   $��1��6M   $	��Top=�6Z   $��1��6r   $��23��6�   $��48��6�   $��Center=�6�   �� 5   
   -   5        
 ��Name$
 cmbModel��5      $
��Left=�6%   $��	48,47,0,0q�6@   $��1��6S   $	��Top=�6`   $	��159��6x   $��1��6�   $��26��6�    J� RenderTransformOrigin$ 0.463,0.462&�6�   �� 5   
   -   5         ��Name$ 	submitDec��5      $��SEARCH��6$   $��21,190,19,0q�65   $��1��6J   $	��Top=�6W   ���	#FF76A9F3$��25��6�   $��1��6�    � Cursor$
 Hand��6�   6�   �� 5   
   -   5        $ export2Excel��5      $��EXPORT TO EXCEL RATE��6'   $��21,227,19,0q�6F   $��1��6[   $	��Top=�6h   ���	#FF76F38D$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5   
   -   5         ��Name$ 
checkTimer��5      $��2��6'   $��
Refresh On��6;   $
��Left=�6P   $��
13,144,0,0q�6k   $��1��6   $	��Top=�6�    d� 	IsChecked$ FalseY�6�   �� 5    
   -   5        $
 cmbTime��5       $��2��6$   $
��Left=�68   $��101,141,0,0q�6S   $��1��6h   $	��Top=�6u   $��50��6�   �� 5!   
   $��Minutes��6   $
��Left=�6"   $��151,143,0,0q�6=   $��1��6R   $	��Top=�6_   $��1��6w   $��23��6�   $��58��6�   $��Center=�6�   	� 5"   
   -   5         	�Name$ rdAll��5"      $��2��6%   $	��ALL��69   $
��Left=�6G   $��
48,114,0,0q�6b   $��1��6v   $	��Top=�6�   $
 TrueY�6�   	� 5#   
   -	   5        $
 rdNG��5#      $��2��6$   $��NG��68   $
��Left=�6E   $��109,114,0,0q�6`   $��1��6u   $	��Top=�6�   $ FalseY�6�   	� 5$   
   -
   5        $
 rdOK��5$      $��2��6$   $��OK��68   $
��Left=�6E   $��166,114,0,0q�6`   $��1��6u   $	��Top=�6�   $ FalseY�6�   �� 5%   
   $
��ITEM��6   $
��Left=�6   $��0,78,0,0q�6:   $��1��6L   $	��Top=�6Y   $��1��6q   $��23��6�   $��48��6�   $��Center=�6�   �� 5&   
   -   5        $
 cmbItem��5&      $
��Left=�6$   $��	48,78,0,0q�6?   $��1��6R   $	��Top=�6_   $	��159��6w   $��1��6�   $��26��6�   $ 0.463,0.462&�6�   �� 5'   
   -   5        $ 	btnUpload��5'      $��Upload Data��6$   $��21,264,19,0q�6:   $��1��6O   $	��Top=�6\   
���2͚�$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5(   
   $��2��6   $��SEARCH CONDITION��6$   $��Center=�6?   $
��Left=�6c   $��1��6~   $	��Top=�6�   $	��215��6�   $��30��6�   +  %MahApps.Metro.Controls.ControlsHelper   ContentCharacterCasing$ Upper=�6�   $
��Bold"�6�   �� 5)   
   -   5         ��Name$ 
labelStock��5)      $��-��6$   $��1��60   $
��Left=�6@   $��	214,8,0,0q�6[   $	��Top=�6n   ���	#FFF1DCC6$��26��6�   $��2��6�   $	��168��6�   5+      5,      