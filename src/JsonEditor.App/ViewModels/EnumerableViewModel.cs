namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    abstract class EnumerableViewModel<TToken, TViewModel> : TokenViewModel, ITokenContainer, IEnumerable<ItemViewModel>
        where TToken : JToken
        where TViewModel : ItemViewModel
    {
        private readonly ObservableCollection<TViewModel> _children;
        private Boolean _expanded;

        public EnumerableViewModel(TToken value, ITokenContainer container)
            : base(value, container)
        {
            var items = ItemsOf(value);
            _children = new ObservableCollection<TViewModel>(items);
            _expanded = true;
        }

        public Boolean IsExpanded
        {
            get { return _expanded; }
            set { _expanded = value; RaisePropertyChanged(); }
        }

        public IEnumerable<TViewModel> Children
        {
            get { return _children; }
        }

        public TToken Value
        {
            get { return (TToken)Token; }
        }

        public IEnumerator<ItemViewModel> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public void Replace(JToken original, JToken replacement)
        {
            if (!Object.ReferenceEquals(original, replacement))
            {
                PerformReplace(original, replacement);
                IsChanged = true;
            }
        }

        protected override void SetSchema(JSchema schema)
        {
            base.SetSchema(schema);

            foreach (var child in _children)
            {
                var childSchema = schema?.GetItemSchema();
                child.Validate(childSchema);
            }
        }

        protected void AddNewItem(TViewModel item)
        {
            _children.Add(item);
            IsChanged = true;
        }

        protected void RemoveExistingItem(TViewModel item)
        {
            _children.Remove(item);
            IsChanged = true;
        }

        protected void PerformMoveItemDown(TViewModel item)
        {
            var index = _children.IndexOf(item);

            if (index > -1 && index < _children.Count - 1)
            {
                var next = index + 1;
                var tmp = _children[next];
                _children[next] = item;
                _children[index] = tmp;
                IsChanged = true;
            }
        }

        protected void PerformMoveItemUp(TViewModel item)
        {
            var index = _children.IndexOf(item);

            if (index > 0 && index < _children.Count)
            {
                var previous = index - 1;
                var tmp = _children[previous];
                _children[previous] = item;
                _children[index] = tmp;
                IsChanged = true;
            }
        }

        protected abstract IEnumerable<TViewModel> ItemsOf(TToken token);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract void PerformReplace(JToken original, JToken replacement);
    }
}
