   M S B A M L   `   `   `  ����V  RFileBatchRenamerPerformance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:FileBatchRenamerPerformanceFileBatchRenamerPerformance  5   	   ,    &FileBatchRenamerPerformance.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:FileBatchRenamerPerformance     5   	   $��16(�5      $��Segoe UI Light)�6(     � 
ResizeMode$  NoResize=�5   	    � Title$ Image Sequence Renamer��5	   	   $	��430��6(   $��889.327��65   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $��277.264 �6   �� 5      $��62.736 �6   �� 5      $
��auto �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� $(��"Use controls below to get started.��6   $��1��6=   $��1��6J   $��16(�6Z   $��3��6h   $��0.333,0.333,315,0.333q�6|   �� 5   
   -   5         ��Name$ btnFindFiles��5      $��Select input files.��6'   $��1��6E   $��2��6U   $��0.333,9.667,16.931,262q�6b   6�   �� 5   
   -   5        $ btnOutputFolder��5      $'��!Select output folder destination.��6*   $��1��6V   $��2��6f   $��0.333,41.667,17.667,229q�6s   6�   �� 5   
   $��
Action log��6   $��1��6%   $��2��62   $��2��6B   $��63,29,1,274q�6V   $��2��6k   �� 5   
   -   5         ��Name$ scrollViewer��5      $��2��6X   $��3��6e   ���	#FF707070$��2��5       $	��500��6&   $	��300��62   $��
0,28,1,-26q�6?   .��5!      �� -   5         ��Name$ ctrlActionLog��5!       �� Orientation$ Vertical=�60   5"      �� 5#   
   $��Output��6   $��2��6!   $��1��6.   $��8,163,208,109q�6>   �� 5$   
   -   5         ��Name$ txtOutputName��5$      $��FileName{0}@2x.png��6)   $��Calibri)�6C   $��2��6X   $��1��6e   $"��70.333,168.667,9.667,108.667q�6u   �� 5%   
   -   5        $ 
btnConvert��5%      {�.  $��Start conversion��67   $��1��6R   $��2��6b   $��10,202.667,9.667,68q�6o   6�   �� 5&   
   -   5        $ btnReset��5&      {�.  $��Reset��65   $��1��6E   $��2��6U   $��10,237,8,34q�6b   6w   �� 5'   
   -   5        $��Enable logging��5'      	 d� 	IsChecked$
	 TrueY�6,   $��3��6=   $
��Left=�6M   $��	364,6,0,2q�6h   $��1��6{   6�   �� 5(   
   -	   5        
 ��Name$
 chkAutoScroll��5(      $��Enable AutoScroll��6*   $
	 TrueY�6F   $��3��6W   $
��Left=�6g   $��364,32,0,277q�6�   $��1��6�   $��2��6�   5)      5*      