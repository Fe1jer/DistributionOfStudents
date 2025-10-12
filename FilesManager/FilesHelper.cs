using Microsoft.AspNetCore.Http;
using NLog;
using SkiaSharp;

namespace FilesManager
{
    public class FilesHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string UploadFile(IFormFile file, string path, string? oldPath = null)
        {
            if (!string.IsNullOrEmpty(oldPath))
            {
                DeleteFile(oldPath);
            }

            var localPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, ManagerConstants.SUB_DIR, path);

            // creates all directories and subdirectories, creates in path
            Directory.CreateDirectory(Path.GetDirectoryName(localPath) ?? string.Empty);
            // save the file to the path folder in the {ManagerConstants.ROOT_PATH} directory
            using var fileStream = new FileStream(localPath, FileMode.Create);

            file.CopyTo(fileStream);

            return ManagerConstants.CombinePath(ManagerConstants.SUB_DIR, path);
        }

        public static string? ScaleAndUploadFile(IFormFile file, string path, int width, int height, string? oldPath = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(oldPath))
                {
                    DeleteFile(oldPath);
                }

                var localPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, ManagerConstants.SUB_DIR, path);
                Directory.CreateDirectory(Path.GetDirectoryName(localPath) ?? string.Empty);

                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using SKBitmap image = SKBitmap.Decode(memoryStream);
                SKBitmap resultImage = Scale(image, width, height);
                SKImage saveImage = SKImage.FromBitmap(resultImage);
                using var data = saveImage.Encode();
                using var fileStream = File.OpenWrite(localPath);
                data.SaveTo(fileStream);

                return ManagerConstants.CombinePath(ManagerConstants.SUB_DIR, path);
            }
            catch (Exception e)
            {
                Logger.Error(e);

                return null;
            }
        }

        public static void DeleteFile(string? path)
        {
            try
            {
                var localPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, path);

                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                    DeleteThumbs(path);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void DeleteFilesExclude(string directory, IEnumerable<string> exclude)
        {
            try
            {
                directory = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, directory);
                exclude = [.. exclude.Select(p => Path.GetFullPath(Path.Combine(ManagerConstants.ROOT_PATH, p)))];
                var dir = new DirectoryInfo(directory);

                if (!dir.Exists)
                {
                    return;
                }

                var existingFiles = dir.GetFiles();
                foreach (var file in existingFiles)
                {
                    if (!exclude.Contains(file.FullName))
                    {
                        file.Delete();
                    }
                }

                foreach (var inner in dir.GetDirectories())
                {
                    DeleteFilesExclude(inner, exclude);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void DeleteFilesExclude(DirectoryInfo dir, IEnumerable<string> exclude)
        {
            if (!dir.Exists)
            {
                return;
            }

            foreach (var file in dir.GetFiles())
            {
                if (!exclude.Contains(file.FullName))
                {
                    file.Delete();
                }
            }

            foreach (var inner in dir.GetDirectories())
            {
                DeleteFilesExclude(inner, exclude);
            }
        }

        public static string GetCroppedThumbUrl(string srcPath, int width, int height)
        {
            string directory = Path.GetDirectoryName(srcPath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcPath);
            string thumbPath = ManagerConstants.CombinePath(directory, ManagerConstants.THUMB_SUB_DIR, fileNameWithoutExtension) + "_" + width + "x" + height + Path.GetExtension(srcPath);
            string fullSrcPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, srcPath);
            string fullThumbPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, thumbPath);

            if (!File.Exists(fullThumbPath) && File.Exists(fullSrcPath))
            {
                CreateCroppedThumb(fullSrcPath, fullThumbPath, width, height);
            }

            return thumbPath;
        }

        private static void CreateThumb(string srcPath, string thumbPath, int width, int height)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));

                using var inputStream = File.OpenRead(srcPath);
                using SKBitmap image = SKBitmap.Decode(inputStream);
                SKBitmap resultImage = Scale(image, width, height);
                SKImage saveImage = SKImage.FromBitmap(resultImage);
                using var data = saveImage.Encode();
                using var fileStream = File.OpenWrite(thumbPath);
                data.SaveTo(fileStream);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private static void CreateCroppedThumb(string srcPath, string thumbPath, int width, int height)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));

                using var inputStream = File.OpenRead(srcPath);
                using SKBitmap image = SKBitmap.Decode(inputStream);
                using SKBitmap resultImage = ScaleAndCrop(image, width, height);
                using SKImage saveImage = SKImage.FromBitmap(resultImage);
                using var data = saveImage.Encode();
                using var fileStream = File.OpenWrite(thumbPath);
                data.SaveTo(fileStream);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private static void DeleteThumbs(string? srcPath)
        {
            string directory = Path.GetDirectoryName(srcPath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcPath);
            string extension = Path.GetExtension(srcPath);

            string thumbDirectory = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, directory, ManagerConstants.THUMB_SUB_DIR);

            if (!Directory.Exists(thumbDirectory))
            {
                return;
            }

            string searchPattern = $"{fileNameWithoutExtension}_*x*{extension}";
            string[] thumbnailFiles = Directory.GetFiles(thumbDirectory, searchPattern);

            foreach (string thumbPath in thumbnailFiles)
            {
                try
                {
                    File.Delete(thumbPath);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        // result of image with 2252x4000 is 400x300, where width = 400 and height = 300
        private static SKBitmap ScaleAndCrop(SKBitmap image, int width, int height)
        {
            if (image.Width == width && image.Height == height)
            {
                return image;
            }

            SKBitmap resizedBitmap = new(width, height);
            using SKCanvas canvas = new(resizedBitmap);
            using var paint = GetCanvasSettings();

            SKRect destRect = GetCroppedDimensions(image, width, height);
            SKRect srcRect = new(0, 0, image.Width, image.Height);
            canvas.DrawBitmap(image, srcRect, destRect, paint);

            return resizedBitmap;
        }

        // result of image with 2252x4000 is 608x1080, where width = 1920 and height = 1080
        // result of image with 4000x2000 is 1920x960, where width = 1920 and height = 1080
        private static SKBitmap Scale(SKBitmap image, int width, int height)
        {
            if (image.Width <= width && image.Height <= height)
            {
                return image;
            }

            var destRect = GetScaledDimensions(image, width, height);
            SKBitmap resizedBitmap = new((int)destRect.Width, (int)destRect.Height);
            using SKCanvas canvas = new(resizedBitmap);
            using var paint = GetCanvasSettings();

            SKRect srcRect = new(0, 0, image.Width, image.Height);
            canvas.DrawBitmap(image, srcRect, destRect, paint);

            return image;
        }

        public static string Move(string src, string dest)
        {
            dest = ManagerConstants.CombinePath("Files", dest);

            if (src == dest)
            {
                return src;
            }

            var srcPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, src);
            var destPath = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, ManagerConstants.SUB_DIR, dest);

            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
            }

            if (File.Exists(srcPath))
            {
                File.Move(srcPath, destPath);
            }

            return dest;
        }

        private static SKRect GetScaledDimensions(SKBitmap image, int maxWidth, int maxHeight)
        {
            double widthRatio = (double)maxWidth / image.Width;
            double heightRatio = (double)maxHeight / image.Height;

            double scalingFactor = Math.Min(widthRatio, heightRatio);

            int newWidth = (int)(image.Width * scalingFactor);
            int newHeight = (int)(image.Height * scalingFactor);

            return new SKRect(0, 0, newWidth, newHeight);
        }

        private static SKRect GetCroppedDimensions(SKBitmap source, int targetWidth, int targetHeight)
        {
            float scalingX = (float)source.Width / targetWidth;
            float scalingY = (float)source.Height / targetHeight;
            float scaling = Math.Min(scalingX, scalingY);

            int newWidth = (int)(source.Width / scaling);
            int newHeight = (int)(source.Height / scaling);

            int shiftX = (newWidth - targetWidth) / 2;
            int shiftY = (newHeight - targetHeight) / 2;

            return new SKRect(-shiftX, -shiftY, newWidth - shiftX, newHeight - shiftY);
        }

        private static SKPaint GetCanvasSettings()
        {
            return new SKPaint
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true
            };
        }
    }
}
