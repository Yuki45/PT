   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5      > :hms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:repairhhp.ViewModelrepairhhp.ViewModel 5         repairhhp.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      7	ViewModel!clr-namespace:repairhhp.ViewModel     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6T     � Title$	  HMS��5      $	��589��5      $	��316��5	         AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5
       � Icon$ Resources/helmet.png��5       � 
ResizeMode$ NoResize=�5         ShowIconOnTitleBar .    ShowMaxRestoreButton .   � WindowStartupLocation$ CenterScreen=�5      .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      � 	 � Color$	 	#FF50ACDD��6   
 � Offset$
 0Q�61   � 5      $	 	#FFF1F1F1��6   $
 1Q�61   5       � ColumnDefinitions 5   
   �� 5      $	��14* �6   �� 5      $��8* �6   �� 5      $	��31* �6   �� 5      $	��10* �6   �� 5      $	��34* �6   �� 5      $
��304* �6   �� 5      $	��85* �6   �� 5       $	��97* �6   .+�5"   
   �� $��198,120,0,0q�6   $
��Left=�5#      $	��Top=�5$      $��USER ID��5%      $��Arial monospaced for SAP)�5&      $��14(�5'      $
��Bold"�5(      $��5��6   ���	#FF21476C6&   �� 5)   
   $��174,168,0,0q�6   $
��Left=�5*      $	��Top=�5+      $��PASSWORD��5,      $��Arial monospaced for SAP)�5-      $��14(�5.      $
��Bold"�5/      $��5��6   ���	#FF21476C6&   �� 50   
   -   5         ��Name$ userid��50      $	��183��51      $��23��52      $��270,120,0,0q�53      $
��Left=�54      $	��Top=�55       �� TextWrapping$
 Wrap=�56      $��5��6   $��3��6(   G� 57   
   -   5         G�Name$ password��57      $	��183��58      $��270,168,0,0q�59      $
��Left=�5:      $	��Top=�5;      $��3��6   $��5��60   �� 5<   
   -   5        $��85��5<      $��48��5=      $��
64,199,0,0q�5>      $
��Left=�5?      $	��Top=�5@      $��Close Window��5A       � Cursor$
 Hand��5B      $��12(�5C       �� IsCancel . $��6��5D   %   $��2��65    � ToolTip$ Login Acount Please��6I   ��5E      �� 5F       �� 
TargetType" ��6"   .R�5G      ��  ��Name$ image��6   $?��9Resources/1487692750_preferences-desktop-cryptography.png��5H      $
��Fill=�5I      $��-13,0,17,-18q�6    �� Triggers 5J      P� 5K       P� Property �@J�IsMouseOver P� Value . . �5L      ��  �� 
TargetName$ image��6%    �� Property � �� �� Value$? 9Resources/1487692750_preferences-desktop-cryptography.png��6J   5M      5O      5Q      �� 5R   
   $	��160��6   $	��160��5S      $��6,96,0,0q�5T      $
��Left=�5U      $	��Top=�5V      $��Resources/worker.png��5W      $��4��6"   $
��Fill=�62   $��2��6A   �� 5X   
   $��	10,10,0,0q�6   $
��Left=�5Y      $	��Top=�5Z      $	��HMS��5[      $$��Tw Cen MT Condensed Extra Bold)�5\      $��56(�5]      $
��Bold"�5^      $��6��6   ��5_      �� 5`      $ 0.5,0&�6&   $ 0.5,1&�69   .��5a      � $
 0Q�6#   $	 	#FF435980��6.   � 5b      $
 1Q�6#   $	 	#FF143E87��6.   5c      5e      �� 5f   
   $��	26,32,0,0q�6   $
��Left=�5g      $	��Top=�5h      $	��HHP��5i      $��Malgun Gothic)�5j      $��14(�5k      $
��Bold"�5l      $��5��5m      ���	#FF21476C6   �� 5n   
   $��	66,32,0,0q�6   $
��Left=�5o      $	��Top=�5p      $��MANAGEMENT SYSTEM��5q      $��Malgun Gothic)�5r      $��14(�5s      $
��Bold"�5t      $��5��6   ���	#FF21476C6&   �� 5u   
   -   5         ��Name$	 xIp��5u      $��	7,263,0,0q�6   $
��Left=�5v      $	��Top=�5w      $��7��5x      ��6   �� *UserIP+6   �� 5y   
   $��
28,254,0,0q�6   $
��Left=�5z   
   $	��Top=�5{   
   $��Copyright @ SEIN HHP MTC��5|   
   $��Malgun Gothic)�5}   
   $��14(�5~   
   $
��Bold"�5   
   ���	#FF21476C$��4��63   $��2��6G   �� 5�   
   -   5        $��48��5�   
   $��278,199,26,0q�5�   
   $	��Top=�5�   
   $��Close Window��5�   
   $
 Hand��5�   
   $��12(�5�   
    . $��5��5�      $��2��6*   $ Guest Login Please��6S   ��5�      �� 5�      " ��6"   .R�5�      �� $ image��6   $��Resources/user.png��5�      $
��Fill=�5�      $��-13,0,17,-18q�6    5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ image��6%    � ��$ Resources/user.png��6J   5�      5�      5�      5�      5�      