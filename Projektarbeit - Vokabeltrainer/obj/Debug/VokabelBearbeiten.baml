   M S B A M L   `   `   `  ����  ProjektarbeitVokabeltrainer3    -ProjektarbeitVokabeltrainer.VokabelBearbeiten   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � -   5        $��mainGrid��5      .+�5	   
   &   System.Windows.Controls.DataGrid  -   5        $��dataGrid��5	          AutoGenerateColumns  .     CanUserAddRows .     
IsReadOnly .    CanUserDeleteRows .     CanUserResizeColumns .     CanUserReorderColumns .  $��10,10,10,53q�6�      SelectionMode$ Single=�6�   ��6�   Z�    Columns 5
      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Deutsch��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 66   �� 
 �� Path$
 Deutsch�  5      $ Deutsch alternativ��6%   	 6A   �� $
 Deutsch2�  5      $ Englisch��6%   	 67   �� $
 Englisch�  5      $ Englisch alternativ��6%   	 6B   �� $
 	Englisch2�  5      $
 Fach��6%   	 63   �� $

 Fach� � ContextMenu 5      �� 5      .@�5      m� -   5        $��
Bearbeiten��5      63   m� 5      -   5        $��Löschen��5      60   5      5      �� 5   
   -   5         �� 	IsDefault . $��Zurück��5   6   $
��Left=�6G   $��
10,284,0,0q�6b   $	��Top=�6v   $	��474��6�   $��5��6�   5      5      