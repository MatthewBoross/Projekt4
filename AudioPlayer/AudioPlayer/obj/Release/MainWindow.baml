   M S B A M L   `   `   `  ����   AudioPlayer    AudioPlayer.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  Audio Player��$	��560��$	��800�� � 
ResizeMode$ CanMinimize=����	#FF323232 � Icon$ mainIcon.png��.��� .+��� $��Audio Player��$
��Left=�$��	30,30,0,0q�$	��Top=�$��50��$	��190��$��30(�
��������� -    ��Name$ 
InfoButton��$��ℹ️��$
��Left=�$��
225,35,0,0q�$	��Top=�$��30��$��30�����	#FF6800FF
�������$��16(��� -   $ MiniPlayerButton��$
��🗕��$
��Left=�$��
260,35,0,0q�$	��Top=�$��30��
����������	#FF646464$��30���� -   $ PlayPauseButton��$	��⏯��$
��Left=�$��
30,100,0,0q�$	��Top=�$��50��$��50��$��30(�$��Center=�$��Center=����	#FF12B900
��������� -   $ 
StopButton��$	��⏹��$
��Left=�$��
90,100,0,0q�$	��Top=�$��50��$��50��$��30(����	#FF590000
��������� -   $ 
PrevButton��$	��⏮��$
��Left=�$��150,100,0,0q�$	��Top=�$��50��$��50��$��30(����	#FF1E1A70
��������� -   $ 
NextButton��$	��⏭��$
��Left=�$��210,100,0,0q�$	��Top=�$��50��$��50��$��30(����	#FF1E1A70
��������� -   $ RepeatButton��$��🔁️��$
��Left=�$��270,100,0,0q�$	��Top=�$��50��$��50��$��30(�&   System.Windows.Media.TextOptions   TextHintingMode$ Fixed=����	#FF9C9C9C
��������� -   $ ShuffleButton��$��🔀️��$
��Left=�$��330,100,0,0q�$	��Top=�$��50��$��50��
����������	#FF9C9C9C$��30(��� -	    ��Name$ PositionLabel��$��00:00/00:00��$
��Left=�$��
410,85,0,0q�$	��Top=�$��45��$	��140��$��Bottom=�$��20(�
��������� -
    ��Name$ PositionSlider��$
��Left=�$��410,130,0,0q�$	��Top=�$��20��$	��300�� � Maximum$ 1Q� �� TickPlacement$ BottomRight=�	 � LargeChange$	 0Q�
���   ����	#FF212121
���������.  {�.  
 � SmallChange$
 0Q��� $��Volume��$
��Left=�$��
635,30,0,0q�$	��Top=�$��40��$��90��$��20(�
�������$��Right=��� -   $ VolumeLabel��$	��50%��$
��Left=�$��
687,70,0,0q�$	��Top=�
��������� -   $ VolumeSlider��$
��Left=�$��
730,30,0,0q�$	��Top=�$��20�� �� Orientation$ Vertical=�$	��120�� � Value$	 0.5Q�$ 1Q�$ BottomRight=�$	 0Q�$
 0Q�
���   ����	#FF212121
���������.  �� $��Balance��$
��Left=�$��
435,30,0,0q�$	��Top=�$��25��$��50��
�������$��Bottom=��� $��L��$
��Left=�$��
410,55,0,0q�$	��Top=�
�������$��20��$��25���� -   $ BalanceSlider��$
��Left=�$��
435,60,0,0q�$	��Top=�$��20��$	��115��$ 1Q�$ BottomRight=�$	 0Q�
���   ����	#FF212121
���������.  $
 0Q� � Minimum$ -1Q��� $��R��$
��Left=�$��
555,55,0,0q�$	��Top=�
�������$��20��$��25���� -   $ BalanceResetButton��$��Reset��$
��Left=�$��
510,30,0,0q�$	��Top=�$��40��
����������	#FF646464$��20���� -   $ IsPlayingLabel��$��Stopped��$
��Left=�$��
30,155,0,0q�$	��Top=�$��40��$	��100��$��20(�
��������� -   $ IsRepeatedLabel��$��	No Repeat��$
��Left=�$��135,155,0,0q�$	��Top=�$��40��$	��185��$��20(�
�������$��Right=��� -   $ ShuffleLabel��$��
No Shuffle��$
��Left=�$��330,155,0,0q�$	��Top=�
�������$��20(�$��40��$	��185���� $��Current Playlist��$
��Left=�$��570,165,0,0q�$	��Top=�$��24(�$	��180��$��46��
�������$��Right=�$��Bottom=��� -    ��Name$ SongsListBox��$
��Left=�$	��200��$��
30,220,0,0q�$	��Top=�$	��720��$��10(����	#FF646464
��������� -   $ AddSongsButton��$��Add songs to playlist...��$
��Left=�$��
30,440,0,0q�$	��Top=�$	��135��$��50��
����������	#FF646464�� -   $ RemoveSongButton��$+��%Remove highlighted song from playlist��$
��Left=�$��185,440,0,0q�$	��Top=�$	��225��$��50��
����������	#FF646464�� -   $ ClearButton��$��Clear Playlist...��$
��Left=�$��430,440,0,0q�$	��Top=�$��90�����	#FF646464
�������$��50���� -   $ SavePlaylistButton��$��Save playlist...��$
��Left=�$��540,440,0,0q�$	��Top=�$��90��$��50��
����������	#FF646464�� -   $ LoadPlaylistButton��$��Load playlist...��$
��Left=�$��650,440,0,0q�$	��Top=�$	��100��$��50��
����������	#FF646464