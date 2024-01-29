using System;

namespace NavigationManager.Frame.Extension.WPF
{
    public interface INavigationManager
    {
        event EventHandler<string> Navigated;

        bool CanGoBack { get; }

        void Initialize(System.Windows.Controls.Frame shellFrame);

        bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

        void GoBack();

        void UnsubscribeNavigation();

        void CleanNavigation();
    }
}
