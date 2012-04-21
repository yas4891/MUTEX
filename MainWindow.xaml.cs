using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using GSTLibrary.tile;
using GSTLibrary.token;

namespace GreedyStringTiling
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Color[] _ltBackground = 
        {
            Colors.Goldenrod, Colors.DarkTurquoise, Colors.DarkSalmon, Colors.DarkKhaki, 
            Colors.Chocolate, Colors.Brown, Colors.CadetBlue, Colors.Crimson
        };

        private static readonly Color[] _ltForeground =
        {
            Colors.Black, Colors.Black, Colors.Black, Colors.Black,
            Colors.Black, Colors.Black, Colors.Black, Colors.Black
        };

        private static int _indexColorLookup = 0;

        private GSTAlgorithm<GSTToken<char>> Algorithm;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            TextABox.Name = "A";
            TextBBox.Name = "B";
        }

        private void StartAlgorithm(object sender, RoutedEventArgs e)
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString(TextABox.GetTextWithoutLineBreaks().Replace("\r\n", "AA")),
                GSTHelper.FromString(TextBBox.GetTextWithoutLineBreaks().Replace("\r\n", "AA")))
            {
                MinimumMatchLength = Int32.Parse(MMLTB.Text) - 1
            };

            //WriteToConsole(TextABox.GetTextWithoutLineBreaks());
            Reset();
        }

        private void WriteToConsole(string text)
        {
            Console.WriteLine();
            foreach (var ch in text.ToCharArray())
            {
                Console.Write("{0}\t", ch);
            }
            Console.WriteLine();
            foreach (var ch in text.ToCharArray())
            {
                Console.Write("{0:X2}\t", (int)ch);
            }
            Console.WriteLine();
        }

        private void RunToCompletion(object sender, RoutedEventArgs e)
        {
            SetInitialAlgorithm();

            while (!Algorithm.Finished)
                DoOneStep(null, null);
        }

        private void SetInitialAlgorithm()
        {
            if (null == Algorithm)
                StartAlgorithm(null, null);
        }

        private void DoOneStep(object sender, RoutedEventArgs e)
        {
            SetInitialAlgorithm();

            Algorithm.DoOneRun();

            if(Algorithm.Finished)
            {
                StepButton.IsEnabled = false;
                RunButton.IsEnabled = false;
                labelFinished.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));

                return;
            }
            
            //ResetColors();
            var matches = Algorithm.TilesMatchedInLastRun;

            foreach(var tile in matches)
            {
                /*
                ColorizeText(TextABox, tile);
                ColorizeText(TextBBox, tile);
                /* 
                ColorizeText(TextABox, tile.GetTokensAsString());
                ColorizeText(TextBBox, tile.GetTokensAsString());
                /* */
                int lenTile = tile.Tokens.Count();
                ColorizeText(TextABox, tile.IndexOnA, lenTile);
                ColorizeText(TextBBox, tile.IndexOnB, lenTile);
                /* */
                Console.WriteLine("Tile:{0}", tile);
                _indexColorLookup++;
            }
        }

        private void ResetColors()
        {
            ResetTextColor(TextABox);
            ResetTextColor(TextBBox);
        }

        private static void ColorizeText(RichTextBox box, Tile<GSTToken<char>> tile)
        {
            TextRange tr ;
            
            
            if("A" == box.Name)
            {
                /*
                tr = new TextRange(box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnA),
                                   box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnA + tile.Tokens.Count()));
                /* */
                box.Selection.Select(box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnA),
                                box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnA + tile.Tokens.Count()));
            }
            else
            {
                box.Selection.Select(box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnB),
                                box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnB + tile.Tokens.Count()));
                /*
                tr = new TextRange(box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnB),
                                   box.Document.ContentStart.GetPositionAtOffset(tile.IndexOnB + tile.Tokens.Count()));  
                /* */
            }

            box.Selection.ApplyPropertyValue(TextElement.ForegroundProperty,
                                                 new SolidColorBrush(
                                                     _ltForeground[_indexColorLookup % _ltForeground.Length]));

            box.Selection.ApplyPropertyValue(TextElement.BackgroundProperty,
                                                 new SolidColorBrush(
                                                     _ltBackground[_indexColorLookup % _ltForeground.Length]));
        }
        
        private static void ColorizeText(RichTextBox box, string inputText)
        {
            var textRange = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            var currentPosition = box.Document.ContentStart;
            var contentEnd = box.Document.ContentEnd;

            while(null != currentPosition && currentPosition.CompareTo(contentEnd) < 0 )
            {
                if(currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    var text = currentPosition.GetTextInRun(LogicalDirection.Forward);
                    
                    var p1 = currentPosition.GetPositionAtOffset(text.IndexOf(inputText));
                    var p2 = currentPosition.GetPositionAtOffset(text.IndexOf(inputText) + inputText.Length);

                    if (null != p2 && null != p1)
                    {
                        Console.WriteLine("ind:{0}, txt:{1}, p1: {2}, p2: {3}", text.IndexOf(inputText),
                                          text, p1, p2);
                        var range = new TextRange(p1, p2);


                        range.ApplyPropertyValue(TextElement.ForegroundProperty,
                                                 new SolidColorBrush(
                                                     _ltForeground[_indexColorLookup%_ltForeground.Length]));

                        range.ApplyPropertyValue(TextElement.BackgroundProperty,
                                                 new SolidColorBrush(
                                                     _ltBackground[_indexColorLookup%_ltForeground.Length]));
                    }
                }

                currentPosition = currentPosition.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
        
        private static void ColorizeText(RichTextBox box, int offset, int length)
        {
            var textRange = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            
            var startPos = GetPoint(box.Document.ContentStart, offset);
            
            var endPos = GetPoint(box.Document.ContentStart, offset + length);
            Console.WriteLine("{0},offset from start:{1}, end: {2}", box.Name, box.Document.ContentStart.GetOffsetToPosition(startPos), endPos);

            textRange.Select(startPos, endPos);

            textRange.ApplyPropertyValue(TextElement.ForegroundProperty,
                    new SolidColorBrush(_ltForeground[_indexColorLookup % _ltForeground.Length]));

            textRange.ApplyPropertyValue(TextElement.BackgroundProperty,
                    new SolidColorBrush(_ltBackground[_indexColorLookup % _ltForeground.Length]));
        }

        /// <summary>
        /// resets all data to initial state
        /// </summary>
        private void Reset()
        {
            StepButton.IsEnabled = true;
            RunButton.IsEnabled = true;
            labelFinished.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            ResetTextColor(TextABox);
            ResetTextColor(TextBBox);
            _indexColorLookup = 0;
        }

        /// <summary>
        /// resets the colors for all of the text in the given text box
        /// </summary>
        /// <param name="box"></param>
        private static void ResetTextColor(RichTextBox box)
        {
            var textRange = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            
            var startPos = GetPoint(box.Document.ContentStart, 0);


            textRange.Select(box.Document.ContentStart, box.Document.ContentEnd);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty,
                    new SolidColorBrush(Colors.Black));

            textRange.ApplyPropertyValue(TextElement.BackgroundProperty,
                    new SolidColorBrush(Colors.White));
        }

        private static TextPointer GetPoint(TextPointer start, int x)
        {
            if (null == start)
                throw new ArgumentNullException("start");

            var ret = start;

            while (x > 0)
            {
                //Console.WriteLine("{0}", ret.GetPointerContext(LogicalDirection.Backward));

                if (ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text ||
                    ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None)
                    x--;

                if (ret.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
                    return ret;
                ret = ret.GetPositionAtOffset(1, LogicalDirection.Forward);
            }
            return ret;
        }
    }
}
