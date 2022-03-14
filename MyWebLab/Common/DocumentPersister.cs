using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x02000017 RID: 23
    public class DocumentPersister
    {
        // Token: 0x060000C6 RID: 198 RVA: 0x00007454 File Offset: 0x00005654
        public static StringBuilder PrepareDocumentXML(BlockCollection blocks)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<DOC>");
            foreach (Block block in blocks)
            {
                Paragraph p = block as Paragraph;
                sb.Append("<Paragraph ");
                sb.Append("Name='" + p.Name + "' ");
                sb.Append("FontFamily='" + p.FontFamily + "' ");
                sb.Append("FontSize='" + p.FontSize + "' ");
                sb.Append("FontStretch='" + p.FontStretch + "' ");
                sb.Append("FontStyle='" + p.FontStyle + "' ");
                sb.Append("FontWeight='" + p.FontWeight + "' ");
                sb.Append("Foreground='" + (p.Foreground as SolidColorBrush).Color.ToString() + "' ");
                sb.Append(">");
                foreach (Inline inline in (block as Paragraph).Inlines)
                {
                    if (inline is Run)
                    {
                        sb.Append(DocumentPersister.prepareRun(inline as Run, false));
                    }
                    else if (inline is Hyperlink)
                    {
                        sb.Append(DocumentPersister.prepareHyperlink(inline as Hyperlink));
                    }
                    else if (inline is InlineUIContainer)
                    {
                        sb.Append(DocumentPersister.prepareImage(inline as InlineUIContainer));
                    }
                }
                sb.Append("</Paragraph>");
            }
            sb.Append("</DOC>");
            return sb;
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x000076D0 File Offset: 0x000058D0
        private static string prepareImage(InlineUIContainer r)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline Type='InlineUIContainer' ");
            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "' ");
            sb.Append("ImageWidth='" + (r.Child as Image).ActualWidth.ToString() + "' ");
            sb.Append("ImageHeight='" + (r.Child as Image).ActualHeight.ToString() + "' ");
            sb.Append("ImageSource='" + (r.Child as Image).Tag.ToString() + "'/>");
            return sb.ToString();
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x00007890 File Offset: 0x00005A90
        private static string prepareHyperlink(Hyperlink r)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline Type='Hyperlink' ");
            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("NavigateUri='" + r.NavigateUri + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "'>");
            sb.Append("<Inlines>");
            foreach (Inline inline in r.Inlines)
            {
                if (inline is Run)
                {
                    StringBuilder stringBuilder = sb;
                    bool isHyperlink = true;
                    stringBuilder.Append(DocumentPersister.prepareRun(inline as Run, isHyperlink));
                }
            }
            sb.Append("</Inlines>");
            sb.Append("</Inline>");
            return sb.ToString();
        }

        // Token: 0x060000C9 RID: 201 RVA: 0x00007A80 File Offset: 0x00005C80
        private static string prepareRun(Run r, bool IsHyperlink = false)
        {
            string suffix = "";
            if (IsHyperlink)
            {
                suffix = "HL";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline" + suffix + " Type='Run' ");
            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("Text='" + r.Text.Replace("<", "&lt").Replace(">", "&gt") + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "'/>");
            return sb.ToString();
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00007C10 File Offset: 0x00005E10
        public static void ParseSavedDocument(string xml, BlockCollection blocks)
        {
            if (string.IsNullOrEmpty(xml))
            {
                xml = "<DOC></DOC>";
            }
            else if (!xml.Contains("<DOC>"))
            {
                xml = "<DOC><Paragraph Name='' FontFamily='Portable User Interface' FontSize='11' FontStretch='Normal' FontStyle='Normal' FontWeight='Normal' Foreground='#FF000000'><Inline Type='Run' Name='' FontFamily='Portable User Interface' FontSize='11' FontStretch='Normal' FontStyle='Normal' FontWeight='Normal' Foreground='#FF000000' Text='" + xml + "' TextDecorations=''/></Paragraph></DOC>";
            }
            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);
            XDocument document = XDocument.Load(xr);
            foreach (XElement element in document.Descendants("Paragraph"))
            {
                Paragraph p = new Paragraph();
                p.FontFamily = new FontFamily(element.Attribute(XName.Get("FontFamily")).Value);
                p.FontSize = double.Parse(element.Attribute(XName.Get("FontSize")).Value);
                p.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(element.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
                p.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(element.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
                p.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(element.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
                string color = element.Attribute(XName.Get("Foreground")).Value;
                color = color.Remove(0, 1);
                p.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
                foreach (XElement inline in element.Descendants("Inline"))
                {
                    if (inline.Attribute(XName.Get("Type")).Value == "Run")
                    {
                        p.Inlines.Add(DocumentPersister.parseRun(inline));
                    }
                    else if (inline.Attribute(XName.Get("Type")).Value == "Hyperlink")
                    {
                        p.Inlines.Add(DocumentPersister.parseHyperlink(inline));
                    }
                    else if (inline.Attribute(XName.Get("Type")).Value == "InlineUIContainer")
                    {
                        p.Inlines.Add(DocumentPersister.parseImage(inline));
                    }
                }
                blocks.Add(p);
            }
            blocks.Remove(blocks.FirstBlock);
            sr.Close();
            xr.Close();
        }

        // Token: 0x060000CB RID: 203 RVA: 0x00007F98 File Offset: 0x00006198
        private static InlineUIContainer parseImage(XElement inline)
        {
            InlineUIContainer i = new InlineUIContainer();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
            {
                i.TextDecorations = TextDecorations.Underline;
            }
            string imageSource = WLConstants.SERVICE_ADDRESS + "ClientBin" + inline.Attribute(XName.Get("ImageSource")).Value;
            Image img = DocumentPersister.CreateImageFromUri(new Uri(imageSource, UriKind.RelativeOrAbsolute));
            img.Width = double.Parse(inline.Attribute(XName.Get("ImageWidth")).Value);
            img.Height = double.Parse(inline.Attribute(XName.Get("ImageHeight")).Value);
            i.Child = img;
            return i;
        }

        // Token: 0x060000CC RID: 204 RVA: 0x000081C4 File Offset: 0x000063C4
        private static Hyperlink parseHyperlink(XElement inline)
        {
            Hyperlink i = new Hyperlink();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
            {
                i.TextDecorations = TextDecorations.Underline;
            }
            i.NavigateUri = new Uri(inline.Attribute(XName.Get("NavigateUri")).Value);
            IEnumerable<XElement> ils = inline.Descendants(XName.Get("Inlines"));
            foreach (XElement item in ils.Descendants<XElement>())
            {
                if (item.Attribute(XName.Get("Type")).Value == "Run")
                {
                    i.Inlines.Add(DocumentPersister.parseRun(item));
                }
            }
            return i;
        }

        // Token: 0x060000CD RID: 205 RVA: 0x0000842C File Offset: 0x0000662C
        private static Run parseRun(XElement inline)
        {
            Run i = new Run();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
            {
                i.TextDecorations = TextDecorations.Underline;
            }
            i.Text = inline.Attribute(XName.Get("Text")).Value.Replace("&lt", "<").Replace("&gt", ">");
            return i;
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00008610 File Offset: 0x00006810
        public static Image CreateImageFromUri(Uri URI)
        {
            Image img = new Image();
            img.Stretch = Stretch.None;
            BitmapImage bi = new BitmapImage(URI);
            img.Source = bi;
            img.Tag = bi.UriSource.ToString();
            return img;
        }
    }
}
