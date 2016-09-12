using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using TigeR.YuGiOh.Core.Effects;

namespace DeckBuilder
{
    /// <summary>
    /// Interaktionslogik für EffectEditorWindow.xaml
    /// </summary>
    public partial class EffectEditorWindow : Window
    {
        public EffectEditorWindow()
        {
            InitializeComponent();

            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("DeckBuilder.Resources.Lua.xshd"))
            using (var reader = new XmlTextReader(s))
            {
                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
