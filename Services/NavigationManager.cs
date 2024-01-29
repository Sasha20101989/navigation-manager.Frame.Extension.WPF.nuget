using System;
using System.Windows.Controls;
using System.Windows.Navigation;

using PageManager.WPF;

namespace NavigationManager.Frame.Extension.WPF
{
    public class NavigationManager : INavigationManager
    {
        private readonly IPageManager _pageManager;
        private System.Windows.Controls.Frame _frame;
        private object _lastParameterUsed;

        public event EventHandler<string> Navigated;

        public bool CanGoBack => _frame.CanGoBack;

        public NavigationManager(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public void Initialize(System.Windows.Controls.Frame shellFrame)
        {
            if (_frame == null)
            {
                _frame = shellFrame;
                _frame.Navigated += OnNavigated;
            }
        }

        public void UnsubscribeNavigation()
        {
            _frame.Navigated -= OnNavigated;
            _frame = null;
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                object vmBeforeNavigation = _frame.GetDataContext();
                _frame.GoBack();
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }
        }

        public bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false)
        {
            Type pageType = _pageManager.GetPageType(pageKey);

            if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
            {
                _frame.Tag = clearNavigation;
                Page page = _pageManager.GetPage(pageKey);
                bool navigated = _frame.Navigate(page, parameter);
                if (navigated)
                {
                    _lastParameterUsed = parameter;
                    object dataContext = _frame.GetDataContext();
                    if (dataContext is INavigationAware navigationAware)
                    {
                        navigationAware.OnNavigatedFrom();
                    }
                }

                return navigated;
            }

            return false;
        }

        public void CleanNavigation()
        {
            _frame.CleanNavigation();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is System.Windows.Controls.Frame frame)
            {
                bool clearNavigation = (bool)frame.Tag;
                if (clearNavigation)
                {
                    frame.CleanNavigation();
                }

                object dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.ExtraData);
                }

                Navigated?.Invoke(sender, dataContext.GetType().FullName);
            }
        }
    }
}
