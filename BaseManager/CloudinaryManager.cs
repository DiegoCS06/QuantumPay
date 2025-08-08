using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BaseManager
{
    public class CloudinaryManager
    {

        private readonly Cloudinary _cloudinary;

        public CloudinaryManager() // Constructor
        {
            // Leer variables de entorno - configurar en azure
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")
                ?? throw new ArgumentNullException("CLOUDINARY_CLOUD_NAME no está configurada en variables de entorno.");
            var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")
                ?? throw new ArgumentNullException("CLOUDINARY_API_KEY no está configurada en variables de entorno.");
            var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                ?? throw new ArgumentNullException("CLOUDINARY_API_SECRET no está configurada en variables de entorno.");
            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public string UploadBase64Image(string base64Image, string folder, string publicId = null)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                throw new ArgumentNullException(nameof(base64Image));

            // Decodificar base64 a bytes
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Subir a Cloudinary
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(publicId ?? Guid.NewGuid().ToString(), new MemoryStream(imageBytes)),
                Folder = folder,
                PublicId = publicId,
                Overwrite = true
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Error al subir imagen a Cloudinary: {uploadResult.Error?.Message}");

            return uploadResult.SecureUrl.AbsoluteUri; // Devuelve URL HTTPS
        }

    }
}
