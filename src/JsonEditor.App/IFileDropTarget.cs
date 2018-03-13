namespace JsonEditor.App
{
    using System;
    using System.Collections.Generic;

    interface IFileDropTarget
    {
        void Dropped(IEnumerable<String> files);
    }
}