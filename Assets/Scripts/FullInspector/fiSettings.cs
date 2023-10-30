using FullInspector.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FullInspector
{
	public class fiSettings
	{
		public static bool EnableMultiEdit;

		public static bool EnableGlobalOrdering;

		public static bool EnableSettingsProcessor;

		public static bool EnableLogs;

		public static bool PrettyPrintSerializedJson;

		public static CommentType DefaultCommentType;

		public static bool ForceDisplayInlineObjectEditor;

		public static bool EnableAnimation;

		public static bool ForceSaveAllAssetsOnSceneSave;

		public static bool ForceSaveAllAssetsOnRecompilation;

		public static bool ForceRestoreAllAssetsOnRecompilation;

		public static bool AutomaticReferenceInstantation;

		public static bool InspectorAutomaticReferenceInstantation;

		public static bool InspectorRequireShowInInspector;

		public static bool SerializeAutoProperties;

		public static bool EmitWarnings;

		public static bool EmitGraphMetadataCulls;

		public static float MinimumFoldoutHeight;

		public static bool EnableOpenScriptButton;

		public static bool ForceDisableMultithreadedSerialization;

		public static float LabelWidthPercentage;

		public static float LabelWidthOffset;

		public static float LabelWidthMax;

		public static float LabelWidthMin;

		public static bool DisplaySingleCategory;

		public static int DefaultPageMinimumCollectionLength;

		public static string RootDirectory;

		public static List<string> TypeSelectionDefaultFilters;

		public static List<string> TypeSelectionBlacklist;

		public static string RootGeneratedDirectory;

		static fiSettings()
		{
			EnableMultiEdit = true;
			EnableSettingsProcessor = true;
			DefaultCommentType = CommentType.Info;
			EnableAnimation = true;
			InspectorAutomaticReferenceInstantation = true;
			SerializeAutoProperties = true;
			MinimumFoldoutHeight = 80f;
			EnableOpenScriptButton = true;
			LabelWidthPercentage = 0.45f;
			LabelWidthOffset = 30f;
			LabelWidthMax = 600f;
			DisplaySingleCategory = true;
			DefaultPageMinimumCollectionLength = 20;
			RootDirectory = "Assets/FullInspector2/";
			if (EnableSettingsProcessor)
			{
				foreach (fiSettingsProcessor assemblyInstance in fiRuntimeReflectionUtility.GetAssemblyInstances<fiSettingsProcessor>())
				{
					assemblyInstance.Process();
				}
			}
			if (fiUtility.IsEditor)
			{
				EnsureRootDirectory();
			}
			if (RootGeneratedDirectory == null)
			{
				RootGeneratedDirectory = RootDirectory.TrimEnd('/') + "_Generated/";
			}
			if (fiUtility.IsEditor && !fiDirectory.Exists(RootGeneratedDirectory))
			{
				UnityEngine.Debug.Log("Creating directory at " + RootGeneratedDirectory);
				fiDirectory.CreateDirectory(RootGeneratedDirectory);
			}
		}

		private static void EnsureRootDirectory()
		{
			if (RootDirectory == null || !fiDirectory.Exists(RootDirectory))
			{
				string text = FindDirectoryPathByName("Assets", "FullInspector2");
				if (text == null)
				{
					UnityEngine.Debug.LogError("Unable to locate \"FullInspector2\" directory. Please make sure that Full Inspector is located within \"FullInspector2\"");
				}
				else
				{
					text = (RootDirectory = text.Replace('\\', '/').TrimEnd('/') + '/');
				}
			}
		}

		private static string FormatCustomizerForNewPath(string path)
		{
			return "using FullInspector;" + Environment.NewLine + Environment.NewLine + "public class UpdateFullInspectorRootDirectory : fiSettingsProcessor {" + Environment.NewLine + "    public void Process() {" + Environment.NewLine + "        fiSettings.RootDirectory = \"" + path + "\";" + Environment.NewLine + "    }" + Environment.NewLine + "}" + Environment.NewLine;
		}

		private static string FindDirectoryPathByName(string currentDirectory, string targetDirectory)
		{
			targetDirectory = Path.GetFileName(targetDirectory);
			foreach (string directory in fiDirectory.GetDirectories(currentDirectory))
			{
				if (Path.GetFileName(directory) == targetDirectory)
				{
					return directory;
				}
				string text = FindDirectoryPathByName(directory, targetDirectory);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}
	}
}
