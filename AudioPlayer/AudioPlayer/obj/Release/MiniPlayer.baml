   M S B A M L   `   `   `  ����   AudioPlayer    AudioPlayer.MiniPlayer   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  Mini Player��$	��245��$	��245�����	#FF323232 � 
ResizeMode$ CanMinimize=� � Icon$ mainIcon.png��.��� .+��� -    ��Name$ SongsListBox��$
��Left=�$	��206��$	��Top=�$	��224�����	#FF646464
�������$y�Hidden=��� -    ��Name$ 
InfoButton��$��ℹ️��$
��Left=�$��	10,10,0,0q�$	��Top=�$��20��$��20�����	#FF6800FF
��������� $
��Left=�$��104.5��$��	60,10,0,0q�$	��Top=�$��104.5��$��note.png���� -   $ NormalPlayerButton��$
��🗖��$
��Left=�$��
194,10,0,0q�$	��Top=�$��20��
����������	#FF646464�� -   $ 
StopButton��$��⏹️��$
��Left=�$��
10,120,0,0q�$	��Top=�$��30��$��30��$��18(����	#FF590000
�������$��Center=�$��Center=��� -   $ 
PrevButton��$��⏮️��$
��Left=�$��
45,120,0,0q�$	��Top=�$��30��$��30��$��18(����	#FF1E1A70
��������� -   $ PlayPauseButton��$��⏯️��$
��Left=�$��
80,120,0,0q�$	��Top=�$��30��$��30��$��18(����	#FF12B900
��������� -   $ 
NextButton��$��⏭️��$
��Left=�$��115,120,0,0q�$	��Top=�$��30��$��30��$��18(����	#FF1E1A70
��������� -   $ RepeatButton��$��🔁️��$
��Left=�$��150,120,0,0q�$	��Top=�$��30��$��30��$��18(����	#FF9C9C9C
��������� -	   $ ShuffleButton��$��🔀️��$
��Left=�$��185,120,0,0q�$	��Top=�$��30��$��30��$��18(�
����������	#FF9C9C9C�� -
    ��Name$ PositionSlider��$
��Left=�$��
10,155,0,0q�$	��Top=�$	��135��$��20�� �� TickPlacement$ BottomRight=� � LargeChange$ 0Q� � Maximum$ 1Q�	 � SmallChange$	 0Q�
���   ����	#FF212121
���������.  {�.  �� -   
 ��Name$
 PositionLabel��$��00:00/00:00��$
��Left=�$��
10,180,0,0q�$	��Top=�$��26��
�������$	��135���� -   $ VolumeSlider��$
��Left=�$��150,155,0,0q�$	��Top=�$��65��$��20��$ 1Q�$	 0Q� �� TickFrequency$ 0Q�$ 0Q� � Value$	 0.5Q�$ BottomRight=�
���   ����	#FF212121
���������.  �� -   $
 VolumeLabel��$	��50%��$
��Left=�$��150,180,0,0q�$	��Top=�
�������$��26��$��64��