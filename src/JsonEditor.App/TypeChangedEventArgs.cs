namespace JsonEditor.App
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class TypeChangedEventArgs : EventArgs
    {
        public TypeChangedEventArgs(String type)
        {
            NewType = type;
        }

        public JToken NewToken
        {
            get { return CreateNew(NewType); }
        }

        public String NewType
        {
            get;
            private set;
        }

        private static JToken CreateNew(String type)
        {
            switch (type)
            {
                case "integer": return new JValue(0);
                case "number":  return new JValue(0.0);
                case "boolean": return new JValue(false);
                case "string":  return JValue.CreateString(String.Empty);
                case "object":  return new JObject();
                case "array":   return new JArray();
                case "enum":    return JValue.CreateNull();
            }

            return null;
        }
    }
}
