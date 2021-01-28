#if UNITY_EDITOR
using System.IO;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX
using UnityEditor.Build;
using System.Linq;
using UnityEditor.iOS.Xcode;
using UnityEngine;
#endif

namespace HovelHouse.GameKit
{


    [InitializeOnLoad]
    public class BuildProcess
    {
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.StandaloneOSX)
            {
                var osVersion = SystemInfo.operatingSystem;
                Debug.Log($"OS Version is: {osVersion}");

                if (TryGetSemanticVersion(osVersion, out var major, out var minor, out var patch))
                {
                    if(major < 11)
                        Debug.LogWarning($"Building on mac os version {major} with a bundle built for major version 11 (BigSur). Attempting to use any plugin functions will result in a DLL not found exception on this operating system.");
                }
            }
        }

        private static bool TryGetSemanticVersion(
            string versionString, out int major, out int minor, out int patch)
        {
            var regex = new Regex("([0-9]+).?([0-9]+)?.?([0-9]+)?");
            var match = regex.Match(versionString);
            
            if (match.Success)
            {
                var groupCount = match.Groups.Count;
                major = groupCount > 1 ? int.Parse(match.Groups[1].Value) : 0;
                minor = groupCount > 2 ? int.Parse(match.Groups[2].Value) : 0;
                patch = groupCount > 3 ? int.Parse(match.Groups[3].Value) : 0;
                return true;
            }

            major = 0;
            minor = 0;
            patch = 0;

            return false;
        }
    }
}
#endif