﻿namespace System.Windows.Controls
{
    public static class FrameExtensions
    {
        public static object GetDataContext(this Frame frame)
        {
            return frame.Content is FrameworkElement element
                ? element.DataContext
                : null;
        }

        public static void CleanNavigation(this Frame frame)
        {
            while (frame.CanGoBack)
            {
                _ = frame.RemoveBackEntry();
            }
        }
    }
}
