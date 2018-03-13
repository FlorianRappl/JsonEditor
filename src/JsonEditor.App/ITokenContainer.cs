namespace JsonEditor.App
{
    using Newtonsoft.Json.Linq;
    using System;

    interface ITokenContainer
    {
        Boolean IsChanged { get; set; }

        void Replace(JToken original, JToken replacement);
    }
}
