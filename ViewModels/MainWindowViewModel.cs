﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Interweb_Searcher.Models;

namespace Interweb_Searcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private int _selectedTabIndex;
        private bool _suppressSelectionChanged;

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            AddInitialTabs();

            AddTabCommand = new RelayCommand(AddTab);
            RemoveTabCommand = new RelayCommand(RemoveTab, CanRemoveTab);
        }

        public ObservableCollection<TabViewModel> Tabs { get; }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged();

                    if (!_suppressSelectionChanged && value >= 0 && value < Tabs.Count)
                    {
                        var selectedTab = Tabs[value];
                        if (selectedTab.IsSpecialTab)
                        {
                            _suppressSelectionChanged = true;
                            AddTabBeforeSpecialTab();
                            _suppressSelectionChanged = false;
                        }
                    }
                }
            }
        }

        public ICommand AddTabCommand { get; }
        public ICommand RemoveTabCommand { get; }

        private void AddInitialTabs()
        {
            var firstTab = new TabViewModel(this);
            Tabs.Add(firstTab);
            AddSpecialTab();
            SelectedTabIndex = 0;
        }

        private void AddTab(object parameter)
        {
            var newTab = new TabViewModel(this);
            Tabs.Insert(Tabs.Count - 1, newTab);
            SelectedTabIndex = Tabs.Count - 2;
        }

        private void AddSpecialTab()
        {
            var specialTab = new TabViewModel(this) { TabText = "+", IsSpecialTab = true };
            Tabs.Add(specialTab);
        }

        private void AddTabBeforeSpecialTab()
        {
            var newTab = new TabViewModel(this);
            Tabs.Insert(Tabs.Count - 1, newTab);
            SelectedTabIndex = Tabs.Count - 2;
        }

        public void RemoveTab(object parameter)
        {
            if (parameter is TabViewModel tab && Tabs.Contains(tab))
            {
                int index = Tabs.IndexOf(tab);

                if (Tabs.Count == 1)
                {
                    AddTab(null);
                }
                else if (index >= Tabs.Count - 2)
                {
                    if (index > 0 && !Tabs[index - 1].IsSpecialTab)
                    {
                        SelectedTabIndex = index - 1;
                    }
                    else
                    {
                        SelectedTabIndex = Tabs.Count - 2;
                    }
                }
                else
                {
                    SelectedTabIndex = index;
                }

                Tabs.Remove(tab);
                tab.Dispose();
            }
        }

        private bool CanRemoveTab(object parameter)
        {
            return Tabs.Count > 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
