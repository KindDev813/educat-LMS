﻿using EduCATS.Controls.Pickers;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Eemc.ViewModels;
using EduCATS.Pages.Eemc.Views.ViewCell;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.Views
{
	public class EemcPageView : ContentPage
	{
		const double _spacing = 1;
		const int _rowsCount = 2;
		const double _buttonHeight = 50;
		static Thickness _headerPadding = new Thickness(10);
		static Thickness _subjectsMargin = new Thickness(0, 0, 0, 10);
		static Thickness _emptyViewMargin = new Thickness(10, 0);
		static Thickness _backButtonMargin = new Thickness(30, 0, 30, 15);

		public EemcPageView(int searchId)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new EemcPageViewModel(new PlatformServices(), searchId);
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var subjectsView = createSubjectsPicker();
			var documentCollectionView = createCollection(subjectsView);
			var backButton = createBackButton();

			Content = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Spacing = _spacing,
				Children = {
					headerImage,
					documentCollectionView,
					backButton
				}
			};
		}

		CachedImage createHeaderImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile(Theme.Current.EemcHeaderImage)
			};
		}

		CollectionView createCollection(View header)
		{
			var documentsCollectionView = new CollectionView {
				SelectionMode = SelectionMode.Single,
				ItemTemplate = new DataTemplate(typeof(EemcPageViewCell)),
				ItemsLayout = new GridItemsLayout(_rowsCount, ItemsLayoutOrientation.Vertical),
				EmptyView = new StackLayout {
					BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
					Children = { createEmptyView() }
				},
				Header = new StackLayout
				{
					BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
					Padding = _headerPadding,
					Children = {
						header
					}
				}
			};

			documentsCollectionView.SetBinding(SelectableItemsView.SelectedItemProperty, "SelectedItem");
			documentsCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Concepts");
			return documentsCollectionView;
		}

		Frame createEmptyView()
		{
			return new Frame {
				HasShadow = false,
				Margin = _emptyViewMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = new Label {
					Style = AppStyles.GetLabelStyle(),
					HorizontalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Text = CrossLocalization.Translate("base_no_data"),
					TextColor = Color.FromHex(Theme.Current.BaseNoDataTextColor)
				}
			};
		}

		SubjectsPickerView createSubjectsPicker()
		{
			return new SubjectsPickerView
			{
				Margin = _subjectsMargin,
			};
		}

		Button createBackButton()
		{
			var backButton = new Button {
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.EndAndExpand,
				CornerRadius = (int)_buttonHeight / 2,
				HeightRequest = _buttonHeight,
				Margin = _backButtonMargin,
				TextColor = Color.FromHex(Theme.Current.EemcBackButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.EemcBackButtonColor),
				Text = CrossLocalization.Translate("eemc_back_text"),
				Style = AppStyles.GetButtonStyle()
			};

			backButton.SetBinding(Button.CommandProperty, "BackCommand");
			backButton.SetBinding(IsVisibleProperty, "IsBackActionPossible");
			return backButton;
		}
	}
}
