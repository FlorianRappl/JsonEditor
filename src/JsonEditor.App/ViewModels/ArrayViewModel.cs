namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    sealed class ArrayViewModel : EnumerableViewModel<JArray, ItemViewModel>
    {
        public ArrayViewModel(JArray token, ITokenContainer container)
            : base(token, container)
        {
            AddItem = Cmd(PerformAddItem);
            RemoveItem = Cmd<ItemViewModel>(PerformRemoveItem);
            MoveItemUp = Cmd<ItemViewModel>(PerformMoveItemUp);
            MoveItemDown = Cmd<ItemViewModel>(PerformMoveItemDown);
        }

        public ICommand AddItem { get; }

        public ICommand RemoveItem { get; }

        public ICommand MoveItemUp { get; }

        public ICommand MoveItemDown { get; }

        protected override IEnumerable<ItemViewModel> ItemsOf(JArray token)
        {
            return token.Children().Select(item => new ItemViewModel(item, this));
        }

        private void PerformAddItem()
        {
            var schema = Schema?.GetItemSchema();
            var value = schema?.CreateInstance() ?? JValue.CreateString("");
            var item = new ItemViewModel(value, this);
            item.Value.Schema = schema;
            Value.Add(value);
            AddNewItem(item);
        }

        private void PerformRemoveItem(ItemViewModel item)
        {
            Value.Remove(item.Value.Token);
            RemoveExistingItem(item);
        }

        protected override void PerformReplace(JToken original, JToken replacement)
        {
            var arr = Value;
            var index = arr.IndexOf(original);
            arr.RemoveAt(index);
            arr.Insert(index, replacement);
        }
    }
}
