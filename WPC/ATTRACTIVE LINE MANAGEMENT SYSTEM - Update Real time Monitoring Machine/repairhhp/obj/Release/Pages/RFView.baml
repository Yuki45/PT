   M S B A M L   `   `   `  ���� p  lSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting   hms   repairhhp.Pages.RFView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     -   ����   �� EndPoint$  0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFE8F5FD�� � Offset$ 0Q�� $ 	#FFDDF2FF��$ 1Q�.��� $��0,0,0,0q����	#FFD3F7F7 � RowDefinitions �� $��40 ��� $	��400 ��� $
��200* � � ColumnDefinitions �� $��0 ��� $	��190��$	��215 ��� $	��190��$
��367* ��� $	��190��$
��367* ��� $��0 �.+��� $��GATHERING DATA MACHINE��$��Center=�$
��Bold"�$��40��$��1��$��Center=�$	��215��$��Center=�$��Center=�&   System.Windows.Controls.DataGrid  -     Name$ DataGridFiles��$��2��   MinColumnWidth$	 100Q�   GridLinesVisibility$	 All=�	   
IsReadOnly	 . $��1���� $��2��$��Gathering Data��$��0q�$��3��$	��1,2q����	#CC7B8DB2.��  -   $ 	dgHistory��$	 100Q�$	 All=�	 . $��0,0,1,-1q��� $��3��$��Worst Defect��$��4,0,1,0q�$��3��$	��1,2q����	#CC7B8DB2.��  -   $ dgWorst��$	 100Q�$	 All=�	 . $��	0,0,-4,-1q��� $��3��$��DEFECT CHART BY LINE��$��4,0,1,0q�$	��1,2q����	#CC7B8DB2$��1��.��>   8System.Windows.Controls.DataVisualization.Charting.Chart  -   
  Name$
 mcChart��
����İ�   Series. E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  ?   9System.Windows.Controls.DataVisualization.Charting.Series   Title$ 
Experience��$��0,0,2,0q�H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   IndependentValueBinding �� *Key+   DependentValueBinding �� *Value+(  "System.Windows.Controls.DatePicker  -     Name$ dtStart��$
��Left=�$��	38,42,0,0q�$��1��$	��Top=�$	��113��$��1��$��26��   SelectedDate �� *SellStartDate+ �� StringFormat$ {0:yyyy-MM-dd}�� � 	Resources :  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType �� Property �� Value ��Name �� RelativeSource �� AncestorType��%�   '        �� "  .���  � �� �� .R��� $ dtStartText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}���� $��Start��$
��Left=�$��3,42,0,0q�$��1��$	��Top=�$��1��$��23��$��33��$��Right=�$��RightToLeft=��� -    ��Name$ cmbTimeStart��$
��Left=�$��
156,42,0,0q�$��1��$	��Top=�$��54��$��1��$��26��  -   $ dtEnd��$
��Left=�$��	38,73,0,0q�$��1��$	��Top=�$	��113��$��1��$��26�� �� *SellStartDate+$ {0:yyyy-MM-dd}�� ��%�   '        �� "  .���  � �� �� .R��� $ 	dtEndText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}���� $	��End��$
��Left=�$��3,73,0,0q�$��1��$	��Top=�$��1��$��23��$��33��$��Center=��� -	   $ 
cmbtimeEnd��$
��Left=�$��
156,73,0,0q�$��1��$	��Top=�$��54��$��1��$��26���� $
��Line��$
��Left=�$��	3,111,0,0q�$��1��$	��Top=�$��1��$��23��$��33��$��Center=��� -
   $ cmbLine��$
��Left=�$��
38,111,0,0q�$��1��$	��Top=�$	��172��$��1��$��26�� J� RenderTransformOrigin$ 0.463,0.462&��� -    ��Name$ 	submitDec��$��SEARCH��$��21,156,19,0q�$��1��$	��Top=����	#FF76A9F3$��25��$��1�� � Cursor$
 Hand���� $��SEARCH CONDITION��$��Center=�$
��Bold"�$��40��$��1��$��Center=�$	��215��$��Center=�$��Center=�$��	0,2,0,358q�$��1���� -   $ export2Excel��$��EXPORT TO EXCEL��$��21,193,19,0q�$��1��$	��Top=����	#FF76F38D$��25��$��1��$
 Hand��