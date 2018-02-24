using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows.Media;
using FontAwesome.WPF;

namespace Brain2CPU.WPF.Helper
{
    public class FontAwesomeImageSourceExtension : MarkupExtension
    {
        private static readonly IDictionary<string, FontAwesomeIcon> ClassNameLookup =
            new Dictionary<string, FontAwesomeIcon>(StringComparer.OrdinalIgnoreCase);

        static FontAwesomeImageSourceExtension()
        {
            foreach(object obj in Enum.GetValues(typeof(FontAwesomeIcon)))
            {
                object[] customAttributes = typeof(FontAwesomeIcon).GetMember(obj.ToString())[0].GetCustomAttributes(typeof(IconIdAttribute), false);
                if(customAttributes.Length != 0)
                {
                    string id = ((IconIdAttribute)customAttributes[0]).Id;
                    if(!ClassNameLookup.ContainsKey(id))
                    {
                        ClassNameLookup.Add(id, (FontAwesomeIcon)obj);
                    }
                }
            }
        }

        public static Brush DefaultForegroundBrush { get; set; } = Brushes.Black;

        private readonly string _name;
        private readonly Brush _foregroundBrush;

        public FontAwesomeImageSourceExtension(string name) : this(name, DefaultForegroundBrush)
        {
        }

        public FontAwesomeImageSourceExtension(string name, Brush foreground)
        {
            _name = name;
            _foregroundBrush = foreground;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            FontAwesomeIcon fontAwesomeIcon;

            if(string.IsNullOrEmpty(_name))
                fontAwesomeIcon = FontAwesomeIcon.None;
            else
                if(!ClassNameLookup.TryGetValue(_name, out fontAwesomeIcon))
                    fontAwesomeIcon = FontAwesomeIcon.Square;

            return ImageAwesome.CreateImageSource(fontAwesomeIcon, _foregroundBrush);
        }
    }
}
