   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5   Z   E'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  6�   $    MercuryHub.Views.CreateAccount   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      /local#clr-namespace:MercuryHub.ViewModels  6Z   <customcontrols'clr-namespace:MercuryHub.CustomControls  6�     � 	Resources  5      2   ,MercuryHub.ViewModels.CreateAccountViewModel   CreateAccount��%   &	          5   	   6&   5       � DataContext 5      �� 5   
    �� ResourceKey$ CreateAccount��6   .��5      � $��16q�6   .+�5   
   �� $��Center=�6   $��Center=�62   $	��300��6M   y�6Y   �� *VisibilityCreate+.+�5      �� $��Create Account��6    $  MaterialDesignHeadline6TextBlock#��[ 5      $��Center=�5      �� 5      $��1��5      $	��0,5q�5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Username��5       % !MaterialDesignFloatingHintTextBox#���  5      ��5       �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=��� 5!      $��1��5"   
   $	��0,5q�5#   
   $ Password��5$   
   #���  5%   
   ��5&   
   �� *
Password+$ PropertyChanged=��� 5+      $��0,8,0,0q�6    �� DisplayMemberPath$ Value��5-       �� SelectedValuePath$	 Key��5.      ��5,      �� *	RolesList+ �� SelectedValue 5/      �� *SelectedRoleId+$ PropertyChanged=��� 52      $��Create User��6   ��53      �� *CreateAccountCommand+55      �� 57   
   $��Center=�6   $��Center=�62   $	��300��6M   y�6Y   �� *VisibilityEdit+.+�58      �� $��Edit Account��6   #��[ 59      $��Center=�5:      �� 5<      $��1��5=      $	��0,5q�5>      $ Username��5?      #���  5@      ��5A      �� *
Username+$ PropertyChanged=��� 5B      $��1��5C      $	��0,5q�5D      $ Password��5E      #���  5F      ��5G      �� *
Password+$ PropertyChanged=��� 5I      $��0,8,0,0q�6   $ Value��5K      $	 Key��5L      ��5J      �� *	RolesList+ 5M      �� *SelectedRoleId+$ PropertyChanged=��� 5P      $��Cancel��6   ��5Q      �� *CancelEditCommand+�� 5S      $��	Edit User��6   ��5T      �� *PostEditUserCommand+��5U      �� *SelectedUserId+$ PropertyChanged=�5W      �� 5Z   
   $��Right=�6   $��Center=�61   $	��300��6L   .+�5[      �� $	��200��64   $��10q�6@   ��6   �� *Users+��5\      �� 5]      .R�5^      ��  �� Orientation$ Vertical=�6%   $��5q�6<   $	��200��6G   	 � ContextMenu	 5_      �� 5`   "   .@�5b   &   m� $
��Edit��6/   
 m� Command
 5c      �� *EditUserCommand+ �� Source# [   m� CommandParameter 5d      �� *
userName+m� 5e   &   $��Delete��6/   
 5f      �� *DeleteUserCommand+# [   5g      �� *
userName+5h   #   .+�5l      �� $
��Bold"�6B   ��6(   �� *
userName+6B   �� 5m      $��Italic$�6B   $��12(�6U   ��6(   �� *
RoleName+6U   5n      5o      5q      5t      5u      5v      