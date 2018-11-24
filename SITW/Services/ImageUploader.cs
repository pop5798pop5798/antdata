// Copyright 2015 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace GoogleCloudSamples.Services
{
    public class ImageUploader
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public ImageUploader(string bucketName)
        {
            _bucketName = bucketName;
            // [START storageclient]
            _storageClient = StorageClient.Create();
            // [END storageclient]
        }

        public List<StorageObject> ListOfObjects(string path)
        {
            List<StorageObject> soList = new List<StorageObject>();
            var objects = _storageClient.ListObjects(_bucketName, path + "/");
            foreach (var obj in objects)
            {
                if (obj.Name != path + "/")
                {
                    soList.Add(new StorageObject { FileName = obj.Name, MediaLink = obj.MediaLink });
                }
            }
            return soList;
        }

        // [START uploadimage]
        public async Task<String> UploadImage(HttpPostedFileBase image, string fileName,string path)
        {
            var imageAcl = PredefinedObjectAcl.PublicRead;

            var imageObject = await _storageClient.UploadObjectAsync(
                bucket: _bucketName,
                objectName: path + "/" + fileName,
                contentType: image.ContentType,
                source: image.InputStream,
                options: new UploadObjectOptions { PredefinedAcl = imageAcl }
            );

            return imageObject.MediaLink;
        }
        // [END uploadimage]

        public async Task DeleteUploadedImage(string fileName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, fileName);
            }
            catch (Google.GoogleApiException exception)
            {
                // A 404 error is ok.  The image is not stored in cloud storage.
                if (exception.Error.Code != 404)
                    throw;
            }
        }

    }

    public class StorageObject
    {
        public string FileName { get; set; }
        public string MediaLink { get; set; }
    }

}