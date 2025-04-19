using HarmonyLib;
using HMLLibrary;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.SceneManagement;
using RaftModLoader;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


namespace MiscCheats
{
    public class Main : Mod
    {
        public static Main instance;
        Harmony harmony;
        public bool returnArmor = false;
        public float puzzleTime = 1;
        public float enginePower = 1;
        public bool hardRespawn = false;
        bool _rc = false;
        public bool refineComb
        {
            get => _rc;
            set
            {
                if (_rc == value)
                    return;
                _rc = value;
                var i = value ? ItemManager.GetItemByIndex(294) : null;
                foreach (var e in Resources.FindObjectsOfTypeAll<Placeable_BiofuelExtractor>())
                {
                    var l = Traverse.Create(e.honeyTank).Field("acceptableTypes").GetValue<List<Item_Base>>();
                    if (value)
                        l.Add(i);
                    else
                        l.RemoveAll(x => !x || x.UniqueIndex == 294);
                }
            }
        }
        public float lootTime = 1;
        public float mineTime = 1;
        public float seagullAware = 1;
        public double metalTreasure = 1;
        public bool freeVending = false;
        public bool sniperRange = false;
        public string zoomKey = null;
        public int zoomFOV = 30;
        public float autoOff = -1;
        public bool fishingEqual = false;
        public bool freeCrafting = false;
        public int consoleLines = -1;
        public float fishingWait = 1;
        public float fishingWindow = 1;
        public double itemLooting = 1;
        public bool keepInventory = false;
        public bool antennaAnywhere = false;
        public bool recieverAnywhere = false;
        public bool noDurability = false;
        public bool noPower = false;
        public double batteryMax = 1;
        public double equipmentMax = 1;
        public double toolMax = 1;
        public double foodMax = 1;
        public double collectorMax = 1;
        public bool recycleMetal = false;
        public float sprinklerRadius = 1;
        public float sprinklerDistance = 1;
        public CookingTableMode cookingTableMode = CookingTableMode.NoChange;
        public float playerReach = 1;
        public DishReturnMode returnDishes = DishReturnMode.NoChange;
        public bool autoDoors = false;
        public bool forceMining = false;
        public SeagullMode seagulls = SeagullMode.Default;
        public int sharkAttackThresholdMin = 0;
        public int sharkAttackThresholdMax = -1;
        public bool invincibleScarecrow = false;
        public int FOVOverride = -1;
        public float? timeSpeed = null;
        public bool pausedConsoleFix = false;
        public AxeDurabilityMode axeDur = AxeDurabilityMode.NoChange;
        public bool axeWeapon = false;
        public float zipAccel = 1;
        public float zipSpeed = 1;
        public bool buyBlueprints = false;
        public bool permHearty = false;
        public bool boostedStacks = false;
        public float brickDry = 1;
        public float animalProduce = 1;
        public float meleeWeaponRange = 1;
        public float meleeWeaponRadius = 1;
        public float shovelTime = 1;
        public int treasureSpeed = 1;
        public bool AllowSharkAttack => ComponentManager<RaftBounds>.Value.FoundationCount >= sharkAttackThresholdMin && (sharkAttackThresholdMax < 0 || sharkAttackThresholdMax >= ComponentManager<RaftBounds>.Value.FoundationCount);
        public static Item_Base scrappedGlass;
        public static RectTransform parent;
        public static GameObject consoleItemPrefab;
        public static Transform consoleItemParent;
        public static List<Object> createdObjects = new List<Object>();
        public void Start()
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            var glass = ItemManager.GetItemByName("Glass");
            var honey = ItemManager.GetItemByName("Jar_Honey");
            scrappedGlass = glass.Clone(750, "Glass_Scrapped");
            createdObjects.Add(scrappedGlass);
            var c = honey.settings_recipe.NewCost.FirstOrDefault(x => x.items.Any(y => y.UniqueIndex == glass.UniqueIndex));
            if (c != null)
                glass.SetRecipe(new[] { new CostMultiple(new[] { scrappedGlass }, honey.settings_recipe.AmountToCraft / c.amount) }, learnedFromBeginning: true);
            scrappedGlass.settings_Inventory.LocalizationTerm = null;
            scrappedGlass.settings_Inventory.DisplayName = "Scrapped Glass";
            scrappedGlass.SetRecipe(new CostMultiple[0], CraftingCategory.Hidden, 1);
            RAPI.RegisterItem(scrappedGlass);
            (harmony = new Harmony("com.aidanamite.LotsOLittleMods")).PatchAll();
            Log("Mod has been loaded!");
        }

        public void OnModUnload()
        {
            foreach (var o in createdObjects)
                if (o)
                {
                    if (o is Item_Base i)
                        ItemManager.GetAllItems().RemoveAll(x => x.UniqueIndex == i.UniqueIndex);
                    Destroy(o);
                }
            harmony.UnpatchAll(harmony.Id);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (boostedStacks)
            {
                boostedStacks = false;
                UpdateStackSizes();
            }
            Log("Mod has been unloaded!");
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == Raft_Network.GameSceneName)
                InitCache();
        }

        public bool ExtraSettingsAPI_Loaded = false;
        void ExtraSettingsAPI_Load() => ExtraSettingsAPI_SettingsClose();
        void ExtraSettingsAPI_SettingsClose()
        {
            returnArmor = ExtraSettingsAPI_GetCheckboxState("returnArmor");
            puzzleTime = ExtraSettingsAPI_GetInputValue("puzzleTime").ParseFloat();
            enginePower = ExtraSettingsAPI_GetInputValue("enginePower").ParseFloat();
            hardRespawn = ExtraSettingsAPI_GetCheckboxState("hardRespawn");
            refineComb = ExtraSettingsAPI_GetCheckboxState("refineComb");
            lootTime = ExtraSettingsAPI_GetInputValue("lootTime").ParseFloat();
            mineTime = ExtraSettingsAPI_GetInputValue("mineTime").ParseFloat();
            seagullAware = ExtraSettingsAPI_GetInputValue("seagullAware").ParseFloat();
            metalTreasure = ExtraSettingsAPI_GetInputValue("metalTreasure").ParseDouble();
            freeVending = ExtraSettingsAPI_GetCheckboxState("freeVending");
            sniperRange = ExtraSettingsAPI_GetCheckboxState("sniperRange");
            autoOff = ExtraSettingsAPI_GetInputValue("autoOff").ParseFloat(-1);
            fishingEqual = ExtraSettingsAPI_GetCheckboxState("fishingEqual");
            freeCrafting = ExtraSettingsAPI_GetCheckboxState("freeCrafting");
            consoleLines = ExtraSettingsAPI_GetInputValue("consoleLines").ParseInt(-1);
            if (consoleLines > 0)
                SetConsoleCount(consoleLines);
            zoomFOV = ExtraSettingsAPI_GetInputValue("zoomFOV").ParseInt(-1);
            zoomKey = ExtraSettingsAPI_GetKeybindName("zoomKey");
            fishingWait = ExtraSettingsAPI_GetInputValue("fishingWait").ParseFloat();
            fishingWindow = ExtraSettingsAPI_GetInputValue("fishingWindow").ParseFloat();
            itemLooting = ExtraSettingsAPI_GetInputValue("itemLooting").ParseDouble();
            keepInventory = ExtraSettingsAPI_GetCheckboxState("keepInventory");
            recieverAnywhere = ExtraSettingsAPI_GetCheckboxState("recieverAnywhere");
            antennaAnywhere = ExtraSettingsAPI_GetCheckboxState("antennaAnywhere");
            batteryMax = ExtraSettingsAPI_GetInputValue("batteryMax").ParseDouble();
            equipmentMax = ExtraSettingsAPI_GetInputValue("equipmentMax").ParseDouble();
            toolMax = ExtraSettingsAPI_GetInputValue("toolMax").ParseDouble();
            foodMax = ExtraSettingsAPI_GetInputValue("foodMax").ParseDouble();
            collectorMax = ExtraSettingsAPI_GetInputValue("collectorMax").ParseDouble();
            recycleMetal = ExtraSettingsAPI_GetCheckboxState("recycleMetal");
            sprinklerRadius = ExtraSettingsAPI_GetInputValue("sprinklerRadius").ParseFloat();
            sprinklerDistance = ExtraSettingsAPI_GetInputValue("sprinklerDistance").ParseFloat();
            cookingTableMode = (CookingTableMode)ExtraSettingsAPI_GetComboboxSelectedIndex("cookingTableMode");
            playerReach = ExtraSettingsAPI_GetInputValue("playerReach").ParseFloat();
            returnDishes = (DishReturnMode)ExtraSettingsAPI_GetComboboxSelectedIndex("returnDishes");
            autoDoors = ExtraSettingsAPI_GetCheckboxState("autoDoor");
            forceMining = ExtraSettingsAPI_GetCheckboxState("forceMining");
            seagulls = (SeagullMode)ExtraSettingsAPI_GetComboboxSelectedIndex("seagulls");
            sharkAttackThresholdMin = ExtraSettingsAPI_GetInputValue("sharkAttackThresholdMin").ParseInt(0);
            sharkAttackThresholdMax = ExtraSettingsAPI_GetInputValue("sharkAttackThresholdMax").ParseInt(-1);
            invincibleScarecrow = ExtraSettingsAPI_GetCheckboxState("invincibleScarecrow");
            FOVOverride = ExtraSettingsAPI_GetInputValue("FOVOverride").ParseInt(-1);
            timeSpeed = ExtraSettingsAPI_GetInputValue("timeSpeed").ParseNFloat();
            pausedConsoleFix = ExtraSettingsAPI_GetCheckboxState("pausedConsoleFix");
            axeDur = (AxeDurabilityMode)ExtraSettingsAPI_GetComboboxSelectedIndex("axeDur");
            axeWeapon = ExtraSettingsAPI_GetCheckboxState("axeWeapon");
            zipAccel = ExtraSettingsAPI_GetInputValue("zipAccel").ParseFloat();
            zipSpeed = ExtraSettingsAPI_GetInputValue("zipSpeed").ParseFloat();
            buyBlueprints = ExtraSettingsAPI_GetCheckboxState("buyBlueprints");
            permHearty = ExtraSettingsAPI_GetCheckboxState("permHearty");
            brickDry = ExtraSettingsAPI_GetInputValue("brickDry").ParseFloat();
            animalProduce = ExtraSettingsAPI_GetInputValue("animalProduce").ParseFloat();
            meleeWeaponRange = ExtraSettingsAPI_GetInputValue("meleeWeaponRange").ParseFloat();
            meleeWeaponRadius = ExtraSettingsAPI_GetInputValue("meleeWeaponRadius").ParseFloat();
            boostedStacks = ExtraSettingsAPI_GetCheckboxState("boostedStacks");
            shovelTime = ExtraSettingsAPI_GetInputValue("shovelTime").ParseFloat();
            treasureSpeed = ExtraSettingsAPI_GetInputValue("treasureSpeed").ParseInt();
            UpdateStackSizes();
        }

        void SetConsoleCount(int count)
        {
            if (consoleItemPrefab == null)
            {
                parent = new GameObject("consolePrefabParent").AddComponent<RectTransform>();
                createdObjects.Add(parent.gameObject);
                parent.gameObject.SetActive(false);
                DontDestroyOnLoad(parent.gameObject);
                var i = RConsole.logPool.Count > 0 ? RConsole.logPool[0] : RConsole.logs[0];
                consoleItemPrefab = Instantiate(i, parent);
                consoleItemPrefab.SetActive(false);
                consoleItemParent = i.transform.parent;
            }
            while (RConsole.logPool.Count + RConsole.logs.Count < count)
                RConsole.logPool.Add(Instantiate(consoleItemPrefab, consoleItemParent));
            while (RConsole.logPool.Count + RConsole.logs.Count > count)
                if (RConsole.logPool.Count > 0)
                {
                    Destroy(RConsole.logPool[0]);
                    RConsole.logPool.RemoveAt(0);
                } else
                {
                    Destroy(RConsole.logs[0]);
                    RConsole.logs.RemoveAt(0);
                }
        }

        Dictionary<Item_Base, int> PrevStackSizes = new Dictionary<Item_Base, int>();
        void UpdateStackSizes()
        {
            if (!boostedStacks)
            {
                foreach (var p in PrevStackSizes)
                    Traverse.Create(p.Key.settings_Inventory).Field("stackSize").SetValue(p.Value);
                PrevStackSizes.Clear();
                return;
            }
            foreach (var i in ItemManager.GetAllItems())
                if (i.settings_Inventory.StackSize > 1 || i.MaxUses <= 1)
                {
                    var f = Traverse.Create(i.settings_Inventory).Field<int>("stackSize");
                    if (!PrevStackSizes.ContainsKey(i))
                        PrevStackSizes[i] = f.Value;
                    f.Value = Math.Min(i.MaxUses > 1 ? int.MaxValue / i.MaxUses : int.MaxValue, short.MaxValue);
                }
        }

        void ExtraSettingsAPI_ButtonPress(string SettingName)
        {
            if (SettingName == "reset")
                ExtraSettingsAPI_ResetAllSettings();
        }

        void ExtraSettingsAPI_ButtonPress(string SettingName, int Index)
        {
            if (SettingName == "learning")
            {
                if (Index == 0)
                {
                    foreach (var i in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
                        i.LearnButton();
                }
                else if (Index == 1) { 
                    var l = new List<Message>();
                    foreach (var i in Traverse.Create(ComponentManager<Inventory_ResearchTable>.Value).Field("availableResearchItems").GetValue<Dictionary<Item_Base, AvaialableResearchItem>>())
                        if (!i.Value.Researched && !i.Key.settings_recipe.IsBlueprint)
                        {
                            l.Add(new Message_ResearchTable_ResearchOrLearn(Messages.ResearchTable_Research, RAPI.GetLocalPlayer(), RAPI.GetLocalPlayer().steamID, i.Key.UniqueIndex));
                            if (Raft_Network.IsHost)
                                ComponentManager<Inventory_ResearchTable>.Value.Research(i.Key, false);
                        }
                    var m = new Packet_Multiple(EP2PSend.k_EP2PSendReliable) { messages = l.ToArray() };
                    if (Raft_Network.IsHost)
                        ComponentManager<Raft_Network>.Value.RPC(m, Target.Other, NetworkChannel.Channel_Game);
                    else
                        ComponentManager<Raft_Network>.Value.SendP2P(ComponentManager<Raft_Network>.Value.HostID, m, NetworkChannel.Channel_Game);
                }
                else if (Index == 2)
                {
                    var l = new List<Message>();
                    foreach (var i in ItemManager.GetAllItems())
                        if (ComponentManager<Inventory_ResearchTable>.Value && ComponentManager<Inventory_ResearchTable>.Value.CanResearchBlueprint(i))
                        {
                            l.Add(new Message_ResearchTable_ResearchOrLearn(Messages.ResearchBlueprint, RAPI.GetLocalPlayer(), RAPI.GetLocalPlayer().steamID, i.UniqueIndex));
                            if (Raft_Network.IsHost)
                                ComponentManager<Inventory_ResearchTable>.Value.ResearchBlueprint(i);
                        }
                    var m = new Packet_Multiple(EP2PSend.k_EP2PSendReliable) { messages = l.ToArray() };
                    if (Raft_Network.IsHost)
                        ComponentManager<Raft_Network>.Value.RPC(m, Target.Other, NetworkChannel.Channel_Game);
                    else
                        ComponentManager<Raft_Network>.Value.SendP2P(ComponentManager<Raft_Network>.Value.HostID, m, NetworkChannel.Channel_Game);
                }
            }
            if (SettingName == "unlearning")
            {
                if (Index == 0)
                {
                    if (ComponentManager<Raft_Network>.Value.remoteUsers.Any(x => !x.Value.IsLocalPlayer))
                    {
                        Debug.LogError($"[{name}]: Cannot forget learned recipes while other players are connected");
                        return;
                    }
                    foreach (var i in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
                        if (i.Learned)
                        {
                            Traverse.Create(i).Field("learned").SetValue(false);
                            Traverse.Create(i).Field("canvasgroup").GetValue<CanvasGroup>().alpha = 1;
                            Traverse.Create(i).Field("learnedText").GetValue<Text>().gameObject.SetActive(false);
                            Traverse.Create(i).Field("learnButton").GetValue<Button>().gameObject.SetActive(Traverse.Create(i).Field("learnButton").GetValue<Button>().interactable = i.SortBingoPercent == 1);
                            i.GetItem().settings_recipe.Learned = false;
                        }
                    ComponentManager<Inventory_ResearchTable>.Value.SortMenuItems();
                }
                else if (Index == 1)
                {
                    if (ComponentManager<Raft_Network>.Value.remoteUsers.Any(x => !x.Value.IsLocalPlayer))
                    {
                        Debug.LogError($"[{name}]: Cannot forget researched items while other players are connected");
                        return;
                    }
                    var h = new HashSet<int>();
                    foreach (var i in Traverse.Create(ComponentManager<Inventory_ResearchTable>.Value).Field("availableResearchItems").GetValue<Dictionary<Item_Base, AvaialableResearchItem>>())
                        if (i.Value.Researched)
                        {
                            h.Add(i.Key.UniqueIndex);
                            i.Value.SetResearchedState(false);
                        }
                    ComponentManager<Inventory_ResearchTable>.Value.GetResearchedItems().RemoveAll(x => h.Contains(x.UniqueIndex));
                    foreach (var i in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
                    {
                        foreach (var b in Traverse.Create(i).Field("bingoMenuItems").GetValue<List<BingoMenuItem>>())
                            if (b.BingoState)
                            {
                                b.SetBingoState(false);
                                b.SetSprite(b.BingoItem.settings_Inventory.Sprite);
                            }
                        Traverse.Create(i).Field("itemImage").GetValue<Image>().color = new Color(1f, 1f, 1f, 0.5f);
                        Traverse.Create(i).Field("learnButton").GetValue<Button>().gameObject.SetActive(Traverse.Create(i).Field("learnButton").GetValue<Button>().interactable = false);
                    }
                    ComponentManager<Inventory_ResearchTable>.Value.SortMenuItems();
                }
                else if (Index == 2)
                {
                    if (ComponentManager<Raft_Network>.Value.remoteUsers.Any(x => !x.Value.IsLocalPlayer))
                    {
                        Debug.LogError($"[{name}]: Cannot forget found blueprints while other players are connected");
                        return;
                    }
                    var h = new HashSet<int>();
                    var h2 = new HashSet<int>();
                    foreach (var i in ComponentManager<Inventory_ResearchTable>.Value.GetResearchedItems())
                        if (i.settings_recipe.IsBlueprint)
                        {
                            h.Add(i.UniqueIndex);
                            foreach (var b in i.settings_recipe.GetBlueprintItemIndexes())
                                h2.Add(b);
                        }
                    ComponentManager<Inventory_ResearchTable>.Value.GetResearchedItems().RemoveAll(x => h.Contains(x.UniqueIndex));
                    foreach (var i in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
                        if (h2.Contains(i.GetItem().UniqueIndex))
                            i.gameObject.SetActive(false);
                    ComponentManager<Inventory_ResearchTable>.Value.SortMenuItems();
                }
            }
        }
        public bool ExtraSettingsAPI_HandleSettingVisible(string SettingName)
        {
            if (SettingName == "unlearning")
                return ComponentManager<Raft_Network>.Value && ComponentManager<Raft_Network>.Value.remoteUsers.All(x => x.Value.IsLocalPlayer);
            return false;
        }

        public override void WorldEvent_OnPlayerConnected(CSteamID steamid, RGD_Settings_Character characterSettings) => ExtraSettingsAPI_CheckSettingVisibility();
        public override void WorldEvent_OnPlayerDisconnected(CSteamID steamid, DisconnectReason disconnectReason) => ExtraSettingsAPI_CheckSettingVisibility();

        [MethodImpl(MethodImplOptions.NoInlining)]
        bool ExtraSettingsAPI_GetCheckboxState(string SettingName) => false;

        [MethodImpl(MethodImplOptions.NoInlining)]
        string ExtraSettingsAPI_GetInputValue(string SettingName) => null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        string ExtraSettingsAPI_GetKeybindName(string SettingName) => null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        int ExtraSettingsAPI_GetComboboxSelectedIndex(string SettingName) => 0;

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ExtraSettingsAPI_ResetAllSettings() { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ExtraSettingsAPI_CheckSettingVisibility() { }


        public static Dictionary<ItemInstance_Consumeable, Item_Base> consumeableCache = new Dictionary<ItemInstance_Consumeable, Item_Base>();
        public static void InitCache()
        {
            consumeableCache.Clear();
            foreach (var i in ItemManager.GetAllItems())
                consumeableCache[i.settings_consumeable] = i;
        }
        static Main() => InitCache();

        public static float openDistance = 1;
        public static int CreateMask(int layer)
        {
            var mask = 0;
            for (int i = 0; i < 32; i++)
                if (!Physics.GetIgnoreLayerCollision(i, layer))
                    mask |= 1 << i;
            return mask;
        }

        public static IEnumerator DoNothing()
        {
            yield break;
        }
    }

    [HarmonyPatch(typeof(RemovePlaceables), "ReturnItemsFromBlock")]
    static class Patch_ReturnItemsFromBlock
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "get_NewCost") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ReturnItemsFromBlock), nameof(ModifyReturnCost)))
                });
            code.InsertRange(
                code.FindIndex(x => x.opcode == OpCodes.Stloc_0) + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ReturnItemsFromBlock), nameof(ReturnArmor)))
                });
            return code;
        }
        static CostMultiple[] ModifyReturnCost(CostMultiple[] original, Block block)
        {
            if (Main.instance.freeCrafting)
                return new CostMultiple[0];
            if (Main.instance.returnArmor && Patch_UpgradingBlock.ReturnArmorFromUpgrade(block.ObjectIndex) && block.Reinforced && !(block.buildableItem?.settings_buildable?.GetBlockPrefab(0)?.Reinforced ?? false))
                return original.AddRangeToArray(ItemManager.GetItemByIndex(96).settings_recipe.NewCost);
            return original;
        }
        static void ReturnArmor(Block block, Network_Player player, bool giveItems)
        {
            if (giveItems && player && Main.instance.returnArmor && Patch_UpgradingBlock.ReturnArmorFromUpgrade(block.ObjectIndex) && !Main.instance.freeCrafting && block.Reinforced && !(block.buildableItem?.settings_buildable?.GetBlockPrefab(0)?.Reinforced ?? false))
                foreach (var c in ItemManager.GetItemByIndex(96).settings_recipe.NewCost)
                    if (c?.items?.Length > 0 && c.amount > 0)
                        player.Inventory.AddItem(new ItemInstance(c.items[0], c.amount, c.items[0].MaxUses));
        }
    }
    public enum CookingTableMode
    {
        NoChange,
        DisablePickupItem,
        BetterItemDetect
    }
    public enum DishReturnMode
    {
        NoChange,
        MultiplayerSafe,
        Full
    }
    public enum SeagullMode
    {
        Default,
        IgnoreFlowers,
        IgnoreMediumPlants,
        IgnoreMediumAndFlowers,
        DisableAttacking
    }
    public enum AxeDurabilityMode
    {
        NoChange,
        OnBreak,
        Disabled
    }

    [HarmonyPatch(typeof(BlockCreator), "UpgradeBlock")]
    class Patch_UpgradingBlock
    {
        static Dictionary<uint, bool> upgrades = new Dictionary<uint, bool>();
        static void Prefix(Item_Base newBlockItem, Block oldBlock) => upgrades[oldBlock.ObjectIndex] = newBlockItem?.settings_buildable?.GetBlockPrefab(0)?.Reinforced ?? false;

        public static bool ReturnArmorFromUpgrade(uint ObjectIndex) {
            if (upgrades.TryGetValue(ObjectIndex, out var r))
            {
                upgrades.Remove(ObjectIndex);
                return r;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CostMultiple), "HasEnoughInInventory")]
    static class Patch_HasEnoughForCostMultiple
    {
        static void Postfix(Inventory inventory, ref bool __result)
        {
            if (Main.instance.freeCrafting && inventory is PlayerInventory)
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Inventory), "RemoveCostMultiple")]
    static class Patch_RemoveCostFromInventory
    {
        static bool Prefix(Inventory __instance, CostMultiple[] costMultiple, bool manipulateCostAmount)
        {
            if (Main.instance.freeCrafting && __instance is PlayerInventory)
            {
                if (manipulateCostAmount)
                    foreach (var c in costMultiple)
                        c.amount = 0;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CostCollection), "MeetsRequirements")]
    static class Patch_CostCollectionComplete
    {
        static void Postfix(ref bool __result)
        {
            if (Main.instance.freeCrafting)
                __result = true;
        }
    }

    [HarmonyPatch(typeof(FishingBaitHandler), "GetRandomItemFromCurrentBaitPool")]
    static class Patch_Fishing
    {
        public static bool calling = false;
        static void Prefix() => calling = Main.instance.fishingEqual;
        static void POstfix() => calling = false;
    }

    [HarmonyPatch(typeof(SO_RandomDropper), "GetRandomItem")]
    static class Patch_GetRandomDropperItem
    {
        static void Prefix(SO_RandomDropper __instance, ref RandomItem[] __state)
        {
            if (Patch_Fishing.calling)
            {
                __state = __instance.randomizer.items;
                __instance.randomizer.items = new RandomItem[__state.Length];
                for (int i = 0; i < __state.Length; i++)
                {
                    __instance.randomizer.items[i] = new RandomItem();
                    __instance.randomizer.items[i].CopyFieldsOf(__state[i]);
                    __state[i].weight = 1;
                }
            }
        }
        static void Finalizer(SO_RandomDropper __instance, RandomItem[] __state)
        {
            if (__state != null)
                __instance.randomizer.items = __state;
        }
    }

    [HarmonyPatch(typeof(Reciever), "Update")]
    static class Patch_RecieverOn
    {
        static Dictionary<Reciever, bool> wasOn = new Dictionary<Reciever, bool>();
        static void Postfix(Reciever __instance)
        {
            wasOn.TryGetValue(__instance, out var r);
            if (__instance.battery.On != r)
            {
                if (__instance.battery.On)
                    __instance.StartCoroutine(TurnOffTimer(__instance));
                wasOn[__instance] = !r;
            }
        }

        static IEnumerator TurnOffTimer(Reciever reciever)
        {
            var time = 0f;
            while (reciever.battery.On && Main.instance.autoOff >= 0 && Main.instance.autoOff > time)
            {
                yield return null;
                time += Time.deltaTime;
            }
            if (reciever.battery.On && Main.instance.autoOff >= 0)
                reciever.battery.On = false;
            yield break;
        }
    }

    [HarmonyPatch(typeof(VendingMachine_Element))]
    static class Patch_VendingMachine
    {
        [HarmonyPatch("HasInteractRequirement")]
        static void Postfix(ref bool __result)
        {
            if (Main.instance.freeVending)
                __result = true;
        }
        [HarmonyPatch("ConsumeRequiredItems")]
        static bool Prefix() => !Main.instance.freeVending;
    }

    [HarmonyPatch(typeof(Landmark), "GenerateTreasures")]
    static class Patch_GenerateLandmarkTreasures
    {
        static void Prefix(Landmark __instance, ref (int, int)? __state)
        {
            if (Main.instance.metalTreasure != 1 && __instance?.treasureLootSetting?.numberOfTreasures != null)
            {
                __state = (__instance.treasureLootSetting.numberOfTreasures.minValue, __instance.treasureLootSetting.numberOfTreasures.maxValue);
                __instance.treasureLootSetting.numberOfTreasures.minValue = __instance.treasureLootSetting.numberOfTreasures.minValue.Multiply(Main.instance.metalTreasure);
                __instance.treasureLootSetting.numberOfTreasures.maxValue = __instance.treasureLootSetting.numberOfTreasures.maxValue.Multiply(Main.instance.metalTreasure);
            }
        }
        static void Postfix(Landmark __instance, (int, int)? __state)
        {
            if (__state != null)
                (__instance.treasureLootSetting.numberOfTreasures.minValue, __instance.treasureLootSetting.numberOfTreasures.maxValue) = __state.Value;
        }
    }

    [HarmonyPatch(typeof(ArrayRandomizer), "GenerateRandom", typeof(int), typeof(int), typeof(int))]
    static class Patch_Randomizer
    {
        static void Prefix(ref int count, int min, int max)
        {
            if (count > (max - min))
                count = max - min;
        }
    }

    [HarmonyPatch(typeof(Seagull), "PlayerIsWithinVicinity")]
    static class Patch_SeagullCheckForPlayer
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(code.FindIndex(x => x.opcode == OpCodes.Ldarg_2) + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SeagullCheckForPlayer), nameof(ModifyRadius))));
            return code;
        }
        static float ModifyRadius(float original) => original * Main.instance.seagullAware;
    }

    [HarmonyPatch(typeof(PickupChanneling), "ProgressChannel")]
    static class Patch_ProgressPickup
    {
        static void Prefix(PickupChanneling __instance, ref float time)
        {
            if (Main.instance.lootTime != 1 && __instance.GetComponentInParent<Network_Entity>())
                time = time.SafeDivide(Main.instance.lootTime);
            if (Main.instance.mineTime != 1)
            {
                var i = __instance.GetComponentInParent<PickupItem>();
                if (i && i.tag == "PickupHook")
                    time = time.SafeDivide(Main.instance.mineTime);
            }
        }
    }

    [HarmonyPatch(typeof(LiquidFuelManager), "GetFilteredFuelValueSoFromItem")]
    static class Patch_FuelValue
    {
        static SO_FuelValue v;
        static SO_FuelValue value
        {
            get
            {
                if (!v)
                {
                    v = ScriptableObject.CreateInstance<SO_FuelValue>();
                    v.fuelFiltrationType = FuelValue_FiltrationType.FV_COMPOSTHONEYTANK;
                    v.itemType = ItemManager.GetItemByIndex(294);
                    v.fuelValueOfType = 1;
                }
                return v;
            }
        }
        static void Postfix(Item_Base item, FuelValue_FiltrationType fVFilterType, ref SO_FuelValue __result)
        {
            if (Main.instance.refineComb && item?.UniqueIndex == 294 && fVFilterType == FuelValue_FiltrationType.FV_COMPOSTHONEYTANK)
                __result = value;
        }
    }

    [HarmonyPatch(typeof(DeathMenu), "MenuOpen")]
    static class Patch_OpenDeathMenu
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(code.FindIndex(x => x.opcode == OpCodes.Ldfld && (x.operand as FieldInfo).Name == "canSurrender") + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_OpenDeathMenu), nameof(OverrideCanSurrender))));
            return code;
        }
        static bool OverrideCanSurrender(bool original) => original || Main.instance.hardRespawn;
    }

    [HarmonyPatch(typeof(MotorWheel), MethodType.Getter)]
    static class Patch_Motor
    {
        [HarmonyPatch("MotorStrength")]
        [HarmonyPostfix]
        static void MotorStrength(ref int __result) => __result = __result.Multiply(Main.instance.enginePower);

        [HarmonyPatch("ExtraMotorStrength")]
        [HarmonyPostfix]
        static void ExtraMotorStrength(ref int __result) => __result = __result.Multiply(Main.instance.enginePower);
    }

    [HarmonyPatch(typeof(ComputerChallenge), "StartTimer")]
    static class Patch_StartComputerChallengeTimer
    {
        static void Postfix(ref float ___timer)
        {
            ___timer = ___timer * Main.instance.puzzleTime;
        }
    }

    [HarmonyPatch]
    static class Patch_ThrowThrowable
    {
        static int ProjectileLayer
        {
            get
            {
                var layer = 0;
                for (int i = 0; i < 32; i++)
                    if (!Physics.GetIgnoreLayerCollision(i, 22))
                        layer |= 1 << i;
                return layer;
            }
        }
        static MethodBase TargetMethod() => typeof(Raft).Assembly.GetTypes().First(x => x.Name.StartsWith("<StartThrow>")).GetMethod("MoveNext", ~BindingFlags.Default);
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        {
            var code = instructions.ToList();
            for (int i = 0; i < code.Count; i++)
                if (code[i].opcode == OpCodes.Stloc_2)
                {
                    code.InsertRange(i, new[] {
                        new CodeInstruction(OpCodes.Ldloc_1),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ThrowThrowable), nameof(OverrideThrowForce)))
                    });
                    i += 2;
                }
                else if (code[i].opcode == OpCodes.Callvirt && (code[i].operand as MethodInfo).Name == "get_position")
                    code.InsertRange(i + 1, new[] {
                        new CodeInstruction(OpCodes.Ldloc_1),
                        new CodeInstruction(OpCodes.Ldloc_2),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ThrowThrowable), nameof(OverrideLaunchPosition)))
                    });
            return code;
        }
        static Vector3 OverrideThrowForce(Vector3 original, ThrowableComponent throwable)
        {
            if (Main.instance.sniperRange)
                return throwable.playerNetwork.Camera.transform.forward * original.magnitude;
            return original;
        }
        static Vector3 OverrideLaunchPosition(Vector3 original, ThrowableComponent throwable, Vector3 launchForce)
        {
            if (Main.instance.sniperRange && Physics.Raycast(original, launchForce, out var hit, 1000, ProjectileLayer, QueryTriggerInteraction.Ignore))
                return hit.point - launchForce.normalized * 0.2f;
            return original;
        }
    }

    [HarmonyPatch(typeof(CharacterModelModifications), "Update")]
    static class Patch_FOVUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = 0; i < code.Count; i++)
                if (code[i].opcode == OpCodes.Callvirt && (code[i].operand as MethodInfo).Name == "get_value")
                    code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_FOVUpdate), nameof(OverrideFOV))));
            return code;
        }
        static float OverrideFOV(float original)
        {
            if (Main.instance.ExtraSettingsAPI_Loaded && CanvasHelper.ActiveMenu == MenuType.None && MyInput.GetButton(Main.instance.zoomKey))
                return Main.instance.zoomFOV;
            if (Main.instance.FOVOverride > 0)
                return Main.instance.FOVOverride;
            return original;
        }
    }

    [HarmonyPatch(typeof(Bobber))]
    static class Patch_Bobber
    {
        [HarmonyPatch("FishOnHook")]
        [HarmonyPrefix]
        static void FishOnHook(ref float timeDelay) => timeDelay *= Main.instance.fishingWait;

        [HarmonyPatch("FishEscaped")]
        [HarmonyPrefix]
        static void FishEscaped(ref float timeDelay) => timeDelay *= Main.instance.fishingWindow;
    }

    [HarmonyPatch]
    static class Patch_PickingUp
    {
        public static bool calling = false;

        [HarmonyPatch(typeof(Pickup), "AddItemToInventory")]
        [HarmonyPrefix]
        static void Prefix_AddItemToInventory(PickupItem item)
        {
            if (!item.GetComponent<DropItem>() && !item.GetComponent<Arrow>())
            {
                calling = Main.instance.itemLooting != 1;
                if (Main.instance.itemLooting != 1 && item.itemInstance != null && item.itemInstance.Valid)
                    item.itemInstance.Amount = item.itemInstance.Amount.Multiply(Main.instance.itemLooting);
            }
        }

        [HarmonyPatch(typeof(Pickup), "AddItemToInventory")]
        [HarmonyFinalizer]
        static void Postfix_AddItemToInventory() => calling = false;

        [HarmonyPatch(typeof(YieldHandler), "CollectYield")]
        [HarmonyPrefix]
        static void Prefix_CollectYield() => calling = true;

        [HarmonyPatch(typeof(YieldHandler), "CollectYield")]
        [HarmonyFinalizer]
        static void Postfix_CollectYield() => calling = false;

        [HarmonyPatch(typeof(HarvestableTree), "Harvest")]
        [HarmonyPrefix]
        static void Prefix_Harvest() => calling = Main.instance.itemLooting != 1;

        [HarmonyPatch(typeof(HarvestableTree), "Harvest")]
        [HarmonyPostfix]
        static void Postfix_Harvest() => calling = false;
    }

    [HarmonyPatch(typeof(PlayerInventory), "AddItem")]
    static class Patch_AddItemToPlayer
    {
        [HarmonyPatch(new[] { typeof(string), typeof(int) })]
        static void Prefix(ref int amount)
        {
            if (Patch_PickingUp.calling)
                amount = amount.Multiply(Main.instance.itemLooting);
        }
    }

    [HarmonyPatch]
    static class Patch_Respawn {
        [HarmonyPatch(typeof(Player), "StartRespawn")]
        [HarmonyPrefix]
        static void StartRespawn(ref bool clearInventory)
        {
            if (Main.instance.keepInventory)
                clearInventory = false;
        }

        [HarmonyPatch(typeof(Player), "RespawnWithoutBed")]
        [HarmonyPrefix]
        static void RespawnWithoutBed(ref bool clearInventory)
        {
            if (Main.instance.keepInventory)
                clearInventory = false;
        }
    }

    [HarmonyPatch(typeof(Reciever_Antenna), "HasValidConnection", MethodType.Getter)]
    static class Patch_AntennaValid
    {
        static void Postfix(Reciever_Antenna __instance, ref bool __result)
        {
            if (Main.instance.antennaAnywhere)
                __result = __instance.ConnectedToReciever;
        }
    }
    [HarmonyPatch(typeof(Reciever), "HasCorrectAltitude", MethodType.Getter)]
    static class Patch_RecieverValid
    {
        static void Postfix(Reciever __instance, ref bool __result)
        {
            if (Main.instance.recieverAnywhere)
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Item_Base), "MaxUses", MethodType.Getter)]
    static class Patch_ItemMaxUses
    {
        static void Postfix(Item_Base __instance, ref int __result)
        {
            if (__result > 1)
            {
                if (__instance.UniqueName.StartsWith("Battery"))
                    __result = __result.Multiply(Main.instance.batteryMax);
                else if (__instance.settings_consumeable?.FoodType > FoodType.None)
                    __result = __result.Multiply(Main.instance.foodMax);
                else if (__instance.settings_equipment?.EquipType > EquipSlotType.None)
                    __result = __result.Multiply(Main.instance.equipmentMax);
                else
                    __result = __result.Multiply(Main.instance.toolMax);
                if (__result < 1)
                    __result = 1;
            }
        }
    }

    [HarmonyPatch]
    static class Patch_GetMaxUsesFixer
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var l = new List<MethodBase>();
            var a = typeof(Raft).Assembly;
            foreach (var t in a.GetTypes())
                foreach (var m in t.GetMethods(~BindingFlags.Default))
                    try
                    {
                        var code = PatchProcessor.GetCurrentInstructions(m, out var iL);
                        for (int i = 0; i < code.Count; i++)
                            if (code[i].operand is MethodInfo method && method.Name == "get_MaxUses")
                            {
                                l.Add(m);
                                break;
                            }
                    }
                    catch { }
            return l;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions;
    }

    [HarmonyPatch(typeof(ItemCollector), "OnTriggerEnter")]
    static class Patch_ItemCollectorMax
    {
        static void Prefix(ref int ___maxNumberOfItems, ref int? __state)
        {
            if (Main.instance.collectorMax != 1)
            {
                __state = ___maxNumberOfItems;
                ___maxNumberOfItems = ___maxNumberOfItems.Multiply(Main.instance.collectorMax);
            }
        }
        static void Postfix(ref int ___maxNumberOfItems, int? __state)
        {
            if (__state != null)
                ___maxNumberOfItems = __state.Value;
        }
    }

    [HarmonyPatch(typeof(CookingSlot))]
    static class Patch_CookingSlot
    {
        const int metalOre = 19;
        const int bolt = 154;
        const int hinge = 155;
        static bool dontSkip = true;
        [HarmonyPatch("CanCookItem")]
        [HarmonyPostfix]
        static void CanCookItem(CookingSlot __instance, Item_Base itemToCheck, ref bool __result)
        {
            if (Main.instance.recycleMetal && dontSkip && !__result && (itemToCheck?.UniqueIndex == bolt || itemToCheck?.UniqueIndex == hinge)) {
                dontSkip = false;
                __result = __instance.CanCookItem(ItemManager.GetItemByIndex(metalOre));
                dontSkip = true;
            }
        }

        [HarmonyPatch("StartCooking")]
        [HarmonyPrefix]
        static void StartCooking(CookingSlot __instance, ref Item_Base cookableItem)
        {
            if (Main.instance.recycleMetal && (cookableItem?.UniqueIndex == bolt || cookableItem?.UniqueIndex == hinge))
            {
                var metal = ItemManager.GetItemByIndex(metalOre);
                dontSkip = false;
                try
                {
                    if (!__instance.CanCookItem(cookableItem) && __instance.CanCookItem(metal))
                        cookableItem = metal;
                } catch { }
                dontSkip = true;
            }
        }
    }

    [HarmonyPatch(typeof(Sprinkler), "WaterNearbyPlots")]
    static class Patch_SprinklerRange
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(code.FindLastIndex(code.FindIndex(x => (x.operand as MethodInfo)?.Name == nameof(Physics.OverlapSphere)), x => x.opcode == OpCodes.Ldc_R4) + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SprinklerRange), nameof(SprinklerRadius))));
            code.Insert(code.FindIndex(code.FindIndex(x => (x.operand as MethodInfo)?.Name == nameof(Mathf.Abs)), x => x.opcode == OpCodes.Ldc_R4) + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SprinklerRange), nameof(SprinklerDistance))));
            return code;
        }

        static float SprinklerRadius(float original)
        {
            if (Main.instance.sprinklerRadius != 1)
                return original * Main.instance.sprinklerRadius;
            return original;
        }
        static float SprinklerDistance(float original)
        {
            if (Main.instance.sprinklerDistance != 1)
                return original * Main.instance.sprinklerDistance;
            return original;
        }
    }

    [HarmonyPatch(typeof(CookingTable), "HandlePickupFood")]
    static class Patch_HandlePickupCookingTableFood
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetSelectedHotbarItem") + 1;
            code.InsertRange(ind, new[] {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(CookingTable),"pickupFoodItem")),
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(Patch_HandlePickupCookingTableFood),nameof(OverrideTargetItem)))
            });
            return code;
        }

        static ItemInstance OverrideTargetItem(ItemInstance original, CookingTable cookingTable, Item_Base pickupItem)
        {
            if (Main.instance.cookingTableMode == CookingTableMode.NoChange)
                return original;
            if (Main.instance.cookingTableMode == CookingTableMode.DisablePickupItem)
                return new ItemInstance(pickupItem, 1, pickupItem.MaxUses);
            if (original != null && original.Valid && original.UniqueIndex == pickupItem.UniqueIndex)
                return original;
            var amount = cookingTable.localPlayer.Inventory.GetItemCount(pickupItem);
            if (amount > 0)
                return new ItemInstance(pickupItem, amount, pickupItem.MaxUses);
            return null;
        }
    }

    [HarmonyPatch(typeof(CookingTable), "PickupFood")]
    static class Patch_PickupCookingTableFood
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindLastIndex(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "RemoveItem"), x => x.operand is MethodInfo m && m.Name == "get_UniqueName") + 1;
            code.Insert(ind, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_PickupCookingTableFood), nameof(OverrideRemoveItem))));
            return code;
        }

        static string OverrideRemoveItem(string original)
        {
            if (Main.instance.cookingTableMode == CookingTableMode.DisablePickupItem)
                return null;
            return original;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.UseDistance), MethodType.Getter)]
    static class Patch_PlayerUseDistance
    {
        static void Postfix(ref float __result) => __result *= Main.instance.playerReach;
    }

    [HarmonyPatch(typeof(ItemInstance))]
    static class Patch_ItemInstance
    {
        [HarmonyPatch(MethodType.Constructor, typeof(Item_Base), typeof(int), typeof(int), typeof(string))]
        [HarmonyPostfix]
        static void ctor(ItemInstance __instance)
        {
            Main.consumeableCache[__instance.settings_consumeable] = __instance.baseItem;
        }

        [HarmonyPatch(nameof(ItemInstance.Clone))]
        [HarmonyPostfix]
        static void Clone(ItemInstance __result) => ctor(__result);
    }

    [HarmonyPatch(typeof(ItemInstance_Consumeable), nameof(ItemInstance_Consumeable.ItemAfterUse), MethodType.Getter)]
    static class Patch_ItemAfterUse
    {
        static FieldInfo _r = AccessTools.Field(typeof(CookingTable), "allRecipes");
        static SO_CookingTable_Recipe[] allRecipes {
            get
            {
                var v = _r.GetValue(null);
                if (v == null)
                {
                    v = Resources.LoadAll<SO_CookingTable_Recipe>("SO_CookingRecipes");
                    _r.SetValue(null, v);
                }
                return (SO_CookingTable_Recipe[])v;
            }
        }

        static Item_Base _p;
        static Item_Base PotItem => _p ? _p : (_p = ItemManager.GetItemByName("Claybowl_Empty"));

        static Item_Base _g;
        static Item_Base JuiceItem => _g ? _g : (_g = ItemManager.GetItemByName("DrinkingGlass"));

        static void Postfix(ItemInstance_Consumeable __instance, ref Cost __result)
        {
            if (Main.instance.returnDishes == DishReturnMode.NoChange)
                return;
            if (Main.consumeableCache.TryGetValue(__instance, out var i))
            {
                if (i.UniqueName == "Jar_Honey")
                    __result = new Cost(Main.scrappedGlass, 1);
                else if (Main.instance.cookingTableMode != CookingTableMode.DisablePickupItem && allRecipes.Find(x => x.Result.UniqueIndex == i.UniqueIndex, out var r))
                    __result = new Cost(r.RecipeType == CookingRecipeType.CookingPot ? PotItem : JuiceItem, 1);
            }
        }
    }

    [HarmonyPatch(typeof(Slot), nameof(Slot.IncrementUses))]
    static class Patch_IncrementSlotUses
    {
        static FieldInfo _i = AccessTools.Field(typeof(Slot), "inventory");
        static Inventory GetInventory(Slot slot) => (Inventory)_i.GetValue(slot);
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code[code.FindIndex(x => x.operand is MethodInfo m && m.Name == "SetItem")] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_IncrementSlotUses), nameof(AddItem)));
            return code;
        }

        static void AddItem(Slot slot, Item_Base item, int amount) => GetInventory(slot).AddItem(new ItemInstance(item, amount, item.MaxUses));
    }

    [HarmonyPatch(typeof(ItemObjectEnabler), nameof(ItemObjectEnabler.ActivateModel), typeof(Item_Base))]
    static class Patch_CookingTableActivateItemModel
    {
        static void Prefix(ItemObjectEnabler __instance, ref ItemModelConnection[] ___itemConnections, Item_Base item)
        {
            if (___itemConnections == null || !item || !(__instance is ItemObjectEnabler_CookingTable) || item.UniqueName != "Bucket")
                return;
            ItemModelConnection i = null;
            foreach (var c in ___itemConnections)
                if (c.item.UniqueName == "Bucket")
                    return;
                else if (c.item.UniqueName == "Bucket_Milk")
                    i = c;
            if (i != null)
            {
                var n = Object.Instantiate(i.model, i.model.transform.parent);
                while (n.transform.childCount > 0)
                    Object.DestroyImmediate(n.transform.GetChild(0).gameObject);
                var c = new ItemModelConnection() { item = ItemManager.GetItemByName("Bucket"), model = n };
                ___itemConnections = ___itemConnections.AddToArray(c);
            }
        }
    }

    [HarmonyPatch(typeof(CookingTable_Slot), nameof(CookingTable_Slot.ThrowItemIntoPotAnimation))]
    static class Patch_CookingTableThrowItem
    {
        static bool Prefix(CookingTable_Slot __instance, ItemInstance ___currentItem)
        {
            if (Main.instance.returnDishes == DishReturnMode.Full && ___currentItem?.settings_consumeable?.ItemAfterUse?.item)
            {
                var r = ___currentItem.settings_consumeable.ItemAfterUse;
                __instance.InsertItem(null, new ItemInstance(r.item, r.amount, r.item.MaxUses));
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PersonController), "GroundControll")]
    static class Patch_UpdateGroundControll
    {
        static Vector3 last;
        static HashSet<Door> currentlyOpen;
        static void Postfix(PersonController __instance, Network_Player ___playerNetwork)
        {
            if (!___playerNetwork.IsLocalPlayer)
                return;
            if (!Main.instance.autoDoors && (currentlyOpen == null || currentlyOpen.Count == 0))
                return;
            var cur = __instance.transform.position;
            var travel = cur - last;
            var doors = new HashSet<Door>();
            if (Main.instance.autoDoors)
            {
                var cap = (__instance.controller.transform.TransformPoint(__instance.controller.center + new Vector3(0, __instance.controller.height / 2, 0)),
                        __instance.controller.transform.TransformPoint(__instance.controller.center - new Vector3(0, __instance.controller.height / 2, 0)),
                        __instance.controller.radius,
                        Main.CreateMask(__instance.controller.gameObject.layer));

                if (travel != Vector3.zero)
                {
                    travel = travel.normalized;
                    var hits = Physics.CapsuleCastAll(cap.Item1, cap.Item2, cap.Item3, travel, Main.openDistance, cap.Item4, QueryTriggerInteraction.Collide);
                    foreach (var h in hits)
                    {
                        var d = h.transform.GetComponentInParent<Door>();
                        if (d != null)
                            doors.Add(d);
                    }
                }
                var cols = Physics.OverlapCapsule(cap.Item1, cap.Item2, cap.Item3, cap.Item4, QueryTriggerInteraction.Collide);
                foreach (var c in cols)
                {
                    var d = c.GetComponentInParent<Door>();
                    if (d != null)
                        doors.Add(d);
                }
            }
            UpdateOpen(doors);
            last = cur;
        }

        static void UpdateOpen(HashSet<Door> doors)
        {
            var r = new HashSet<Door>();
            if (currentlyOpen == null)
                currentlyOpen = new HashSet<Door>();
            foreach (var d in currentlyOpen)
                if (d)
                {
                    if (doors.Contains(d))
                        d.SetOpenNetworked(true);
                    else
                    {
                        r.Add(d);
                        d.SetOpenNetworked(false);
                    }
                }
                else
                    r.Add(d);
            currentlyOpen.RemoveWhere(r.Contains);
            foreach (var d in doors)
                if (!d.open && currentlyOpen.Add(d))
                    d.SetOpenNetworked(true);
        }
    }

    [HarmonyPatch(typeof(Hook), "Update")]
    static class Patch_HookUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindIndex(x => x.opcode == OpCodes.Stloc_3);
            var lbl = code[ind].labels;
            code[ind].labels = new List<Label>();
            code.InsertRange(ind, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0) { labels = lbl },
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HookUpdate), nameof(OverrideLockHook)))
            });
            return code;
        }

        public static bool OverrideLockHook(bool original, Hook hook) => original || (Main.instance.forceMining && !hook.throwable.InWater && MyInput.GetButton("RMB"));
    }

    [HarmonyPatch(typeof(Hook), "HandleGathering")]
    static class Patch_HookGathering
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(code.FindIndex(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_SubmersionState"), x => x.operand is Label), new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HookUpdate), nameof(Patch_HookUpdate.OverrideLockHook)))
            });
            code.Insert(code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "gatherTime") + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HookGathering), nameof(ModifyMineTime))));
            return code;
        }

        static float ModifyMineTime(float original) => original * Main.instance.mineTime;
    }

    [HarmonyPatch(typeof(Seagull), "FindCropplotAndNest")]
    static class Patch_SeagullTargeting
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_ContainsCrops") + 1, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SeagullTargeting), nameof(OverrideCanAttack)))
            });
            return code;
        }

        static bool OverrideCanAttack(bool original, Cropplot cropplot)
        {
            if (Main.instance.seagulls == SeagullMode.Default || !original)
                return original;
            if (Main.instance.seagulls == SeagullMode.DisableAttacking)
                return false;
            foreach (var s in cropplot.GetSlots())
                if (s?.plant && s.plant.item)
                {
                    if (Main.instance.seagulls >= SeagullMode.IgnoreMediumPlants && !s.plant.item.CanPlantInSmall())
                        continue;
                    if ((Main.instance.seagulls == SeagullMode.IgnoreFlowers || Main.instance.seagulls == SeagullMode.IgnoreMediumAndFlowers) && s.plant.item.IsFlowerSeed())
                        continue;
                    return true;
                }
            return false;
        }
    }

    [HarmonyPatch(typeof(AI_State_Attack_Block_Shark), nameof(AI_State_Attack_Block_Shark.UpdateState))]
    static class Patch_SharkFindTargetBlock
    {
        static bool Prefix(AI_StateMachine_Shark ___stateMachineShark)
        {
            if (Raft_Network.IsHost && !Main.instance.AllowSharkAttack)
            {
                ___stateMachineShark.SetSearchBlockProgressCooldown(10f);
                ___stateMachineShark.ChangeState(___stateMachineShark.circulationSurfaceState);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.DamageScarecrow))]
    static class Patch_DamageScarecrow
    {
        static bool Prefix() => !Main.instance.invincibleScarecrow;
    }

    [HarmonyPatch(typeof(UnityEngine.AzureSky.AzureSkyController), "Update")]
    static class Patch_UpdateSky
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo m && m.Name == "m_timeProgression") + 1, new[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_UpdateSky), nameof(OverrideTimeSpeed)))
            });
            return code;
        }

        static float OverrideTimeSpeed(float original) => original == 0 ? 0 : Main.instance.timeSpeed ?? original;
    }

    [HarmonyPatch]
    static class Patch_ConsoleDetectInput
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            bool Target(MethodInfo method)
            {
                try
                {
                    return PatchProcessor.GetCurrentInstructions(method, out _).Any(x => x.operand is MethodInfo m && m.Name == "Play" && m.DeclaringType == typeof(Animation));
                } catch { }
                return false;
            }
            foreach (var t in new[] { typeof(RConsole), typeof(MainMenu) })
            {
                foreach (var m in t.GetMethods(~BindingFlags.Default))
                    if (Target(m))
                        yield return m;
                foreach (var n in t.GetNestedTypes(~BindingFlags.Default))
                    foreach (var m in n.GetMethods(~BindingFlags.Default))
                        if (Target(m))
                            yield return m;
            }
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = 0; i < code.Count; i++)
                if (code[i].operand is MethodInfo m && m.Name == "Play" && m.DeclaringType == typeof(Animation))
                    code[i] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ConsoleDetectInput), nameof(Play), (new[] { typeof(Animation) }).AddRangeToArray(m.GetParameters().Cast(x => x.ParameterType)))) { blocks = code[i].blocks, labels = code[i].labels };
            return code;
        }

        static bool Play(Animation animation) => Play(animation, () => animation.Play(), animation.clip, () => animation.Stop());
        static bool Play(Animation animation, PlayMode mode) => Play(animation, () => animation.Play(mode), animation.clip, () => animation.Stop());
        static bool Play(Animation animation, AnimationPlayMode mode) => Play(animation, () => animation.Play(mode), animation.clip, () => animation.Stop());
        static bool Play(Animation animation, string stateName) => Play(animation, () => animation.Play(stateName), stateName, () => animation.Stop(stateName));
        static bool Play(Animation animation, string stateName, PlayMode mode) => Play(animation, () => animation.Play(stateName, mode), stateName, () => animation.Stop(stateName));
        static bool Play(Animation animation, string stateName, AnimationPlayMode mode) => Play(animation, () => animation.Play(stateName, mode), stateName, () => animation.Stop(stateName));
        static bool Play(Animation animation, Func<bool> play, object search, Action stop)
        {
            bool r = play();
            if (!r)
            {
                stop();
                r = play();
            }
            if (r && Main.instance.pausedConsoleFix && Time.timeScale < 0.2)
            {
                if (search is AnimationClip c)
                {
                    foreach (AnimationState s in animation)
                        if (s.clip == c)
                        {
                            s.normalizedTime = 1;
                            break;
                        }
                }
                else if (search is string s)
                    animation[s].normalizedTime = 1;
            }
            return r;
        }
    }

    [HarmonyPatch(typeof(Axe), nameof(Axe.OnAxeHit))]
    static class Patch_AxeHit
    {
        public static Axe instance;
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var inF = AccessTools.Field(typeof(Patch_AxeHit), nameof(instance));
            var ind = code.FindIndex(x => x.operand is MethodInfo m && m.Name == nameof(PlayerInventory.RemoveDurabillityFromHotSlot));
            code.InsertRange(ind + 1, new[] {
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Stsfld,inF)
            });
            code.InsertRange(ind, new[] {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Stsfld, inF)
            });
            ind = code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "hitmask");
            code.Insert(ind + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AxeHit), nameof(ModifyLayerMask))));
            ind = code.FindIndex(x => x.operand is MethodInfo m && m.Name == nameof(Helper.HitAtCursor));
            code.InsertRange(ind + 1, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AxeHit), nameof(HitSuccessful)))
            });
            return code;
        }

        static LayerMask ModifyLayerMask(LayerMask original) => Main.instance.axeWeapon ? new LayerMask() { value = original.value | (1 << 12) } : original;

        static bool HitSuccessful(bool original, Axe axe)
        {
            if (!original || !Main.instance.axeWeapon)
                return original;
            var h = axe.RayHit();
            var o = h.transform.gameObject;
            if (o.layer == 12)
            {
                var e = o.GetComponentInParent<Network_Entity_Redirect>();
                if (e && e.entity)
                {
                    ComponentManager<Network_Host>.Value.DamageEntity(e.entity, h.transform, 5, h.point, h.normal, EntityType.Player, null);
                    if (!e.entity.IsInvurnerable && e.entity.removesDurabilityWhenHit)
                        return true;
                }
            }
            return original;
        }
    }

    [HarmonyPatch(typeof(Axe), "Update")]
    static class Patch_AxeUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindIndex(x => x.opcode == OpCodes.Stloc_3);
            code.InsertRange(ind + 1, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AxeUpdate), nameof(MaybeDamage)))
            });
            return code;
        }
        static void MaybeDamage(Axe instance)
        {
            if (Main.instance.axeDur == AxeDurabilityMode.OnBreak)
                instance.GetComponentInParent<Network_Player>().Inventory.RemoveDurabillityFromHotSlot();
        }
    }

    [HarmonyPatch(typeof(ZiplinePlayer), "UpdateZiplineMovement")]
    static class Patch_ZiplineMovementUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1; i >= 0; i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f)
                {
                    if (f.Name == "electricZiplineSpeed")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ZiplineMovementUpdate), nameof(ModifyAccel))));
                    if (f.Name == "maxElectricZiplineSpeed")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ZiplineMovementUpdate), nameof(ModifySpeed))));
                }
            return code;
        }

        static float ModifyAccel(float original) => original * Main.instance.zipAccel;
        static float ModifySpeed(float original) => original * Main.instance.zipSpeed;
    }

    [HarmonyPatch(typeof(PlayerInventory),nameof(PlayerInventory.RemoveDurabillityFromHotSlot))]
    static class Patch_RemoveDurabillityFromHotSlot
    {
        static bool Prefix()
        {
            if (Patch_AxeHit.instance)
            {
                var i = Patch_AxeHit.instance.RayHit().transform;
                Patch_AxeHit.instance = null;
                if (Main.instance.axeDur != AxeDurabilityMode.NoChange && i && i.GetComponentInParent<Block>())
                    return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TradingPostUI),nameof(TradingPostUI.Open))]
    static class Patch_OpenTradingUI
    {
        static int GetTier(Item_Base item) => item.UniqueName.Contains("Electric") || item.UniqueName.Contains("Titanium") ? 3 : item.UniqueName.Contains("Advanced") ? 2 : 1;
        static List<SO_TradingPost_Buyable.Instance> _b;
        public static List<SO_TradingPost_Buyable.Instance> CustomBuyables
        {
            get
            {
                if (_b == null)
                {
                    _b = new List<SO_TradingPost_Buyable.Instance>();
                    var b = new List<Item_Base>();
                    Item_Base coin = null;
                    Item_Base cube = null;
                    foreach (var i in ItemManager.GetAllItems())
                        if (i.UniqueName.StartsWith("Blueprint_"))
                            b.Add(i);
                        else if (i.UniqueName == "TradeToken")
                            coin = i;
                        else if (i.UniqueName == "Trashcube")
                            cube = i;

                    foreach (var i in b)
                    {
                        var t = GetTier(i);
                        _b.Add(new SO_TradingPost_Buyable.Instance()
                        {
                            cost = new[]
                            {
                                new Cost(cube, t * 2),
                                new Cost(coin, t * 2)
                            },
                            reward = new Cost(i, 1),
                            stock = 1,
                            tier = (TradingPost.Tier)Enum.Parse(typeof(TradingPost.Tier), "tier" + t, true)
                        });
                    }
                }
                return _b;
            }
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "buyableItems");
            code.Insert(ind + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_OpenTradingUI), nameof(ModifyBuyableItems))));
            return code;
        }

        static List<SO_TradingPost_Buyable.Instance> ModifyBuyableItems(List<SO_TradingPost_Buyable.Instance> original)
        {
            if (!Main.instance.buyBlueprints)
                return original;
            var l = original.ToList();
            foreach (var i in CustomBuyables)
                if (i.reward.item && !original.Any(x => x.reward.item && x.reward.item.UniqueIndex == i.reward.item.UniqueIndex))
                    l.Add(i);
            return l;
        }
    }

    [HarmonyPatch(typeof(TradingPost), nameof(TradingPost.PurchaseItem))]
    static class Patch_BuyTradingItem
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var ind = code.FindIndex(x => x.opcode == OpCodes.Stloc_0);
            code.InsertRange(ind, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_BuyTradingItem), nameof(DefaultInstance)))
            });
            return code;
        }

        static SO_TradingPost_Buyable.Instance DefaultInstance(SO_TradingPost_Buyable.Instance original, int itemIndex)
        {
            if (!Main.instance.buyBlueprints)
                return original;
            foreach (var i in Patch_OpenTradingUI.CustomBuyables)
                if (i.reward.item && i.reward.item.UniqueIndex == itemIndex)
                    return i;
            return original;
        }
    }

    [HarmonyPatch(typeof(Buff),"UpdateTimer")]
    static class Patch_BuffTimerUpdate
    {
        static bool Prefix(Buff __instance, BuffDisplayObject ___buffDisplayObject, float ___durationTimer, ref bool __result)
        {
            if (Main.instance.permHearty && __instance.Asset.ApplyDeathPrevention)
            {
                var f = Traverse.Create(___buffDisplayObject).Field<float>("progress");
                if (f.Value > ___durationTimer)
                    f.Value = ___durationTimer;
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Brick_Wet), "Update")]
    static class Patch_BrickDryProgress
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_BrickDryProgress), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original != 0 ? original.SafeDivide(Main.instance.brickDry) : original;
    }

    [HarmonyPatch]
    static class Patch_AnimalsUpdateDrop
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(AI_NetworkBehaviour_Chicken), "UpdateHost");
            yield return AccessTools.Method(typeof(AI_NetworkBehaviour_Domestic_Resource), "UpdateHost");
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AnimalsUpdateDrop), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original != 0 ? original.SafeDivide(Main.instance.animalProduce) : original;
    }

    [HarmonyPatch]
    static class Patch_MeleeWeaponRange
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(MeleeWeapon), "OnMeleeWeaponSuccesfullRayHit");
            yield return AccessTools.Method(typeof(MeleeWeapon), "OnMeleeWeaponSuccesfullRayHitAll");
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1; i >= 0; i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f)
                {
                    if (f.Name == "attackRange")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_MeleeWeaponRange), nameof(ModifyRange))));
                    if (f.Name == "attackRadius")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_MeleeWeaponRange), nameof(ModifyRadius))));
                }
            return code;
        }

        static float ModifyRange(float original) => original * Main.instance.meleeWeaponRange;
        static float ModifyRadius(float original) => original * Main.instance.meleeWeaponRadius;
    }

    [HarmonyPatch(typeof(UseableItem),"ChannelItem")]
    static class Patch_ChannelUseableItem
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ChannelUseableItem), nameof(ModifyTime)))
                });
            return code;
        }
        static float ModifyTime(float original, UseableItem usingItem) => original != 0 && usingItem is Shovel ? original.SafeDivide(Main.instance.shovelTime) : original;
    }

    [HarmonyPatch(typeof(TreasurePoint), "SendNetworkProgressExcevation")]
    static class Patch_ProgressTreasureExcevation
    {
        static void Prefix(ref int amount)
        {
            if (Main.instance.treasureSpeed != 0)
                amount *= Main.instance.treasureSpeed;
        }
    }

    static class ExtentionMethods
    {
        public static void CopyFieldsOf(this object value, object source)
        {
            var t1 = value.GetType();
            var t2 = source.GetType();
            while (!t1.IsAssignableFrom(t2))
                t1 = t1.BaseType;
            while (t1 != typeof(Object) && t1 != typeof(object))
            {
                foreach (var f in t1.GetFields(~BindingFlags.Default))
                    if (!f.IsStatic)
                        f.SetValue(value, f.GetValue(source));
                t1 = t1.BaseType;
            }
        }
        public static float ParseFloat(this string value, float EmptyFallback = 1) => value.ParseNFloat() ?? EmptyFallback;
        public static float? ParseNFloat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (value.Contains(",") && !value.Contains("."))
                value = value.Replace(',', '.');
            var c = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-NZ");
            Exception e = null;
            float r = 0;
            try
            {
                r = float.Parse(value);
            }
            catch (Exception e2)
            {
                e = e2;
            }
            CultureInfo.CurrentCulture = c;
            if (e != null)
                throw e;
            return r;
        }
        public static double ParseDouble(this string value, double EmptyFallback = 1) => value.ParseNDouble() ?? EmptyFallback;
        public static double? ParseNDouble(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (value.Contains(",") && !value.Contains("."))
                value = value.Replace(',', '.');
            var c = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-NZ");
            Exception e = null;
            double r = 0;
            try
            {
                r = double.Parse(value);
            }
            catch (Exception e2)
            {
                e = e2;
            }
            CultureInfo.CurrentCulture = c;
            if (e != null)
                throw e;
            return r;
        }
        public static int Multiply(this int value, double multiplier)
        {
            if (multiplier == 1)
                return value;
            var v = value * multiplier;
            if (v >= int.MaxValue)
                return int.MaxValue;
            if (v <= int.MinValue)
                return int.MinValue;
            var i = (int)Math.Round(v);
            if (((value < 0) ^ (multiplier < 0)) == (i < 0))
                return i;
            return i < 0 ? int.MaxValue : int.MinValue;
        }
        public static int Multiply(this int value, float multiplier)
        {
            if (multiplier == 1)
                return value;
            var v = value * multiplier;
            if (v >= int.MaxValue)
                return int.MaxValue;
            if (v <= int.MinValue)
                return int.MinValue;
            var i = (int)Mathf.Round(v);
            if (((value < 0) ^ (multiplier < 0)) == (i < 0))
                return i;
            return i < 0 ? int.MaxValue : int.MinValue;
        }
        public static int ParseInt(this string value, int EmptyFallback = 1)
        {
            if (string.IsNullOrWhiteSpace(value))
                return EmptyFallback;
            if (value.Contains(",") && !value.Contains("."))
                value = value.Replace(',', '.');
            var c = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-NZ");
            Exception e = null;
            int r = 0;
            try
            {
                r = int.Parse(value);
            }
            catch (Exception e2)
            {
                e = e2;
            }
            CultureInfo.CurrentCulture = c;
            if (e != null)
                throw e;
            return r;
        }

        public static bool Find<T>(this IEnumerable<T> collection, Predicate<T> predicate, out T first) {
            foreach (var i in collection)
                if (predicate(i))
                {
                    first = i;
                    return true;
                }
            first = default;
            return false;
        }

        public static Item_Base Clone(this Item_Base source, int uniqueIndex, string uniqueName)
        {
            Item_Base item = ScriptableObject.CreateInstance<Item_Base>();
            item.Initialize(uniqueIndex, uniqueName, source.MaxUses);
            item.settings_buildable = source.settings_buildable.Clone();
            item.settings_consumeable = source.settings_consumeable.Clone();
            item.settings_cookable = source.settings_cookable.Clone();
            item.settings_equipment = source.settings_equipment.Clone();
            item.settings_Inventory = source.settings_Inventory.Clone();
            item.settings_recipe = source.settings_recipe.Clone();
            item.settings_usable = source.settings_usable.Clone();
            return item;
        }
        public static void SetRecipe(this Item_Base item, CostMultiple[] cost, CraftingCategory category = CraftingCategory.Resources, int amountToCraft = 1, bool learnedFromBeginning = false, string subCategory = null, int subCatergoryOrder = 0)
        {
            Traverse recipe = Traverse.Create(item.settings_recipe);
            recipe.Field("craftingCategory").SetValue(category);
            recipe.Field("amountToCraft").SetValue(amountToCraft);
            recipe.Field("learnedFromBeginning").SetValue(learnedFromBeginning);
            recipe.Field("subCategory").SetValue(subCategory);
            recipe.Field("subCatergoryOrder").SetValue(subCatergoryOrder);
            item.settings_recipe.NewCost = cost;
        }

        public static void SetOpenNetworked(this Door door, bool open)
        {
            if (door.open == open)
                return;
            var network = ComponentManager<Raft_Network>.Value;
            if (!Raft_Network.IsHost)
            {
                network.SendP2P(network.HostID, new Message_NetworkBehaviour(door.open ? Messages.Door_Close : Messages.Door_Open, door), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
                return;
            }
            if (door.open)
            {
                door.Close();
                network.RPC(new Message_NetworkBehaviour(Messages.Door_Close, door), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
                return;
            }
            door.Open();
            network.RPC(new Message_NetworkBehaviour(Messages.Door_Open, door), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
        }

        static MethodInfo _close => AccessTools.Method(typeof(Door), "Close");
        public static void Close(this Door door) => _close.Invoke(door, new object[0]);

        static int smallCropplotIndex = -1;
        public static bool CanPlantInSmall(this Item_Base item)
        {
            var l = ItemManager.GetAllItems();
            if (smallCropplotIndex == -1 || smallCropplotIndex >= l.Count || !l[smallCropplotIndex] || l[smallCropplotIndex].UniqueName == "Placeable_Cropplot_Small")
            {
                smallCropplotIndex = l.FindIndex(x => x && x.UniqueName == "Placeable_Cropplot_Small");
                if (smallCropplotIndex == -1)
                    return true;
            }
            var i = l[smallCropplotIndex];
            var c = i.settings_buildable.GetBlockPrefab(0) as Cropplot;
            if (!c)
                return true;
            return c.AcceptsPlantType(item);
        }
        public static bool IsFlowerSeed(this Item_Base item) => item.UniqueName.StartsWith("Seed_Flower");

        public static T GetOrAddComponent<T>(this Component component) where T : Component => component.gameObject.GetOrAddComponent<T>();
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var c = gameObject.GetComponent<T>();
            if (c)
                return c;
            return gameObject.AddComponent<T>();
        }

        public static Y[] Cast<X,Y>(this X[] a, Func<X,Y> caster)
        {
            var n = new Y[a.Length];
            for (int i = 0; i < n.Length; i++)
                n[i] = caster(a[i]);
            return n;
        }

        static FieldInfo _rayHit => AccessTools.Field(typeof(Axe), "rayHit");
        public static RaycastHit RayHit(this Axe axe) => (RaycastHit)_rayHit.GetValue(axe);

        public static float SafeDivide(this float divided, float divider) => divided == 0 ? 0 : divider == 0 ? divided < 0 ? float.NegativeInfinity : float.PositiveInfinity : (divided / divider);
    }
}