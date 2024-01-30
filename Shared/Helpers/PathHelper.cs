using Microsoft.AspNetCore.Hosting;

namespace Shared.Helpers
{
    public class PathHelper
    {
        private IWebHostEnvironment _env;

        public PathHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public static string GetFullPathNormalized(string path)
        {
            return Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        }

        public string MapPath(string path, string? basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = _env.WebRootPath;
            }

            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return GetFullPathNormalized(Path.Combine(basePath, path));
        }
    }
}
