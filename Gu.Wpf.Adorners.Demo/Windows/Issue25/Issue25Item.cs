namespace Gu.Wpf.Adorners.Demo.Windows.Issue25
{
    using System.Globalization;

    public class Issue25Item
    {
        public Issue25Item(int value)
        {
            this.Value = value;
        }

        public int Value { get; }

        public string Text => this.Value.ToString(CultureInfo.InvariantCulture);
    }
}
