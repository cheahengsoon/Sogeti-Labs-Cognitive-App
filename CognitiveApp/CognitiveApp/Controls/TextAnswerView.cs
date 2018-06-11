using System.Globalization;
using CognitiveApp.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Xamarin.Forms;
using XFShapeView;
using Color = Xamarin.Forms.Color;

namespace CognitiveApp.Controls {

    public class TextAnswerView : ShapeView {
        private const string ScoreFormatter = "0%";

        private const string NoTextText = "No score";

        private const double DefaultLabelFontSize = 14;
        private const double DefaultValueFontSize = 16;

        private static readonly Color DefaultLabelColor = Color.FromHex("#00398B");
        private static readonly Color DefaultValueColor = Color.FromHex("#414346");

        private readonly Label _positiveLabel = new Label {
            Text = "Positive",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _negativeLabel = new Label {
            Text = "Negative",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _positive = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _negative = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        //private readonly Image _answerImage = new Image {
        //    Aspect = Aspect.AspectFit,
        //    HorizontalOptions = LayoutOptions.Center,
        //    VerticalOptions = LayoutOptions.Center
        //};

        public static readonly BindableProperty AnswerProperty = BindableProperty.Create(nameof(Answer), typeof(double?), typeof(TextAnswerView), default(EmotionScores), BindingMode.TwoWay, propertyChanged: (bindable, value, newValue) => {
            double? score = newValue as double?;

            TextAnswerView view = (TextAnswerView)bindable;

            if(view._positive != null) {
                view._positive.Text = score?.ToString(ScoreFormatter, CultureInfo.InvariantCulture);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Positive ? FontAttributes.Bold : FontAttributes.None;

                view._positiveLabel.FontAttributes = attribute;
                view._positive.FontAttributes = attribute;
            }

            if(view._negative != null) {
                view._negative.Text = (1 - score)?.ToString(ScoreFormatter, CultureInfo.InvariantCulture);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Negative ? FontAttributes.Bold : FontAttributes.None;

                view._negativeLabel.FontAttributes = attribute;
                view._negative.FontAttributes = attribute;
            }
        });

        public double? Answer {
            get => (double?)GetValue(AnswerProperty);
            set => SetValue(AnswerProperty, value);
        }

        public static readonly BindableProperty CurrentEmotionTypeProperty = BindableProperty.Create(nameof(CurrentEmotionType), typeof(EmotionType), typeof(TextAnswerView), EmotionType.Neutral, BindingMode.TwoWay);

        public EmotionType CurrentEmotionType {
            get => (EmotionType)GetValue(CurrentEmotionTypeProperty);
            set => SetValue(CurrentEmotionTypeProperty, value);
        }

        public TextAnswerView() {
            ShapeType = ShapeType.Box;
            Color = Color.FromHex("#FFFFFF");
            CornerRadius = 3;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;

            Grid grid = new Grid {
                Padding = 7,
                RowDefinitions = new RowDefinitionCollection {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }, ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            grid.Children.Add(_positiveLabel, 0, 0);
            grid.Children.Add(_positive, 1, 0);

            grid.Children.Add(_negativeLabel, 0, 1);
            grid.Children.Add(_negative, 1, 1);

            Content = grid;
        }
    }
}
