namespace ParseTreeVisualizer.Util {
    public class Selectable<TModel> : ViewModelBase<TModel> {
        public Selectable(TModel model) : base(model) { }

        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => NotifyChanged(ref isSelected, value);
        }
    }
}
