namespace Gu.Wpf.Adorners.Demo.Windows.Issue25
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Issue25ViewModel
    {
        public ObservableCollection<Issue25Item> People { get; } = new ObservableCollection<Issue25Item>(Enumerable.Range(0, 100).Select(x => new Issue25Item(x)));
    }
}
