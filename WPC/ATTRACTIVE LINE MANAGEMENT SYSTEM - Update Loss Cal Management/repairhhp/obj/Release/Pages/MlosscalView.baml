   M S B A M L   `   `   `  ���� p  lSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting   hms"   repairhhp.Pages.MlosscalView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f9Custom,http://metro.mahapps.com/winfx/xaml/controls  -   ����   �� EndPoint$  0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFE8F5FD�� � Offset$ 0Q�� $ 	#FFDDF2FF��$ 1Q�.��� $��0,0,0,0q����	#FFD3F7F7 � RowDefinitions �� $��40 ��� $
��500* � � ColumnDefinitions �� $��0 ��� $	��190��$	��215 ��� $	��190��$
��500* ��� $��0 �.+��� $��STANDART LOSS ANALYS��$	��Top=�$
��Bold"�$��40��$��1��$	��215��$��Center=�$��Center=�&   System.Windows.Controls.DataGrid  -     Name$ DataGridFiles��$��2��   MinColumnWidth$	 100Q�   GridLinesVisibility$	 All=�	   
IsReadOnly	 . $��1���� $��Model��$
��Left=�$��0,99,0,0q�$��1��$	��Top=�$��1��$��23��$��48��$��Center=��� -   
 ��Name$
 cmbModel��$
��Left=�$��	48,99,0,0q�$��1��$	��Top=�$	��159��$��1��$��26�� J� RenderTransformOrigin$ 0.463,0.462&��� -    ��Name$ 	submitDec��$��SEARCH��$��21,233,19,0q�$��1��$	��Top=����	#FF76A9F3$��25��$��1�� � Cursor$
 Hand���� -   $ export2Excel��$��EXPORT TO EXCEL RATE��$��21,270,19,0q�$��1��$	��Top=����	#FF76F38D$��25��$��1��$
 Hand���� -    ��Name$ 
checkTimer��$��2��$��
Refresh On��$
��Left=�$��
13,196,0,0q�$��1��$	��Top=� d� 	IsChecked$ FalseY��� -   $
 cmbTime��$��2��$
��Left=�$��101,193,0,0q�$��1��$	��Top=�$��50���� $��Minutes��$
��Left=�$��151,195,0,0q�$��1��$	��Top=�$��1��$��23��$��58��$��Center=�	� -    	�Name$ rdAll��$��2��$	��ALL��$
��Left=�$��
45,162,0,0q�$��1��$	��Top=�$
 TrueY�	� -	   $
 rdNG��$��2��$��NG��$
��Left=�$��106,162,0,0q�$��1��$	��Top=�$ FalseY�	� -
   $
 rdOK��$��2��$��OK��$
��Left=�$��163,162,0,0q�$��1��$	��Top=�$ FalseY��� $
��ITEM��$
��Left=�$��	0,130,0,0q�$��1��$	��Top=�$��1��$��23��$��48��$��Center=��� -   $
 cmbItem��$
��Left=�$��
48,130,0,0q�$��1��$	��Top=�$	��159��$��1��$��26��$ 0.463,0.462&��� -   $ 	btnUpload��$��Upload Data��$��21,307,19,0q�$��1��$	��Top=�
���2͚�$��25��$��1��$
 Hand���� $��2��$��SEARCH CONDITION��$��Center=�$
��Left=�$��1��$	��Top=�$	��215��$��30��+  %MahApps.Metro.Controls.ControlsHelper   ContentCharacterCasing$ Upper=�$
��Bold"��� -    ��Name$ 
labelStock��$��-��$��2��$
��Left=�$��4,8,0,0q�$	��Top=����	#FFF1DCC6$��26��$	��168��(  "System.Windows.Controls.DatePicker  -     Name$ dtStart��$
��Left=�$��	49,34,0,0q�$��1��$	��Top=�$	��158��$��1��$��26��   SelectedDate �� *SellStartDate+ �� StringFormat$ {0:yyyy-MM-dd}�� � 	Resources :  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType �� Property �� Value ��Name �� RelativeSource �� AncestorType��%�   '        �� "  .���  � �� �� .R��� $ dtStartText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}���� $��Start��$
��Left=�$��	10,34,0,0q�$��1��$	��Top=�$��1��$��23��$��33��$��Right=�$��RightToLeft=�  -   $ dtEnd��$
��Left=�$��	49,65,0,0q�$��1��$	��Top=�$	��158��$��1��$��26�� �� *SellStartDate+$ {0:yyyy-MM-dd}�� ��%�   '        �� "  .���  � �� �� .R��� $ 	dtEndText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}���� $	��End��$
��Left=�$��	10,65,0,0q�$��1��$	��Top=�$��1��$��23��$��33��$��Center=�