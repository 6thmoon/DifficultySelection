using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using RoR2;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

[assembly: AssemblyVersion(Local.Difficulty.Selection.Plugin.versionNumber)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
		// Allow private member access via publicized assemblies.

namespace Local.Difficulty.Selection
{
	[BepInPlugin("local.difficulty.selection", "DifficultySelection", versionNumber)]
	public class Plugin : BaseUnityPlugin
	{
		public const string versionNumber = "0.1.0";
		private static ConfigFile configuration;

		public void Awake()
		{
			configuration = this.Config;
			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(RuleCatalog), nameof(RuleCatalog.Init))]
		[HarmonyFinalizer]
		private static void UpdateSelection(Exception __exception)
		{
			if ( __exception is object ) throw __exception;

			RuleDef rule = RuleCatalog.allRuleDefs?.FirstOrDefault();
			var difficulties = rule?.choices;

			if ( difficulties is null )
			{
				System.Console.WriteLine("ERROR: Unable to update difficulty selection.");
				return;
			}

			string[] options = difficulties.Select( choice => choice.localName ).ToArray();
			string defaultChoice = configuration.Bind(
					section: "General",
					key: "Default Difficulty",
					defaultValue: difficulties.Find(
							choice => choice.difficultyIndex == DifficultyIndex.Hard
						)?.localName,
					new ConfigDescription(
							"If no other option is chosen in the lobby, "
								+ "this one will be selected by default.",
							new AcceptableValueList<string>(options)
				)).Value;

			for ( int i = 0; i < difficulties.Count; ++i )
			{
				RuleChoiceDef choice = difficulties[i];

				choice.excludeByDefault = configuration.Bind(
						section: "Options",
						key: choice.localName,
						description: "Adjust this value to "
							+ ( choice.excludeByDefault ? "show" : "hide" ) + " the \""
							+ Language.GetString(choice.tooltipNameToken)
							+ "\" difficulty option in the lobby.",
						defaultValue: ! choice.excludeByDefault ||
								choice.difficultyIndex == DifficultyIndex.Eclipse8
					).Value is false;

				if ( defaultChoice == choice.localName )
				{
					rule.defaultChoiceIndex = i;
					choice.excludeByDefault = false;
				}
			}
		}
	}
}
