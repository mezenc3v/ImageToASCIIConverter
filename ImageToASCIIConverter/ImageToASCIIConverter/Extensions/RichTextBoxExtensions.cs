using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ImageToASCIIConverter.Extensions
{
    public static class RichTextBoxExtensions
    {
        public static void AppendLine(this RichTextBox richTextBox, string text)
        {
            richTextBox.AppendText(DateTime.Now.ToString("hh:mm:ss") + " -> " +$"{text}\n");
        }

        public static void AppendLine(this RichTextBox richTextBox, string text, Brush foregroundColor)
        {
            var tr = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd)
            {
                Text = DateTime.Now.ToString("hh:mm:ss") + " -> " + $"{text}\n"
            };
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, foregroundColor);
            }
            catch (FormatException) { }
        }

        public static void AppendLine(this RichTextBox richTextBox, string text, Color foregroundColor)
        {
            var brush = new SolidColorBrush(foregroundColor);
            richTextBox.AppendLine(text, brush);
        }
    }
}
