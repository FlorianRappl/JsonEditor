namespace JsonEditor.App.Converters
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    sealed class JsonTokenConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var token = value as JToken;

            if (token == null)
            {
                var str = value as String;
                return str ?? String.Empty;
            }
            else if (token.Type == JTokenType.Null)
            {
                return "<null>";
            }

            return token.ToString();
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
