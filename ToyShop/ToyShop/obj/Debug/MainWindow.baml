   M S B A M L   `   `   `  ����B  >ToyShop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ToyShopToyShop  5   	       ToyShop.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:ToyShop     5   	     � Title$#  Магазин игрушек��5   	    � Icon$ Assets/toy_icon.png��6!   $	��650��6<   $	��950��6I    � 
ResizeMode$ CanMinimize=�6U   ��5	      �� 5
   
    �� ImageSource$ Assets/toy_background.png��6   .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $	��69* �6   �� 5      $��50 �6   .+�5   
   � $��0��6      ColorSecondary#��[  6   � 5   
   $��2��6   #��[  6   �� 5   
   $��Assets/toy_logo.png��6   $	��188��6-   $��5,5,0,5q�69   $
��Left=�6J   6g   �� 5   
   -   5        $��BtnBack��5   <   $��
Назад��6   $
��left=�6!   $��2��6a    � 	Resources 5       �� 
TargetType �� Property �� Value��%_   '��       �� 5      " ��6   .�5      ��  �@��CornerRadius$ 10��65   5      5      5      �� 5   
     BlockHeader#��[ 6   $��0,10,0,0q�6t   $	��Top=�6�   ��69   �� 	 �� ElementName$	 	FrameMain��
 �� Path$
 Content.Title�6�    � 5   
   -   5          �Name$ 	FrameMain��5        � NavigationUIVisibility$ Hidden=�6#   $��1��6C   5      5       