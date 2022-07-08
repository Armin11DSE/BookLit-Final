using System;
using BookLit.Core;
using BookLit.MVVM.View;
using BookLit.Front;

namespace BookLit.MVVM.ViewModel
{
    internal class UserMainViewModel : ObservableObject
    {
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand MyBooksCommand { get; set; }
        public RelayCommand BookmarksCommand { get; set; }
        public RelayCommand BecomeVipCommand { get; set; }
        public RelayCommand WalletCommand { get; set; }
        public RelayCommand CartCommand { get; set; }
        public RelayCommand ProfileCommand { get; set; }

        UserHomeViewModel HomeVM { get; set; }
        UserMyBooksViewModel MyBooksVM { get; set; }
        UserBookmarksViewModel BookmarksVM { get; set; }
        UserBecomeVipViewModel BecomeVipVM { get; set; }
        UserWalletViewModel WalletVM { get; set; }
        UserCartViewModel CartVM { get; set; }
        UserProfileViewModel ProfileVM { get; set; }

        private object _CurrentView;
        public object CurrentView
        {
            get => _CurrentView;
            set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }

        public UserMainViewModel()
        {
            HomeVM = new();
            _CurrentView = HomeVM;

            MyBooksVM = new();
            BookmarksVM = new();
            BecomeVipVM = new();
            WalletVM = new();
            CartVM = new();
            ProfileVM = new();

            HomeCommand = new(s =>
            {
                CurrentView = HomeVM;
            });

            MyBooksCommand = new(s =>
            {
                CurrentView = MyBooksVM;
            });

            BookmarksCommand = new(s =>
            {
                CurrentView = BookmarksVM;
            });

            BecomeVipCommand = new(s =>
            {
                if (UserWindow.user.IsVip)
                {
                    CurrentView = new UserIsVipModel();
                }
                else
                {
                    CurrentView = BecomeVipVM;
                }
            });

            WalletCommand = new(s =>
            {
                CurrentView = WalletVM;
            });

            CartCommand = new(s =>
            {
                CurrentView = CartVM;
            });

            ProfileCommand = new(s =>
            {
                CurrentView = ProfileVM;
            });
        }
    }
}
