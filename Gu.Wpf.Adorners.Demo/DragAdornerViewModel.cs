namespace Gu.Wpf.Adorners.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DragAdornerViewModel : INotifyPropertyChanged
    {
        private DragItem item1 = new DragItem("Kajsa");
        private DragItem item2;

        public event PropertyChangedEventHandler PropertyChanged;

        public DragItem Item1
        {
            get => this.item1;

            set
            {
                if (ReferenceEquals(value, this.item1))
                {
                    return;
                }

                this.item1 = value;
                this.OnPropertyChanged();
            }
        }

        public DragItem Item2
        {
            get => this.item2;

            set
            {
                if (ReferenceEquals(value, this.item2))
                {
                    return;
                }

                this.item2 = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}