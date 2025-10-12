using Microsoft.AspNetCore.Hosting;

namespace Shared.Helpers
{
    public class PathHelper(IWebHostEnvironment env)
    {
        private readonly IWebHostEnvironment _env = env;

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
