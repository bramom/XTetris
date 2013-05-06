using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;

namespace GameUtility
{
    public static class SoundUtility
    {
        private static Panel rootPanel;
        private static Dictionary<string, MediaElement> mediaElements = new Dictionary<string, MediaElement>();

        public static void Initialize(Panel root)
        {
            rootPanel = root;
        }
        
        public static void LoadMedia(string key)
        {
            Uri uri = new Uri(string.Format("XamlTetris;component/{0}", key), UriKind.Relative);
            StreamResourceInfo sri = System.Windows.Application.GetResourceStream(uri);

            MediaElement element = new MediaElement();            
            element.AutoPlay = false;
            element.Visibility = Visibility.Collapsed;
            element.SetSource(sri.Stream);
            rootPanel.Children.Add(element);

            mediaElements.Add(key, element);
        }

        public static void Play(string key)
        {
            mediaElements[key].Stop();
            mediaElements[key].Play();
        }
    }
}
