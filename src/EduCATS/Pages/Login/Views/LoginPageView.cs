﻿using System;
using EduCATS.Helpers.Themes;
using EduCATS.Pages.Login.ViewModels;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Login.Views
{
	public class LoginPageView : ContentPage
	{
		readonly string[] backgrounds = {
			Theme.Current.LoginBackground1Image,
			Theme.Current.LoginBackground2Image,
			Theme.Current.LoginBackground3Image,
		};

		readonly Thickness commonSpacing = new Thickness(0, 10, 0, 0);

		public LoginPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new LoginPageViewModel();
			createView();
		}

		void createView()
		{
			var backgroundImage = createBackgroundImage();
			var mainLayout = createLoginForm();

			Content = new Grid {
				Children = {
					backgroundImage,
					mainLayout
				}
			};
		}

		StackLayout createLoginForm()
		{
			var mascotImage = createMascotImage();
			var usernameEntry = createUsernameEntry();
			var passwordEntryGrid = createPasswordGrid();
			var loginButton = createLoginButton();
			var activityIndicator = createActivityIndicator();

			return new StackLayout {
				Spacing = 0,
				Padding = new Thickness(20, 0),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					mascotImage,
					usernameEntry,
					passwordEntryGrid,
					loginButton,
					activityIndicator
				}
			};
		}

		CachedImage createBackgroundImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromFile(getRandomBackgroundImage())
			};
		}

		CachedImage createMascotImage()
		{
			return new CachedImage {
				HeightRequest = 200,
				Source = ImageSource.FromFile(Theme.Current.LoginMascotImage)
			};
		}

		Entry createUsernameEntry()
		{
			var username = new Entry {
				Style = getEntryStyle(),
				ReturnType = ReturnType.Next,
				Placeholder = CrossLocalization.Translate("login_username")
			};

			username.SetBinding(Entry.TextProperty, "Username");
			return username;
		}

		Grid createPasswordGrid()
		{
			var passwordEntry = createPasswordEntry();
			var showPasswordImage = createShowPasswordImage();

			return new Grid {
				Children = {
					passwordEntry,
					showPasswordImage
				}
			};
		}

		Entry createPasswordEntry()
		{
			var password = new Entry {
				Style = getEntryStyle(),
				IsPassword = true,
				ReturnType = ReturnType.Done,
				Margin = commonSpacing,
				Placeholder = CrossLocalization.Translate("login_password")
			};

			password.SetBinding(Entry.TextProperty, "Password");
			password.SetBinding(Entry.IsPasswordProperty, "IsPasswordHidden");
			return password;
		}

		Button createLoginButton()
		{
			var loginButton = new Button {
				Text = CrossLocalization.Translate("login_text"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = commonSpacing,
				HeightRequest = 50
			};

			loginButton.SetBinding(Button.CommandProperty, "LoginCommand");
			return loginButton;
		}

		CachedImage createShowPasswordImage()
		{
			var showPasswordImage = new CachedImage {
				Aspect = Aspect.AspectFit,
				Margin = new Thickness(0, 10, 5, 0),
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.LoginShowPasswordImage)
			};

			var showPasswordTapGesture = new TapGestureRecognizer();
			showPasswordTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "ShowPasswordCommand");
			showPasswordImage.GestureRecognizers.Add(showPasswordTapGesture);
			return showPasswordImage;
		}

		ActivityIndicator createActivityIndicator()
		{
			var activityIndicator = new ActivityIndicator {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Margin = commonSpacing,
				Color = Color.White
			};

			activityIndicator.SetBinding(IsVisibleProperty, "IsLoading");
			activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
			return activityIndicator;
		}

		Style getEntryStyle()
		{
			return new Style(typeof(Entry)) {
				Setters = {
					new Setter {
						Property = HeightRequestProperty,
						Value = 50
					},

					new Setter {
						Property = BackgroundColorProperty,
						Value = Theme.Current.LoginEntryBackgroundColor
					}
				}
			};
		}

		string getRandomBackgroundImage()
		{
			var random = new Random();
			var randomBackgroundIndex = random.Next(0, backgrounds.Length - 1);
			return backgrounds[randomBackgroundIndex];
		}
	}
}