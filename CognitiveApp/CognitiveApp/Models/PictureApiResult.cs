using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace CognitiveApp.Models {

    public class PictureApiResult {
        [JsonProperty("categories")]
        public List<Category> Categories {
            get; set;
        }

        [JsonProperty("description")]
        public Description Description {
            get; set;
        }

        [JsonProperty("color")]
        public Color Color {
            get; set;
        }

        [JsonProperty("requestId")]
        public string RequestId {
            get; set;
        }

        [JsonProperty("metadata")]
        public Metadata Metadata {
            get; set;
        }
    }

    public class Category {
        [JsonProperty("name")]
        public string Name {
            get; set;
        }

        [JsonProperty("score")]
        public double Score {
            get; set;
        }

        [JsonProperty("detail")]
        public Detail Detail { get; set; }
    }

    public class Color {
        [JsonProperty("dominantColorForeground")]
        public string DominantColorForeground {
            get; set;
        }

        [JsonProperty("dominantColorBackground")]
        public string DominantColorBackground {
            get; set;
        }

        [JsonProperty("dominantColors")]
        public List<string> DominantColors {
            get; set;
        }

        [JsonProperty("accentColor")]
        public string AccentColor {
            get; set;
        }

        [JsonProperty("isBwImg")]
        public bool IsBwImg {
            get; set;
        }
    }

    public class Description {
        [JsonProperty("tags")]
        public List<string> Tags {
            get; set;
        }

        [JsonProperty("captions")]
        public List<Caption> Captions {
            get; set;
        }
    }

    public class Caption {
        [JsonProperty("text")]
        public string Text {
            get; set;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get; set;
        }

        public override string ToString() {
            return Text + " Confidence: " + Confidence.ToString("0%", CultureInfo.InvariantCulture);
        }

    }

    public class Detail {
        [JsonProperty("landmarks")]
        public List<object> Landmarks { get; set; }
    }

    public class Metadata {
        [JsonProperty("height")]
        public long Height {
            get; set;
        }

        [JsonProperty("width")]
        public long Width {
            get; set;
        }

        [JsonProperty("format")]
        public string Format {
            get; set;
        }
    }
}
