   M S B A M L   `   `   `  ���� U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  > :hms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:repairhhp.ViewModelrepairhhp.ViewModel    repairhhp.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     7	ViewModel!clr-namespace:repairhhp.ViewModel     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$	  HMS��$	��589��$	��316��   AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �    � Icon$ Resources/helmet.png�� � 
ResizeMode$ NoResize=�   ShowIconOnTitleBar .    ShowMaxRestoreButton .   � WindowStartupLocation$ CenterScreen=�.��� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.��� 	 � Color$	 	#FF50ACDD��
 � Offset$
 0Q�� $	 	#FFF1F1F1��$
 1Q� � ColumnDefinitions �� $	��14* ��� $��8* ��� $	��31* ��� $	��10* ��� $	��34* ��� $
��304* ��� $	��85* ��� $	��97* �.+��� $��198,120,0,0q�$
��Left=�$	��Top=�$��USER ID��$��Arial monospaced for SAP)�$��14(�$
��Bold"�$��5�����	#FF21476C�� $��174,168,0,0q�$
��Left=�$	��Top=�$��PASSWORD��$��Arial monospaced for SAP)�$��14(�$
��Bold"�$��5�����	#FF21476C�� -    ��Name$ userid��$	��183��$��23��$��270,120,0,0q�$
��Left=�$	��Top=� �� TextWrapping$
 Wrap=�$��5��$��3��G� -    G�Name$ password��$	��183��$��270,168,0,0q�$
��Left=�$	��Top=�$��3��$��5���� -   $��85��$��48��$��
64,199,0,0q�$
��Left=�$	��Top=�$��Close Window�� � Cursor$
 Hand��$��12(� �� IsCancel . $��6��$��2�� � ToolTip$ Login Acount Please������  �� 
TargetType" ��.R���  ��Name$ image��$?��9Resources/1487692750_preferences-desktop-cryptography.png��$
��Fill=�$��-13,0,17,-18q� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  �� 
TargetName$ image�� �� Property � �� �� Value$? 9Resources/1487692750_preferences-desktop-cryptography.png���� $	��160��$	��160��$��6,96,0,0q�$
��Left=�$	��Top=�$��Resources/worker.png��$��4��$
��Fill=�$��2���� $��	10,10,0,0q�$
��Left=�$	��Top=�$	��HMS��$$��Tw Cen MT Condensed Extra Bold)�$��56(�$
��Bold"�$��6������ $ 0.5,0&�$ 0.5,1&�.��� $
 0Q�$	 	#FF435980��� $
 1Q�$	 	#FF143E87���� $��	26,32,0,0q�$
��Left=�$	��Top=�$	��HHP��$��Malgun Gothic)�$��14(�$
��Bold"�$��5�����	#FF21476C�� $��	66,32,0,0q�$
��Left=�$	��Top=�$��MANAGEMENT SYSTEM��$��Malgun Gothic)�$��14(�$
��Bold"�$��5�����	#FF21476C�� -    ��Name$	 xIp��$��	7,263,0,0q�$
��Left=�$	��Top=�$��7������ *UserIP+�� $��
28,254,0,0q�$
��Left=�$	��Top=�$��Copyright @ SEIN HHP MTC��$��Malgun Gothic)�$��14(�$
��Bold"����	#FF21476C$��4��$��2���� -   $��48��$��278,199,26,0q�$	��Top=�$��Close Window��$
 Hand��$��12(� . $��5��$��2��$ Guest Login Please������ " ��.R��� $ image��$��Resources/user.png��$
��Fill=�$��-13,0,17,-18q� P�  �@J�IsMouseOver . . ��� $ image�� � ��$ Resources/user.png��