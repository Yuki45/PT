   M S B A M L   `   `   `  ����  hms%    repairhhp.Pages.FilterAgentView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        .��5      � $��0,0,0,0q�6   ���	#FFD3F7F7  � RowDefinitions  5	   
   �� 5
      $��40 �6   �� 5      $
��138* �6    � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $	��190��6   $	��215 �6.   �� 5      $	��500��6   $
��500* �6.   �� 5      $��0 �6   .+�5   
   �� $��MANAGEMENT FILTER DEFECT��6   $��Center=�63   $
��Bold"�6N   $��40��6`   $��1��6l   $��Center=�6|   $	��215��6�   $��Center=�6�   $��Center=�6�   �� 5   
   $��1��6   $��LIST DATA DEFECT��6#   $��0,0,1,0q�6=   $	��1,2q�6N   ���	#CC7B8DB2$��2��6s   $��2��6�   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ 	dgSummary��5         MinColumnWidth$	 100Q�6*      GridLinesVisibility$	 All=�6?      
IsReadOnly . $��0,0,0,-1q�6k      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6}   $��15(�6�   6�   5      �� 5   
   $��Search��6   $��2��6!   $
��Left=�61   $��0,7,0,0q�6L   $	��Top=�6]   $��26��6u   $��52��6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ 
searchGrid��5      $��2��6&   $��26��66   $��55,7,0,0q�6B    �� TextWrapping$
 Wrap=�6T   $	��Top=�6h   $
��Left=�6�   $	��197��6�   �� 5   
   -   5        	 ��Name$	 	submitDec��5      $��Refresh��6$   $��2��66   $
��Left=�6F   $��	334,7,0,0q�6a   $	��Top=�6t   $��54��6�   $��25��6�   
 � Cursor$

 Hand��6�   6�   �� 5   
   -   5         ��Name$ cmbUse��5      $
��Left=�6#   $��	263,7,0,0q�6>   $	��Top=�6Q   $��63��6i   $��2��6t   $��26��6�    J� RenderTransformOrigin$ 0.463,0.462&�6�   �� 5   
   -   5         ��Name$ 
labelStock��5      $��-��6$   $��2��60   $
��Left=�6@   $��	406,6,0,0q�6[   $	��Top=�6n   ���	#FFF1DCC6$��26��6�   $ 0.589,0.605&�6�   $��84��6�   5      5       