namespace Group1RazorPages.ViewModels
{
    public class TextInputViewModel
    {
        public TextInputViewModel(string propertyName, string inputType, string label, string placeholder)
        {
            PropertyName = propertyName;
            InputType = inputType;
            Label = label;
            Placeholder = placeholder;
        }

        public string PropertyName { get; set; }
        public string InputType { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
    }
}
