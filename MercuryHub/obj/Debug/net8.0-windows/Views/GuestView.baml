   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MercuryHub.ViewsMercuryHub.Views  5      =#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5	   M   E'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  6�        MercuryHub.Views.GuestView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:MercuryHub.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      0local1#clr-namespace:MercuryHub.ViewModels  6M   <customcontrols'clr-namespace:MercuryHub.CustomControls  6�   .��5      � .+�5      � $
��1400��6   $	��900��6     � ColumnDefinitions  5   
   �� 5      $	��700 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Center=�6%   $��Center=�5      $	��400��5      .+�5      ��  �� Orientation$ 
Horizontal=�6   $	��0,5q�62   .+�5      �� $	��200��6   $��0,0,10,0q�6&   $��Search Guest��68   ��5      �� *SearchGuestCommand+�� 5      $	��200��6   $��Create Guest��6&   ��5      �� *CreateGuestCommand+5      �� 5       $   MaterialDesignHeadline6TextBlock#��[  5      $��Center=�5      ��5      �� *
FromName+5      �� 5!      $	��0,5q�6   .+�5"      �� $��Personal Data��6   $
��Bold"�5#      $��16(�5$      $��0,0,0,5q�5%      �� 5&      $ 
Horizontal=�6   $	��0,5q�66   .+�5'      �� $	��200��5(   
   $��0,0,5,0q�5)   
   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
First Name��5*   
    % !MaterialDesignFloatingHintTextBox#���  5+   
   ��5,   
   �� *PersonalInformation.FirstName+ �� UpdateSourceTrigger$ PropertyChanged=��� 5-      $	��200��5.   
   $ 	Last Name��5/   
   #���  50   
   ��51   
   �� *PersonalInformation.LastName+$ PropertyChanged=�53      �� 55      $ 
Horizontal=�6   $	��0,5q�66   .+�56      �� $	��200��57      $��0,0,5,0q�58      $ Email��59      #���  5:      ��5;      �� *PersonalInformation.Email+$ PropertyChanged=��� 5<      $	��200��5=      $ PhoneNumber��5>      #���  5?      ��5@      �� *!PersonalInformation.PhoneNumber+$ PropertyChanged=�5A      �� 5B      $��2��5C      $	��0,5q�5D      $	 CNP��5E      #���  5F      ��5G      �� *PersonalInformation.CNP+$ PropertyChanged=�5H      �� 5J      $	��0,5q�6   .+�5K      �� $��Address��6   $
��Bold"�5L      $��16(�5M      $��0,0,0,5q�5N      y�5O      �� *VisibilityCreate+� 5Q       � 
ItemHeight$ 70��6D    � Orientation$ 
Horizontal=�6T   $
��Left=�6m   y�6   �� *VisibilityCreate+.+�5R   
   �� $	��190��5S      $��5q�5T      $ Country��5U      #���  5V      ��5W      �� *Address.Country+�� 5X   
   $	��190��5Y      $��5q�5Z      $
 Town��5[      #���  5\      ��5]      �� *Address.Town+�� 5_   
   $	��190��5`      $��5q�5a      $ Street��5b      #���  5c      ��5d      �� *Address.Street+�� 5e   
   $��40��5f      $��5q�5g      $	 Nr.��5h      #���  5i      ��5j      �� *Address.Number+�� 5k   
   $��40��5l      $��5q�5m      $
 Bld.��5n      #���  5o      ��5p      �� *Address.Building+�� 5q   
   $��40��5r      $��5q�5s      $
 Ent.��5t      #���  5u      ��5v      �� *Address.Scara+�� 5w   
   $��40��5x      $��5q�5y      $	 Ap.��5z      #���  5{      ��5|      �� *Address.Apartment+5}      5~      �� 5      $	��200��6   $��0,10,0,0q�6"   $��Search Guest��64   y�5�      �� *VisibilitySearch+��5�      �� *SearchGuestButton+�� 5�      $	��200��6   $��0,10,0,0q�6"   $��Create Guest��64   y�5�      �� *VisibilityCreate+��5�      �� *CreateGuestButton+5�      �� 5�   
   $��1��6<   $��Found Guests��6L   $
��Bold"�6`   $��16(�6r   $��20,0,0,0q�6�   y�6   �� *VisibilitySearch+6�   �� 5�   
   $
��Auto=�6?   $��1��6b   ���#f9f9f9$��20q�6�   y�6   �� *VisibilitySearch+.��5�      �� .+�5�      �� ��6   �� *PossibleGuests+��5�      �� 5�      .R�5�   "   �� 
���� �� �� CornerRadius$ 4��6=   $	��0,5q�6N    �� Padding$ 5q�6[   
���   �$	��0.5q�6{   ._�5�   &   �   5�   *   �� 5�   .   $��* �6?   �� 5�   .   $
��Auto �6?   .+�5�   *   �� $��0��65   .+�5�   .   �� $��SemiBold"�6R   
���������68   �� *
FullName+6h   �� 5�   .   $��SemiBold"�6O   
���������68   �� *Email+6e   �� 5�   .   $��SemiBold"�6U   
���������68   �� *PhoneNumber+6k   �� 5�   .   $��SemiBold"�6M   
���������68   �� *CNP+6c   5�   +   �� 5�   *   -   5        
����p��$��1��5�   b   .��6r   Show6x   5�   '   5�   #   5�      5�      5�      5�      �� 5�      -   5         ��Name$ 
RightPanel��5�   -   $��0��6   $��Center=�5�      $��Center=�5�      $	��700��5�      	 K� ZIndex$	 99��5�      
���  ��$y�Hidden=�5�      5�      5�      5�      