using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace CognitiveApp.UtilServices {

    public class CameraService {

        public async Task<string> TakePhotoAsync() {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                throw new Exception("Your device does not currently support taking photos or your camera is not currently available.");
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                Directory = "Photos",
                Name = "photo_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss") + ".jpg",
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92,
                DefaultCamera = CameraDevice.Front
            });

            return file?.Path;
        }

        public async Task<string> PickPhotoAsync() {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported) {
                throw new Exception("Your device does not currently support choosing a file from your library.");
            }

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            });

            return file?.Path;
        }

        public void DeletePhoto(string filePath) {
            File.Delete(filePath);
        }
    }
}
