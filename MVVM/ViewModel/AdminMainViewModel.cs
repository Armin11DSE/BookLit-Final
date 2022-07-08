using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLit.Core;

namespace BookLit.MVVM.ViewModel
{
    internal class AdminMainViewModel : ObservableObject
    {
        public RelayCommand BookAdditionCommand { get; set; }
        public RelayCommand BooksListCommand { get; set; }
        public RelayCommand UsersListCommand { get; set; }
        public RelayCommand SetVipCommand { get; set; }
        public RelayCommand WalletCommand { get; set; }

        AdminBookAdditionViewModel BookAddtionVM { get; set; }
        AdminBooksListViewModel BooksListVM { get; set; }
        UsersListViewModel UsersListViewVM { get; set; }
        AdminSetVipViewModel SetVipVM { get; set; }
        AdminWalletViewModel WalletVM { get; set; }

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

        public AdminMainViewModel()
        {
            BookAddtionVM = new();
            _CurrentView = BookAddtionVM;

            BooksListVM = new();
            UsersListViewVM = new();
            SetVipVM = new();
            WalletVM = new();

            BookAdditionCommand = new(s =>
            {
                CurrentView = BookAddtionVM;
            });

            BooksListCommand = new(s =>
            {
                CurrentView = BooksListVM;
            });

            UsersListCommand = new(s =>
            {
                CurrentView = UsersListViewVM;
            });

            SetVipCommand = new(s =>
            {
                CurrentView = SetVipVM;
            });

            WalletCommand = new(s =>
            {
                CurrentView = WalletVM;
            });
        }
    }
}
