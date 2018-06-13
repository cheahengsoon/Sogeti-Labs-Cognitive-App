using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CognitiveApp.ApiServices;
using CognitiveApp.Models;
using CognitiveApp.UtilServices;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace CognitiveApp.ViewModels {

    public class MainPageViewModel : BaseViewModel {

        private static readonly Random Rando = new Random();

        private const string NoScoreText = "No score";

        private const string NoTextText = "No info";

        private const string ScoreFormatter = "0%";

        private readonly CameraService _camService;
        private readonly FaceApiService _faceApiService;
        private readonly TextApiService _textApiService;
        private readonly GeneralImageApiService _generalImageApiService;

        private readonly List<Direction> _directions = new List<Direction> {
            new Direction {
                Name = "Make a happy face!",
                SecondaryName = "Make a happier face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Happiness
            }, new Direction {
                Name = "Make an angry face!",
                SecondaryName = "Make an angrier face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Anger
            }, new Direction {
                Name = "Make a disgusted face!",
                SecondaryName = "Make a more disgusted face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Disgust
            }, new Direction {
                Name = "Make a sad face!",
                SecondaryName = "Make a sadder face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Sadness
            }, new Direction {
                Name = "Make a surprised face!",
                SecondaryName = "Make a more surprised face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Surprise
            }, new Direction {
                Name = "Make a scared face!",
                SecondaryName = "Make a more scared face!",
                DirectionType = DirectionType.Face,
                EmotionType = EmotionType.Fear
            }, new Direction {
                Name = "Write something negative!",
                SecondaryName = "Write something more scared negative!",
                DirectionType = DirectionType.Text,
                EmotionType = EmotionType.Negative
            }, new Direction {
                Name = "Write something positive!",
                SecondaryName = "Write something more scared positive!",
                DirectionType = DirectionType.Text,
                EmotionType = EmotionType.Positive
            }, new Direction {
                Name = "Take a picture of anything!",
                SecondaryName = "",
                DirectionType = DirectionType.GeneralPicture,
                EmotionType = EmotionType.Neutral
            }
        };

        public string CognitiveServicesType {
            get {
                if(CurrentDirection == null) {
                    return "Cognitive Services Demo";
                }

                switch(CurrentDirection.DirectionType) {
                    case DirectionType.Face:
                        return "Service: Face API";

                    case DirectionType.GeneralPicture:
                        return "Service: Computer Vision";

                    case DirectionType.Shape:
                        return "Service: Ink Analysis";

                    case DirectionType.Text:
                        return "Service: Text Analytics";

                    default:
                        throw new ArgumentOutOfRangeException(nameof(CurrentDirection.DirectionType), "Unexpected direction type detected: " + Enum.GetName(typeof(DirectionType), CurrentDirection.DirectionType));
                }
            }
        }

        private bool _showResults;

        public bool ShowResults {
            get => _showResults;
            set {
                OnPropertyChanged(ref _showResults, value);
                OnPropertyChanged(nameof(ShowFaceResults));
                OnPropertyChanged(nameof(ShowTextResults));
                OnPropertyChanged(nameof(ShowPictureResults));
                OnPropertyChanged(nameof(ActionButtonText));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        public bool TryAgain { get; set; }

        public bool ShowFaceResults => ShowResults && _currentDirection != null && _currentDirection.DirectionType == DirectionType.Face;

        public bool ShowTextResults => ShowResults && _currentDirection != null && _currentDirection.DirectionType == DirectionType.Text;

        public bool ShowPictureResults => ShowResults && _currentDirection != null && _currentDirection.DirectionType == DirectionType.GeneralPicture;

        public bool ShowCurrentImage => _currentDirection != null && (_currentDirection.DirectionType == DirectionType.GeneralPicture ||  _currentDirection.DirectionType == DirectionType.Face);

        public bool ShowTextField => _currentDirection != null && _currentDirection.DirectionType == DirectionType.Text;

        public string ActionButtonText {
            get {
                if(CurrentDirection == null) {
                    return "Please click Next";
                }

                if(ShowResults) {
                    return TryAgain ? "Try Again" : "Try Another One";
                }

                switch(CurrentDirection.DirectionType) {
                    case DirectionType.Face:
                        return "Find a face";

                    case DirectionType.Text:
                        return "Submit some text";

                    case DirectionType.GeneralPicture:
                        return "Find a picture";

                    default:
                        throw new ArgumentOutOfRangeException(nameof(CurrentDirection.DirectionType), "Unexpected direction type detected: " + Enum.GetName(typeof(DirectionType), CurrentDirection.DirectionType));
                }
            }
        }

        private Direction _currentDirection;

        public Direction CurrentDirection {
            get => _currentDirection ?? (_currentDirection = _directions[0]);
            set {
                OnPropertyChanged(ref _currentDirection, value);
                OnPropertyChanged(nameof(ShowFaceResults));
                OnPropertyChanged(nameof(ShowTextResults));
                OnPropertyChanged(nameof(ShowPictureResults));
                OnPropertyChanged(nameof(ActionButtonText));
                OnPropertyChanged(nameof(CurrentImagePath));
                OnPropertyChanged(nameof(ShowCurrentImage));
                OnPropertyChanged(nameof(ShowTextField));
            }
        }

        private string CurrentImagePath => _currentDirection == null || !ShowResults ? null : _currentDirection.DirectionType == DirectionType.Face ? FaceAnswerPath : GeneralPicAnswerPath;

        private string _textAnswer;

        public string TextAnswer {
            get => _textAnswer ?? (_textAnswer = string.Empty);
            set => OnPropertyChanged(ref _textAnswer, value);
        }

        #region Face Answer

        private string _faceAnswerPath;

        public string FaceAnswerPath {
            get => _faceAnswerPath;
            set {
                OnPropertyChanged(ref _faceAnswerPath, value);
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        private EmotionScores _faceAnswerApiScores;
        public EmotionScores FaceAnswerApiScores {
            get => _faceAnswerApiScores;
            set {
                OnPropertyChanged(ref _faceAnswerApiScores, value);
                OnPropertyChanged(nameof(AngerScore));
                OnPropertyChanged(nameof(ContemptScore));
                OnPropertyChanged(nameof(DisgustScore));
                OnPropertyChanged(nameof(FearScore));
                OnPropertyChanged(nameof(HappinessScore));
                OnPropertyChanged(nameof(NeutralScore));
                OnPropertyChanged(nameof(SadnessScore));
                OnPropertyChanged(nameof(SurpriseScore));
            }
        }

        public string AngerScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Anger.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string ContemptScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Contempt.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string DisgustScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Disgust.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string FearScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Fear.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string HappinessScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Happiness.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string NeutralScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Neutral.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string SadnessScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Sadness.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string SurpriseScore => FaceAnswerApiScores == null ? NoScoreText : FaceAnswerApiScores.Surprise.ToString(ScoreFormatter, CultureInfo.InvariantCulture);

        #endregion

        #region Text Answer

        private double? _textAnswerScore;

        public double? TextAnswerScore {
            get => _textAnswerScore;
            set {
                OnPropertyChanged(ref _textAnswerScore, value);
                OnPropertyChanged(nameof(PositiveTextScore));
                OnPropertyChanged(nameof(NegativeTextScore));
            }
        }

        public string PositiveTextScore => TextAnswerScore == null ? NoScoreText : TextAnswerScore.Value.ToString(ScoreFormatter, CultureInfo.InvariantCulture);
        public string NegativeTextScore => TextAnswerScore == null ? NoScoreText : (1 - TextAnswerScore.Value).ToString(ScoreFormatter, CultureInfo.InvariantCulture);

        #endregion

        #region General Picture Answer

        private string _generalPicAnswerPath;

        public string GeneralPicAnswerPath {
            get => _generalPicAnswerPath;
            set {
                OnPropertyChanged(ref _generalPicAnswerPath, value);
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        private PictureApiResult _pictureAnswerApiScores;
        public PictureApiResult PictureAnswerApiScores {
            get => _pictureAnswerApiScores;
            set {
                OnPropertyChanged(ref _pictureAnswerApiScores, value);
                OnPropertyChanged(nameof(Captions));
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(Names));
            }
        }

        public string Captions => PictureAnswerApiScores?.Description?.Captions == null ? NoTextText : string.Join("\n", PictureAnswerApiScores.Description.Captions);

        public string Tags => PictureAnswerApiScores?.Description?.Tags == null ? NoTextText : string.Join("\n", PictureAnswerApiScores.Description.Tags);

        public string Names => PictureAnswerApiScores?.Categories == null ? NoTextText : string.Join("\n", PictureAnswerApiScores.Categories.Select(c => c.Name));

        #endregion

        public MainPageViewModel() {
            _camService = new CameraService();

            _faceApiService = new FaceApiService();
            _textApiService = new TextApiService();
            _generalImageApiService = new GeneralImageApiService();

            _directions.Shuffle(Rando);
        }

        public async Task<string> TakePhotoAsync() {
            string filePath = await _camService.TakePhotoAsync();

            if(filePath == null) {
                return null;
            }

            ClearAnswerInfo(true);

            SetImageAnswer(filePath);

            return filePath;
        }

        public async Task<string> PickPhotoAsync() {
            string filePath = await _camService.PickPhotoAsync();

            if(filePath == null) {
                return null;
            }

            ClearAnswerInfo(true);

            SetImageAnswer(filePath);

            return filePath;
        }

        public async Task<bool> TestFaceAsync() {
            Face[] facesResult = await _faceApiService.UploadAndDetectFaces(FaceAnswerPath);

            if(facesResult == null || facesResult.Length < 1) {
                throw new Exception("No face found in photo.");
            }

            if(facesResult.Length > 1) {
                throw new Exception("Too many faces found in the photo.");
            }

            FaceAnswerApiScores = facesResult[0].FaceAttributes.Emotion;

            return Direction.DirectionTypeToFaceApiProp(CurrentDirection.EmotionType, facesResult[0].FaceAttributes.Emotion) >= 0.5;
        }

        public async Task<bool> TestTextAsync() {
            string answer = TextAnswer;

            if(string.IsNullOrWhiteSpace(answer)) {
                throw new Exception("Please enter a sentence in the text field.");
            }

            SentimentBatchResult textResult = await _textApiService.UploadAndDetectText(TextAnswer, new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token);

            if(textResult?.Documents == null || textResult.Documents.Count < 1 || textResult.Documents[0].Score == null) {
                throw new Exception("An error occurred.");
            }

            TextAnswerScore = textResult.Documents[0].Score;

            return CurrentDirection.EmotionType == EmotionType.Positive ? textResult.Documents[0].Score.Value >= 0.5 : 1 - textResult.Documents[0].Score.Value >= 0.5;
        }

        public async Task TestPhotoAsync() {
            PictureAnswerApiScores = await _generalImageApiService.MakeAnalysisRequestAsync(GeneralPicAnswerPath,  new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token);
        }

        public void SetNextDirection() {
            int currentDirectionIndex = _directions.IndexOf(CurrentDirection);
            int nextDirectionIndex;

            if(currentDirectionIndex + 1 >= _directions.Count) {
                nextDirectionIndex = 0;
            } else {
                nextDirectionIndex = currentDirectionIndex + 1;
            }

            CurrentDirection = _directions[nextDirectionIndex];
        }

        public void ClearAnswerInfo(bool clearInputField = false) {
            if(!string.IsNullOrEmpty(FaceAnswerPath)) {
                _camService.DeletePhoto(FaceAnswerPath);
            }

            TryAgain = false;

            ShowResults = false;

            if(clearInputField) {
                TextAnswer = null;
            }

            FaceAnswerPath = null;
            FaceAnswerApiScores = null;
            GeneralPicAnswerPath = null;
            TextAnswerScore = null;
        }

        public void ResetSecondaryText() {
            CurrentDirection.ShowSecondaryName = false;
        }

        private void SetImageAnswer(string imagePath) {
            switch(CurrentDirection.DirectionType) {
                case DirectionType.Face:
                    FaceAnswerPath = imagePath;
                    break;

                case DirectionType.GeneralPicture:
                    GeneralPicAnswerPath = imagePath;
                    break;
            }
        }
    }

    public static class ListExtensions {
        /// <summary>
        /// https://stackoverflow.com/a/22668974/3850012
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, Random rnd) {
            for(int i = 0; i < list.Count; i++) {
                list.Swap(i, rnd.Next(i, list.Count));
            }
        }

        private static void Swap<T>(this IList<T> list, int i, int j) {
            T temp  = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
