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
using Random = UnityEngine.Random;
using UltimateWater;
using UnityEngine.AzureSky;
using UnityEngine.Experimental.Rendering;
using System.Text;
using System.Runtime.InteropServices;


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
        public string zoomKey;
        public int zoomFOV = 30;
        public float autoOff = -1;
        public bool fishingEqual = false;
        public bool freeCrafting = false;
        int _cl = -1;
        public int consoleLines
        {
            get => _cl;
            set
            {
                if (_cl == value)
                    return;
                _cl = value;
                if (_cl > 0)
                    SetConsoleCount(_cl);
            }
        }
        public float fishingWait = 1;
        public float fishingWindow = 1;
        public double itemLooting = 1;
        public KeepInventoryMode keepInventory = KeepInventoryMode.Default;
        bool _ra = false;
        public bool recieverAnywhere
        {
            get => _ra;
            set
            {
                _ra = value;
                foreach (var r in FindObjectsOfType<Reciever>())
                    r.CheckSignal();
            }
        }
        bool _aa = false;
        public bool antennaAnywhere
        {
            get => _aa;
            set
            {
                _aa = value;
                foreach (var r in FindObjectsOfType<Reciever>())
                    r.CheckSignal();
            }
        }
        bool _ard = false;
        public bool antennaAnyDistReciever
        {
            get => _ard;
            set
            {
                _ard = value;
                foreach (var r in FindObjectsOfType<Reciever>())
                    r.CheckSignal();
            }
        }
        bool _aad = false;
        public bool antennaAnyDistAntenna
        {
            get => _aad;
            set
            {
                _aad = value;
                foreach (var r in FindObjectsOfType<Reciever>())
                    r.CheckSignal();
            }
        }
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
        public float timeSpeed = float.NaN;
        public bool pausedConsoleFix = false;
        public AxeDurabilityMode axeDur = AxeDurabilityMode.NoChange;
        public bool axeWeapon = false;
        public float zipAccel = 1;
        public float zipSpeed = 1;
        public bool buyBlueprints = false;
        public bool permHearty = false;
        bool _bs = false;
        public bool boostedStacks
        {
            get => _bs;
            set
            {
                if (_bs == value)
                    return;
                _bs = value;
                UpdateStackSizes();
            }
        }
        public float brickDry = 1;
        public float animalProduce = 1;
        public float meleeWeaponRange = 1;
        public float meleeWeaponRadius = 1;
        public float shovelTime = 1;
        public int treasureSpeed = 1;
        public bool seagullEggs =false;
        RecycleButton _rb = RecycleButton.Disabled;
        public RecycleButton recycleButton
        {
            get => _rb;
            set
            {
                if (_rb == value)
                    return;
                _rb = value;
                foreach (var r in Resources.FindObjectsOfTypeAll<SelectedRecipeBox>())
                    if (Traverse.Create(r).Field("initialized").GetValue<bool>())
                        Patch_UncraftButton.TryAddButton(r);
            }
        }
        public RecycleMode recycleMode = RecycleMode.Random;
        public float returnPercent = 0.5f;
        public bool infinitePower = false;
        float _aM = 1;
        float _aE = 1;
        float _wM = 1;
        float _wE = 1;
        public float amplitudeMul
        {
            get => _aM;
            set
            {
                if (_aM == value)
                    return;
                _aM = value;
                Patch_WaveCalculatorCreate.RemodifySpectrums();
            }
        }
        public float amplitudeExp
        {
            get => _aE;
            set
            {
                if (_aE == value)
                    return;
                _aE = value;
                Patch_WaveCalculatorCreate.RemodifySpectrums();
            }
        }
        public float windSpeedMul
        {
            get => _wM;
            set
            {
                if (_wM == value)
                    return;
                _wM = value;
                Patch_WaveCalculatorCreate.RemodifySpectrums();
            }
        }
        public float windSpeedExp
        {
            get => _wE;
            set
            {
                if (_wE == value)
                    return;
                _wE = value;
                Patch_WaveCalculatorCreate.RemodifySpectrums();
            }
        }
        public float deathItemLoss = -1;
        public float deathUseLoss = -1;
        public bool allowMultiEquipment = false;
        public bool returnArrows = false;
        public bool infiniteDurability = false;
        public float hungerRate = 1;
        public float thirstRate = 1;
        public bool invisible = false;
        public bool invisibleHost = false;
        public bool invincible = false;
        public bool reverseScroll = false;
        public bool tikiFix = false;
        public bool handMining = false;
        public bool fullLook = false;
        public float fogDistance = 1;
        public float binoFogDistance = 1;
        public float FinalFogDistance => wasBino ? binoFogDistance : fogDistance;
        float _fc = 1;
        public float farClip
        {
            get => _fc;
            set
            {
                if (_fc == value)
                    return;
                _fc = value;
                if (lastCam && !wasBino)
                    lastCam.farClipPlane = prevFarClip * _fc;
            }
        }
        float _fc2 = 1;
        public float binoFarClip
        {
            get => _fc2;
            set
            {
                if (_fc2 == value)
                    return;
                _fc2 = value;
                if (lastCam && wasBino)
                    lastCam.farClipPlane = prevFarClip * _fc2;
            }
        }
        float _lb = 1;
        public float LODBias
        {
            get => _lb;
            set
            {
                if (_lb == value)
                    return;
                _lb = value;
                if (lastQuality != int.MinValue && !wasBino)
                    QualitySettings.lodBias = prevLOD * _lb;
            }
        }
        float _lb2 = 1;
        public float binoLODBias
        {
            get => _lb2;
            set
            {
                if (_lb2 == value)
                    return;
                _lb2 = value;
                if (lastQuality != int.MinValue && wasBino)
                    QualitySettings.lodBias = prevLOD * _lb2;
            }
        }
        public OxygenRegenMode oxygenMode = OxygenRegenMode.Vanilla;
        public float oxyGenSpeed = 1;
        public float oxyLossSpeed = 1;
        public float healthRegen = 1;
        public float healthyOverride = float.NaN;
        public float unhealthyOverride = float.NaN;
        public int sharkCount = 1;
        public float sharkExtraRespawn = 300;
        public float sharkRespawn = 1;
        public int seagullCount = -1;
        public float fuelConsumption = 1;
        public bool disableBinoOverlay = false;
        public int fruitTime = -1;
        bool _fe = false;
        public bool firstpersonEquipment
        {
            get => _fe;
            set
            {
                if (_fe == value)
                    return;
                _fe = value;
                RAPI.GetLocalPlayer().Safe()?.PlayerEquipment.SetAllEquipmentModelVisible(true);
            }
        }
        bool _dr = false;
        public bool disableRudder
        {
            get => _dr;
            set
            {
                if (_dr == value)
                    return;
                _dr = value;
                foreach (var b in BlockCreator.GetPlacedBlocks())
                    if (b && b.GetComponent<SteeringWheel>())
                    {
                        var attach = b.GetComponent<SteeringWheel>().rudderAttachment;
                        if (_dr)
                        {
                            foreach (Transform child in attach.rudderAttachmentPipeParent)
                                Destroy(child.gameObject);
                        }
                        else
                            attach.BuildRudderPipesAttachment(b.GetComponent<SteeringWheel>());
                    }
            }
        }
        public bool unlimitedTrade = false;
        public bool radarSmall = false;
        public bool radarRaft = false;
        public bool radarAch = false;
        public float rcolorSmall = 0.16667f;
        public float rcolorRaft = 0;
        public float rcolorAch = 0.83333f;
        public float cropSpeed = 1;
        public float weatherSpeed = 1;
        public float windDirection = 0;
        public float spawnSpeed = 1;
        public float flipperDurability = 1;
        public float bottleDurability = 1;
        public float lampDurability = 1;
        public float damage = 1;
        public bool rotateWithRaft = false;
        public float waterFade = -1;
        public float sellExp = 1;
        public float hostSellExp = 1;
        public float hookArea = 1;
        public int selectedMulti = 0;
        public bool storeMulti = false;
        public float trashSpawn = 1;
        public float trashQuantity = 1;
        public float trashArea = 1;
        public TrashMode trashMode = TrashMode.Vanilla;
        public AnimalSpecs animalSpecs = new AnimalSpecs();
        string specSelect = "";
        public bool specDespawn
        {
            get => animalSpecs.GetOrDefault(specSelect).IsDisabled;
            set => animalSpecs.GetOrCreate(specSelect).IsDisabled = value;
        }
        public bool specPassive
        {
            get => animalSpecs.GetOrDefault(specSelect).IsPassive;
            set => animalSpecs.GetOrCreate(specSelect).IsPassive = value;
        }
        public bool quickMoveFreeze = true;
        public TrashLagMode trashLagMode = TrashLagMode.Vanilla;
        public float trashLagFix = 0;
        public int trashLagInterval = 0;
        public float trashLagThreshold = 0;
        public float breakSpeed = 1;
        public float removeSpeed = 1;
        public bool hungerRateFix = true;
        bool _rp = true;
        public bool roachPeaceful
        {
            get => _rp;
            set
            {
                _rp = value;
                foreach (var c in Resources.FindObjectsOfTypeAll<AI_Component_SeePlayerSwitchState>())
                    if (c.connectedState && c.GetComponentInParent<AI_NetworkBehaviour>() && c.GetComponentInParent<AI_NetworkBehaviour>().behaviourType == AI_NetworkBehaviourType.Roach)
                    {
                        Traverse.Create(c).Field("isThisTamed").SetValue(false);
                        c.Start();
                    }
            }
        }
        public bool radarFix = true;
        bool _nd = true;
        public bool netDriftFix
        {
            get => _nd;
            set
            {
                if (_nd = value)
                    foreach (var block in BlockCreator.GetPlacedBlocks())
                        Patch_CreateBlock.Postfix(block);
            }
        }
        public bool gradualUnhealthy = false;
        public bool alwaysSpecial = false;
        public float dropChance = 1;
        public float dropDespawn = 1;

        public bool AllowSharkAttack => ComponentManager<RaftBounds>.Value.FoundationCount >= sharkAttackThresholdMin && (sharkAttackThresholdMax < 0 || sharkAttackThresholdMax >= ComponentManager<RaftBounds>.Value.FoundationCount);
        public bool UsingBino => ComponentManager<CanvasHelper>.Value && ComponentManager<CanvasHelper>.Value.binocularImage && ComponentManager<CanvasHelper>.Value.binocularImage.activeSelf;
        public static Item_Base scrappedGlass;
        public static RectTransform parent;
        public static GameObject consoleItemPrefab;
        public static Transform consoleItemParent;
        public static List<Object> createdObjects = new List<Object>();
        public void Awake()
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
            OnEventAttribute.CallAll<OnModLoad>();
            Log("Mod has been loaded!");
        }

        public void OnModUnload()
        {
            harmony?.UnpatchAll(harmony.Id);
            foreach (var o in createdObjects)
                if (o)
                {
                    if (o is Item_Base i)
                        ItemManager.GetAllItems().RemoveAll(x => x.UniqueIndex == i.UniqueIndex);
                    Destroy(o);
                }
            OnEventAttribute.CallAll<OnModUnload>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (boostedStacks)
            {
                boostedStacks = false;
                UpdateStackSizes();
            }
            if (lastCam) lastCam.farClipPlane = prevFarClip;
            if (lastQuality != int.MinValue) QualitySettings.lodBias = prevLOD;
            Log("Mod has been unloaded!");
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == Raft_Network.GameSceneName)
                InitCache();
            else if (scene.name == Raft_Network.MenuSceneName)
                Patch_DelayedSharkSpawn.pendingCount = 0;
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
                    f.Value = i.MaxUses > 1 ? Math.Min(int.MaxValue / i.MaxUses, short.MaxValue) : short.MaxValue;
                }
        }

        void ExtraSettingsAPI_SettingsOpen()
        {
            var items = animalSpecs.Keys.Where(x => !string.IsNullOrWhiteSpace(x)).Concat(Enum.GetValues(typeof(AI_NetworkBehaviourType)).Cast<AI_NetworkBehaviourType>().Select(x => x.ToString()).Where(x => !animalSpecs.ContainsKey(x) && !x.StartsWith("NPC_"))).ToArray();
            Array.Sort(items);
            ExtraSettingsAPI_SetComboboxContentKeepItem("specSelect", items);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ExtraSettingsAPI_SetComboboxContentKeepItem(string settingName, string[] items) { }

        void ExtraSettingsAPI_ButtonPress(string SettingName)
        {
            if (SettingName == "reset")
                ExtraSettingsAPI_ResetAllSettings();
            else if (SettingName == "splitReset")
                ExtraSettingsAPI_ResetSplitSettings();
        }

        void ExtraSettingsAPI_ButtonPress(string SettingName, int Index)
        {
            if (SettingName == "clearTrash")
            {
                if (Index == 1 && !Raft_Network.IsHost)
                {
                    Debug.LogError($"[{modlistEntry.jsonmodinfo.name}]: [Clear and Spawn Trash Button] Only the host can spawn trash");
                    return;
                }
                foreach (var objectSpawner in FindObjectsOfType<ObjectSpawner_RaftDirection>())
                    for (int i = objectSpawner.spawnedObjects.Count - 1; i >= 0; i--)
                        PickupObjectManager.RemovePickupItemNetwork(objectSpawner.spawnedObjects[i]);
                if (Index == 1)
                    ComponentManager<ObjectSpawnerManager>.Value.PrewarmStart();
            }
            else if (SettingName == "learning")
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
            else if (SettingName == "weatherType")
                ComponentManager<WeatherManager>.Value.SetWeather((UniqueWeatherType)Index,true);
            else if (SettingName == "specReset")
            {
                if (Index == 0)
                    ExtraSettingsAPI_ResetSplitSetting("animalSpecs");
                else
                    ExtraSettingsAPI_ResetSetting("animalSpecs");

            }
        }
        public bool ExtraSettingsAPI_HandleSettingVisible(string SettingName, bool IsInWorld)
        {
            if (SettingName == "unlearning")
                return ComponentManager<Raft_Network>.Value && ComponentManager<Raft_Network>.Value.remoteUsers.All(x => x.Value.IsLocalPlayer);
            return !IsInWorld || Raft_Network.IsHost;
        }

        public override void WorldEvent_OnPlayerConnected(CSteamID steamid, RGD_Settings_Character characterSettings) => ExtraSettingsAPI_CheckSettingVisibility();
        public override void WorldEvent_OnPlayerDisconnected(CSteamID steamid, DisconnectReason disconnectReason) => ExtraSettingsAPI_CheckSettingVisibility();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ExtraSettingsAPI_CheckSettingVisibility() { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ExtraSettingsAPI_ResetAllSettings() { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ExtraSettingsAPI_ResetSplitSettings() { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ExtraSettingsAPI_ResetSetting(string SettingName) { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ExtraSettingsAPI_ResetSplitSetting(string SettingName) { }

        const int sizePerDirection = 9 * 2;
        const int totalSize = sizePerDirection * 4;
        static StringBuilder builder = new StringBuilder();
        public string ExtraSettingsAPI_HandleSliderText(string name, float value)
        {
            if (name == "windDirection")
            {
                var str = builder;
                str.Clear();
                var b = totalSize.Multiply(windDirection / 360) - 8;
                for (int i = 0; i <= 16; i++)
                {
                    var ind = i + b % totalSize;
                    if (ind < 0)
                        ind += totalSize;
                    if (i == 5)
                        str.Append("<color=#FFFFFF>");
                    else if (i == 8)
                        str.Append("<color=#00FF00>");
                    else if (i < 5)
                    {
                        var l = (i * 20 + 155).ToString("X2");
                        str.Append("<color=#");
                        for (int j = 0; j < 3; j++)
                            str.Append(l);
                        str.Append('>');
                    }

                    if ((ind & 1) == 1)
                        str.Append('-');
                    else if (ind % sizePerDirection == 0)
                        str.Append("NESW", ind / sizePerDirection % 4, 1);
                    else
                        str.Append('|');

                    if (i == 8 || i >= 11)
                        str.Append("</color>");
                }
                return str.ToString();
            }
            if (name == "trashLagFix")
                return $"Every {Math.Ceiling(value* 100) / 100:0.00}s";
            return $"<color=#{ColorConvert.FromHSL(value, 1, 1).GetHex()}>█████</color>";
        }

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

        static Camera lastCam;
        static float prevFarClip;
        static int lastQuality = int.MinValue;
        static float prevLOD;
        static bool wasBino = false;
        public void Update()
        {
            if (wasBino != UsingBino)
            {
                wasBino = !wasBino;
                if (lastQuality != int.MinValue)
                    QualitySettings.lodBias = prevLOD * (wasBino ? binoLODBias : LODBias);
                if (lastCam)
                    lastCam.farClipPlane = prevFarClip * (wasBino ? binoFarClip : farClip);
            }
            if (lastQuality != QualitySettings.GetQualityLevel())
            {
                lastQuality = QualitySettings.GetQualityLevel();
                prevLOD = QualitySettings.lodBias;
                QualitySettings.lodBias *= wasBino ? binoLODBias : LODBias;
            }
            if (Camera.main != lastCam)
            {
                if (lastCam)
                    lastCam.farClipPlane = prevFarClip;
                if (Camera.main)
                {
                    prevFarClip = (lastCam = Camera.main).farClipPlane;
                    lastCam.farClipPlane = prevFarClip * (wasBino ? binoFarClip : farClip);
                }
                else
                    lastCam = null;
            }
            if (Raft_Network.IsHost)
            {
                var ne = ComponentManager<Network_Host_Entities>.Value;
                if (ne)
                {
                    var j = ne.SharkCount;
                    while (j++ + Patch_DelayedSharkSpawn.pendingCount < sharkCount)
                        ne.StartCoroutine(ne.CreateShark(sharkExtraRespawn, ne.GetSharkSpawnPosition(), AI_NetworkBehaviourType.Shark));
                }
            }
            else
                Patch_DelayedSharkSpawn.pendingCount = 0;

            var spawner = ComponentManager<ObjectSpawnerManager>.Value;
            if (spawner)
            {
                if (trashLagMode == TrashLagMode.Vanilla)
                    TrashFloater.Get(spawner).DoNotUpdate();
                else
                    TrashFloater.Get(spawner).DoUpdate();
            }
        }

        static Dictionary<ChunkPointType, (Sprite sprite, float color)> chunkBlips = new Dictionary<ChunkPointType, (Sprite, float)>();
        public static Sprite GetBlip(ChunkPointType type, Sprite original,float hue)
        {
            if (!chunkBlips.TryGetValue(type, out var tup) || !tup.sprite)
                chunkBlips[type] = tup = (original.GetReadable(), -1);
            if (tup.color != hue)
            {
                var p = tup.sprite.texture.GetPixels(0);
                for (int i = 0; i < p.Length; i++)
                {
                    var c = p[i];
                    c.ToHSL(out _, out var ps, out var pl);
                    var nc = ColorConvert.FromHSL(hue, ps, pl);
                    nc.a = c.a;
                    p[i] = nc;
                        }
                tup.sprite.texture.SetPixels(p, 0);
                tup.sprite.texture.Apply(true);
                chunkBlips[type] = tup = (tup.sprite, hue);
            }
            return tup.sprite;
        }
    }
    public enum TrashMode
    {
        Vanilla,
        Distance,
        Area
    }

    public class OnModUnload : OnEventAttribute { }

    public class OnModLoad : OnEventAttribute { }

    public class OnEventAttribute : Attribute
    {
        public static void CallAll<T>() where T : OnEventAttribute
        {
            void DoType(Type type)
            {
                if (!type.ContainsGenericParameters)
                    foreach (var m in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        if (!m.ContainsGenericParameters && m.GetParameters().Length == 0)
                        {
                            foreach (var a in m.GetCustomAttributes(false))
                                if (a is T)
                                {
                                    try
                                    {
                                        m.Invoke(null, Array.Empty<object>());
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogError(e);
                                    }
                                    break;
                                }
                        }
                foreach (var t in type.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    DoType(t);
            }
            foreach (var t in typeof(Main).Assembly.GetTypes())
                DoType(t);
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
            code.Insert(
                code.FindIndex(
                    code.FindLastIndex(x =>
                        x.opcode == OpCodes.Ldfld
                        && x.operand is FieldInfo f
                        && f.DeclaringType == typeof(CostMultiple)
                        && f.Name == nameof(CostMultiple.amount))
                    , x => x.opcode == OpCodes.Ldc_R4
                ) + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ReturnItemsFromBlock), nameof(ModifyReturnPercent)))
            );
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
        static float ModifyReturnPercent(float original) => Main.instance.returnPercent;
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
    public enum OxygenRegenMode
    {
        Vanilla,
        BetterVanilla,
        Linear
    }
    public enum TrashLagMode
    {
        Vanilla,
        Interval,
        NoMovement
    }
    public enum KeepInventoryMode
    {
        Default,
        KeepInventory,
        DropItems
    }
    public class AnimalSpecs : IDictionary<AI_NetworkBehaviourType,AnimalSpec>, IDictionary<string,string>, IDictionary<string, AnimalSpec>, IEnumerable<KeyValuePair<string, AnimalSpec>>
    {
        Dictionary<string, AnimalSpec> _internal = new Dictionary<string, AnimalSpec>();
        static string GetKey(AI_NetworkBehaviourType type) => type.ToString();

        public AnimalSpec GetOrCreate(AI_NetworkBehaviourType type) => GetOrCreate(GetKey(type));
        public AnimalSpec GetOrCreate(string key)
        {
            if (!_internal.TryGetValue(key, out var value))
                _internal[key] = value = new AnimalSpec();
            return value;
        }
        public AnimalSpec GetOrDefault(AI_NetworkBehaviourType type) => GetOrDefault(GetKey(type));
        public AnimalSpec GetOrDefault(string key)
        {
            if (!_internal.TryGetValue(key, out var value))
                value = AnimalSpec.Default;
            return value;
        }
        public bool ContainsKey(AI_NetworkBehaviourType type) => _internal.ContainsKey(GetKey(type));
        public bool ContainsKey(string key) => _internal.ContainsKey(key);
        public void Add(AI_NetworkBehaviourType type, AnimalSpec value) => _internal.Add(GetKey(type), value);
        public void Add(string key, AnimalSpec value) => _internal.Add(key, value);
        public void Add(string key, string value) => _internal.Add(key, new AnimalSpec(value));
        public bool Remove(AI_NetworkBehaviourType type) => _internal.Remove(GetKey(type));
        public bool Remove(string key) => _internal.Remove(key);
        public bool TryGetValue(AI_NetworkBehaviourType type, out AnimalSpec value) => _internal.TryGetValue(GetKey(type),out value);
        public bool TryGetValue(string key, out AnimalSpec value) => _internal.TryGetValue(key, out value);
        bool IDictionary<string, string>.TryGetValue(string key, out string value)
        {
            if (_internal.TryGetValue(key,out var spec))
            {
                value = spec.ToString();
                return true;
            }
            value = null;
            return false;
        }
        public AnimalSpec this[AI_NetworkBehaviourType type]
        {
            get => _internal[GetKey(type)];
            set => _internal[GetKey(type)] = value;
        }
        public AnimalSpec this[string key]
        {
            get => _internal[key];
            set => _internal[key] = value;
        }
        string IDictionary<string,string>.this[string key]
        {
            get => _internal[key].ToString();
            set => _internal[key].Parse(value);
        }
        public ICollection<string> Keys => _internal.Keys;
        ICollection<AI_NetworkBehaviourType> IDictionary<AI_NetworkBehaviourType, AnimalSpec>.Keys => throw new NotImplementedException();
        public ICollection<AnimalSpec> Values => _internal.Values;
        ICollection<string> IDictionary<string, string>.Values => throw new NotImplementedException();
        void ICollection<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>>.Add(KeyValuePair<AI_NetworkBehaviourType, AnimalSpec> value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, AnimalSpec>>.Add(KeyValuePair<string, AnimalSpec> value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> value) => throw new NotImplementedException();
        public void Clear() => _internal.Clear();
        bool ICollection<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>>.Contains(KeyValuePair<AI_NetworkBehaviourType, AnimalSpec> value) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<string, AnimalSpec>>.Contains(KeyValuePair<string, AnimalSpec> value) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>>.CopyTo(KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>[] value, int index) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, AnimalSpec>>.CopyTo(KeyValuePair<string, AnimalSpec>[] value, int index) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] value, int index) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>>.Remove(KeyValuePair<AI_NetworkBehaviourType, AnimalSpec> value) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<string, AnimalSpec>>.Remove(KeyValuePair<string, AnimalSpec> value) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> value) => throw new NotImplementedException();
        public int Count => _internal.Count;
        public bool IsReadOnly => false;
        public IEnumerator<KeyValuePair<string, AnimalSpec>> GetEnumerator() => _internal.GetEnumerator();
        IEnumerator<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>> IEnumerable<KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>>.GetEnumerator()
            => _internal.Select(x => new KeyValuePair<AI_NetworkBehaviourType, AnimalSpec>((AI_NetworkBehaviourType)Enum.Parse(typeof(AI_NetworkBehaviourType), x.Key), x.Value)).GetEnumerator();
        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
            => _internal.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class AnimalSpec
    {
        public bool IsPassive;
        public bool IsDisabled;
        public static AnimalSpec Default = new AnimalSpec();
        public AnimalSpec() { }
        public AnimalSpec(string value) => Parse(value);
        public void Parse(string value)
        {
            IsDisabled = value?.Length > 0 && value[0] == '1';
            IsPassive = value?.Length > 1 && value[1] == '1';
        }
        public override string ToString() => $"{(IsDisabled ? '1' : '0')}{(IsPassive ? '1' : '0')}";
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
            if (Main.instance.zoomKey != null && CanvasHelper.ActiveMenu == MenuType.None && MyInput.GetButton(Main.instance.zoomKey))
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
            if (Main.instance.keepInventory == KeepInventoryMode.KeepInventory)
                clearInventory = false;
        }

        [HarmonyPatch(typeof(Player), "RespawnWithoutBed")]
        [HarmonyPrefix]
        static void RespawnWithoutBed(ref bool clearInventory)
        {
            if (Main.instance.keepInventory == KeepInventoryMode.KeepInventory)
                clearInventory = false;
        }
    }

    [HarmonyPatch(typeof(Reciever), "CheckSignal")]
    static class Patch_AntennaValid
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "toCloseToReciever"),
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(Patch_AntennaValid),nameof(RecieverDistanceOK)))
            );
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "toFarAwayFromReciever"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AntennaValid), nameof(RecieverDistanceOK)))
            );
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "onSameAltitudeAsReciever"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AntennaValid), nameof(RecieverHeightOK)))
            );
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "toCloseToOtherAntenna"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AntennaValid), nameof(AntennaDistanceOK)))
            );
            return code;
        }

        static bool RecieverDistanceOK(bool original) => original && !Main.instance.antennaAnyDistReciever;
        static bool RecieverHeightOK(bool original) => original || Main.instance.antennaAnywhere;
        static bool AntennaDistanceOK(bool original) => original && !Main.instance.antennaAnyDistAntenna;

        [OnModUnload]
        static void OnUnload()
        {
            foreach (var r in Object.FindObjectsOfType<Reciever>())
                r.CheckSignal();
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
                __result = __result.Multiply(
                        __instance.UniqueName.StartsWith("Battery") ? Main.instance.batteryMax
                        : __instance.settings_consumeable?.FoodType > FoodType.None ? Main.instance.foodMax
                        : __instance.settings_equipment?.EquipType > EquipSlotType.None ? Main.instance.equipmentMax
                        : Main.instance.toolMax
                    , 1, short.MaxValue);
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

        public static bool OverrideLockHook(bool original, Hook hook) => original || (Main.instance.forceMining && hook.throwable.InHand && MyInput.GetButton("RMB"));
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

        static float OverrideTimeSpeed(float original) => original == 0 || float.IsNaN( Main.instance.timeSpeed) ? original : Main.instance.timeSpeed;
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
            code.Insert(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AxeUpdate), nameof(ModifyTime))));
            return code;
        }
        static void MaybeDamage(Axe instance)
        {
            if (Main.instance.axeDur == AxeDurabilityMode.OnBreak)
                instance.GetComponentInParent<Network_Player>().Inventory.RemoveDurabillityFromHotSlot();
        }
        static float ModifyTime(float original) => original.SafeDivide(Main.instance.breakSpeed);
    }

    [HarmonyPatch(typeof(RemovePlaceables), "Update")]
    static class Patch_RemovePlaceablesUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_RemovePlaceablesUpdate), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original.SafeDivide(Main.instance.removeSpeed);
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

    [HarmonyPatch(typeof(Seagull), "LeaveNest")]
    static class Patch_LeaveNest
    {
        static void Prefix(ref bool ___dropEggInNest)
        {
            ___dropEggInNest = Main.instance.seagullEggs;
        }
    }

    public enum RecycleMode
    {
        Random,
        First,
        Last,
        FirstRandom,
        LastRandom
    }
    public enum RecycleButton
    {
        Disabled,
        Horizontal,
        Vertical
    }

    [HarmonyPatch(typeof(SelectedRecipeBox), "Initialize")]
    static class Patch_UncraftButton
    {
        public static void RemoveButton(UncraftButtonUpdater button)
        {
            if (!button)
                return;
            var r = button.recipeBox.GetComponent<RectTransform>();
            if (button.vertical)
            {
                var f = r.FindChildRecursively("ItemCostList").GetComponent<RectTransform>();
                var i = r.FindChildRecursively("requires text").GetComponent<RectTransform>();
                var s = button.GetComponent<RectTransform>().rect.height * 1.1f;
                r.offsetMin += new Vector2(0, 2);
                if (i)
                    i.sizeDelta -= new Vector2(0, s);
                if (f)
                    f.localPosition += new Vector3(0, s, 0);
            }
            else
            {
                var d = r.FindChildRecursively("Divider (1)").GetComponent<RectTransform>();
                var f = r.FindChildRecursively("Skin Grid Layout").parent.GetComponent<RectTransform>();
                var i = r.FindChildRecursively("item_label").GetComponent<RectTransform>();
                var s = button.GetComponent<RectTransform>().rect.width * 1.1f;
                r.offsetMax -= new Vector2(s, 0);
                if (d)
                    d.offsetMax -= new Vector2(s, 0);
                if (f)
                    f.sizeDelta -= new Vector2(s, 0);
                if (i)
                    i.offsetMax -= new Vector2(s, 0);
                Traverse.Create(button.recipeBox).Field<float>("startRectHeight").Value -= s;
            }
            Object.Destroy(button.gameObject);
        }
        [HarmonyPrefix]
        public static void TryAddButton(SelectedRecipeBox __instance)
        {
            //Main.LogTree(__instance.transform);
            var vert = Main.instance.recycleButton == RecycleButton.Vertical;
            var button = UncraftButtonUpdater.all.Find(x => x.recipeBox == __instance);
            if (Main.instance.recycleButton == RecycleButton.Disabled || (button && vert != button.vertical))
                RemoveButton(button);
            if (Main.instance.recycleButton == RecycleButton.Disabled)
                return;
            var r = __instance.GetComponent<RectTransform>();
            var c = r.FindChildRecursively("Craft button").GetComponent<RectTransform>();
            var u = Object.Instantiate(c, c.parent, true);
            u.name = "Uncraft button";
            if (vert)
            {
                var f = r.FindChildRecursively("ItemCostList").GetComponent<RectTransform>();
                var i = r.FindChildRecursively("requires text").GetComponent<RectTransform>();
                var s = u.rect.height * 1.1f;
                u.localPosition -= new Vector3(0, s, 0);
                r.offsetMin -= new Vector2(0, s);
                if (i)
                    i.sizeDelta += new Vector2(0, s);
                if (f)
                    f.localPosition -= new Vector3(0, s, 0);
            }
            else
            {
                var d = r.FindChildRecursively("Divider (1)").GetComponent<RectTransform>();
                var f = r.FindChildRecursively("Skin Grid Layout").parent.GetComponent<RectTransform>();
                var i = r.FindChildRecursively("item_label").GetComponent<RectTransform>();
                var s = u.rect.width * 1.1f;
                u.localPosition += new Vector3(s, 0, 0);
                r.offsetMax += new Vector2(s, 0);
                if (d)
                    d.offsetMax += new Vector2(s, 0);
                if (f)
                    f.sizeDelta += new Vector2(s, 0);
                if (i)
                    i.offsetMax += new Vector2(s, 0);
                Traverse.Create(__instance).Field<float>("startRectHeight").Value -= s;
            }
            Object.DestroyImmediate(u.GetComponentInChildren<I2.Loc.Localize>());
            u.GetComponentInChildren<Text>().text = "Uncraft";
            var UncraftButton = u.GetComponent<Button>();
            UncraftButton.onClick = new Button.ButtonClickedEvent();
            UncraftButton.onClick.AddListener(delegate {
                UncraftButton_OnClick(__instance, ComponentManager<PlayerInventory>.Value);
            });
            button = UncraftButton.gameObject.AddComponent<UncraftButtonUpdater>();
            button.vertical = vert;
            button.button = UncraftButton;
            button.recipeBox = __instance;
            button.inventory = ComponentManager<PlayerInventory>.Value;
        }

        public static void UncraftButton_OnClick(SelectedRecipeBox box, PlayerInventory inventory)
        {
            Patch_Slot_RemoveItem.count = 0;
            Patch_Slot_RemoveItem.countUses = true;
            inventory.RemoveItem(box.ItemToCraft.UniqueName, box.ItemToCraft.settings_recipe.AmountToCraft);
            Patch_Slot_RemoveItem.countUses = false;
            var costs = box.ItemToCraft.settings_recipe.NewCost;
            var result = new List<Cost>();
            foreach (var c in costs)
                if (c.items.Length == 1)
                    result.Add(new Cost(c.items[0], c.amount));
                else
                {
                    var col = new List<Cost>();
                    for (int i = 0; i < c.amount; i++)
                    {
                        var item = PickItem(c.items);
                        var r = col.Find(x => x.item.UniqueIndex == item.UniqueIndex);
                        if (r == null)
                            col.Add(new Cost(item, 1));
                        else
                            r.amount++;
                    }
                    result.AddRange(col);
                }
            if (box.ItemToCraft.MaxUses > 1)
            {
                var leave = (int)(result.Sum(x => x.amount) * ((float)Patch_Slot_RemoveItem.count / (box.ItemToCraft.MaxUses * box.ItemToCraft.settings_recipe.AmountToCraft)));
                for (int i = 0; i < result.Count; i++)
                    if (leave <= 0)
                        result[i].amount = 0;
                    else if (leave >= result[i].amount)
                        leave -= result[i].amount;
                    else
                    {
                        result[i].amount -= leave;
                        leave = 0;
                    }
            }
            foreach (var c in result)
                if (c.amount > 0)
                    inventory.AddItem(c.item.UniqueName, c.amount);
        }

        public static Item_Base PickItem(Item_Base[] items)
        {
            if (items.Length == 1 || Main.instance.recycleMode == RecycleMode.First)
                return items[0];
            if (Main.instance.recycleMode == RecycleMode.Last)
                return items[items.Length - 1];
            if (Main.instance.recycleMode == RecycleMode.Random)
                return items[(int)(Random.value * items.Length)];
            if (Main.instance.recycleMode == RecycleMode.FirstRandom)
                return items[(int)(Math.Pow(Random.value, 2) * items.Length)];
            if (Main.instance.recycleMode == RecycleMode.LastRandom)
                return items[(int)(Math.Sqrt(Random.value) * items.Length)];
            return items[0];
        }
    }

    [HarmonyPatch(typeof(Slot), "RemoveItem")]
    static class Patch_Slot_RemoveItem
    {
        public static bool countUses = false;
        public static int count = 0;
        static void Prefix(Slot __instance, int amount)
        {
            if (!countUses)
                return;
            if (__instance.itemInstance != null && __instance.itemInstance.Amount <= amount)
                count += __instance.itemInstance.UsesInStack;
            else if (__instance.itemInstance != null)
                count += __instance.itemInstance.BaseItemMaxUses * amount;
        }
    }

    public class UncraftButtonUpdater : MonoBehaviour
    {
        public static List<UncraftButtonUpdater> all = new List<UncraftButtonUpdater>();
        public Button button;
        public PlayerInventory inventory;
        public SelectedRecipeBox recipeBox;
        public bool vertical;
        public UncraftButtonUpdater()
        {
            all.Add(this);
        }
        void OnDestroy()
        {
            all.Remove(this);
        }
        void Update()
        {
            button.interactable = recipeBox.ItemToCraft && inventory.GetItemCount(recipeBox.ItemToCraft) >= recipeBox.ItemToCraft.settings_recipe.AmountToCraft;
        }

        [OnModUnload]
        static void OnUnload()
        {
            foreach (var b in all.ToArray())
                if (b)
                    Patch_UncraftButton.RemoveButton(b);
        }
    }

    [HarmonyPatch(typeof(Battery), "Update", typeof(int))]
    static class Patch_Battery_UpdateUses
    {
        static bool Prefix(Battery __instance, int batteryUses) => !Main.instance.infinitePower || !Raft_Network.IsHost || __instance.BatteryUses < batteryUses;
    }

    [HarmonyPatch(typeof(WaterWavesSpectrum), MethodType.Constructor, new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) })]
    static class Patch_WaveCalculatorCreate
    {
        static List<WeakReference<WaterWavesSpectrum>> spectrums = new List<WeakReference<WaterWavesSpectrum>>();
        static ConditionalWeakTable<WaterWavesSpectrum, memory> original = new ConditionalWeakTable<WaterWavesSpectrum, memory>();
        static void Prefix(WaterWavesSpectrum __instance, ref float amplitude, ref float windSpeed)
        {
            spectrums.Add(new WeakReference<WaterWavesSpectrum>(__instance));
            var mem = original.GetOrCreateValue(__instance);
            mem.amplitude = amplitude;
            mem.windSpeed = windSpeed;
            amplitude = EditValue(amplitude, Main.instance.amplitudeMul, Main.instance.amplitudeExp);
            windSpeed = EditValue(windSpeed, Main.instance.windSpeedMul, Main.instance.windSpeedExp);
        }

        [OnModLoad]
        public static void InitializeSpectrums()
        {
            foreach (var p in Resources.FindObjectsOfTypeAll<WaterProfile>())
            {
                spectrums.Add(new WeakReference<WaterWavesSpectrum>(p.Data.Spectrum));
                var mem = original.GetOrCreateValue(p.Data.Spectrum);
                mem.amplitude = p.Data.Spectrum.GetAmplitude();
                mem.windSpeed = p.Data.Spectrum.GetWindSpeed();
            }
        }

        public static void RemodifySpectrums()
        {
            for (int i = spectrums.Count - 1; i >= 0; i--)
                if (spectrums[i].TryGetTarget(out var spectrum))
                {
                    var mem = original.GetOrCreateValue(spectrum);
                    spectrum.SetAmplitude(EditValue(mem.amplitude, Main.instance.amplitudeMul, Main.instance.amplitudeExp));
                    spectrum.SetWindSpeed(EditValue(mem.windSpeed, Main.instance.windSpeedMul, Main.instance.windSpeedExp));
                }
                else
                    spectrums.RemoveAt(i);
            MarkDirty();
        }

        [OnModUnload]
        public static void UnmodifySpectrums()
        {
            foreach (var r in spectrums)
                if (r.TryGetTarget(out var spectrum) && original.TryGetValue(spectrum,out var mem))
                {
                    spectrum.SetAmplitude(mem.amplitude);
                    spectrum.SetWindSpeed(mem.windSpeed);
                }
            spectrums.Clear();
            MarkDirty();
        }

        static void MarkDirty()
        {
            foreach (var w in Resources.FindObjectsOfTypeAll<Water>())
            {
                foreach (var p in w.ProfilesManager.Profiles)
                    p.Profile.Dirty = true;
                w.WindWaves.SpectrumResolver.GetCachedSpectraDirect().Clear();
            }
        }

        static float EditValue(float val, float mul, float exp) => (float)Math.Pow(val, exp) * mul;

        class memory
        {
            public float amplitude;
            public float windSpeed;
        }
    }

    [HarmonyPatch]
    static class Patch_EquipEquippable
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Inventory), "MoveItem");
            yield return AccessTools.Method(typeof(Inventory), "SwitchSlots");
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetEquipSlotWithTag") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_EquipEquippable), nameof(ReplaceSlot)))
            );
            return code;
        }
        static Slot_Equip ReplaceSlot(Slot_Equip slot)
        {
            if (Main.instance.allowMultiEquipment)
                return null;
            return slot;
        }
    }

    [HarmonyPatch(typeof(PlayerInventory), "ClearInventoryLeaveSome")]
    static class Patch_LoseSomeItems
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "FloorToInt") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_LoseSomeItems), nameof(ModifyRemainingAmount)))
                }
            );
            code.InsertRange(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "FloorToInt") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_LoseSomeItems), nameof(ModifyRemainingUses)))
                }
            );
            return code;
        }
        static int ModifyRemainingAmount(int original,ItemInstance item)
        {
            if (Main.instance.deathItemLoss <= 0)
                return original;
            return (int)(item.Amount * Main.instance.deathItemLoss);
        }
        static int ModifyRemainingUses(int original, ItemInstance item)
        {
            if (Main.instance.deathUseLoss <= 0)
                return original;
            return (int)(item.BaseItemMaxUses * Main.instance.deathUseLoss);
        }
    }

    [HarmonyPatch(typeof(ThrowableComponent), "HandleCollision")]
    static class Patch_PickupOnHit
    {
        static void Postfix(ThrowableComponent __instance, Throwable_Object throwable)
        {
            if (Main.instance.returnArrows && __instance.playerNetwork && __instance.playerNetwork.IsLocalPlayer && throwable && throwable.pickupItem && throwable.pickupItem.CanBePickedUp())
                __instance.playerNetwork.PickupScript.PickupItem(throwable.pickupItem.PickupItem);
        }
    }

    [HarmonyPatch(typeof(Slot), "IncrementUses")]
    static class Patch_SpendItemUses
    {
        static bool Prefix(int amountOfUsesToAdd) => !Main.instance.infiniteDurability || amountOfUsesToAdd > 0;
    }

    [HarmonyPatch(typeof(Stat_Consumable), "LostPerSecond", MethodType.Getter)]
    static class Patch_StatLostPerSecond
    {
        static void Postfix(Stat_Consumable __instance, ref float __result, float ___lostPerSecondDefault)
        {
            var s = __instance.GetComponentInParent<PlayerStats>();
            var flag = s ? s.stat_hunger.normalConsumable == __instance || s.stat_hunger.bonusConsumable == __instance : false;
            if (flag && Main.instance.hungerRateFix)
            {
                var nv = GameModeValueManager.GetCurrentGameModeValue().nourishmentVariables;
                if (nv.thirstDecrementRateMultiplier != 0)
                    __result *= nv.foodDecrementRateMultiplier / nv.thirstDecrementRateMultiplier;
                else
                    __result = ___lostPerSecondDefault * nv.foodDecrementRateMultiplier;
            }
            __result *= flag ? Main.instance.hungerRate : Main.instance.thirstRate;
        }
    }

    [HarmonyPatch]
    static class Patch_ValidTarget
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(AI_StateMachine), "Update");
            yield return AccessTools.Method(typeof(AI_Component), "Update");
        }
        static void Prefix(object __instance, ref List<Player> __state)
        {
            __state = new List<Player>();
            var passive =
                Main.instance.invisible
                || (
                    (__instance is AI_StateMachine machine ? machine : ((AI_Component)__instance).connectedState.stateMachine) is AI_StateMachine_Animal animal
                    && Main.instance.animalSpecs.GetOrCreate(animal.networkBehaviour.behaviourType).IsPassive
                );
            foreach (Network_Player player in ComponentManager<Raft_Network>.Value.remoteUsers.Values)
                if (!player.PlayerScript.IsDead && (passive || (Main.instance.invisibleHost && player.IsLocalPlayer)))
                {
                    player.PlayerScript.IsDead = true;
                    __state.Add(player.PlayerScript);
                    break;
                }
        }
        static void Finalizer(List<Player> __state)
        {
            foreach (Player player in __state)
                player.IsDead = false;
        }
    }

    [HarmonyPatch(typeof(AI_NetworkBehaviour), "Update")]
    static class Patch_NetworkBehaviourUpdate
    {
        static ConditionalWeakTable<AI_NetworkBehaviour, object> req = new ConditionalWeakTable<AI_NetworkBehaviour, object>();
        static void Prefix(AI_NetworkBehaviour __instance)
        {
            if (__instance is AI_NetworkBehaviour_Animal && !req.TryGetValue(__instance, out _) && Main.instance.animalSpecs.GetOrCreate(__instance.behaviourType).IsDisabled)
            {
                req.GetOrCreateValue(__instance);
                Debug.Log("Send remove signal to " + __instance);
                NetworkIDManager.SendIDBehaviourDead(__instance.ObjectIndex, typeof(AI_NetworkBehaviour), true);
            }
        }
    }

    [HarmonyPatch(typeof(Network_Entity),"Damage")]
    static class Patch_DamageEntity
    {
        static void Prefix(Network_Entity __instance, ref float damage)
        {
            if (Main.instance.invincible && __instance is PlayerStats && __instance.GetComponent<Network_Player>().IsLocalPlayer)
                damage = 0;
        }
    }

    [HarmonyPatch(typeof(Landmark), "GenerateTreasures")]
    static class Patch_GenerateTreasures
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetValidPoints"),
                new[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_GenerateTreasures), nameof(ClearCache)))
                }
            );
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetRandomItem") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_GenerateTreasures), nameof(ModifyTreasure)))
                }
            );
            return code;
        }
        static List<TreasurePoint> tikis;
        static void ClearCache() => tikis = null;
        static TreasurePoint ModifyTreasure(TreasurePoint original, Landmark land)
        {
            if (!Main.instance.tikiFix)
                return original;
            var current = GetTiki(original);
            if (!current)
                return original;
            if (tikis == null)
            {
                var collect = new Dictionary<string, TreasurePoint>();
                foreach (var option in land.treasureLootSetting.lootTable.GetAllItems<TreasurePoint>())
                {
                    var optionI = GetTiki(option);
                    if (optionI)
                        collect[optionI.UniqueName] = option;
                }
                if (collect.Count == 0)
                    tikis = new List<TreasurePoint>();
                else
                {
                    var count = collect.ToDictionary(x => x.Value, x => 0);
                    foreach (var inv in StorageManager.allStorages.Select(x => x.GetInventoryReference()).Concat(new[] { RAPI.GetLocalPlayer().Inventory }))
                    {
                        foreach (var slot in inv.allSlots)
                            if (slot.HasValidItemInstance() && collect.TryGetValue(slot.itemInstance.UniqueName, out var point))
                                count[point] += slot.itemInstance.Amount;
                    }
                    if (collect.Count > 0)
                        foreach (var b in BlockCreator.GetPlacedBlocks())
                            if (b.buildableItem && collect.TryGetValue(b.buildableItem.UniqueName, out var point))
                                count[point]++;
                    tikis = new List<TreasurePoint>();
                    var min = int.MaxValue;
                    foreach (var p in count)
                    {
                        if (min == p.Value)
                            tikis.Add(p.Key);
                        else if (min > p.Value)
                        {
                            tikis.Clear();
                            tikis.Add(p.Key);
                        }
                    }
                }
            }
            if (tikis.Count == 0)
                return original;
            return tikis.GetRandom();
        }
        static Item_Base GetTiki(TreasurePoint point) => point.Safe()?.pickupNetworked.Safe()?.PickupItem.Safe()?.yieldHandler.Safe()?.yieldAsset.Safe()?.yieldAssets?.FirstOrDefault(IsTiki)?.item;
        public static bool IsTiki(Item_Base item) => item && item.UniqueName.Contains("Placeable_Tiki");
        public static bool IsTiki(Cost item) => IsTiki(item.item);
    }

    [HarmonyPatch(typeof(Hotbar),"HandleHotbarSelection")]
    static class Patch_HotbarDirection
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetAxis") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HotbarDirection), nameof(ModifyScroll)))
            );
            return code;
        }
        static float ModifyScroll(float scrollAmount) => Main.instance.reverseScroll ? -scrollAmount : scrollAmount;
    }

    [HarmonyPatch(typeof(Pickup), "Update")]
    static class Patch_Pickup
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindLastIndex(x => x.opcode == OpCodes.Stloc_1),
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(Pickup),"pickupItem")),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_Pickup), nameof(ModifyCanPickup)))
                }
            );
            return code;
        }
        static bool ModifyCanPickup(bool original, Pickup pickup, RaycastInteractable interactable, PickupItem item)
        {
            if (!original)
            {
                if (item && Main.instance.handMining && interactable.CompareTag("PickupHook"))
                {
                    if (MyInput.GetButtonDown("Interact"))
                        pickup.PickupItemByType(item, true);
                    return true;
                }
                if (Main.instance.fruitTime >= 0 && interactable.CompareTag("Tree"))
                {
                    var data = TreeData.Get(interactable.GetComponentInParent<HarvestableTree>()?.PickupItem);
                    if (data.items.Count > 0)
                    {
                        var time = data.lastCollect + Main.instance.fruitTime - Time.time;
                        Traverse.Create(pickup).Field("showingText").SetValue(true);
                        if (time < 0)
                        {
                            ComponentManager<CanvasHelper>.Value.displayTextManager.ShowText(Helper.GetTerm("Game/Harvest", false), MyInput.Keybinds["Interact"].MainKey, 0, 1, true);
                            if (MyInput.GetButtonDown("Interact"))
                            {
                                data.lastCollect = Time.time;
                                var player = RAPI.GetLocalPlayer();
                                player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem, false);
                                var ritem = data.items.GetRandom();
                                player.Inventory.AddItem(new ItemInstance(ritem, 1, ritem.MaxUses));
                                ComponentManager<CanvasHelper>.Value.displayTextManager.HideDisplayTexts();
                            }
                        }
                        else
                            ComponentManager<CanvasHelper>.Value.displayTextManager.ShowText("Next harvest: " + GetTimer((int)time), MyInput.Keybinds["Interact"].MainKey, 0, 1, true);
                    }
                }
            }
            return original;
        }

        static string GetTimer(int seconds) => seconds < 3600 ? $"{seconds / 60:00}:{seconds % 60:00}" : $"{seconds / 3600:00}:{seconds / 60 % 60:00}:{seconds % 60:00}";

        public class TreeData
        {
            public List<Item_Base> items = new List<Item_Base>();
            public float lastCollect;
            TreeData(PickupItem item)
            {
                lastCollect = Time.time;
                if (item.yieldHandler && item.yieldHandler.yieldAsset && item.yieldHandler.yieldAsset.yieldAssets != null)
                    foreach (var c in item.yieldHandler.yieldAsset.yieldAssets)
                        if (c?.amount > 0)
                            MaybeAddItem(c.item);
                if (item.dropper)
                {
                    var asset = Traverse.Create(item.dropper).Field("randomDropperAsset").GetValue<SO_RandomDropper>();
                    if (asset)
                        foreach (var i in asset.randomizer.GetAllItems<Item_Base>())
                            MaybeAddItem(i);
                }
                if (item.itemInstance != null && item.itemInstance.Valid)
                    MaybeAddItem(item.itemInstance.baseItem);
            }
            void MaybeAddItem(Item_Base item)
            {
                if (item && item.UniqueName != "Plank" && item.UniqueName != "Thatch" && items.All(x => x.UniqueName != item.UniqueName))
                    items.Add(item);
            }

            static ConditionalWeakTable<PickupItem, TreeData> table = new ConditionalWeakTable<PickupItem, TreeData>();
            public static TreeData Get(PickupItem item)
            {
                if (!table.TryGetValue(item, out var data))
                    table.Add(item, data = new TreeData(item));
                return data;
            }
        }
    }

    [HarmonyPatch(typeof(PickupItem),"Start")]
    static class Patch_NewPickupItem
    {
        static void Prefix(PickupItem __instance) => Patch_Pickup.TreeData.Get(__instance);

        static void OnLoad()
        {
            foreach (var t in Resources.FindObjectsOfTypeAll<HarvestableTree>())
                if (t && t.PickupItem)
                    Prefix(t.PickupItem);
        }
    }

    [HarmonyPatch]
    static class Patch_LookYMinMax
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(MouseLook), "SetTargetRotYToCurrentRotation");
            yield return AccessTools.Method(typeof(MouseLook), "Update");
            yield return AccessTools.Method(typeof(PersonController), "GroundControll");
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1; i >= 0; i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f)
                {
                    if (f.Name == "minimumY")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_LookYMinMax), nameof(EditMin))));
                    else if (f.Name == "maximumY")
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_LookYMinMax), nameof(EditMax))));
                }
            return code;
        }
        static float EditMin(float original) => Main.instance.fullLook ? -90 : original;
        static float EditMax(float original) => Main.instance.fullLook ? 90 : original;
    }

    [HarmonyPatch(typeof(AzureSkyController))]
    static class Patch_AzureSkyController
    {
        [HarmonyPatch("Update")]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "fogDistance") + 1,
                new[]
                {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(Patch_AzureSkyController),nameof(Postfix)))
                });
            return code;
        }
        [HarmonyPatch("BlendWeatherProfiles")]
        static void Postfix(AzureSkyController __instance)
        {
            if (Main.instance.FinalFogDistance < 0)
                __instance.fogDistance = -1f;
            else
                __instance.fogDistance *= Main.instance.FinalFogDistance;
        }
    }

    [HarmonyPatch(typeof(PlayerStats),"Update")]
    static class Patch_PlayerStatsUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "healthLostPerSecondDrowning") + 1,
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(Patch_PlayerStatsUpdate),nameof(ModifyDrowningDamage))));
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "healthLostPerSecondZeroed") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_PlayerStatsUpdate), nameof(ModifyStarvedDamage))));
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "healthLostPerSecondWellbeing") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_PlayerStatsUpdate), nameof(ModifyBadHealthDamage))));
            return code;
        }
        static float ModifyDrowningDamage(float original) => Main.instance.invincible ? 0 : original;
        static float ModifyStarvedDamage(float original) => Main.instance.invincible ? 0 : original;
        static float ModifyBadHealthDamage(float original) => Main.instance.invincible ? 0 : original;
    }

    [HarmonyPatch(typeof(Stat_Health), "Regenerate")]
    static class Patch_HealthRegen
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HealthRegen), nameof(ModifyRegenAmount)))
                });
            return code;
        }
        static float ModifyRegenAmount(float original,Stat_Health health) => original * Main.instance.healthRegen;
    }

    [HarmonyPatch]
    static class Patch_WellBeingThreashold
    {
        [HarmonyPatch(typeof(Stat_WellBeing),"Update")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> WellBeingUpdate(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldsfld && x.operand is FieldInfo f && f.Name == "WellBeingLimit") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyBadLimit))));
            code.Insert(
                code.FindLastIndex(x => x.opcode.FlowControl == FlowControl.Cond_Branch),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyGoodLimit))));
            return code;
        }
        [HarmonyPatch(typeof(Stat), "get_MeetsWellBeing")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> MeetsWellBeing(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldsfld && x.operand is FieldInfo f && f.Name == "WellBeingLimit") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyBadLimit))));
            return code;
        }

        public static class Methods
        {
            public static float ModifyBadLimit(float original) => float.IsNaN(Main.instance.unhealthyOverride) ? original : Main.instance.unhealthyOverride;
            public static float ModifyGoodLimit(float original) => float.IsNaN(Main.instance.healthyOverride) ? original : Main.instance.healthyOverride;
        }
    }

    [HarmonyPatch(typeof(Stat_Oxygen), "Update")]
    static class Patch_OxygenUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator iL)
        {
            var code = instructions.ToList();
            var loc = iL.DeclareLocal(typeof(float));
            var ind = code.FindIndex(x => x.operand is MethodInfo m && m.Name == "Lerp");
            code.InsertRange(
                ind + 1,
                new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc,loc),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_OxygenUpdate), nameof(ModifyRegenAmount)))
                });
            code.InsertRange(ind,
                new[]
                {
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Stloc,loc)
                });
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_OxygenUpdate), nameof(ModifyRegenSpeed))));
            code.Insert(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_OxygenUpdate), nameof(ModifyLossSpeed))));
            return code;
        }
        static float ModifyRegenAmount(float original, Stat_Oxygen stat, float delta)
        {
            if (Main.instance.oxygenMode == OxygenRegenMode.Vanilla)
                return original;
            if (Main.instance.oxygenMode == OxygenRegenMode.Linear)
                return stat.Value + stat.Max * delta / 4.5f;
            return (float)Math.Pow(Math.Pow(stat.Value / stat.Max, warp) + delta / 4.5f, warp2) * stat.Max;
        }
        const double warp = 2;
        const double warp2 = 1 / warp;
        static float ModifyRegenSpeed(float original) => original * Main.instance.oxyGenSpeed;
        static float ModifyLossSpeed(float original) => original * Main.instance.oxyLossSpeed;
    }

    [HarmonyPatch(typeof(Network_Host_Entities), "CreateShark")]
    static class Patch_DelayedSharkSpawn
    {
        public static int pendingCount;
        static void Postfix(ref IEnumerator __result)
        {
            __result = new Wrapper(__result);
        }

        class Wrapper : IEnumerator
        {
            IEnumerator Enumerator;
            bool done = false;
            public Wrapper(IEnumerator enumerator)
            {
                Enumerator = enumerator;
                pendingCount++;
            }
            public bool MoveNext()
            {
                if (Enumerator.MoveNext())
                    return true;
                if (!done)
                {
                    pendingCount--;
                    done = true;
                }
                return false;
            }
            public void Reset()
            {
                done = false;
                Enumerator.Reset();
            }
            public object Current => Enumerator.Current;
        }
    }

    [HarmonyPatch(typeof(AI_State_Decay_Shark), "CheckToSpawnOnDecay")]
    static class Patch_CheckSharkDecaySpawn
    {
        static void Prefix(AI_State_Decay_Shark __instance)
        {
            var entities = ComponentManager<Network_Host_Entities>.Value;
            if (entities.SharkCount > 1 && entities.SharkCount <= Main.instance.sharkCount + 1)
                entities.StartCoroutine(entities.CreateShark(__instance.decayRespawnTime, entities.GetSharkSpawnPosition(), AI_NetworkBehaviourType.Shark));
        }
    }

    [HarmonyPatch(typeof(Network_Host_Entities), "CreateSeagull")]
    static class Patch_TrySeagullSpawn
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is FieldInfo f && f.Name == "seagullMaxCount") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_TrySeagullSpawn), nameof(ModifySeagullMax))));
            return code;
        }
        static int ModifySeagullMax(int original) => Main.instance.seagullCount == -1 ? original : Main.instance.seagullCount;
    }

    [HarmonyPatch(typeof(Tank), "SetTankAmount")]
    static class Patch_SetTankAmount
    {
        static void Prefix(Tank __instance, ref float newAmount, Tank.TankAcceptance ___tankAcceptance)
        {
            if (Main.instance.fuelConsumption == 1)
                return;
            if (___tankAcceptance == Tank.TankAcceptance.Input && (__instance.tankFiltrationType == FuelValue_FiltrationType.FV_MOTORTANK || __instance.tankFiltrationType == FuelValue_FiltrationType.FV_FUELTANK) && newAmount < __instance.CurrentTankAmount)
                newAmount = __instance.CurrentTankAmount + (__instance.CurrentTankAmount - newAmount) * Main.instance.fuelConsumption;
        }
    }

    [HarmonyPatch(typeof(CanvasHelper),"Awake")]
    static class Patch_CreateHUD
    {
        public static void Prefix(CanvasHelper __instance)
        {
            var a = __instance.binocularImage.activeSelf;
            var g = new GameObject("MiscCheatsBinoController", typeof(RectTransform));
            g.SetActive(false);
            var r = g.GetComponent<RectTransform>();
            r.SetParent(__instance.binocularImage.transform.parent, false);
            r.anchorMin = r.offsetMax = r.offsetMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            __instance.binocularImage.transform.SetParent(r, true);
            g.AddComponent<Component>().target = __instance.binocularImage;
            __instance.binocularImage = g;
            g.SetActive(a);
        }

        [OnModLoad]
        public static void Init()
        {
            if (ComponentManager<CanvasHelper>.Value)
                Prefix(ComponentManager<CanvasHelper>.Value);
        }

        [OnModUnload]
        public static void Destroy()
        {
            if (!ComponentManager<CanvasHelper>.Value)
                return;
            var c = ComponentManager<CanvasHelper>.Value.binocularImage.GetComponent<Component>();
            if (!c)
                return;
            ComponentManager<CanvasHelper>.Value.binocularImage = c.target;
            c.target.transform.SetParent(c.transform.parent, true);
            Object.Destroy(c.gameObject);
        }

        class Component : MonoBehaviour
        {
            public GameObject target;
            bool active = Main.instance.disableBinoOverlay;
            public void OnEnable() => Update();
            public void Update()
            {
                if (active == Main.instance.disableBinoOverlay)
                    target.SetActive(active = !active);
            }
        }
    }

    [HarmonyPatch(typeof(Equipment_Model), "SetModelState")]
    static class Patch_EquipmentModel
    {
        static void Postfix(Transform ___localModel)
        {
            if (Main.instance.firstpersonEquipment && ___localModel)
                ___localModel.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(SteeringWheelRudderAttachment), "BuildRudderPipesAttachment")]
    static class Patch_BuildRudder
    {
        static bool Prefix() => !Main.instance.disableRudder;
    }

    [HarmonyPatch(typeof(ItemCollector), "OnTriggerEnter")]
    static class Patch_CollectorEnter
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1;i >= 0;i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f && f.Name == "maxNumberOfItems")
                    code.InsertRange(
                        i + 1,
                        new[]
                        {
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_CollectorEnter), nameof(ModifyMax)))
                        });
            return code;
        }
        static int ModifyMax(int original, ItemCollector instance) => instance.GetComponentInParent<Block>() ? original.Multiply(Main.instance.collectorMax) : original;
    }

    [HarmonyPatch]
    static class Patch_Purchase
    {
        [HarmonyPatch(typeof(TradingPostUI), "Open")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> TradingPostUI_Open_Transpiler(IEnumerable<CodeInstruction> instructions) => Transpiler(instructions);

        [HarmonyPatch(typeof(TradingPostUI), "RefreshUI")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> TradingPostUI_RefreshUI_Transpiler(IEnumerable<CodeInstruction> instructions) => Transpiler(instructions);

        [HarmonyPatch(typeof(TradingPost), "PurchaseItem")]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "stock") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyStock))));
            var ind = code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "stock");
            if (ind >= 0)
                code.Insert(
                    ind,
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyNewStock))));
            return code;
        }

        [HarmonyPatch(typeof(TradingPostUI), "Button_Buy")]
        static bool Prefix(TradingPost ___currentTradingPost, SO_TradingPost_Buyable.Instance ___selectedBuyableItem, Network_Player ___localPlayer)
        {
            if (Main.instance.unlimitedTrade && ___currentTradingPost && ___selectedBuyableItem != null && ___selectedBuyableItem.stock <= 0 && ___selectedBuyableItem.reward.item.UniqueIndex >= 0)
            {
                ___currentTradingPost.PurchaseItem(___selectedBuyableItem.reward.item.UniqueIndex, ___localPlayer);
                return false;
            }
            return true;
        }

        static class Methods
        {
            public static int ModifyStock(int original) => Main.instance.unlimitedTrade && original <= 0 ? 1 : original;
            public static int ModifyNewStock(int original) => Main.instance.unlimitedTrade && original < 0 ? 0 : original;
        }
    }

    [HarmonyPatch(typeof(Reciever), "UpdateRadarPointsFromChunkManager")]
    static class Patch_RecieverPointTypes
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "typesToDisplay") + 1,
                new[]
                {
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_RecieverPointTypes), nameof(ModifyTypes)))
                });
            code.InsertRange(
                code.FindIndex(x => x.opcode == OpCodes.Ldsfld && x.operand is FieldInfo f && f.Name == "unlockedChunkPointType") + 1,
                new[]
                {
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_RecieverPointTypes), nameof(ModifyTypes)))
                });
            code.Insert(
                code.FindLastIndex(x => x.opcode == OpCodes.Ldloc_1) + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_RecieverPointTypes), nameof(ModifyRemoveIndex))));
            return code;
        }
        static List<List<ChunkPointType>> tmp = new List<List<ChunkPointType>>();
        static List<ChunkPointType> ModifyTypes(List<ChunkPointType> original, int ind)
        {
            if (!Main.instance.radarSmall && !Main.instance.radarAch && !Main.instance.radarRaft)
                return original;
            List<ChunkPointType> t;
            while (tmp.Count <= ind)
                tmp.Add(new List<ChunkPointType>());
            t = tmp[ind];
            t.Clear();
            t.AddRange(original);
            if (Main.instance.radarSmall)
                t.Add(ChunkPointType.Landmark_Small);
            if (Main.instance.radarAch)
            {
                t.Add(ChunkPointType.Landmark_Pilot);
                t.Add(ChunkPointType.Landmark_Boat);
            }
            if (Main.instance.radarRaft)
                t.Add(ChunkPointType.Landmark_FloatingRaft);
            return t;
        }
        static int ModifyRemoveIndex(int original) => Main.instance.radarFix ? original - 1 : original;
    }

    [HarmonyPatch(typeof(Reciever_Dot), "SetTargetedByReciever")]
    static class Patch_ChangeDotColour
    {
        static void Postfix(Reciever_Dot __instance, Image ___dotImage, Sprite ___dotSprite)
        {
            var t = __instance.chunkPoint.rule.ChunkPointType;
            if (t == ChunkPointType.Landmark_Small || t == ChunkPointType.Landmark_Pilot || t == ChunkPointType.Landmark_Boat || t == ChunkPointType.Landmark_FloatingRaft)
                ___dotImage.sprite = Main.GetBlip(t,___dotSprite,
                    t == ChunkPointType.Landmark_Small
                    ? Main.instance.rcolorSmall
                    : t == ChunkPointType.Landmark_FloatingRaft
                    ? Main.instance.rcolorRaft
                    : Main.instance.rcolorAch);
        }
    }

    [HarmonyPatch(typeof(Plant), "Grow")]
    static class Patch_GrowPlant
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_GrowPlant), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original.SafeDivide(Main.instance.cropSpeed);
    }

    [HarmonyPatch(typeof(WeatherManager), "Update")]
    static class Patch_WeatherUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_WeatherUpdate), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original * Main.instance.weatherSpeed;
    }

    [HarmonyPatch]
    static class Patch_WindDirection
    {
        [HarmonyPatch(typeof(Raft), "FixedUpdate")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> RaftUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "moveDirection"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RotateTowards))));
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetNormalizedDirection") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RotateAway))));
            code.Insert(
                code.FindLastIndex(x => x.opcode == OpCodes.Ldloc_3) + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RotateTowards))));
            return code;
        }
        [HarmonyPatch(typeof(Streamer), "Update")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> StreamerUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "worldRotationTargetDir") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RotateTowards))));
            return code;
        }
        static class Methods
        {
            public static Vector3 RotateTowards(Vector3 original) => Main.instance.windDirection != 0 ? Quaternion.Euler(0, Main.instance.windDirection, 0) * original : original;
            public static Vector3 RotateAway(Vector3 original) => Main.instance.windDirection != 0 ? Quaternion.Euler(0, -Main.instance.windDirection, 0) * original : original;
        }
    }

    [HarmonyPatch(typeof(LandmarkEntitySpawner_Repeating), "Update")]
    static class Patch_EntitySpawnerUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_EntitySpawnerUpdate), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original.SafeDivide(Main.instance.spawnSpeed);
    }

    [HarmonyPatch(typeof(Equipment_Flipper), "UpdateEquipment")]
    static class Patch_FlipperUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_FlipperUpdate), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original * Main.instance.flipperDurability;
    }

    [HarmonyPatch(typeof(Equipment_AirBottle), "UpdateEquipment")]
    static class Patch_AirBottleUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_AirBottleUpdate), nameof(ModifyTime))));
            return code;
        }
        static float ModifyTime(float original) => original * Main.instance.bottleDurability;
    }

    [HarmonyPatch(typeof(Equipment_HeadLight), "UpdateEquipment")]
    static class Patch_HeadLightUpdate
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HeadLightUpdate), nameof(ModifyTime)))
                });
            return code;
        }
        static float ModifyTime(float original, Equipment_HeadLight equipment)
        {
            if (Main.instance.allowMultiEquipment)
            {
                var light = equipment.GetLight();
                var allLamps = equipment.GetPlayer().GetComponentsInChildren<Equipment_HeadLight>();
                foreach (var l in allLamps)
                    if (l == equipment)
                        break;
                    else if (l.GetLight() == light)
                        return 0;
            }
            return original * Main.instance.lampDurability;
        }
    }

    [HarmonyPatch]
    static class Patch_Attack
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(MeleeWeapon), "OnHitEntity");
            yield return AccessTools.Method(typeof(Throwable_Stone), "OnCollisionEvent");
            yield return AccessTools.Method(typeof(Arrow), "OnCollisionEvent");
            yield break;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var i = false;
            code.Insert(
                code.FindIndex(x => x.operand is FieldInfo f && f.Name == "damage" && ((i = f.FieldType == typeof(int)) || true)) + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), i ? nameof(Methods.ModifyDamageI) : nameof(Methods.ModifyDamageF))));
            return code;
        }
        static class Methods
        {
            public static int ModifyDamageI(int original) => original.Multiply(Main.instance.damage);
            public static float ModifyDamageF(float original) => original * Main.instance.damage;
        }
    }

    [HarmonyPatch(typeof(PersonController), "HandleRotationOfPlayer")]
    static class Patch_HandlePlayerRotation
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is FieldInfo f && f.Name == "rotateWithBuoyancy") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_HandlePlayerRotation), nameof(ShouldRotate))));
            return code;
        }
        static bool ShouldRotate(bool original) => original || Main.instance.rotateWithRaft;
    }

    [HarmonyPatch]
    static class Patch_WaterCameraEffects
    {
        static MethodBase TargetMethod() => typeof(WaterDropsIME).GetNestedTypes(~BindingFlags.Default).First(x => x.Name == "MaskingModule").GetMethod("Blit", ~BindingFlags.Default);
        static void Prefix(Material ____Material)
        {
            var d = table.GetOrCreateValue(____Material);
            if (float.IsNaN(d.original))
                d.original = ____Material.GetFloat("_Fadeout");
            if (Main.instance.waterFade != d.change)
                ____Material.SetFloat("_Fadeout", (d.change = Main.instance.waterFade) < 0 ? d.original : d.change);
        }

        static ConditionalWeakTable<Material, Data> table = new ConditionalWeakTable<Material, Data>();
        class Data
        {
            public float original = float.NaN;
            public float change = float.NaN;
        }

        [OnModUnload]
        static void TryRevertAll()
        {
            foreach (var m in Resources.FindObjectsOfTypeAll<Material>())
                if (table.TryGetValue(m, out var d))
                {
                    m.SetFloat("_Fadeout", d.original);
                    table.Remove(m);
                }
        }
    }

    [HarmonyPatch]
    static class Patch_TradingPostSell
    {
        [HarmonyPatch(typeof(TradingPostUI), "Button_Sell")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SellButton(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1; i >= 0; i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f && f.Name == "reputationReward")
                    code.Insert(
                        i + 1,
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyReward))));
            return code;
        }

        [HarmonyPatch(typeof(TradingPost), "Deserialize")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Deserialize(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(
                    code.FindLastIndex(x => x.operand is MethodInfo m && m.Name == "get_IsHost"),
                    x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "value"
                ) + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyMsgReward))));
            return code;
        }

        static class Methods
        {
            public static int ModifyReward(int original) => Raft_Network.IsHost ? original.Multiply(Main.instance.sellExp * Main.instance.hostSellExp) : original.Multiply(Main.instance.sellExp);
            public static int ModifyMsgReward(int original) => original.Multiply(Main.instance.hostSellExp);
        }
    }

    [HarmonyPatch(typeof(Hook),"Update")]
    static class Patch_HookArea
    {
        static void Postfix(Hook __instance, Network_Player ___playerNetwork)
        {
            if (!___playerNetwork.IsLocalPlayer)
                return;
            Data.Get(__instance);
        }

        class Data
        {
            static ConditionalWeakTable<Hook, Data> table = new ConditionalWeakTable<Hook, Data>();
            public Vector3 original;
            BoxCollider collider;
            public static Data Get(Hook instance)
            {
                if (!table.TryGetValue(instance, out var d))
                {
                    var b = instance.hookObjectCollider as BoxCollider;
                    table.Add(instance, d = new Data() { collider = b, original = b.size });
                }
                if (Main.instance.hookArea * d.original != d.collider.size)
                    d.collider.size = Main.instance.hookArea * d.original;
                return d;
            }

            [OnModUnload]
            static void TryRevertAll()
            {
                foreach (var h in Resources.FindObjectsOfTypeAll<Hook>())
                    if (table.TryGetValue(h, out var d))
                    {
                        d.collider.size = d.original;
                        table.Remove(h);
                    }
            }
        }
    }

    [HarmonyPatch]
    static class Patch_MultiplayerOption
    {
        static ConditionalWeakTable<Dropdown, object> marked = new ConditionalWeakTable<Dropdown,object>();

        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(NewGameBox), "Open");
            yield return AccessTools.Method(typeof(LoadGameBox), "Open");
        }
        static void Postfix(Dropdown ___authSettingDropdown)
        {
            ___authSettingDropdown.value = Main.instance.selectedMulti;
            if (!marked.TryGetValue(___authSettingDropdown,out _))
            {
                marked.Add(___authSettingDropdown, new object());
                ___authSettingDropdown.onValueChanged.AddListener(x =>
                {
                    if (Main.instance.storeMulti)
                        Main.instance.selectedMulti = x;
                });
            }
        }
    }

    [HarmonyPatch(typeof(ObjectSpawner_RaftDirection))]
    static class Patch_TrashSpawnerUpdate
    {
        public static bool Prewarm = false;
        public static ConditionalWeakTable<ObjectSpawner_RaftDirection, SpawnerMemory> table = new ConditionalWeakTable<ObjectSpawner_RaftDirection, SpawnerMemory>();

        [HarmonyPatch("Update")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_deltaTime") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyTime)))
                });

            code.Insert(
                code.FindLastIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "spawnDelay") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyDelay))));
            code.InsertRange(
                code.FindLastIndex(x => x.opcode == OpCodes.Stfld && x.operand is FieldInfo f && f.Name == "spawnTimer"),
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ObjectSpawner_RaftDirection), "spawnTimer")),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyNewTime)))
                });
            return code;
        }

        [HarmonyPatch("SpawnNewItems")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SpawnNewItems_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator iL)
        {
            var code = instructions.ToList();
            var loc = iL.DeclareLocal(typeof(Methods.SpawnArea));
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "GetRandomValue") + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(ObjectSpawner_RaftDirection), "spawnDelay")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldflda,AccessTools.Field(typeof(ObjectSpawner_RaftDirection), "spawnTimer")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloca,loc),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifySpawnCount)))
                });
            code.InsertRange(
                code.FindLastIndex(code.FindIndex(x => x.operand is MethodInfo m && m.Name == "IsSpawnPositionValid"),x => x.opcode == OpCodes.Ldarg_0) + 1,
                new[] {
                    new CodeInstruction(OpCodes.Ldloca_S,6),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldflda,AccessTools.Field(typeof(ObjectSpawner_RaftDirection), "spawnTimer")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc,loc),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyPosition)))
                });
            return code;
        }

        [HarmonyPatch("Update")]
        [HarmonyFinalizer]
        static void Update_Finalizer(ObjectSpawner_RaftDirection __instance)
        {
            if (table.TryGetValue(__instance,out var data) && data.dirty && (Raft_Network.WorldHasBeenRecieved || GameManager.IsInNewGame))
            {
                data.dirty = false;
                data.init = true;
                data.last = Patch_RaftPosition.current;
            }
        }

        [HarmonyPatch("SpawnNewItems")]
        [HarmonyFinalizer]
        static void SpawnNewItems_Finalizer(ObjectSpawner_RaftDirection __instance) => Update_Finalizer(__instance);


        [HarmonyPatch("RemoveItemsOutsideRange")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> RemoveItemsOutsideRange_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator iL)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is FieldInfo f && f.Name == "removeDistanceFromRaft") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.ModifyDespawnRange))));
            return code;
        }

        static class Methods
        {
            public static float ModifyTime(float original, ObjectSpawner_RaftDirection instance)
            {
                var data = table.GetOrCreateValue(instance);
                data.dirty = true;
                if (Main.instance.trashMode != TrashMode.Vanilla)
                    original = data.init ? (Patch_RaftPosition.current - data.last).XZOnly().magnitude / 2 : 0;
                return original * Main.instance.trashSpawn;
            }
            public static float ModifyDelay(float original) => Main.instance.trashMode == TrashMode.Area ? float.Epsilon : original;
            public static float ModifyNewTime(float original, ObjectSpawner_RaftDirection instance, float before) => Main.instance.trashMode == TrashMode.Area ? before : original;
            public static float ModifyDespawnRange(float original) => Main.instance.trashMode == TrashMode.Area ? original * Main.instance.trashArea : original;

            public class SpawnArea
            {
                public float spawnRadius;
                public float sqrSpawnRadius;
                public Vector3 travel;
                public Vector3 travelNorm;
                public float travelDist;
                public float sqrTravelDist;
                public float area;
                public float midWidth;
                public float angle;
                public float segmentArea;
            }

            public static int ModifySpawnCount(int original, float spawnDelay, ref float spawnTimer, ObjectSpawner_RaftDirection spawner, out SpawnArea area)
            {
                if (Main.instance.trashMode == TrashMode.Area)
                {
                    area = new SpawnArea();
                    var data = table.GetOrCreateValue(spawner);
                    area.travel = data.init ? (Patch_RaftPosition.current - data.last).XZOnly() : Vector3.zero;
                    if (Prewarm && area.travel == Vector3.zero)
                        area.travel = Vector3.forward;
                    area.sqrTravelDist = area.travel.sqrMagnitude;
                    if (area.sqrTravelDist == 0)
                        return 0;
                    var maxV = float.NegativeInfinity;
                    var minH = float.PositiveInfinity;
                    var maxH = float.NegativeInfinity;
                    foreach (var pos in spawner.spawnPositions)
                    {
                        if (pos.verticalSize > maxV)
                            maxV = pos.verticalSize;
                        if (pos.horizontalOffset + pos.horizontalSize > maxH)
                            maxH = pos.horizontalOffset + pos.horizontalSize;
                        if (pos.horizontalOffset - pos.horizontalSize < minH)
                            minH = pos.horizontalOffset - pos.horizontalSize;
                    }
                    area.spawnRadius = (spawner.removeItemsAutomaticly ? spawner.removeDistanceFromRaft.Next(-1) : (spawner.spawnDistanceFromRaft + maxV)) * Main.instance.trashArea;
                    if (Prewarm)
                    {
                        area.travel = area.travel.normalized * area.spawnRadius * 2.1f;
                        area.sqrTravelDist = area.travel.sqrMagnitude;
                    }
                    area.sqrSpawnRadius = area.spawnRadius * area.spawnRadius;
                    area.travelDist = (float)Math.Sqrt(area.sqrTravelDist);
                    if (area.travelDist >= area.spawnRadius * 2)
                        area.area = (float)Math.PI * area.sqrSpawnRadius;
                    else
                    {
                        area.midWidth = (float)Math.Sqrt(area.sqrSpawnRadius * 4 - area.sqrTravelDist);
                        area.angle = (float)Math.Asin(area.travelDist / 2 / area.spawnRadius) * 2;
                        area.segmentArea = 0.5f * (area.angle - (float)Math.Sin(area.angle)) * area.sqrSpawnRadius;
                        area.area = area.midWidth * area.travelDist + area.segmentArea;
                        area.travelNorm = area.travel / area.travelDist;
                    }
                    var areaPerItem = ((maxV + maxV) * (maxH - minH)) / (original * Main.instance.trashQuantity) * spawnDelay / 100 / ((Prewarm ? area.travelDist / 2 * Main.instance.trashSpawn : spawnTimer) / area.travelDist);
                    if (areaPerItem < 0.01f)
                        areaPerItem = 0.01f;
                    var itemCount = (int)(area.area / areaPerItem);
                    if (!Prewarm && float.IsFinite(spawnTimer))
                        spawnTimer *= 1 - (itemCount * areaPerItem / area.area);
                    else
                        spawnTimer = 0;
                    return itemCount;
                }
                area = null;
                return original.Multiply(Main.instance.trashQuantity);
            }
            public static void ModifyPosition(ref Vector3 spawnPosition, ref float spawnTimer, ObjectSpawner_RaftDirection spawner, SpawnArea area)
            {
                if (Main.instance.trashMode == TrashMode.Area)
                {
                    if (area.travelDist >= area.spawnRadius * 2)
                    {
                        var x = (float)(Math.Asin(Random.value * 2 - 1) / Math.PI * 2);
                        var z = (Random.value * 2 - 1) * Math.Sqrt(1 - x * x);
                        spawnPosition = Patch_RaftPosition.current + new Vector3((float)x * area.spawnRadius, 0, (float)z * area.spawnRadius);
                    }
                    else
                    {
                        var rand = Random.value;
                        var segmentPercent = area.segmentArea / area.area;
                        float x;
                        float z;
                        if (rand >= segmentPercent && rand <= 1 - segmentPercent)
                        {
                            x = ((rand - segmentPercent) / (1 - segmentPercent * 2) * 2 - 1) * area.spawnRadius;
                            z = area.travelDist * Random.value + (float)Math.Sqrt(1 - Math.Pow(rand * 2 - 1,2)) * area.spawnRadius - area.travelDist / 2;
                        }
                        else
                        {
                            var relativeSegment = (area.segmentArea / ((float)Math.PI * area.sqrSpawnRadius)) / segmentPercent;
                            if (rand < 0.5f)
                                rand *= relativeSegment;
                            else
                                rand = 1 - (1 - rand) * relativeSegment;
                            x = (float)(Math.Asin(rand * 2 - 1) / Math.PI * 2);
                            z = (float)((Random.value * 2 - 1) * Math.Sqrt(1 - x * x)) * area.spawnRadius;
                            x *= area.spawnRadius;
                        }
                        spawnPosition = Patch_RaftPosition.current + area.travelNorm * z + new Vector3(area.travelNorm.z * x, 0,-area.travelNorm.x * x);
                    }
                }
            }
        }

        public class SpawnerMemory
        {
            public bool dirty;
            public bool init;
            public Vector3 last;
        }
    }

    public abstract class ExtendedClass<X, Y> where X : ExtendedClass<X, Y>, new() where Y : class
    {
        static ConditionalWeakTable<Y, X> table = new ConditionalWeakTable<Y, X>();
        public static X Get(Y instance)
        {
            if (table.TryGetValue(instance, out var v))
            {
                v.OnGet(instance);
                return v;
            }
            v = new X();
            table.Add(instance, v);
            v.instance = instance;
            v.OnCreate(instance);
            v.OnGet(instance);
            return v;
        }
        public static void Clear()
        {
            table = new ConditionalWeakTable<Y, X>();
        }
        public Y instance;
        protected virtual void OnCreate(Y instance) { }
        protected virtual void OnGet(Y instance) { }
    }

    class TrashFloater : ExtendedClass<TrashFloater, ObjectSpawnerManager>
    {
        float next;
        int frames;
        public void DoUpdate()
        {
            if (Time.deltaTime == 0)
                return;
            if (Main.instance.trashLagMode == TrashLagMode.NoMovement)
            {
                var spawners = instance.spawners.GetInternal();
                var send = instance.spawners.Count;
                for (int sind = 0; sind < send; sind++)
                {
                    var spawner = spawners[sind];
                    if (!(spawner is ObjectSpawner_RaftDirection))
                        continue;
                    var items = spawner.spawnedObjects.GetInternal();
                    var end = spawner.spawnedObjects.Count;
                    for (int ind = 0; ind < end; ind++)
                    {
                        var i = items[ind];
                        var data = PerItemFloater.Get(i);
                        if (data.state != 2)
                        {
                            data.state = 2;
                            if (data.floater.enabled)
                                data.floater.enabled = false;
                            var p = i.transform.localPosition;
                            p.y = 0;
                            i.transform.localPosition = p;
                        }
                    }
                }
                return;
            }
            frames++;
            if (frames < Main.instance.trashLagInterval)
                return;
            frames = 0;
            var water = SingletonGeneric<GameManager>.Singleton.water;
            var time = water.Time;
            if (time > next)
            {
                next = time + Main.instance.trashLagFix;
                var spawners = instance.spawners.GetInternal();
                var send = instance.spawners.Count;
                for (int sind = 0; sind < send; sind++)
                {
                    var spawner = spawners[sind];
                    if (!(spawner is ObjectSpawner_RaftDirection))
                        continue;
                    var items = spawner.spawnedObjects.GetInternal();
                    var end = spawner.spawnedObjects.Count;
                    for (int ind = 0; ind < end; ind++)
                    {
                        var i = items[ind];
                        var data = PerItemFloater.Get(i);
                        if (data.floater.enabled)
                            data.floater.enabled = false;
                        var p = i.transform.localPosition;
                        if (data.state == 1)
                        {
                            var ny = data.start += data.progress;
                            if (Main.instance.trashLagThreshold == 0 || Math.Abs(ny - p.y) >= Main.instance.trashLagThreshold)
                            {
                                p.y = ny;
                                i.transform.localPosition = p;
                            }
                        }
                        else
                            data.start = p.y;
                        data.progress = water.GetUncompensatedHeightAt(p.x, p.z, next) - p.y;
                        data.state = 1;
                    }
                }
            }
            else
            {
                time = 1 - (next - time) / Main.instance.trashLagFix;
                var spawners = instance.spawners.GetInternal();
                var send = instance.spawners.Count;
                for (int sind = 0; sind < send; sind++)
                {
                    var spawner = spawners[sind];
                    if (!(spawner is ObjectSpawner_RaftDirection))
                        continue;
                    var items = spawner.spawnedObjects.GetInternal();
                    var end = spawner.spawnedObjects.Count;
                    for (int ind = 0; ind < end; ind++)
                    {
                        var i = items[ind];
                        var data = PerItemFloater.Get(i);
                        var p = i.transform.localPosition;
                        if (data.state == 1)
                        {
                            var ny = data.start + data.progress * time;
                            if (Main.instance.trashLagThreshold != 0 && Math.Abs(ny - p.y) < Main.instance.trashLagThreshold)
                                continue;
                            p.y = ny;
                            i.transform.localPosition = p;
                        }
                        else
                        {
                            data.start = p.y;
                            data.progress = water.GetUncompensatedHeightAt(p.x, p.z, next) - p.y;
                            data.state = 1;
                        }
                    }
                }
            }
        }

        public void DoNotUpdate()
        {
            if (next == 0)
                return;
            next = 0;
            foreach (var s in instance.spawners)
                if (s is ObjectSpawner_RaftDirection)
                    foreach (var i in s.spawnedObjects)
                    {
                        PerItemFloater.Get(i).state = 0;
                        var f = i.GetComponent<WaterFloatSemih2>();
                        if (f) f.enabled = true;
                    }
        }

        [OnModUnload]
        static void OnUnload()
        {
            if (ComponentManager<ObjectSpawnerManager>.Value)
                Get(ComponentManager<ObjectSpawnerManager>.Value).DoNotUpdate();
            Clear();
        }
    }
    class PerItemFloater : ExtendedClass<PerItemFloater, PickupItem_Networked>
    {
        public WaterFloatSemih2 floater;
        public float start;
        public float progress;
        public int state;
        protected override void OnCreate(PickupItem_Networked instance)
        {
            floater = instance.GetComponent<WaterFloatSemih2>();
        }

        [OnModUnload]
        static void OnUnload()
        {
            Clear();
        }
    }
    class PerformanceCounter
    {
        public long ticks;
        public long counter;
        long start;
        public void Start()
        {
            GetSystemTimeAsFileTime(out start);
        }
        public void End()
        {
            GetSystemTimeAsFileTime(out var end);
            counter++;
            ticks += end - start;
        }

        [DllImport("kernel32")]
        static extern void GetSystemTimeAsFileTime(out long value);
        public static long GetSystemTickFast()
        {
            GetSystemTimeAsFileTime(out var p);
            return p;
        }
    }

    [HarmonyPatch(typeof(ItemCollector),"CollectItem")]
    static class Patch_CollectorCollect
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_enabled")+1,
                new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_CollectorCollect), nameof(EditValid)))
                });
            return code;
        }
        static bool EditValid(bool original, PickupItem_Networked item) => original || (Main.instance.trashLagMode != TrashLagMode.Vanilla && item.spawner is ObjectSpawner_RaftDirection);
    }

    public class UpdateStagger
    {
        int count;
        int step;
        int max;
        public UpdateStagger(int maxSteps)
        {
            TurnOver(maxSteps);
        }
        public void TurnOver() => TurnOver(max);
        public void TurnOver(int maxSteps)
        {
            if (maxSteps > count)
                step = 0;
            else
            {
                step += max;
                while (step > 0)
                    step -= count;
            }
            count = 0;
            max = maxSteps;
        }
        public bool Allow()
        {
            count++;
            step++;
            return step > 0 && step <= max;
        }
    }

    [HarmonyPatch(typeof(ObjectSpawnerManager), "PrewarmStart")]
    static class Patch_ObjectSpawnerPrewarm
    {
        static bool Prefix(ObjectSpawnerManager __instance)
        {
            if (Main.instance.trashMode != TrashMode.Area)
                return true;
            var prev = Patch_TrashSpawnerUpdate.Prewarm;
            Patch_TrashSpawnerUpdate.Prewarm = true;
            __instance.plankSpawner.SpawnNewItems();
            __instance.itemSpawner.SpawnNewItems();
            Patch_TrashSpawnerUpdate.Prewarm = prev;
            return false;
        }
    }

    [HarmonyPatch(typeof(Raft))]
    static class Patch_RaftPosition
    {
        public static Vector3 current;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void Update(Rigidbody ___body)
        {
            current = ___body.transform.position;
        }

        [HarmonyPatch("OnWorldShift")]
        [HarmonyPostfix]
        static void OnWorldShift(Vector3 shift)
        {
            current -= shift;
            foreach (var s in Resources.FindObjectsOfTypeAll<ObjectSpawner_RaftDirection>())
                if (Patch_TrashSpawnerUpdate.table.TryGetValue(s, out var data) && data.init)
                    data.last -= shift;
        }
    }

    [HarmonyPatch(typeof(ObjectSpawnerManager), "Update")]
    static class Patch_IsSpawnerMoving
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindIndex(x => x.operand is MethodInfo m && m.Name == "get_Moving") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_IsSpawnerMoving), nameof(EditMoving))));
            return code;
        }
        static bool EditMoving(bool original) => original || Main.instance.trashMode != TrashMode.Vanilla;
    }

    [HarmonyPatch(typeof(Slot), "StackIsFull")]
    static class Patch_SlotIsFull
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            for (int i = code.Count - 1; i >= 0; i--)
                if (code[i].opcode == OpCodes.Ldfld && code[i].operand is FieldInfo f && f.Name == "settings_Inventory" && f.DeclaringType == typeof(ItemInstance))
                {
                    code.Insert(
                        i + 1,
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SlotIsFull), nameof(ReplaceSettings))));
                    code.Insert(
                        i,
                        new CodeInstruction(OpCodes.Dup));
                }
            return code;
        }
        static ItemInstance_Inventory ReplaceSettings(ItemInstance item, ItemInstance_Inventory original) => Main.instance.quickMoveFreeze ? item.baseItem.settings_Inventory : original;
    }

    [HarmonyPatch(typeof(GameModeValueManager), "GetDifficultyEntityVariableFromAINetworkBehaviourType")]
    static class Patch_GetEntityVariables
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.InsertRange(
                code.FindLastIndex(x => x.opcode == OpCodes.Ret),
                new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_GetEntityVariables), nameof(EditFallback)))
                });
            return code;
        }
        static EntityVariables EditFallback(EntityVariables original, AI_NetworkBehaviourType type) => original == null && type == AI_NetworkBehaviourType.Roach && Main.instance.roachPeaceful ? GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(AI_NetworkBehaviourType.Rat) : original;

        [OnModUnload]
        static void OnUnload()
        {
            Main.instance.roachPeaceful = false;
        }
    }

    [HarmonyPatch(typeof(BlockCreator), "CreateBlock")]
    static class Patch_CreateBlock
    {
        public static void Postfix(Block __result)
        {
            if (__result is Block_Foundation_ItemNet && Main.instance.netDriftFix)
            {
                var collector = __result.GetComponentInChildren<ItemCollector>();
                var body = collector.GetComponent<Rigidbody>();
                if (!body)
                    return;
                Object.DestroyImmediate(body);
                var p = __result.buildableItem.settings_buildable.GetBlockPrefab(__result.dpsType);
                if (!p)
                    return;
                var pChild = p.transform.Find(collector.transform.GetPathFrom(__result.transform));
                if (pChild)
                    collector.transform.localPosition = pChild.localPosition;
            }
        }
    }


    [HarmonyPatch(typeof(PlayerInventory), "Clear")]
    public class Patch_ClearInventory
    {
        static void Prefix(Network_Player ___playerNetwork) => Patch_ResetSlot.player = ___playerNetwork;
        static void Postfix() => Patch_ResetSlot.player = null;
    }

    [HarmonyPatch(typeof(PlayerInventory), "ClearInventoryLeaveSome")]
    public class Patch_PartialClearInventory
    {
        static void Prefix(Network_Player ___playerNetwork) => Patch_ResetSlot.player = ___playerNetwork;
        static void Finalizer() => Patch_ResetSlot.player = null;
    }

    [HarmonyPatch]
    static class Patch_ResetSlot
    {
        public static Network_Player player;

        [HarmonyPatch(typeof(Slot), "Reset")]
        [HarmonyPrefix]
        static void Reset(Slot __instance)
        {
            if (Main.instance.keepInventory == KeepInventoryMode.DropItems && player && !__instance.IsEmpty)
                Methods.TryDropItem(__instance.itemInstance);
        }

        [HarmonyPatch(typeof(Slot), "SetItem", typeof(Item_Base), typeof(int))]
        [HarmonyPrefix]
        static void SetItem(Slot __instance, int amount)
        {
            if (Main.instance.keepInventory == KeepInventoryMode.DropItems && player && !__instance.IsEmpty)
                Methods.TryDropItem(new ItemInstance(__instance.itemInstance.baseItem, __instance.itemInstance.Amount - amount, __instance.itemInstance.Uses, __instance.itemInstance.exclusiveString));
        }

        [HarmonyPatch(typeof(Slot), "SetUses")]
        [HarmonyPrefix]
        static bool SetUses(Slot __instance)
        {
            if (Main.instance.keepInventory == KeepInventoryMode.DropItems && player && !__instance.IsEmpty && Methods.TryDropItem(__instance.itemInstance))
            {
                __instance.SetItem(null);
                return false;
            }
            return true;
        }

        static class Methods
        {
            public static bool TryDropItem(ItemInstance item)
            {
                if (!(Main.instance.alwaysSpecial && isItemSpecial(player, item)))
                {
                    int itemCount = item.Amount;
                    int newCount = 0;
                    for (int i = 0; i < itemCount; i++)
                        if (Random.value < Main.instance.dropChance)
                            newCount++;
                    if (newCount < itemCount)
                    {
                        if (newCount == 0)
                            item = null;
                        else
                            item.Amount = newCount;
                    }
                }
                if (item != null && item.Valid)
                {
                    Helper.DropItem(item, player.transform.position, new Vector3(Random.value - 0.5f, 0.5f, Random.value - 0.5f), player.PersonController.HasRaftAsParent);
                    return true;
                }
                return false;
            }
            public static bool isItemSpecial(Network_Player player, ItemInstance item)
            {
                if (item.settings_buildable != null && item.settings_buildable.Placeable) return true;
                if (item.settings_equipment != null && item.settings_equipment.EquipType != EquipSlotType.None) return true;
                foreach (ItemConnection connect in player.PlayerItemManager.useItemController.allConnections)
                    if (connect.inventoryItem == item.baseItem)
                        return true;
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(DropItem), "Awake")]
    static class Patch_SpawnDropItem
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            code.Insert(
                code.FindLastIndex(x => x.opcode == OpCodes.Ldfld && x.operand is FieldInfo f && f.Name == "despawnTime") + 1,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnDropItem), nameof(EditDespawnTime))));
            return code;
        }
        static float EditDespawnTime(float original) => original * Main.instance.dropDespawn;
    }

    [HarmonyPatch(typeof(PlayerStats), "HandleUIFeedback")]
    static class Patch_StatsUIFeedback
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var flag = false;
            for (int i = 0; i < code.Count; i++)
                if (code[i].operand is MethodInfo m && m.Name == "MapValue")
                    flag = true;
                else if (flag && code[i].opcode == OpCodes.Stfld && code[i].operand is FieldInfo f)
                {
                    flag = false;
                    code.InsertRange(i, new[]
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_StatsUIFeedback), f.Name == "intensity" ? nameof(EditValue) : nameof(EditValueHealth)))
                    });
                    i += 2;
                }
            return code;
        }
        static float EditValue(float original, PlayerStats instance)
        {
            if (Main.instance.gradualUnhealthy)
                return original * (1 - Math.Min(instance.stat_health.NormalValue, Math.Min(instance.stat_hunger.Normal.NormalValue, instance.stat_thirst.Normal.NormalValue)) / Patch_WellBeingThreashold.Methods.ModifyBadLimit(Stat_WellBeing.WellBeingLimit));
            return original;
        }
        static float EditValueHealth(float original, PlayerStats instance)
        {
            if (Main.instance.gradualUnhealthy)
                return original * (instance.stat_health.NormalValue / Patch_WellBeingThreashold.Methods.ModifyBadLimit(Stat_WellBeing.WellBeingLimit));
            return original;
        }
    }

    public static class ColorConvert
    {
        public static void ToHSL(this Color c, out float hue, out float saturation, out float luminosity) => ToHSL(c.r, c.g, c.b, out hue, out saturation, out luminosity);
        public static void ToHSL(float R, float G, float B, out float hue, out float saturation, out float luminosity)
        {
            var max = Math.Max(Math.Max(R, G), B);
            var min = Math.Min(Math.Min(R, G), B);
            luminosity = max;
            if (min == max)
            {
                hue = 0;
                saturation = 0;
                return;
            }
            saturation = (max - min) / max;
            if (R == max)
            {
                if (G >= B)
                    hue = (G - min) * H60 / (max - min);
                else
                    hue = H360 - (B - min) * H60 / (max - min);
            }
            else if (G == max)
            {
                if (B >= R)
                    hue = H120 + (B - min) * H60 / (max - min);
                else
                    hue = H120 - (R - min) * H60 / (max - min);
            }
            else
            {
                if (R >= G)
                    hue = H240 + (R - min) * H60 / (max - min);
                else
                    hue = H240 - (G - min) * H60 / (max - min);
            }
        }
        public static Color FromHSL(float hue, float saturation, float luminosity)
        {
            FromHSL(hue, saturation, luminosity, out var R, out var G, out var B);
            return new Color(R, G, B);
        }
        const float H60 = 1f / 6;
        const float H120 = 2f / 6;
        const float H180 = 3f / 6;
        const float H240 = 4f / 6;
        const float H300 = 5f / 6;
        const float H360 = 1;
        public static void FromHSL(float hue, float saturation, float luminosity, out float R, out float G, out float B)
        {
            hue %= 1;
            if (hue < 0)
                hue += 1;
            var max = luminosity;
            if (saturation == 0)
            {
                R = G = B = max;
                return;
            }
            var min = max - (saturation * max);
            if (hue <= H60)
            {
                B = min;
                R = max;
                G = min + hue * (max - min) / H60;
            }
            else if (hue <= H120)
            {
                B = min;
                G = max;
                R = min + (H120 - hue) * (max - min) / H60;
            }
            else if (hue <= H180)
            {
                R = min;
                G = max;
                B = min + (hue - H120) * (max - min) / H60;
            }
            else if (hue <= H240)
            {
                R = min;
                B = max;
                G = min + (H240 - hue) * (max - min) / H60;
            }
            else if (hue <= H300)
            {
                G = min;
                B = max;
                R = min + (hue - H240) * (max - min) / H60;
            }
            else
            {
                G = min;
                R = max;
                B = min + (H360 - hue) * (max - min) / H60;
            }
        }

        public static Color Normalized(this Color c)
        {
            c.ToHSL(out var h, out var s, out var l);
            Normalized(ref h, ref s, ref l);
            return FromHSL(h, s, l);
        }

        public static Color Clamped(this Color c) => new Color(Math.Min(1, Math.Max(0, c.r)), Math.Min(1, Math.Max(0, c.g)), Math.Min(1, Math.Max(0, c.b)), Math.Min(1, Math.Max(0, c.a)));
        public static void Normalized(ref float hue, ref float saturation, ref float luminosity)
        {
            saturation = Math.Min(1, Math.Max(0, saturation));
            luminosity = Math.Min(1, Math.Max(0, luminosity));
        }
    }

    static class ExtentionMethods
    {
        public static string GetHex(this Color color) => ((Color32)color).GetHex();
        public static string GetHex(this Color32 color) => color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
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
        public static int Multiply(this int value, double multiplier, int min = int.MinValue, int max = int.MaxValue)
        {
            if (multiplier == 1)
                return value;
            var v = value * multiplier;
            if (v >= max)
                return max;
            if (v <= min)
                return min;
            var i = (int)Math.Round(v);
            if (((value < 0) ^ (multiplier < 0)) == (i < 0))
                return i;
            return i < 0 ? max : min;
        }
        public static int Multiply(this int value, float multiplier, int min = int.MinValue, int max = int.MaxValue)
        {
            if (multiplier == 1)
                return value;
            var v = value * multiplier;
            if (v >= max)
                return max;
            if (v <= min)
                return min;
            var i = (int)Mathf.Round(v);
            if (((value < 0) ^ (multiplier < 0)) == (i < 0))
                return i;
            return i < 0 ? max : min;
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

        static FieldInfo _Amplitude => AccessTools.Field(typeof(WaterWavesSpectrum), "_Amplitude");
        public static void SetAmplitude(this WaterWavesSpectrum spectrum, float value) => _Amplitude.SetValue(spectrum, value);
        public static float GetAmplitude(this WaterWavesSpectrum spectrum) => (float)_Amplitude.GetValue(spectrum);
        static FieldInfo _WindSpeed => AccessTools.Field(typeof(WaterWavesSpectrum), "_WindSpeed");
        public static void SetWindSpeed(this WaterWavesSpectrum spectrum, float value) => _WindSpeed.SetValue(spectrum, value);
        public static float GetWindSpeed(this WaterWavesSpectrum spectrum) => (float)_WindSpeed.GetValue(spectrum);

        static FieldInfo _lightSourceLight => AccessTools.Field(typeof(Equipment_HeadLight), "lightSourceLight");
        public static Light GetLight(this Equipment_HeadLight equip) => (Light)_lightSourceLight.GetValue(equip);
        static FieldInfo _playerNetwork => AccessTools.Field(typeof(Equipment_HeadLight), "playerNetwork");
        public static Network_Player GetPlayer(this Equipment_HeadLight equip) => (Network_Player)_playerNetwork.GetValue(equip);
        static FieldInfo _body => AccessTools.Field(typeof(Raft), "body");
        public static Rigidbody GetBody(this Raft raft) => (Rigidbody)_body.GetValue(raft);
        static MethodInfo _raftUpdate => AccessTools.Method(typeof(Raft), "Update");
        public static void Update(this Raft raft) => _raftUpdate.Invoke(raft,Array.Empty<object>());

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

        public static T Safe<T>(this T obj) where T : Object => obj ? obj : null;

        public static T GetRandom<T>(this ICollection<T> items)
        {
            var item = Random.Range(0, items.Count);
            if (items is IList<T> l)
                return l[item];
            foreach (var i in items)
                if (item-- == 0)
                    return i;
            // This line should never be reached
            return default;
        }
        public static Sprite GetReadable(this Sprite source)
        {
            var s = Sprite.Create(source.texture.GetReadable(), source.rect, source.pivot, source.pixelsPerUnit);
            Main.createdObjects.Add(s);
            return s;
        }

        public static Texture2D GetReadable(this Texture2D source, TextureFormat format = TextureFormat.ARGB32)
        {
            var prev = RenderTexture.active;
            var temp = RenderTexture.GetTemporary(new RenderTextureDescriptor(source.width, source.height, GraphicsFormatUtility.GetGraphicsFormat(format, GraphicsFormatUtility.IsSRGBFormat(source.graphicsFormat)), 16, source.mipmapCount));
            temp.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            temp.useMipMap = true;
            Graphics.Blit(source, temp);
            Graphics.SetRenderTarget(temp, 0);
            Texture2D texture = new Texture2D(source.width, source.height, format, source.mipmapCount, !GraphicsFormatUtility.IsSRGBFormat(source.graphicsFormat));
            texture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, true);
            texture.Apply(true, false);
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(temp);
            return texture;
        }

        public static Y GetOrCreate<X,Y>(this IDictionary<X,Y> d,X key) where Y : new()
        {
            if (!d.TryGetValue(key, out var value))
                d[key] = value = new Y();
            return value;
        }

        public static unsafe float Next(this float value,int steps)
        {
            var intVal = *(int*)&value + steps;
            return *(float*)&intVal;
        }

        public static T[] GetInternal<T>(this List<T> list) => (T[])typeof(List<T>).GetField("_items", ~BindingFlags.Default).GetValue(list);

        public static string GetPathFrom(this Transform child, Transform parent)
        {
            if (!child)
                throw new ArgumentNullException(nameof(child));
            if (child.parent == parent)
                return "";
            var str = new StringBuilder();
            str.Append(child.name);
            var curr = child.parent;
            while (curr && curr != parent)
            {
                str.Insert(0, parent.name + "/");
                curr = curr.parent;
            }
            if (parent && curr != parent)
                return null;
            return str.ToString();
        }

        public static void Log(this List<CodeInstruction> code)
        {
            var s = new StringBuilder();
            for (int j = 0; j < code.Count; j++)
            {
                s.Append('\n');
                s.Append(j);
                for (int n = 4 - j.ToString().Length; n > 0; n--)
                    s.Append(' ');
                s.Append(": ");
                s.Append(code[j].opcode);
                if (code[j].operand != null)
                {
                    for (int n = 20 - code[j].opcode.ToString().Length; n > 0; n--)
                        s.Append(' ');
                    s.Append('|');
                    if (code[j].operand is Label lbl)
                        s.Append(code.FindIndex(x => x.labels.Contains(lbl)));
                    else if (code[j].operand is MemberInfo mem)
                    {
                        s.Append(mem.DeclaringType.FullName);
                        s.Append("::");
                        s.Append(mem);
                    }
                    else if (code[j].operand is LocalBuilder loc)
                        s.Append(loc.LocalIndex);
                    else
                        s.Append(code[j].operand);
                }
            }
            Debug.Log(s.ToString());
        }

        public static T Take<T>(this IList<T> l, int index = -1)
        {
            int i = index < 0 ? l.Count + index : index;
            var v = l[i];
            l.RemoveAt(i);
            return v;
        }
    }
}