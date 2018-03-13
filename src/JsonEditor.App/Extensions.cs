namespace JsonEditor.App
{
    using JsonEditor.App.ViewModels;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Media;

    static class Extensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) 
            where T : DependencyObject
        {
            if (depObj != null)
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (var childOfChild in child.FindVisualChildren<T>())
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void Validate(this ItemViewModel item, JSchema schema)
        {
            item.Value.Schema = schema;
            item.Validation.Validate(schema, item.Value.Token);
        }

        public static void Saved(this TokenViewModel vm)
        {
            var items = vm as IEnumerable<ItemViewModel>;

            if (items != null)
            {
                foreach (var item in items)
                {
                    item.Value.Saved();
                }
            }
            else
            {
                vm.IsChanged = false;
            }
        }

        public static JSchema GetItemSchema(this JSchema schema)
        {
            var result = schema;

            if (schema != null)
            {
                result = schema.Items.FirstOrDefault() ?? schema.AdditionalItems;
            }

            return result;
        }

        public static JSchema GetPropertySchema(this JSchema schema, String propertyName)
        {
            var result = schema;

            if (schema != null && !schema.Properties.TryGetValue(propertyName, out result))
            {
                var patternProperties = schema?.PatternProperties;

                if (patternProperties != null)
                {
                    foreach (var pattern in patternProperties)
                    {
                        var regex = new Regex(pattern.Key, RegexOptions.ECMAScript | RegexOptions.CultureInvariant);

                        if (regex.IsMatch(propertyName))
                        {
                            result = pattern.Value;
                            break;
                        }
                    }
                }

                if (result == null)
                {
                    result = schema.AdditionalProperties;
                }
            }

            return result;
        }

        public static JToken CreateInstance(this JSchema schema)
        {
            var type = schema.Type;

            if (!type.HasValue)
            {
                if (schema.Properties.Count > 0 || schema.PatternProperties.Count > 0 || schema.AdditionalProperties != null)
                {
                    type = JSchemaType.Object;
                }
                //else if (schema.)
            }

            if (type.HasValue)
            {
                switch (type.Value)
                {
                    case JSchemaType.Number:
                        return new JValue(0.0);
                    case JSchemaType.Boolean:
                        return new JValue(false);
                    case JSchemaType.String:
                        return new JValue(String.Empty);
                    case JSchemaType.Object:
                        return new JObject();
                    case JSchemaType.Array:
                        return new JArray();
                }
            }

            return schema.Enum.FirstOrDefault() ?? schema.Default;
        }
    }
}
