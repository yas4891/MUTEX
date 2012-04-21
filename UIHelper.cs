using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GreedyStringTiling
{
    public static class UIHelper
    {
        public static string GetText(this RichTextBox box)
        {
            var range = new TextRange(box.Document.ContentStart,
                                    box.Document.ContentEnd);

            return range.Text;
        }

        public static string GetTextWithoutLineBreaks(this RichTextBox box)
        {
            return box.GetText().Replace(Environment.NewLine, "");
        }
    }
}
