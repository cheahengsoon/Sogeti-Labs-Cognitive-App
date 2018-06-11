using System.Globalization;
using CognitiveApp.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Xamarin.Forms;
using XFShapeView;
using Color = Xamarin.Forms.Color;

namespace CognitiveApp.Controls {
    public class FaceAnswerView : ShapeView {

        private const string ScoreFormatter = "0%";

        private const string NoTextText = "No info";

        private const double DefaultLabelFontSize = 14;
        private const double DefaultValueFontSize = 16;

        private static readonly Color DefaultLabelColor = Color.FromHex("#00398B");
        private static readonly Color DefaultValueColor = Color.FromHex("#414346");

        #region Labels

        private readonly Label _angerLabel = new Label {
            Text = "Anger",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _contemptLabel = new Label {
            Text = "Contempt",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _disgustLabel = new Label {
            Text = "Disgust",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _fearLabel = new Label {
            Text = "Fear",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _happinessLabel = new Label {
            Text = "Happiness",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _neutralLabel = new Label {
            Text = "Neutral",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _sadnessLabel = new Label {
            Text = "Sadness",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        private readonly Label _surpriseLabel = new Label {
            Text = "Surprise",
            FontSize = DefaultLabelFontSize,
            TextColor = DefaultLabelColor
        };

        #endregion

        #region Values

        private readonly Label _anger = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _contempt = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _disgust = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _fear = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _happiness = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _neutral = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _sadness = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _surprise = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        #endregion

        public static readonly BindableProperty AnswerProperty = BindableProperty.Create(nameof(Answer), typeof(EmotionScores), typeof(FaceAnswerView), default(EmotionScores), BindingMode.TwoWay, propertyChanged: (bindable, value, newValue) => {
            EmotionScores score = newValue as EmotionScores;

            FaceAnswerView view = (FaceAnswerView)bindable;

            if(view._anger != null) {
                view._anger.Text = FormatScore(score?.Anger);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Anger ? FontAttributes.Bold : FontAttributes.None;

                view._angerLabel.FontAttributes = attribute;
                view._anger.FontAttributes = attribute;
            }

            if(view._contempt != null) {
                view._contempt.Text = FormatScore(score?.Contempt);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Contempt ? FontAttributes.Bold : FontAttributes.None;

                view._contemptLabel.FontAttributes = attribute;
                view._contempt.FontAttributes = attribute;
            }

            if(view._disgust != null) {
                view._disgust.Text = FormatScore(score?.Disgust);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Disgust ? FontAttributes.Bold : FontAttributes.None;

                view._disgustLabel.FontAttributes = attribute;
                view._disgust.FontAttributes = attribute;
            }

            if(view._fear != null) {
                view._fear.Text = FormatScore(score?.Fear);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Fear ? FontAttributes.Bold : FontAttributes.None;

                view._fearLabel.FontAttributes = attribute;
                view._fear.FontAttributes = attribute;
            }

            if(view._happiness != null) {
                view._happiness.Text = FormatScore(score?.Happiness);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Happiness ? FontAttributes.Bold : FontAttributes.None;

                view._happinessLabel.FontAttributes = attribute;
                view._happiness.FontAttributes = attribute;
            }

            if(view._neutral != null) {
                view._neutral.Text = FormatScore(score?.Neutral);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Neutral ? FontAttributes.Bold : FontAttributes.None;

                view._neutralLabel.FontAttributes = attribute;
                view._neutral.FontAttributes = attribute;
            }

            if(view._sadness != null) {
                view._sadness.Text = FormatScore(score?.Sadness);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Sadness ? FontAttributes.Bold : FontAttributes.None;

                view._sadnessLabel.FontAttributes = attribute;
                view._sadness.FontAttributes = attribute;
            }

            if(view._surprise != null) {
                view._surprise.Text = FormatScore(score?.Surprise);

                FontAttributes attribute = view.CurrentEmotionType == EmotionType.Surprise ? FontAttributes.Bold : FontAttributes.None;

                view._surpriseLabel.FontAttributes = attribute;
                view._surprise.FontAttributes = attribute;
            }
        });

        public EmotionScores Answer {
            get => (EmotionScores)GetValue(AnswerProperty);
            set => SetValue(AnswerProperty, value);
        }

        public static readonly BindableProperty CurrentEmotionTypeProperty = BindableProperty.Create(nameof(CurrentEmotionType), typeof(EmotionType), typeof(FaceAnswerView), EmotionType.Neutral, BindingMode.TwoWay);

        public EmotionType CurrentEmotionType {
            get => (EmotionType)GetValue(CurrentEmotionTypeProperty);
            set => SetValue(CurrentEmotionTypeProperty, value);
        }

        public FaceAnswerView() {
            ShapeType = ShapeType.Box;
            Color = Color.FromHex("#E5FFFFFF");
            CornerRadius = 3;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;

            Grid grid = new Grid {
                Padding = 7,
                RowDefinitions = new RowDefinitionCollection {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }, ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            grid.Children.Add(_angerLabel, 0, 0);
            grid.Children.Add(_anger, 1, 0);

            grid.Children.Add(_contemptLabel, 0, 1);
            grid.Children.Add(_contempt, 1, 1);

            grid.Children.Add(_disgustLabel, 0, 2);
            grid.Children.Add(_disgust, 1, 2);

            grid.Children.Add(_fearLabel, 0, 3);
            grid.Children.Add(_fear, 1, 3);

            grid.Children.Add(_happinessLabel, 2, 0);
            grid.Children.Add(_happiness, 3, 0);

            grid.Children.Add(_neutralLabel, 2, 1);
            grid.Children.Add(_neutral, 3, 1);

            grid.Children.Add(_sadnessLabel, 2, 2);
            grid.Children.Add(_sadness, 3, 2);

            grid.Children.Add(_surpriseLabel, 2, 3);
            grid.Children.Add(_surprise, 3, 3);

            Content = grid;
        }

        private static string FormatScore(float? score = null) {
            return score?.ToString(ScoreFormatter, CultureInfo.InvariantCulture) ?? NoTextText;
        }
    }
}
