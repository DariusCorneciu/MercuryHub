   M S B A M L   `   `   `  ����E  AMercuryHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MercuryHub.ViewsMercuryHub.Views  5      =#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5      =#clr-namespace:MercuryHub.ViewModelsMercuryHub.ViewModels  5   W   E'clr-namespace:MercuryHub.CustomControlsMercuryHub.CustomControls  6�        MercuryHub.Views.LoginView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0localclr-namespace:MercuryHub.Views     5      9	viewModel#clr-namespace:MercuryHub.ViewModels     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      0local1#clr-namespace:MercuryHub.ViewModels  6W   <customcontrols'clr-namespace:MercuryHub.CustomControls  6�     � DataContext  5	      *   $MercuryHub.ViewModels.LoginViewModel  5
   
   6%    � 	Resources 5         BooleanToVisibility��%   &	        �� 5   
   6'   5      y�5      �� 5   
    �� Path$ 	IsVisible�6    �� Mode$ TwoWay=�6#    �� 	Converter# [  61   .��5      � $��30q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Login��6   $��24(�5      $
��Bold"�5      $��0,0,0,20q�5       $��Center=�5!      $��0��5"      �� 5$   
   $��1��5%      $	��0,5q�5&      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Username��5'       % !MaterialDesignFloatingHintTextBox#���  5(      ��5)      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�0   *MercuryHub.CustomControls.BindablePassword  5+   
   $��2��5,      $	��0,5q�5-      $ Password��5.         Password 50      �� *
Password+$ TwoWay=�$ PropertyChanged=��� 54   
   $��3��6   $��Login��55   	   $��0,20,0,0q�56   	     MaterialDesignRaisedButton#���  58   	   $��40��59   	   $��Stretch=�5:   	   ��57   	   �� *LoginCommand+5:   	   5<      5=      