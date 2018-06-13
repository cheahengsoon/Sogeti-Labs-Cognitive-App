using System;
using System.Threading.Tasks;
using CognitiveApp.Models;
using CognitiveApp.ViewModels;
using Xamarin.Forms;

namespace CognitiveApp {

    public partial class MainPage : ContentPage {
        private readonly MainPageViewModel _viewModel;

        public MainPage() {
            BindingContext = _viewModel = new MainPageViewModel();
            InitializeComponent();
        }

        private async void OnClicked(object sender, EventArgs e) {

            if(_viewModel.CurrentDirection == null) {   //If this is null we cannot go much further without failing
                return;
            }

            if(_viewModel.ShowResults && !_viewModel.TryAgain) {    //If the user has correctly answered the request, then send them to a new one
                OnSkipClicked(null, null);
                return;
            }

            IsBusy = true;

            _viewModel.ClearAnswerInfo();

            bool? result;

            try {
                switch(_viewModel.CurrentDirection.DirectionType) {
                    case DirectionType.Face:
                        if(string.IsNullOrWhiteSpace(Constants.FaceApiKey)) {
                            await DisplayAlert("ERROR", "You must add a Face API key to answer this one.", "OK");
                            return;
                        }

                        result = await OnFaceClickActionAsync();
                        break;

                    case DirectionType.Text:
                        if(string.IsNullOrWhiteSpace(Constants.TextAnalyticsApiKey)) {
                            await DisplayAlert("ERROR", "You must add a Text Analytics key to answer this one.", "OK");
                            return;
                        }

                        result = await OnTextClickActionAsync();
                        break;

                    case DirectionType.GeneralPicture:
                        if(string.IsNullOrWhiteSpace(Constants.ComputerVisionApiKey)) {
                            await DisplayAlert("ERROR", "You must add a Computer Vision key to answer this one.", "OK");
                            return;
                        }

                        result = await OnGeneralPicClickActionAsync();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(_viewModel.CurrentDirection.DirectionType));
                }
            } catch(Exception exception) {
                Console.WriteLine(exception);
                await DisplayAlert("ERROR", exception.Message, "OK");
                return;
            } finally {
                IsBusy = false;
                Title = _viewModel.CognitiveServicesType;   //Bindings were not working
            }

            if(!result.HasValue) {
                _viewModel.ShowResults = _viewModel.CurrentDirection.DirectionType == DirectionType.GeneralPicture;
                return;
            }

            if(!result.Value) {
                _viewModel.TryAgain = true;
                _viewModel.TextAnswer = string.Empty;
            }

            _viewModel.ShowResults = true;

            if(_viewModel.CurrentDirection.DirectionType == DirectionType.GeneralPicture) {
                return;
            }

            if(result.Value) {
                await DisplayAlert("WIN", "Nice job!", "OK");
                _viewModel.ResetSecondaryText();
            } else {
                Direction direction = _viewModel.CurrentDirection;

                direction.ShowSecondaryName = true;

                _viewModel.CurrentDirection = direction;

                if(_viewModel.CurrentDirection.DirectionType == DirectionType.Face) {
                    await DisplayAlert("FAIL", "Your face is not " + direction.EmotionDescription.ToLowerInvariant() + " enough! Try again.", "OK");
                } else if(_viewModel.CurrentDirection.DirectionType == DirectionType.Text) {
                    await DisplayAlert("FAIL", "Your text is not " + direction.EmotionDescription.ToLowerInvariant() + " enough! Try again.", "OK");
                }
            }
        }

        private void OnSkipClicked(object sender, EventArgs e) {
            _viewModel.ClearAnswerInfo(true);
            CurrentImage.Source = null;
            _viewModel.ResetSecondaryText();
            _viewModel.SetNextDirection();

            Title = _viewModel.CognitiveServicesType;   //Bindings were not working
        }

        private async Task<bool?> OnFaceClickActionAsync() {
            string path = await ShowImageChoices();

            if(path == null) {
                return null;
            }

            _viewModel.FaceAnswerPath = path;
            CurrentImage.Source = path;

            return await _viewModel.TestFaceAsync();
        }

        private async Task<bool?> OnTextClickActionAsync() {
            return await _viewModel.TestTextAsync();
        }

        private async Task<bool?> OnGeneralPicClickActionAsync() {
            string path = await ShowImageChoices();

            if(path == null) {
                return null;
            }

            _viewModel.GeneralPicAnswerPath = path;
            CurrentImage.Source = path;

            await _viewModel.TestPhotoAsync();

            return true;
        }

        private async Task<string> ShowImageChoices() {
            string answer = await DisplayActionSheet("Find Photo", "Cancel", null, "Take a Photo", "Choose From Library");

            string path;

            switch(answer) {
                case "Take a Photo":
                    path = await _viewModel.TakePhotoAsync();
                    break;

                case "Choose From Library":
                    path = await _viewModel.PickPhotoAsync();
                    break;

                default:
                    return null;
            }

            // Return if canceled.
            if (path == null) {
                await DisplayAlert("ERROR", "Something went wrong!", "OK");
                return null;
            }

            return path;
        }
    }
}
