   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5       hms   repairhhp.Pages.RFView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      -   5        ��5      �� 5	   
     �� EndPoint$  0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5
      �  � Color$ 	#FFE8F5FD��6    � Offset$ 0Q�6-   � 5      $ 	#FFDDF2FF��6   $ 1Q�6-   5      .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7 � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $	��400 �6   �� 5      $
��200* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $	��190��6   $	��215 �6.   �� 5      $	��190��6   $
��367* �6.   �� 5      $	��190��6   $
��367* �6.   �� 5      $��0 �6   .+�5   
   �� $��GATHERING DATA MACHINE��6   $��Center=�61   $
��Bold"�6L   $��40��6^   $��1��6j   $��Center=�6z   $	��215��6�   $��Center=�6�   $��Center=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGridFiles��5      $��2��6*      MinColumnWidth$	 100Q�6:      GridLinesVisibility$	 All=�6O   	   
IsReadOnly	 . $��1��6{   6�   �� 5   
   $��2��6   $��Gathering Data��6#   $��0q�6;   $��3��6F   $	��1,2q�6S   ���	#CC7B8DB2.��5        -   5        $ 	dgHistory��5      $	 100Q�6*   $	 All=�6?   	 . $��0,0,1,-1q�6k   5      �� 5    
   $��3��6   $��Worst Defect��6#   $��4,0,1,0q�69   $��3��6J   $	��1,2q�6W   ���	#CC7B8DB2.��5!        -   5        $ dgWorst��5!      $	 100Q�6(   $	 All=�6=   	 . $��	0,0,-4,-1q�6i   6|   5"      �� 5#   
   $��3��6   $��DEFECT CHART BY LINE��6#   $��4,0,1,0q�6A   $	��1,2q�6R   ���	#CC7B8DB2$��1��6w   .��5$      >   8System.Windows.Controls.DataVisualization.Charting.Chart  -   5        
  Name$
 mcChart��5$   $   
����İ�$��0,0,0,-1q�5%   '      Series. 5'      E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   IndependentValueBinding 5(      �� *Key+   DependentValueBinding 5)      �� *Value+5*      5+      (  "System.Windows.Controls.DatePicker  5-   
   -   5          Name$ dtStart��5-      $
��Left=�6&   $��	38,42,0,0q�6A   $��1��6T   $	��Top=�6a   $	��113��6y   $��1��6�   $��26��6�      SelectedDate 6�   �� *SellStartDate+ �� StringFormat$ {0:yyyy-MM-dd}�� � 	Resources 5.      :  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType �� Property �� Value ��Name �� RelativeSource �� AncestorType��%�   '        �� 5/      "  6   .�50      ��  � �� 51      �� 52      .R�53   "   �� $ dtStartText��6*   ��54   
   �� *SelectedDate+ �� "  $ {0:yyyy-MM-dd}��55      57      58      59      5:      �� 5<   
   $��Start��6   $
��Left=�6    $��3,42,0,0q�6;   $��1��6M   $	��Top=�6Z   $��1��6r   $��23��6�   $��33��6�   $��Right=�6�   $��RightToLeft=�6�   �� 5=   
   -   5         ��Name$ cmbTimeStart��5=      $
��Left=�6)   $��
156,42,0,0q�6D   $��1��6X   $	��Top=�6e   $��54��6}   $��1��6�   $��26��6�     5@   
   -   5        $ dtEnd��5@      $
��Left=�6$   $��	38,73,0,0q�6?   $��1��6R   $	��Top=�6_   $	��113��6w   $��1��6�   $��26��6�    6�   �� *SellStartDate+$ {0:yyyy-MM-dd}�� 5A      ��%�   '        �� 5B      "  6   .�5C      ��  � �� 5D      �� 5E      .R�5F   "   �� $ 	dtEndText��6*   ��5G   
   �� *SelectedDate+ �� "  $ {0:yyyy-MM-dd}��5H      5J      5K      5L      5M      �� 5N   
   $	��End��6   $
��Left=�6   $��3,73,0,0q�69   $��1��6K   $	��Top=�6X   $��1��6p   $��23��6�   $��33��6�   $��Center=�6�   �� 5O   
   -	   5        $ 
cmbtimeEnd��5O      $
��Left=�6(   $��
156,73,0,0q�6C   $��1��6W   $	��Top=�6d   $��54��6|   $��1��6�   $��26��6�   �� 5P   
   $
��Line��6   $
��Left=�6   $��	3,111,0,0q�6:   $��1��6M   $	��Top=�6Z   $��1��6r   $��23��6�   $��33��6�   $��Center=�6�   �� 5Q   
   -
   5        $ cmbLine��5Q      $
��Left=�6$   $��
38,111,0,0q�6?   $��1��6S   $	��Top=�6`   $	��172��6x   $��1��6�   $��26��6�    J� RenderTransformOrigin$ 0.463,0.462&�6�   �� 5R   
   -   5         ��Name$ 	submitDec��5R      $��SEARCH��6$   $��21,190,19,0q�65   $��1��6J   $	��Top=�6W   ���	#FF76A9F3$��25��6�   $��1��6�    � Cursor$
 Hand��6�   6�   �� 5S   
   $��SEARCH CONDITION��6   $��Center=�6+   $
��Bold"�6F   $��40��6X   $��1��6d   $��Center=�6t   $	��215��6�   $��Center=�6�   $��Center=�6�   $��	0,2,0,358q�6�   $��1��6�   �� 5T   
   -   5        $ export2Excel��5T      $��EXPORT TO EXCEL RATE��6'   $��21,227,19,0q�6F   $��1��6[   $	��Top=�6h   ���	#FF76F38D$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5U   
   -   5        $ export2Exceldata��5U      $��EXPORT TO EXCEL HISTORY��6+   $��21,262,19,0q�6M   $��1��6b   $	��Top=�6o   ���	#FF76F38D$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5V   
   -   5        $ export2Excelworst��5V      $��EXPORT TO EXCEL WORST��6,   $��21,298,19,0q�6L   $��1��6a   $	��Top=�6n   ���	#FF76F38D$��25��6�   $��1��6�   $
 Hand��6�   6�   �� 5W   
   -   5         ��Name$ 
checkTimer��5W      $��2��6'   $��
Refresh On��6;   $
��Left=�6P   $��
13,152,0,0q�6k   $��1��6   $	��Top=�6�    d� 	IsChecked$ FalseY�6�   �� 5X   
   -   5        $ cmbTime��5X      $��2��6$   $
��Left=�68   $��101,149,0,0q�6S   $��1��6h   $	��Top=�6u   $��50��6�   �� 5Y   
   $��Minutes��6   $
��Left=�6"   $��151,151,0,0q�6=   $��1��6R   $	��Top=�6_   $��1��6w   $��23��6�   $��58��6�   $��Center=�6�   5[      5\      