using NLog;

namespace FilesManager
{
    public class DirectoriesHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void DeleteDirectory(string path)
        {
            try
            {
                var directory = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, path);

                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void RenameDirectory(string oldPath, string newPath)
        {
            var source = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, oldPath);
            var destination = ManagerConstants.CombinePath(ManagerConstants.ROOT_PATH, ManagerConstants.SUB_DIR, newPath);

            if (Directory.Exists(source))
            {
                Directory.Move(source, destination);
            }
        }
    }
}
