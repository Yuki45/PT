   M S B A M L   `   `   `  ����   hms     repairhhp.Pages.RackGIView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   .��� $��0,0,0,0q����	#FFD3F7F7  � RowDefinitions  �� $��60 ��� $
��100* ��� $	��250 ��� $
��100* � � ColumnDefinitions �� $��0 ��� $��89 ��� .+��� $��MENU GENERAL ISSUE��$	��Top=�$
��Bold"�$��40��$
��Left=�$	��221��$��Center=�$��Center=�$��0,10,0,0q�$��1��$��2���� $��1��$��Data Stock Storage��$��2,0,1,0q�$	��1,2q����	#CC7B8DB2$��1��$��2���� $
��Date��$��2��$
��Left=�$��	165,8,0,0q�$	��Top=�$��40��$��26���� $
��Item��$��2��$
��Left=�$��
165,34,0,0q�$	��Top=�$��56��$��26���� -    ��Name$ cmbItem��$��2��$
��Left=�$��
213,31,0,0q�$	��Top=�$	��268�� J� RenderTransformOrigin$ 
0.35,0.692&�$��26��(  "System.Windows.Controls.DatePicker  -     Name$ dtStart��$
��Left=�$��	213,3,0,0q�$	��Top=�$	��130��$��2��$��26��   SelectedDate �� *SellStartDate+ �� StringFormat$ {0:yyyy-MM-dd}�� � 	Resources :  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType	 �� Property
 �� Value ��Name �� RelativeSource �� AncestorType��%�   '        �� "  .��� 	 � ��
 �� .R��� $ dtStartText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}��  -   $ dtEnd��$
��Left=�$��	351,3,0,0q�$	��Top=�$	��130��$��2��$��26�� �� *SellStartDate+$ {0:yyyy-MM-dd}�� ��%�   '        �� "  .��� 	 � ��
 �� .R��� $ dtStartText������ *SelectedDate+ �� "  $ {0:yyyy-MM-dd}���� $��Search��$��2��$
��Left=�$��	486,3,0,0q�$	��Top=�$��75��$��53���� $��1��$��Data Transaction��$��2,0,1,0q�$	��1,2q����	#CC7B8DB2$��3��$��2��&   System.Windows.Controls.DataGrid  -     Name$ DataTRX��   MinColumnWidth$	 100Q�   GridLinesVisibility$	 All=�   
IsReadOnly . $��3,20,0,0q�   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * $��15(�$��3��$��3��  -   $ 	DataStock��$	 100Q�$	 All=� . $��3,20,4,2q�$ * $��15(�$��3��$��1���� -   $ snSlip��$
��Left=�$��26�� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��251��$��6,17,0,0q�$��2��$��2���� $��2��$��TRX No.*��$
��Left=�$��6,17,0,0q�$	��Top=�$��2��$��23��$��59���� $��
Item Name*��$
��Left=�$��6,49,0,0q�$	��Top=�$��2��$��1��$��23��$��77���� $��2��$
��Qty*��$
��Left=�$��6,84,0,0q�$	��Top=�$��2��$��23��$��59���� -   $	 Qty��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$��83��$��7,81,0,0q�$��2��$��2���� -	    ��Name$ btnSave��$
��Save��$
��Left=�$��	6,218,0,0q�$	��Top=�$	��278��$��2��$��2��$��27�����	#FF72DA37�� -
   $ 	btnCancel��$��CANCEL��$
��Left=�$��305,218,0,0q�$	��Top=�$	��283��$��2��$��2��$��27�����	#FFD41F1F�� -   $ codeItem��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$	��149��$��
263,50,0,0q�$��2�� �� 
IsReadOnly . $��2���� $��	Stock Qty��$
��Left=�$��
110,84,0,0q�$	��Top=�$��2��$��23��$��59��$��2���� -   $ 	Qty_Stock��$
��Left=�$��26��$
 Wrap=� . $�� ��$	��Top=�$��83��$��
174,81,0,0q�$��2��$��2��$ -1.566,0.538&��� $��2��$��Loc Req*��$
��Left=�$��	6,112,0,0q�$	��Top=�$��2��$��23��$��59���� -   $ locReq��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$	��250��$��	7,112,0,0q�$��2��$��2���� $��2��$��PIC Req*��$
��Left=�$��	5,143,0,0q�$	��Top=�$��2��$��23��$��59���� -   $ PicReq��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$	��250��$��	7,143,0,0q�$��2��$��2���� $��2��$��Remark*��$
��Left=�$��	5,174,0,0q�$	��Top=�$��2��$��23��$��59���� -   $	 Rmk��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$	��578��$��	7,174,0,0q�$��2��$��2���� -   $ itemName��$
��Left=�$��26��$
 Wrap=�$�� ��$	��Top=�$	��251��$��6,49,0,0q�$��2��$��2�� . 