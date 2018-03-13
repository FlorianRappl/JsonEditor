namespace JsonEditor.App.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    sealed class FileDropBehavior : Behavior<FrameworkElement>
    {
        public IFileDropTarget Target
        {
            get { return AssociatedObject?.DataContext as IFileDropTarget; }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += OnDrop;
            AssociatedObject.DragOver += OnDragOver;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Drop -= OnDrop;
            AssociatedObject.DragOver -= OnDragOver;
        }

        private void OnDrop(Object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as String[];
                Target?.Dropped(files);
            }
        }

        private void OnDragOver(Object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? 
                DragDropEffects.Link : 
                DragDropEffects.None;
        }
    }
}
