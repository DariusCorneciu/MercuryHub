   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MercuryHub.ViewsMercuryHub.Views  5      =#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5	   X   E'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  6�   (    "MercuryHub.Views.ReservationEditor   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:MercuryHub.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      0local1#clr-namespace:MercuryHub.ViewModels  6X   <customcontrols'clr-namespace:MercuryHub.CustomControls  6�   .��5      � .+�5   
   � $
��1400��6   $	��900��6     � ColumnDefinitions  5      �� 5      $	��700 �6#   �� 5      $��* �6#   .+�5      �� $��0��6   $��Center=�6)   $��Center=�5      $	��400��5      .+�5      ��  �� Orientation$ 
Horizontal=�6   $	��0,5q�66   .+�5      �� $	��200��6   $��0,0,10,0q�6*   $��Search Reservation��6<   ��5      �� *SearchReservtionCommand+�� 5      $	��200��6   $��Create Reservation��6*   ��5      �� *CreateReservationCommand+5      �� 5       $   MaterialDesignHeadline6TextBlock#��[  5       $��Center=�5!      ��5      �� *
FromName+5!      �� 5#      $	��0,5q�6   .+�5$      �� $��
Guest Data��6    $
��Bold"�5%      $��16(�5&      $��0,0,0,5q�5'      �� 5(      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5)      �� $	��200��5*   
   $��0,0,5,0q�5+   
   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	Fist Name��5,   
    % !MaterialDesignFloatingHintTextBox#���  5-   
   ��5.   
   �� *PersonalInformation.FirstName+ �� UpdateSourceTrigger$ PropertyChanged=��� 5/      $	��200��50   
   $ 	Last Name��51   
   #���  52   
   ��53   
   �� *PersonalInformation.LastName+$ PropertyChanged=�55      �� 57      $ 
Horizontal=�6!   $	��0,5q�6:   .+�58      �� $	��200��59      $��0,0,5,0q�5:      $ Email��5;      #���  5<      ��5=      �� *PersonalInformation.Email+$ PropertyChanged=��� 5>      $	��200��5?      $ PhoneNumber��5@      #���  5A      ��5B      �� *!PersonalInformation.PhoneNumber+$ PropertyChanged=�5C      �� 5D      $��2��5E      $	��0,5q�5F      $	 CNP��5G      #���  5H      y�6N   �� *VisibilitySearch+��5I      �� *PersonalInformation.CNP+$ PropertyChanged=�5K      �� 5M      $	��0,5q�6   .+�5N      �� $��Reservation Data��6    $
��Bold"�5O      $��16(�5P      $��0,0,0,5q�5Q      � 5S       � 
ItemHeight$ 70��6     � Orientation$ 
Horizontal=�60   $
��Left=�6I   .+�5T      �� $	��400��5U      $��5q�5V      $ BookingCode��5W      #���  5X      ��5Y      �� * ReservationDetalis.BookingCode+$ PropertyChanged=�y�6o   �� *VisibilitySearch+(  "System.Windows.Controls.DatePicker  5Z      $	��180��5[      $��30��5\        MaterialDesignDatePicker#��[ 5^      $ CheckIn��5_      .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Visible=�5`         SelectedDate 5]      �� *ReservationDetalis.CheckIn+$ PropertyChanged=�{�5a      �� *Avalability+5b        5c      $	��180��5d      $��30��5e      $��40,0,0,0q�5f      #��[ 5h      $ CheckOut��5i      $ Visible=�5k       5g      �� *ReservationDetalis.CheckOut+$ PropertyChanged=�{�5j      �� *Avalability+5l      �� 5n      -   5        $	��120��5o      $��35��5p      $��5q�5q      $ Number Of Guests��5r      #���  5s      ��5t      �� *#!ReservationDetalis.numberOfGuests+$ PropertyChanged=�y�5u      �� *VisibilityCreate+5w      �� 5x      -   5        $	��120��5y      $��35��5z      $��5q�5{      $ Price Offered in EURO��5|      #���  5}      ��5~      �� *ReservationDetalis.price+$ PropertyChanged=�y�5      �� *VisibilityCreate+5�      5�      5�      �� 5�      $	��200��6   $��0,10,0,0q�6&   $��Search Availability��68   y�5�      �� *VisibilityCreate+��5�      �� *ExecuteAvalabilityCommand+�� 5�      $	��200��6   $��0,10,0,0q�6&   $��Search Reservation��68   y�5�      �� *VisibilitySearch+��5�      �� *ExecuteSearchCommand+�� 5�      $	��200��6   $��0,10,0,0q�6&   $��Create Reservation��68   y�5�      �� *VisibilityComplete+��5�      �� *CompleteCommand+5�      �� 5�      $��1��6C   $��Found Rooms��6S   $
��Bold"�6f   $��16(�6x   y�6   �� *VisibilityComplete+6x   �� 5�      $
��0,10q�6;   $��1��6I   $��0q�5�   
   $	��400��5�   
   $
��Auto=�5�   
   $��Disabled=�5�   
   $��Center=�5�   
   ��5�      �� *AvabileRooms+y�5�   '   �� *VisibilityComplete+��5�      �� 5�      .R�5�      � $ 
Horizontal=�6$   5�      ��5�      �� 5�      .R�5�      �� $	��200��6!   $��5q�6-    �� Padding$ 5q�5�      
�������$��1q�5�      	 �� CornerRadius$	 5��6)   ._�5�      �� $ Vertical=�6)   .+�5�   "   �� $
��Bold"�5�      ��5�   +   �� *Name+
 d� 	IsChecked
 5�      �� *
IsSelected+ �� Mode$ TwoWay=���5�      �� *DataContext.ToggleRoomCommand+ �� RelativeSource ��  �� AncestorType ListBox��5�      �� �� 5�   "   $��5,0,0,0q�6,   �� 5�   &   $�
Capacity: ��6*   $��SemiBold"�6<    �� 5�   &   �6*   �� *
capacity+5�   #   5�      5�      5�      5�      �� 5�      $��1��6@   $��Found Reservations��6P   $
��Bold"�6j   $��16(�6|   $��20,0,0,0q�6�   y�6   �� *VisibilitySearch+6�   �� 5�      $
��Auto=�6C   $��1��6f   ���#f9f9f9$��20q�6�   y�6   �� *VisibilitySearch+.��5�      �� $��10q�6   .+�5�      �� ��6#   �� *Reservations+��5�      �� 5�      .R�5�   "   �� $	 6��6)   $	��0,5q�6:   $ 8q�6G   
���   �$	��0.5q�6g   ��5�   &   �� 5�   *    �� 
TargetType" ��60   .�5�   .   ��  �� Property � �� �� Value
 �����6K    �� Triggers 5�   .   �� 5�   2    �� Binding 6>   �� *source+ �� Value Web6Y   .a�5�   6   ��  � ��
 �����6S   5�   3   �� 5�   2    6>   �� *source+	 Local6Y   .a�5�   6   ��  � ��
 ��ح�6S   5�   3   5�   +   ._�5�   &   �   5�   *   �� 5�   .   $��* �6?   �� 5�   .   $
��Auto �6?   .+�5�   *   �� $��0��65   $	��4,0q�6E   .+�5�   .   �� $
��Bold"�6U   $��14(�6g   
���   ���68   �� *BookingCode+6u   �� 5�   .   $��Italic$�6k   $��13(�6~   
���   ���68   �� *source+ �� StringFormat$ Source: {0}��6�   �� 5�   .   
���   ���68   �� *reservationCost+$ Price: {0:C}��6v   �� 5�   .   
���   ���68   �� *numberOfGuests+$ Guests: {0}��6t   �� 5�   .   
���   ��� 5�   2   �66   �� *	checkIn+$ Check-in: {0:dd MMM yyyy}�� �� 5�   2   $�->��66    �� 5�   3   �67   �� *
checkOut+$  Check-out: {0:dd MMM yyyy}��5�   /   �� 5�   .   $
��Wrap=�6[   
���   ���68   �� *concatenatedNotes+6o   �� 5�   .   $
��Wrap=�6v   
���   ���68   �� *reservationStatus+$ Status: {0}��6�   5�   +   �� 5�   *   $��-8,-8,-8,-8q�65   $��1��6J   $��Stretch=�6Z   $ 
Horizontal=�6y   ��5�   .   �� 5�   2   " ��68   .�5�   6   ��  � y�$ 	Collapsed=�6S    5�   6   �� 5�   :    6F   �� *reservationStatus+ 	Confirmed6l   .a�5�   >   ��  � y�$ Visible=�6[   5�   ;   5�   3   .+�5�   .   ��  ��Name$ ConfirmButton��5�   5   -   5        $	��131��5�   5   $��Center=�6B   
�������$
��Edit��5�   5   
����p��6D   5�   +   5�   '   5�   #   5�      5     5     5     �� 5     -   5         ��Name$ 
RightPanel��5  -   $��0��6   $��Center=�5     $��Center=�5     $	��700��5      K� ZIndex$ 99��5     
���  ��$y�Hidden=�5
     5     5     5     