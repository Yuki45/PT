   M S B A M L   `   `   `  ����   hms"    repairhhp.Pages.MdmCauseView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   ����   �� EndPoint$  0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFE8F5FD�� � Offset$ 0Q�� $ 	#FFDDF2FF��$ 1Q�.��� $��0,0,0,0q� � RowDefinitions �� $��0 ��� $��40 ��� �� $��0* � � ColumnDefinitions �� $��0 ��� $
��190* �$	��190���� $
��190* �$	��190���� $
��127* �$	��127���� $
��127* �$	��127���� $
��127* �$	��127���� $��0 �.+��� $��Manage Cause��$
��Left=�$��4,9,0,0q�$	��Top=�$
��Bold"�$��26��$��96��$��1��$��1���� $��Search��$��4��$��Right=�$��0,9,0,0q�$	��Top=�$��1��$��26��$��45���� -    ��Name$ 
searchGrid��$��5��$��26��$��	5,10,67,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��1���� -   $��Refresh��$��5��$��Right=�$��0,9,10,0q�$	��Top=�$��52��$��1��$��25�� � Cursor$
 Hand��&   System.Windows.Controls.DataGrid  -   	  Name$	 DataGridFiles��$��1��$��	10,0,0,10q�$��2��$��2��
   MinColumnWidth$	
 100Q�   GridLinesVisibility$	 All=�   
IsReadOnly . �� $��3��$��SET UP��$��6,0,9,10q�$��2��$	��1,2q���. $��3��.���� $��0,0,0,-1q�
��������� -    ��Name$ clearGI��$��CLEAR��$��14,147,10,0q�$��2��$	��Top=�$��25��$��3��$
 Hand���� -   $ 	submitDec��$��SUBMIT��$��
0,147,23,0q�$��2��$	��Top=����	#FF76F38D$��25��$��5��$
 Hand���� -    ��Name$ 
labelStock��$��-��$��1��$
��Left=�$��	100,7,0,0q�$��1��$	��Top=����	#FFF1DCC6$��26���� $��ID��$
��Left=�$��	14,40,0,0q�$	��Top=�$��36��$��25��$��2��$��3���� -   $ txtID��$��3��$��26��$��100,40,23,0q�$��2��$	��Top=� �� 
IsReadOnly . $��3���� $��Description��$
��Left=�$��	14,91,0,0q�$	��Top=�$��78��$��25��$��2��$��3���� -	   $ txtDesc��$��3��$��26��$��100,91,23,0q�$��2��$	��Top=�$��3���� -
   $ deleteCause��$��DELETE��$��	2,147,8,0q�$��2��$	��Top=�$��25��$��4��$
 Hand�����	#FFF9D4A7