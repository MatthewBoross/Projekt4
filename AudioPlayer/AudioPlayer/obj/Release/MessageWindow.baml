   M S B A M L   `   `   `  ����   AudioPlayer    AudioPlayer.MessageWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$   ��$	��160��$	��420��$	��750��$	��750�����	#FF323232 � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .   � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � Topmost . .���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $��* �.+��� $��0��$��0��$��2������ 	 �� 
StartPoint$		 0,0&�
 �� EndPoint$	
 0,1&� �� Opacity$	 0.5Q�.���  � Color$ 	#FF4F4F4F�� � Offset$	 0.0Q�� $��0��$��2��$��40��.+��� -   $��MessageTitle��$��15,10q�$��16(�$��Center=�
��������� -   $	��img��$��5q�$��1��$��0��$��50��$��50��$
��Fill=��� -   $��txtMsg��$��10,27,25,27q�$��Center=�$
��Wrap=�$��1��$��1��$��14(� �� 
LineHeight$ 20��� $��2��$��2��$��0��.+���  �� Orientation$ 
Horizontal=�$��Right=�$��0,-10,10,10q�.+��� -    ��Name$ 	btnCancel��$	��3,5q�$��Cancel��$��70��$��35��
����������	#FF590000$��14(�$��Center=�$��Stretch=�$��Center=�$��Center=�
���   ��� -   $ btnYes��$	��Yes��$	��3,5q�$��70��$��35��
�������$��14(�$��Center=�$��Stretch=�$��Center=�$��Center=����	#FF12B900
���   ��� -   $ btnNo��$��No��$	��3,5q�$��70��$��35��
�������$��14(����	#FF590000$��Center=�$��Stretch=�$��Center=�$��Center=�
���   ��� -   $ btnOk��$��OK��$	��3,5q�$��70��$��35��
�������$��14(����	#FF12B900$��Center=�$��Stretch=�$��Center=�$��Center=�
���   �