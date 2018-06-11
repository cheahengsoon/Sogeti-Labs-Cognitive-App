using System.Linq;
using CognitiveApp.Models;
using Xamarin.Forms;
using XFShapeView;
using Color = Xamarin.Forms.Color;

namespace CognitiveApp.Controls {

    public class PictureAnswerView : ShapeView {
        private const string NoTextText = "No info";

        private const double DefaultLabelFontSize = 14;
        private const double DefaultValueFontSize = 12;

        private static readonly Color DefaultLabelColor = Color.FromHex("#00398B");
        private static readonly Color DefaultValueColor = Color.FromHex("#414346");

        private readonly Label _names = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _tags = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        private readonly Label _captions = new Label {
            FontSize = DefaultValueFontSize,
            TextColor = DefaultValueColor,
            Text = NoTextText
        };

        public static readonly BindableProperty AnswerProperty = BindableProperty.Create(nameof(Answer), typeof(PictureApiResult), typeof(PictureAnswerView), default(PictureApiResult), BindingMode.TwoWay, propertyChanged: (bindable, value, newValue) => {
            PictureApiResult result = newValue as PictureApiResult;

            PictureAnswerView view = (PictureAnswerView)bindable;

            if(view._names != null) {
                view._names.Text = result?.Categories == null ? NoTextText : string.Join(", ", result.Categories.Select(c => c.Name));
            }

            if(view._tags != null) {
                view._tags.Text = result?.Description?.Tags == null ? NoTextText : string.Join(", ", result.Description.Tags);
            }

            if(view._captions != null) {
                view._captions.Text = result?.Description?.Captions == null ? NoTextText : string.Join("\n", result.Description.Captions);
            }
        });

        public PictureApiResult Answer {
            get => (PictureApiResult)GetValue(AnswerProperty);
            set => SetValue(AnswerProperty, value);
        }

        public PictureAnswerView() {
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
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }, ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            #region Labels

            Label namesLabel = new Label {
                Text = "Names",
                FontSize = DefaultLabelFontSize,
                TextColor = DefaultLabelColor
            };

            Label tagsLabel = new Label {
                Text = "Tags",
                FontSize = DefaultLabelFontSize,
                TextColor = DefaultLabelColor
            };

            Label captionsLabel = new Label {
                Text = "Captions",
                FontSize = DefaultLabelFontSize,
                TextColor = DefaultLabelColor
            };

            #endregion

            grid.Children.Add(namesLabel, 0, 0);
            grid.Children.Add(_names, 0, 1);

            grid.Children.Add(tagsLabel, 0, 2);
            grid.Children.Add(_tags, 0, 3);

            grid.Children.Add(captionsLabel, 0, 4);
            grid.Children.Add(_captions, 0, 5);

            Content = new ScrollView {
                Content = grid
            };
        }
    }
}
