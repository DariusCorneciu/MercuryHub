   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  5      7    1MercuryHub.CustomControls.ReservationDetailsPanel   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:MercuryHub.CustomControls     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      y�5	   7   �� *VisibilityPanel+.��5      �� 
�������
�������$��1q�63     �� Padding$  10q�6G   ._�5   
   �� $
��1000��6   .��5   
   �� .+�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   $��Unalocated Reservation��5      $
��Bold"�5      $��16(�5      $��Center=�5      �� 5      $��1��6   $��40��5   	      MaterialDesignToolButton#��[  5   	   ��5   	   �� *CloseCommand+.��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�6.   5      5       �� 5!      $��Guest Information��6   $��15(�61   $
��Bold"�5"      $��Italic$�6*   $
��0,10q�6=   �� 5$      $��5q�6   $	��400��6    $  5q�5%   	   
�������$��1q�5&   	    �� CornerRadius$ 5��6   $
��Left=�6.   ._�5'      ��  �� Orientation$ Vertical=�6   .+�5(      �� $	��0,5q�6   �� 5)      $�Full Name: ��6   $
��Bold"�6*    �� 5*      �6   �� *Reservation.GuestName+5+      �� 5,      $	��0,5q�6    �� 5-      $�	Details: ��6   $
��Bold"�6+    �� 5.      �6   �� *Reservation.concatenatedNotes+5/      50      51      �� 55      $��Check Information��6   $��15(�61   $
��Bold"�56      $��Italic$�6"   $
��0,10q�65   �� 57      $��5q�6   $	��400��6!   $  5q�58      
�������$��1q�59      $ 5��6%   $
��Left=�66   ._�5:      �� $ Vertical=�6   .+�5;      �� $	��0,0q�6    �� 5<      $�	CheckIn: ��6   $
��Bold"�6,    �� 5=      �6   �� *Reservation.StartDate+ �� StringFormat$ {0:dd MMM yyyy}��5>      �� 5?      $	��0,5q�6    �� 5@      $�
CheckOut: ��6   $
��Bold"�61    �� 5A      �6   �� *
checkOut+5B      �� 5C      $	��0,5q�6    �� 5D      $�Number Of Guests: ��6   $
��Bold"�65    �� 5E      �6   �� *Reservation.guestNumber+5F      5G      5H      �� 5L      $��Rooms Information��6   $��15(�61   $
��Bold"�5M      $��Italic$�6   $
��0,10q�6)   �� 5N      $��5q�6   $  5q�5O      
�������$	��400��6/   $��1q�5P      $ 5��6#   $
��Left=�64   ._�5Q      �� $ 
Horizontal=�6   .+�5R      �� $ Vertical=�6!   .+�5S      �� $��Room Preference:��6    $
��Bold"�68   �� 5T      ��6#   �� *Reservation.PreferredRooms+��5U      �� 5V      .R�5W   "   �� ��6,   �� 5X      5Z      5[      �� 5^      $ Vertical=�6!   $
��20,0q�68   .+�5_      �� $��SelectedRooms:��6$   $
��Bold"�6:   �� 5`      ��6'   �� *SelectedRooms+��5a      �� 5b   "   .R�5c   &   �� ��60   �� *Name+5d   #   5f      5g      5i      5j      �� 5m      $
��Left=�6   $
��0,10q�64   .+�5n      �� )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Reason��5p      $	��250��67   $	��0,5q�6C   y�5n      �� *VisibilityRooms+��5o      �� *RejectReason+ �� UpdateSourceTrigger$ PropertyChanged=�5p   C   �� 5q      $��Reject Reservation��6   $	��200��6   
���  ��
���  ��y�66   �� *VisibilityRooms+��6]   �� *RejectCommand+6�   5r      �� 5t      $
��0,10q�6   .+�5u      �� $��Search existent guest:��6   �� 5w      $	��250��66   $	��0,5q�6B   ��6   �� *	SearchBox+6B   �� 5y      $��Search��6   $	��100��6L   ��6*   �� *SearchCommand+6L   �� 5{      $��Generate User��5|      $	��150��5~      $
��0,10q�5      ��5{      �� *GenerateCommand+5      �� 5�      $
��Next��6   $	��100��6q   y�6(   �� *VisibilityClients+��6Q   �� *NextCommand+6q   �� 5�      $
��0,10q�5�   i   ��5�      �� *MatchingClients+ �� SelectedItem 5�      �� *SelectedClient+	 �� Mode$	 TwoWay=�y�6@   �� *VisibilityClients+��5�      �� 5�      
 �� 
TargetType"
 ��6    .�5�      ��  �� Property � �� �� Value
 ���� 6;   �� 5�       � ��
 �   �6;    �� Triggers 5�      P� 5�   "    P� Property �@��
IsSelected P� Value$
 True��6@   . �5�   &   ��  � ��
 ��p��6C   �� 5�   &    � ��
 �   �6C   5�   #   P� 5�   "    �@J�IsMouseOver$
 True��6A   . �5�   &   ��  � ��
 �� ��6C   5�   #   5�      ��5�      �� 5�      .R�5�      �� .+�5�   "   �� ��6,   �� *	FirstName+�� 5�   "   $��10(�6C   
���   �$��Italic$�6d   ��6,   �� *Email+6d   5�      5�      5�      �� 5�      $��Finish��6   $	��100��6s   y�6*   �� *VisibilityRooms+��6Q   �� *FinishCommand+6s   �� 5�      $0��*Called the guest to confirm the new rooms?��6   $
��Bold"�5�       d� 	IsChecked 5�      �� *CalledGuest+$	 TwoWay=�y�5�      �� *VisibilityCalledGuest+5�      �� 5�      $
��0,10q�6?   $��0q�5�      $	��400��5�      $
��Auto=�5�      $��Disabled=�5�      $��Center=�5�      ��5�      �� *AvabileRooms+y�5�   7   �� *VisibilityRooms+��5�      �� 5�      .R�5�      �  � Orientation$ 
Horizontal=�6(   5�      ��5�      �� 5�      .R�5�      �� $	��200��6%   $��5q�61   $  5q�5�   %   
�������$��1q�5�   %   $ 5��69   ._�5�   "   �� $ Vertical=�6-   .+�5�   "   �� $
��Bold"�5�   /   ��5�   +   �� *Name+ 5�   /   �� *
IsSelected+$	 TwoWay=���5�   /   �� *DataContext.ToggleRoomCommand+ �� RelativeSource ��  �� AncestorType ListBox��5�   /   �� �� 5�   "   $��5,0,0,0q�6,   �� 5�   6   $�
Capacity: ��6:   $��SemiBold"�6L    �� 5�   6   �6:   �� *
capacity+5�   #   5�      5�      5�      5�      5�      5�      5�      5�      5�      