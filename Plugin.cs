using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

[assembly: AssemblyVersion(Local.Difficulty.Selection.Plugin.versionNumber)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace Local.Difficulty.Selection
{
	[BepInPlugin("local.difficulty.selection", "DifficultySelection", versionNumber)]
	public class Plugin : BaseUnityPlugin
	{
		public const string versionNumber = "0.1.3";
		private static ConfigFile configuration;

		public void Awake()
		{
			configuration = this.Config;
			Harmony.CreateAndPatchAll(typeof(Plugin));
		}

		[HarmonyPatch(typeof(RuleCatalog), nameof(RuleCatalog.Init))]
		[HarmonyFinalizer]
		private static void Initialize(Exception __exception)
		{
			if ( __exception is object ) throw __exception;
			else UpdateSelection();
		}

		[HarmonyPatch(typeof(PreGameController), nameof(PreGameController.ResolveChoiceMask))]
		[HarmonyPrefix]
		private static void UpdateSelection()
		{
			RuleDef rule = RuleCatalog.allRuleDefs?.FirstOrDefault();
			List<RuleChoiceDef> difficulties = rule?.choices;

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
							"If no other option is chosen in the lobby,"
								+ " this one will be selected by default.",
							new AcceptableValueList<string>(options)
				)).Value;

			foreach ( RuleChoiceDef choice in difficulties )
			{
				var key = from letter in choice.localName where letter is not (
						'=' or '\n' or '\t' or '\\' or '"' or '\'' or '[' or ']'
					) select letter;

				choice.excludeByDefault = configuration.Bind(
						section: "Options",
						key: string.Join("", key),
						description: "Adjust this value to "
							+ ( choice.excludeByDefault ? "show" : "hide" ) + " the \""
							+ Language.GetString(choice.tooltipNameToken)
							+ "\" difficulty option in the lobby.",
						defaultValue: choice.difficultyIndex is DifficultyIndex.Eclipse8
								 || ! choice.excludeByDefault
					).Value is false;

				if ( defaultChoice == choice.localName )
				{
					rule.defaultChoiceIndex = choice.localIndex;
					choice.excludeByDefault = false;
				}
			}
		}
	}
}
