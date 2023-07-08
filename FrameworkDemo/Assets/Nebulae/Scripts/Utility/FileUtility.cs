using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nebulae
{
    public class FileUtility
    {
        public static string StandardizeBackslashSeparator(string path)
        {
            path = path.Replace("\\", "/");
            return path;
        }

        public static string CombinePaths(params string[] args)
        {
            if (args.Length == 0)
            {
                return "";
            }

            string path = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                var node = RemoveStartPathSeparator(args[i]);
                path = Path.Combine(path, node);
            }

            path = StandardizeBackslashSeparator(path);

            return path;
        }

        public static string CombineDirs(bool isEndWithBackslash, params string[] args)
        {
            string path = CombinePaths(args);

            if (isEndWithBackslash)
            {
                if (path.EndsWith("/") == false)
                {
                    path += "/";
                }
            }
            else
            {
                if (path.EndsWith("/"))
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }

            return path;
        }

        public static string RemoveStartPathSeparator(string path)
        {
            char[] pathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }; // 不同操作系统中的分隔符和备用分隔符
            string trimmedPath = path.TrimStart(pathSeparators); // 从头移除分隔符
            return trimmedPath;
        }
    }
}

