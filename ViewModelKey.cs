using System.ComponentModel;

namespace CapHotkey
{
    public class ViewModelKey : INotifyPropertyChanged
    {
        string textData { get; set; } = "EMPTY";
        public event PropertyChangedEventHandler? PropertyChanged;

        private static readonly PropertyChangedEventArgs NamePropertyChangedEventArgs = new(nameof(TextData));
        public string TextData
        {
            get { return textData; }
            set
            {
                if (textData == value) { return; }
                textData = value;
                PropertyChanged?.Invoke(this, NamePropertyChangedEventArgs);
            }
        }
    }
}
