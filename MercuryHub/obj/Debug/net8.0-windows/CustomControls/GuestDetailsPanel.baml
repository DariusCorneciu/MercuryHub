   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  5      1    +MercuryHub.CustomControls.GuestDetailsPanel   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      3local'clr-namespace:MercuryHub.CustomControls  5      y�5	      �� *VisibilityPanel+.��5
      �� $	��700��6   
�������
�������$��1q�6?     �� Padding$  10q�6S   ._�5   
   �� .+�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   $��Selected Guest��5      $
��Bold"�5      $��16(�5      $��Center=�5      $��Center=�5      �� 5      $��1��6    �� Orientation$ 
Horizontal=�6-   .+�5      �� $��40��5         MaterialDesignToolButton#��[  5      ��5      �� *EditCommand+.��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Edit=�62   5       �� 5#      $��40��5$      #��[  5&      ��5%      �� *CloseCommand+.��5'        $ Close=�62   5(      5+      5-      �� 5/      $��Center=�6   $	��0,5q�66   .+�51      �� $ 
Horizontal=�6   $	��0,5q�66   .+�52      �� $	��200��53      $��0,0,5,0q�54      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	Fist Name��56       % !MaterialDesignFloatingHintTextBox#���  57      {�55      �� *	CanSubmit+��58      �� *Guest.FirstName+ �� UpdateSourceTrigger$ PropertyChanged=��� 59      $	��200��5:      $ 	Last Name��5<      #���  5=      {�5;      �� *	CanSubmit+��5>      �� *Guest.LastName+$ PropertyChanged=�5@      �� 5B      $ 
Horizontal=�6   $	��0,5q�66   .+�5C      �� $	��200��5D      $��0,0,5,0q�5E      $ Email��5G      #���  5H      {�5F      �� *	CanSubmit+��5I      �� *Guest.Email+$ PropertyChanged=��� 5J      $	��200��5K      $ PhoneNumber��5M      #���  5N      {�5L      �� *	CanSubmit+��5O      �� *Guest.PhoneNumber+$ PropertyChanged=�5P      �� 5Q      $��2��5R      $	��0,5q�5S      $	��400��5T      $
��Left=�5U      $	 CNP��5V      #���  5W      {�5X      �� *	CanSubmit+��5Y      �� *	Guest.CNP+$ PropertyChanged=�5Z      �� 5[      $	��0,5q�6   $	��400��5\      y�6!   �� *VisibilityButton+��6I   �� *SaveChangesCommand+.��6p   Save Changes5]      �� 5^      $
��0,10q�6   $��1��6&   
�������61   �� 5_      $��This year reservations��5`      $
��Bold"�5a      $��16(�5b      $��Center=�5c      $��Center=�5d      �� 5e      $	��300��6   $
��Auto=�6(   .��5f      �� ��6   �� *Guest.reservations+��5g      �� 5h      .R�5j      �� 
�������$��1q�68    �� CornerRadius$ 5��6L   $  10q�6]   $��5q�6j   ._�5k      �� $ 
Horizontal=�6)   .+�5l   "   �� $
��10,0q�6,   $
��Bold"�6w   ��6:   �� *BookingCode+ �� StringFormat$ Booking code: {0}��6w   �� 5m   "   $
��10,0q�6,   ��6;   �� *	checkIn+$ Check-in: {0:dd MMM yyyy}���� 5n   "   $
��10,0q�6,   ��6;   �� *
checkOut+$  Check-out: {0:dd MMM yyyy}���� 5o   "   $
��10,0q�6,   ��6;   �� *reservationCost+$ Total price: {0:C}��5q      5r      5t      5v      5w      5x      5z      5{      