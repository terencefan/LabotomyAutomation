﻿using System;
using System.Collections.Generic;
using System.Reflection;

using AutoInority.Extentions;

using Harmony;

using UnityEngine;

using Random = System.Random;

namespace AutoInority
{
    public class Harmony_Patch
    {
        public const string ModName = "Lobotomy.terencefan.AutoInority";

        private const BindingFlags FlagsAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        private static Dictionary<string, int> _limiter = new Dictionary<string, int>();

        public Harmony_Patch()
        {
            Invoke(() =>
            {
                HarmonyInstance mod = HarmonyInstance.Create(ModName);

                // all these methods have to be public.
                PatchGameManager(mod);
                PatchUnitMouseEventManager(mod);
                PatchCommandWindow(mod);

                PatchAgentModel(mod);

                PatchCreatureManager(mod);

                // PatchOrdealManager(mod);
                PatchUseSkill(mod);
                PatchIsolateRoom(mod);
                PatchMapGraph(mod);

                // PatchSefiraManager(mod);
                Log.Info("patch success");
            });
        }

        public static void AgentModel_AttachGift_Postfix(AgentModel __instance, EGOgiftModel gift) => Invoke(() => Automaton.Instance.AgentAttachEGOgift(__instance, gift));

        public static void AgentModel_OnFixedUpdate_Postfix(AgentModel __instance) => Invoke(() => Automaton.Instance.AgentOnFixedUpdate(__instance));

        public static void CommandWindow_OnClick_Prefix(CommandWindow.CommandWindow __instance, AgentModel actor)
        {
            if (actor == null)
            {
                return;
            }

            switch (__instance.CurrentWindowType)
            {
                case CommandType.Management:
                    if (__instance.CurrentTarget is CreatureModel creature)
                    {
                        var skill = SkillTypeList.instance.GetData(__instance.SelectedWork);
                        Invoke(() => ManagementCreature(actor, creature, skill));
                    }
                    return;
                default:

                    // TODO
                    return;
            }
        }

        public static void CreatureManager_OnFixedUpdate_Postfix(CreatureManager __instance)
        {
            Invoke(() => Automaton.Instance.Main(), nameof(Automaton.Instance.Main), 60);
            Invoke(() => Automaton.Instance.HandleUncontrollable(), nameof(Automaton.Instance.HandleUncontrollable), 5);
        }

        public static void FinishWorkSuccessfully_Postfix(UseSkill __instance)
        {
            Invoke(() => CenterBrain.AddRecord(__instance.agent, __instance.targetCreature, __instance.skillTypeInfo));
        }

        public static void GameManager_EndGame()
        {
            Invoke(Automaton.Reset);
            Invoke(CenterBrain.Reset);
        }

        public static void Initialize_Graph(GameManager __instance)
        {
            Invoke(Graph.Initialize);
        }

        public static void IsolateRoom_OnCancelWork_Prefix(IsolateRoom __instance)
        {
            Invoke(() => Automaton.Instance.OnCancelWork(__instance));
        }

        public static bool IsolateRoom_OnEnterRoom_Prefix(IsolateRoom __instance, AgentModel worker, UseSkill skill)
        {
            return Invoke(() => Automaton.Instance.OnEnterRoom(__instance, worker, skill));
        }

        public static bool IsolateRoom_OnExitRoom_Prefix(IsolateRoom __instance)
        {
            return Invoke(() => Automaton.Instance.OnExitRoom(__instance));
        }

        public static void OrdealManager_OnFixedUpdated_Postfix(OrdealManager __instance)
        {
        }

        public static void SefiraManager_OnNotice_Postfix(SefiraManager __instance, string notice, params object[] param)
        {
            if (notice == NoticeName.FixedUpdate)
            {
                // TODO
            }
        }

        public static void UnitMouseEventManager_Update(UnitMouseEventManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Invoke(Automaton.Instance.ToggleAutomation);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Invoke(() => __instance.GetSelectedAgents().ForEach(x => Automaton.Instance.Remove(x)));
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                var currentWindow = CommandWindow.CommandWindow.CurrentWindow;
                if (currentWindow.enabled && currentWindow.IsEnabled && currentWindow.CurrentTarget is CreatureModel creature)
                {
                    Invoke(() => Automaton.Instance.ToggleFarming(creature));
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                var currentWindow = CommandWindow.CommandWindow.CurrentWindow;
                if (currentWindow.enabled && currentWindow.IsEnabled && currentWindow.CurrentTarget is CreatureModel creature)
                {
                    Invoke(() => Automaton.Instance.AssignWork(creature));
                }
                else
                {
                    Invoke(() => Automaton.Instance.TryTrain());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                Invoke(() => Log.DebugEnabled = !Log.DebugEnabled);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                Invoke(Automaton.Instance.Clear);
            }

            // else if (Input.GetKeyDown(KeyCode.B))
            // {
            //     Screen.SetResolution(Screen.width, Screen.height, !Screen.fullScreen);
            // }
        }

        public static void UpdateGameSpeed_Postfix()
        {
            var manager = GameManager.currentGameManager;

            if (manager.state == GameState.PLAYING)
            {
                switch (manager.gameSpeedLevel)
                {
                    case 1:
                        Time.timeScale = 1f;
                        Time.fixedDeltaTime = 0.02f;
                        break;

                    case 2:
                        Time.timeScale = 2f;
                        Time.fixedDeltaTime = 0.03f;
                        break;

                    case 3:
                        Time.timeScale = 5f;
                        Time.fixedDeltaTime = 0.05f;
                        break;
                }
            }
            else if (manager.state == GameState.PAUSE)
            {
                Time.timeScale = 0f;
            }
        }

        public void PatchAgentModel(HarmonyInstance mod)
        {
            var takeDamage = typeof(Harmony_Patch).GetMethod(nameof(AgentModel_OnFixedUpdate_Postfix));
            mod.Patch(typeof(AgentModel).GetMethod(nameof(AgentModel.OnFixedUpdate)), null, new HarmonyMethod(takeDamage));
            Log.Info($"patch AgentModel.OnFixedUpdate.success");

            var attachGift = typeof(Harmony_Patch).GetMethod(nameof(AgentModel_AttachGift_Postfix));
            mod.Patch(typeof(AgentModel).GetMethod(nameof(AgentModel.AttachEGOgift)), null, new HarmonyMethod(attachGift));
            Log.Info($"patch AgentModel.AttachEGOgift success");
        }

        public void PatchCommandWindow(HarmonyInstance mod) => PatchPrefix(mod, typeof(CommandWindow.CommandWindow), nameof(CommandWindow.CommandWindow.OnClick), nameof(CommandWindow_OnClick_Prefix));

        public void PatchCreatureManager(HarmonyInstance mod) => PatchPostfix(mod, typeof(CreatureManager), nameof(CreatureManager.OnFixedUpdate), nameof(CreatureManager_OnFixedUpdate_Postfix));

        public void PatchGameManager(HarmonyInstance mod)
        {
            PatchPrefix(mod, typeof(GameManager), nameof(GameManager.EndGame), nameof(GameManager_EndGame));
            PatchPostfix(mod, typeof(GameManager), "UpdateGameSpeed", nameof(UpdateGameSpeed_Postfix));
        }

        public void PatchIsolateRoom(HarmonyInstance mod)
        {
            PatchPrefix(mod, typeof(IsolateRoom), nameof(IsolateRoom.OnCancelWork), nameof(IsolateRoom_OnCancelWork_Prefix));
            PatchPrefix(mod, typeof(IsolateRoom), nameof(IsolateRoom.OnEnterRoom), nameof(IsolateRoom_OnEnterRoom_Prefix));
            PatchPrefix(mod, typeof(IsolateRoom), nameof(IsolateRoom.OnExitRoom), nameof(IsolateRoom_OnExitRoom_Prefix));
        }

        public void PatchMapGraph(HarmonyInstance mod)
        {
            PatchPostfix(mod, typeof(GameManager), nameof(GameManager.InitGame), nameof(Initialize_Graph));
        }

        public void PatchOrdealManager(HarmonyInstance mod) => PatchPostfix(mod, typeof(OrdealManager), nameof(OrdealManager.OnFixedUpdate), nameof(OrdealManager_OnFixedUpdated_Postfix));

        public void PatchSefiraManager(HarmonyInstance mod) => PatchPrefix(mod, typeof(SefiraManager), nameof(SefiraManager.OnNotice), nameof(SefiraManager_OnNotice_Postfix));

        public void PatchUnitMouseEventManager(HarmonyInstance mod) => PatchPostfix(mod, typeof(UnitMouseEventManager), "Update", nameof(UnitMouseEventManager_Update));

        public void PatchUseSkill(HarmonyInstance mod) => PatchPostfix(mod, typeof(UseSkill), "FinishWorkSuccessfully", nameof(FinishWorkSuccessfully_Postfix));

        private static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static bool Invoke(Func<bool> action)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return true;
        }

        private static void Invoke(Action action, string k, int interval, int offset = 0, bool random = false)
        {
            var r = new Random();
            if (_limiter.TryGetValue(k, out var last))
            {
                if (Time.frameCount - last < interval)
                {
                    return;
                }
                Invoke(action);
                _limiter[k] = Time.frameCount;
            }
            else
            {
                _limiter[k] = offset - (random ? r.Next(interval) : interval);
            }
        }

        private static void ManagementCreature(AgentModel actor, CreatureModel creature, SkillTypeInfo skill)
        {
            if (!creature.GetExtension().CanWorkWith(actor, skill, out var message))
            {
                Angela.Say(message);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Invoke(() => Automaton.Instance.Register(actor, creature, skill));
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                if (actor.HasGift(creature, out var gift))
                {
                    message = string.Format(Angela.Agent.HasEGOGift, actor.name, gift.Name);
                    Angela.Say(message);
                    return;
                }
                else if (gift == null)
                {
                    message = string.Format(Angela.Agent.NoEgoGift, creature.metaInfo.name);
                    Angela.Say(message);
                    return;
                }
                else if (actor.IsRegionLocked(gift))
                {
                    var regionName = UnitEGOgiftSpace.GetRegionName(gift);
                    message = string.Format(Angela.Agent.SlotLocked, actor.name, regionName);
                    Angela.Say(message);
                    return;
                }
                Automaton.Instance.Register(actor, creature, skill, forGift: true);
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                Automaton.Instance.Register(actor, creature, skill, forExp: true);
            }
        }

        private void PatchPostfix(HarmonyInstance instance, Type type, string method, string patch, BindingFlags flags = FlagsAll)
        {
            var postfix = typeof(Harmony_Patch).GetMethod(patch);
            instance.Patch(type.GetMethod(method, flags), null, new HarmonyMethod(postfix));
            Log.Info($"patch {type.Name}.{method} success");
        }

        private void PatchPrefix(HarmonyInstance instance, Type type, string method, string patch, BindingFlags flags = FlagsAll)
        {
            var prefix = typeof(Harmony_Patch).GetMethod(patch);
            instance.Patch(type.GetMethod(method, flags), new HarmonyMethod(prefix), null);
            Log.Info($"patch {type.Name}.{method} success");
        }
    }
}