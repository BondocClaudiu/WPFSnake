   M S B A M L   `   `   `  ����C  ?WpfSnake, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WpfSnakeWpfSnake  5   	       WpfSnake.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:WpfSnake     5   	   -   5          � Title$  	WPF Snake��5   	   $	��450��6   $	��780��6(    � Icon$ ./Images/Snake.png��64    � 
ResizeMode$ 	CanResize=�6N   $��Center=�6~   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��5* �6   �� 5      $��* �6   .+�5   
   �� 
���   �$��2q�6%   ._�5      �� -   5        $��paintCanvas��5   %   $��0��6   $	��400��68   $	��600��6E   ��5      �� 5       �� ImageSource$ ./Images/background.bmp��6!   6I   5      5      �� 5   
   $��1��6   $��Center=�6%   .+�5      �� $
��Wrap=�6   $	��Top=�6,    �� Padding$ 2q�6D   $��	2,10,2,10q�6P   @>Use arrow keys to move around, and P to pause/unpause the game6�   �� 5      $��WPF Snake V1.0��6   $��Calibri)�6-   $��Italic$�6B    J� Opacity$ 20Q�6U   $��2,10,2,0q�6b   6v   �� 5      $��Score:��6   $��30(�6%   
�������$��Italic$�6J   $
��Bold"�6]   $��2,0,2,0q�6o   6�   �� 5      -   5        $��
ScoreLabel��5      $��25(�6&   $��Right=�64   $��Italic$�6P   $��0��6c   6q   5      �� 5!   
   -   5        $��PausedLabel��5!      $��Center=�6#   $��Center=�6@   $��50(�6[   $��Heavy"�6i   $y�Hidden=�6|   .��6�   Paused6�   5"      5#      