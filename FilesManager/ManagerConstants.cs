namespace FilesManager
{
    public static class ManagerConstants
    {
        public static readonly string ROOT_PATH = "wwwroot";
        public static readonly string SUB_DIR = "img";
        public static readonly string THUMB_SUB_DIR = ".thumbs";

        public static string CombinePath(params string[] pathSegments)
        {
            pathSegments = pathSegments.Select(i => i.Replace('\\', '/').Trim('/')).ToArray();
            return Path.Combine(pathSegments).Replace('\\', '/');
        }
    }
}
