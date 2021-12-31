﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CATCheck
{
    public class ComboBoxItem<T>
    {
        private string Text { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public ComboBoxItem(string text, T value)
        {
            Text = text;
            Value = value;
        }
    }
}
