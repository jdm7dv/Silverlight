   M S B A M L   `   `   `  ����R  NMultiTouch.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullbFclr-namespace:MultiTouch.Controls.WPF;assembly=MultiTouch.Controls.WPFMultiTouch.Controls.WPF  5       WpfMultiTouch$   WpfMultiTouch.MultiTouchWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ]
myControlsFclr-namespace:MultiTouch.Controls.WPF;assembly=MultiTouch.Controls.WPF     5        � Title$4  .WPF 4 Multi-Touch gestures and Inertia example��5       � WindowState$ 	Maximized=�5      .��5      � ��5	   
   �� 5
       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FF3B3B3B��6   � 5      $ 	#FF000000��6    � Offset$ 1Q�61   5      .+�5   
   ,   &MultiTouch.Controls.WPF.MultiTouchView  %   InertiaTranslationDeceleration$ 0.000096Q�6$   #   InertiaExpansionDeceleration$ 0.0096Q�5   $   "   InertiaRotationDeceleration$ 	0.0000072Q�5   $   ��5      �� 5      .R�5      �� 	 ��Name$	 
_touchArea��6   $
��1280��61   $	��700��6>   .+�5      �� $��Images/Desert.jpg��6    
 J� IsManipulationEnabled
 . $	��200��6X   $	��-50q�6d   z�5      u� 5   "    u� Matrix$ 1.5,0.5,-0.5,1.5,400,80x�62   5      �� 5      $��Images/Hydrangeas.jpg��6    
 . $	��200��6\   $��50q�6h   z�5      u� 5   "   $ 1.5,0.5,-0.5,1.5,400,80x�62   5      �� 5       $��Images/Jellyfish.jpg��6    
 . $	��200��6[   $	��150q�6g   z�5!      u� 5"   "   $ 1.5,0.5,-0.5,1.5,400,80x�62   5$      5%      5&      5(      5)      5*      