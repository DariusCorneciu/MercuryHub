   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MercuryHub.ViewsMercuryHub.Views  5      =#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5      $    MercuryHub.Views.DashboardView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0localclr-namespace:MercuryHub.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      4
viewModels#clr-namespace:MercuryHub.ViewModels  5      .��5      �   � ColumnDefinitions  5	   	   �� 5
      $	��250 �6   �� 5      $��* �6   .+�5   	   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��16q�6-   .��5      ]� .+�5      �� $	��Top=�6   $��16q�61   .+�5      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ 
PrimaryMid=�6.   $��16q�6@      CornerRadius$ 8��6M   .��5      �� .+�5      �� $��Mercury Hub��6'    $   MaterialDesignHeadline6TextBlock#��[  5   %   $��Center=�5   %   
�������5   %   �� 5       $  MaterialDesignSubtitle1TextBlock#��[ 5   %   $��Center=�5   %   $��0,8,0,0q�5   %   
���������5   '   �� *
Username+5   %   �� 5      #��[ 5   %   $��Center=�5    %   $��0,8,0,0q�5!   %   
���������5   '   �� *
UserRole+ �� StringFormat$ Job: {0}��5"   %   5#      5$      5%      �� 5'      -   5         ��Name$ 	MenuItems��5'      $	��Top=�5(      ��5)      �� 5*       �� 
TargetType" ��6   .�5+      ��  �� Property � �� �� Value
 ���� 6:   �� 5,       � ��$ 0q�6?   �� 5-       �@�OverridesDefaultStyle$
 True��6E   �� 5.       � ��$
 0,10q�66   6E   �� 5/       � �� 50   !   �� 51   %    �� 
TargetType" ��65   .R�52   )   �� 53   &   55      56      .@�59      �� -   5        	 ��Name$	 CreateAccountItem��5:      $��3��59      $��Manage Accounts��5;      $y�	Collapsed=�5<      $��CreateAccount��5=   C     MaterialDesignRaisedButton#���  5>      $��Stretch=�5?      ��5=      �� *ChangeViewCommand+5?      � 5@      .+�5A      �� ��6"   �� *
Properties+��5B      �� 5C      .R�5D   !   �� #���  5E   (   $��Stretch=�5F   (   ��5D   (   �� ��5G   (   �� ��5H   (   �� *DataContext.OpenPopUpCommand+
 �� RelativeSource
 ��  �� AncestorType UserControl5I      5K      � 5L      $��
MousePoint=�6     � 	StaysOpen$ False��5N      ��5M      �� *DataContext.IsPopupOpen+
 ��  UserControl �� Mode$ TwoWay=�.��5O      �� 
������� �� Padding$ 10q�68   
���� ��$��1q�6Z   ._�5P   "   �� .+�5Q   &   �� �� 5R   *   $�Selected Hotel: ��6.    �� 5S   *   $
��Bold"�6Q   $��Italic$�6c   �6.   �� *SelectedHotelName+6c   5T   '   �� 5U   &   $
��0,10q�6-   $��Edit Reservations��6;   ��5V   -   �� *ChangeViewEditCommand+��6W   �� *SelectedHotelName+�� 5X   &   $
��0,10q�6-   $��Show Reservations��6;   ��5Y   .   �� *ChangeViewShowCommand+��6X   �� *SelectedHotelName+5Z   #   5[      5\      5]      �� 5^      $��3��6   $��Guests��5_      $��Guests��5`   A   #���  5a      ��5`      �� *ChangeViewCommand+5a      �� 5b      $��3��6   $��Reports��5c      $��Reports��5d   A   #���  5e      $��Stretch=�5f      ��5d      �� *ChangeViewCommand+5f      �� 5g      $��3��6   $��Sync Reservations��5h      #���  5j      ��5i      �� *SyncCommand+5j      5m      5p      5r      5t      5u      