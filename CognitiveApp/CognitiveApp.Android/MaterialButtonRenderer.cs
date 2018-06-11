using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using CognitiveApp.Controls;
using CognitiveApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ButtonRenderer = Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer;

[assembly: ExportRenderer(typeof(MaterialButton), typeof(MaterialButtonRenderer))]
namespace CognitiveApp.Droid {

    public class MaterialButtonRenderer : ButtonRenderer {

        public MaterialButtonRenderer(Context context) : base(context) { }

        /// <summary>
        /// Set up the elevation from load
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e) {
            base.OnElementChanged(e);

            if(e.NewElement == null) {
                return;
            }

            MaterialButton materialButton = (MaterialButton)Element;

            // we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            // set the elevation manually
            ViewCompat.SetElevation(this, materialButton.Elevation);
            ViewCompat.SetElevation(Control, materialButton.Elevation);
        }

        public override void Draw(Canvas canvas) {
            MaterialButton materialButton = (MaterialButton)Element;
            Control.Elevation = materialButton.Elevation;

            base.Draw(canvas);
        }

        /// <summary>
        /// Update the elevation when updated from Xamarin.Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == nameof(MaterialButton.ElevationProperty.PropertyName)) {
                MaterialButton materialButton = (MaterialButton)Element;
                ViewCompat.SetElevation(this, materialButton.Elevation);
                ViewCompat.SetElevation(Control, materialButton.Elevation);
                UpdateLayout();
            }
        }
    }
}
