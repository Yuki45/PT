   M S B A M L   `   `   `  ����  hms     repairhhp.Pages.RackGIView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7  � RowDefinitions  5	   
   �� 5
      $��60 �6   �� 5      $
��100* �6   �� 5      $	��250 �6   �� 5      $
��100* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $��89 �6   �� 5      .+�5   
   �� $��MENU GENERAL ISSUE��6   $	��Top=�6-   $
��Bold"�6E   $��40��6W   $
��Left=�6c   $	��221��6~   $��Center=�6�   $��Center=�6�   $��0,10,0,0q�6�   $��1��6�   $��2��6�   �� 5   
   $��1��6   $��Data Stock Storage��6#   $��2,0,1,0q�6?   $	��1,2q�6P   ���	#CC7B8DB2$��1��6u   $��2��6�   �� 5   
   $
��Date��6   $��2��6   $
��Left=�6/   $��	165,8,0,0q�6J   $	��Top=�6]   $��40��6u   $��26��6�   �� 5   
   $
��Item��6   $��2��6   $
��Left=�6/   $��
165,34,0,0q�6J   $	��Top=�6^   $��56��6v   $��26��6�   �� 5   
   -   5         ��Name$ cmbItem��5      $��2��6$   $
��Left=�64   $��
213,31,0,0q�6O   $	��Top=�6c   $	��268��6{    J� RenderTransformOrigin$ 
0.35,0.692&�6�   $��26��6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dtStart��5      $
��Left=�6&   $��	213,3,0,0q�6A   $	��Top=�6T   $	��130��6l   $��2��6x   $��26��6�      SelectedDate 6�   �� *SellStartDate+ �� StringFormat$ {0:yyyy-MM-dd}�� � 	Resources 5      :  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType	 �� Property
 �� Value ��Name �� RelativeSource �� AncestorType��%�   '        �� 5      "  6   .�5      �� 	 � ��
 5      �� 5      .R�5   "   �� $ dtStartText��6*   ��5       �� *SelectedDate+ �� "  $ {0:yyyy-MM-dd}��5!      5#      5$      5%      5&        5'   
   -   5        $ dtEnd��5'      $
��Left=�6$   $��	351,3,0,0q�6?   $	��Top=�6R   $	��130��6j   $��2��6v   $��26��6�    6�   �� *SellStartDate+$ {0:yyyy-MM-dd}�� 5(      ��%�   '        �� 5)      "  6   .�5*      �� 	 � ��
 5+      �� 5,      .R�5-   "   �� $ dtStartText��6*   ��5.      �� *SelectedDate+ �� "  $ {0:yyyy-MM-dd}��5/      51      52      53      54      �� 55   
   -   5        $��Search��55      $��2��6"   $
��Left=�62   $��	486,3,0,0q�6M   $	��Top=�6`   $��75��6x   $��53��6�   6�   �� 56   
   $��1��6   $��Data Transaction��6#   $��2,0,1,0q�6=   $	��1,2q�6N   ���	#CC7B8DB2$��3��6s   $��2��6�   &   System.Windows.Controls.DataGrid  57   
   -   5          Name$ DataTRX��57         MinColumnWidth$	 100Q�6$      GridLinesVisibility$	 All=�69      
IsReadOnly . $��3,20,0,0q�6e      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6w   $��15(�6�   $��3��6�   $��3��6�     58   
   -   5        $ 	DataStock��58      $	 100Q�6&   $	 All=�6;    . $��3,20,4,2q�6g   $ * 6y   $��15(�6�   $��3��6�   $��1��6�   �� 59   
   -   5        $ snSlip��59      $
��Left=�6"   $��26��6=    �� TextWrapping$
 Wrap=�6I   $�� ��6]   $	��Top=�6f   $	��251��6~   $��6,17,0,0q�6�   $��2��6�   $��2��6�   6�   �� 5:   
   $��2��6   $��TRX No.*��6$   $
��Left=�67   $��6,17,0,0q�6R   $	��Top=�6d   $��2��6|   $��23��6�   $��59��6�   �� 5;   
   $��
Item Name*��6   $
��Left=�6%   $��6,49,0,0q�6@   $	��Top=�6R   $��2��6j   $��1��6w   $��23��6�   $��77��6�   �� 5<   
   $��2��6   $
��Qty*��6$   $
��Left=�63   $��6,84,0,0q�6N   $	��Top=�6`   $��2��6x   $��23��6�   $��59��6�   �� 5=   
   -	   5        $	 Qty��5=      $
��Left=�6   $��26��6:   $
 Wrap=�6F   $�� ��6Z   $	��Top=�6b   $��83��6z   $��7,81,0,0q�6�   $��2��6�   $��2��6�   �� 5>   
   -
   5         ��Name$ btnSave��5>      $
��Save��6"   $
��Left=�61   $��	6,218,0,0q�6L   $	��Top=�6_   $	��278��6w   $��2��6�   $��2��6�   $��27��6�   ���	#FF72DA376�   �� 5?   
   -   5        $ 	btnCancel��5?      $��CANCEL��6$   $
��Left=�65   $��305,218,0,0q�6P   $	��Top=�6e   $	��283��6}   $��2��6�   $��2��6�   $��27��6�   ���	#FFD41F1F6�   �� 5@   
   -   5        $ codeItem��5@      $
��Left=�6$   $��26��6?   $
 Wrap=�6K   $�� ��6_   $	��Top=�6g   $	��149��6   $��
263,50,0,0q�6�   $��2��6�    �� 
IsReadOnly . $��2��6�   �� 5A   
   $��	Stock Qty��6   $
��Left=�6$   $��
110,84,0,0q�6?   $	��Top=�6S   $��2��6k   $��23��6x   $��59��6�   $��2��6�   �� 5B   
   -   5        $ 	Qty_Stock��5B      $
��Left=�6%   $��26��6@   $
 Wrap=�6L    . $�� ��6r   $	��Top=�6z   $��83��6�   $��
174,81,0,0q�6�   $��2��6�   $��2��6�   $ -1.566,0.538&�6�   �� 5C   
   $��2��6   $��Loc Req*��6$   $
��Left=�67   $��	6,112,0,0q�6R   $	��Top=�6e   $��2��6}   $��23��6�   $��59��6�   �� 5D   
   -   5        $ locReq��5D      $
��Left=�6"   $��26��6=   $
 Wrap=�6I   $�� ��6]   $	��Top=�6e   $	��250��6}   $��	7,112,0,0q�6�   $��2��6�   $��2��6�   �� 5E   
   $��2��6   $��PIC Req*��6$   $
��Left=�67   $��	5,143,0,0q�6R   $	��Top=�6e   $��2��6}   $��23��6�   $��59��6�   �� 5F   
   -   5        $ PicReq��5F      $
��Left=�6"   $��26��6=   $
 Wrap=�6I   $�� ��6]   $	��Top=�6e   $	��250��6}   $��	7,143,0,0q�6�   $��2��6�   $��2��6�   �� 5G   
   $��2��6   $��Remark*��6$   $
��Left=�66   $��	5,174,0,0q�6Q   $	��Top=�6d   $��2��6|   $��23��6�   $��59��6�   �� 5H   
   -   5        $	 Rmk��5H      $
��Left=�6   $��26��6:   $
 Wrap=�6F   $�� ��6Z   $	��Top=�6b   $	��578��6z   $��	7,174,0,0q�6�   $��2��6�   $��2��6�   �� 5I   
   -   5        $ itemName��5I      $
��Left=�6$   $��26��6?   $
 Wrap=�6K   $�� ��6_   $	��Top=�6h   $	��251��6�   $��6,49,0,0q�6�   $��2��6�   $��2��6�    . 6�   5K      5L      