using Xamarin.Forms;

namespace CognitiveApp.Controls {
    public class MaterialButton : Button {
        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(MaterialButton), 4.0f);

        public float Elevation {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }
    }
}
