using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace CognitiveApp.ApiServices {
    public class FaceApiService {
        private static readonly IEnumerable<FaceAttributeType> FaceAttributes = new [] { FaceAttributeType.Emotion };

        private readonly IFaceServiceClient _faceServiceClient = new FaceServiceClient(Constants.FaceApiKey, "https://southcentralus.api.cognitive.microsoft.com/face/v1.0");

        public async Task<Face[]> UploadAndDetectFaces(string imageFilePath) {
            try {
                using(Stream imageFileStream = File.OpenRead(imageFilePath)) {
                    Face[] faces = await _faceServiceClient.DetectAsync(imageFileStream, returnFaceId: false, returnFaceLandmarks: false, returnFaceAttributes: FaceAttributes);
                    return faces;
                }
            } catch(FaceAPIException e) {
                throw new Exception("Error Code: " + e.ErrorCode + " Error Message: " + e.ErrorMessage, e);
            }
        }
    }
}
