using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MichaelWolfGames
{
	//-///////////////////////////////////////////////////////////
	/// 
    public class PostBuildPublisher
    {
        private static string PROJECT_NAME = "HiveRise";

        private static string webGLBuildFolderName => PROJECT_NAME + "_Dev";

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log(pathToBuiltProject);

            if (target == BuildTarget.WebGL)
            {
                Debug.Log("Built WebGL!");
            }
        }

        [MenuItem("Build/WebGL (Dev)/Build and Publish")]
        public static void BuildAndPublishWebGL()
        {
            // Update version text file used when publishing with butler.
            string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/WebGL/");
            File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("v{0}", Application.version));

            BuildWebGL(() => { PublishWebGL(); });
        }

        [MenuItem("Build/WebGL (Dev)/Build")]
        public static void BuildWebGL()
        {
            BuildWebGL(null);
        }

        private static void BuildWebGL(Action onBuildSuccessful = null)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            //buildPlayerOptions.locationPathName = "Builds/WebGL/" + PROJECT_NAME + "_Dev";
            buildPlayerOptions.locationPathName = "Builds/WebGL/" + webGLBuildFolderName;
            buildPlayerOptions.target = BuildTarget.WebGL;
            buildPlayerOptions.options = BuildOptions.None;

            // DEFAULT SCENES FROM BUILD SETTINGS
            List<string> scenes = new List<string>();
            scenes.AddRange(EditorBuildSettings.scenes.Where(scene => scene.enabled)
                .Select(scene => scene.path).ToArray());

            /*
            // Load dynamic level scenes.
            //scenes.AddRange(GetLevelScenesForBuild());

            // Apply to buildPlayerOptions.
            //buildPlayerOptions.scenes = scenes.ToArray();
	        */

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");

                onBuildSuccessful?.Invoke();
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        // private static List<string> GetLevelScenesForBuild()
        // {
        //     List<string> scenes = new List<string>();
        //     LevelCollection levelCollection = Resources.Load<LevelCollection>("LevelCollection");
        //     foreach (LevelData level in levelCollection.levels)
        //     {
        //         if (level.scene != null && level.scene.scenePath != "")
        //         {
        //             scenes.Add("Assets/" + level.scene.scenePath + ".unity");
        //         }
        //     }
        //
        //     return scenes;
        // }

        [MenuItem("Build/WebGL (Dev)/Publish")]
        public static void PublishWebGL()
        {
            Debug.Log("Publishing Last Build.");

            string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/WebGL/");
            string arguments = string.Format("\"{0}\" \"{1}\"", (buildFolderPath + webGLBuildFolderName), (buildFolderPath + "buildnumber.txt"));
            RunShellScript("Builds/WebGL/Butler_Publish.sh", arguments);
        }

        [MenuItem("Build/WebGL (Dev)/Publish (Manual Bash)")]
        public static void PublishWebGL_ManualBash()
        {
            RunShellScript("Builds/WebGL/Butler_Publish_Manual.sh");
        }

        public static void RunShellScript(string relativePath, string arguments = null)
        {
            string projectPath = Application.dataPath.Replace("/Assets", "/");

            Process proc = new Process();
            ProcessStartInfo procStartInfo = new ProcessStartInfo
            {
                FileName = projectPath + relativePath,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                CreateNoWindow = true,
            };
            if (!string.IsNullOrEmpty(arguments))
            {
                procStartInfo.Arguments = arguments;
            }

            proc.StartInfo = procStartInfo;
            proc.Start();
        }

    }
}