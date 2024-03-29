﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace GameUtility.Keyboard
{
    /// <summary>
    /// Keyboard input helper
    /// </summary>
    public class KeyHandler
    {
        Dictionary<Key, bool> isPressed = new Dictionary<Key, bool>(); 
        FrameworkElement targetElement = null;

        public bool IsAnyKeyPressed
        {
            get { return isPressed.Count > 0; }
        }

        public void ClearKeyPresses()
        {
            isPressed.Clear();
        }

        public KeyHandler(FrameworkElement target)
        {
            ClearKeyPresses();
            targetElement = target;
            target.KeyDown += new KeyEventHandler(target_KeyDown);
            target.KeyUp += new KeyEventHandler(target_KeyUp);
            target.LostFocus += new RoutedEventHandler(target_LostFocus);
        }

        void target_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isPressed.ContainsKey(e.Key))
            {
                isPressed.Add(e.Key, true);
            }
        }

        void target_KeyUp(object sender, KeyEventArgs e)
        {
            if (isPressed.ContainsKey(e.Key))
            {
                isPressed.Remove(e.Key);
            }
        }
            
        void target_LostFocus(object sender, RoutedEventArgs e)
        {
            ClearKeyPresses();            
        }

        public bool IsKeyPressed(Key k)
        {
            return isPressed.ContainsKey(k);
        }
    }
}
