using System;
using Microsoft.ProjectOxford.Common.Contract;

namespace CognitiveApp.Models {
    public enum DirectionType {
        Text = 0,
        //Voice,
        Shape,
        Face,
        GeneralPicture
    }

    public enum EmotionType {
        Anger = 0,
        Contempt,
        Disgust,
        Fear,
        Happiness,
        Neutral,
        Sadness,
        Surprise,
        Positive,
        Negative
    }

    public class Direction {
        public string Name { get; set; }
        public string SecondaryName { get; set; }
        public bool ShowSecondaryName { get; set; }

        public string FinalName => ShowSecondaryName ? SecondaryName : Name;

        public string EmotionDescription {
            get {
                switch(EmotionType) {
                    case EmotionType.Anger:
                        return "Angry";

                    case EmotionType.Contempt:
                        return "Contempt";

                    case EmotionType.Disgust:
                        return "Disgusted";

                    case EmotionType.Fear:
                        return "Fearful";

                    case EmotionType.Happiness:
                        return "Happiness";

                    case EmotionType.Neutral:
                        return "Meh";

                    case EmotionType.Sadness:
                        return "Sad";

                    case EmotionType.Surprise:
                        return "Surprised";

                    case EmotionType.Positive:
                        return "Positive";

                    case EmotionType.Negative:
                        return "Negative";

                    default:
                        throw new ArgumentOutOfRangeException(nameof(EmotionType));
                }
            }
        }

        public DirectionType DirectionType { get; set; }
        public EmotionType EmotionType { get; set; }

        public static float DirectionTypeToFaceApiProp(EmotionType type, EmotionScores emotion) {
            switch(type) {
                case EmotionType.Anger:
                    return emotion.Anger;

                case EmotionType.Contempt:
                    return emotion.Contempt;

                case EmotionType.Disgust:
                    return emotion.Disgust;

                case EmotionType.Fear:
                    return emotion.Fear;

                case EmotionType.Happiness:
                    return emotion.Happiness;

                case EmotionType.Neutral:
                    return emotion.Neutral;

                case EmotionType.Sadness:
                    return emotion.Sadness;

                case EmotionType.Surprise:
                    return emotion.Surprise;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
