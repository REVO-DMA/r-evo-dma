using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class ExfilManager
    {
        public IReadOnlyList<Exfil> _exfils;
        private byte _exfilsCount = 0;

        public ExfilManager(ulong localGameWorld, bool isPMC)
        {
            ulong exfilController = Memory.ReadPtr(localGameWorld + Offsets.ClientLocalGameWorld.ExfilController, false);
            ulong listOffset = isPMC ? Offsets.ExfilController.ExfiltrationPointArray : Offsets.ExfilController.ScavExfiltrationPointArray;
            ulong exfilPoints = Memory.ReadPtr(exfilController + listOffset, false);
            
            MemArray<ulong> exfils = new(exfilPoints, false);
            if (!exfils.Items.Any())
                throw new ValueOutOfRangeException(nameof(exfils));

            List<Exfil> list = new();
            foreach (ulong exfilAddr in exfils.Items)
            {
                try
                {
                    Exfil exfil = new(exfilAddr, isPMC, _exfilsCount, false);
                    _exfilsCount++;
                    list.Add(exfil);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[EXFIL MANAGER] -> .ctor(): Error allocating exfil ~ {ex}");
                }
            }

            try
            {
                ulong transitController = Memory.ReadPtr(localGameWorld + Offsets.ClientLocalGameWorld.TransitController, false);
                ulong transitPointsAddress = Memory.ReadPtr(transitController + Offsets.TransitController.TransitPoints, false);
                MemDictionary<ulong, ulong> transitPoints = new(transitPointsAddress, false);
                foreach (var transitPointKV in transitPoints.Items)
                {
                    try
                    {
                        Exfil exfil = new(transitPointKV.Value, isPMC, _exfilsCount, true);
                        _exfilsCount++;
                        list.Add(exfil);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[EXFIL MANAGER] -> .ctor(): Error allocating Transit ~ {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[EXFIL MANAGER] -> .ctor(): Failed to instantiate Transits ~ {ex}");
            }

            _exfils = list;
            Refresh();
        }

        public void Refresh()
        {
            try
            {
                EFTScatterMap map = new(_exfils.Count);
                var round1 = map.AddRound();
                
                for (int i = 0; i < _exfils.Count; i++)
                {
                    round1.AddEntry<int>(i, 0, _exfils[i].BaseAddr + Offsets.Exfil._status);
                }
                
                map.Execute();
                
                for (int i = 0; i < _exfils.Count; i++)
                {
                    var exfil = _exfils[i];

                    if (exfil.IsTransit)
                        continue;

                    try
                    {
                        if (map.Results[i][0].TryGetResult<int>(out var status))
                            _exfils[i].Update((Enums.EExfiltrationStatus)status);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }

    #region Classes_Enums
    public sealed class Exfil
    {
        public Vector3 Position = Vector3.Zero;

        public bool IsPMCExfil { get; }
        public ulong BaseAddr { get; }
        public byte ID { get; }
        public string Name { get; }
        public string Description { get; } = "";
        public HashSet<string> EligibleEntryPoints { get; private set; } = new(StringComparer.OrdinalIgnoreCase);
        public ExfilStatus Status { get; private set; } = ExfilStatus.Closed;
        public bool IsTransit { get; }

        public Exfil(ulong baseAddr, bool isPMCExfil, byte id, bool isTransit)
        {
            ID = id;
            BaseAddr = baseAddr;
            IsPMCExfil = isPMCExfil;
            IsTransit = isTransit;

            var transform_internal = Memory.ReadPtrChain(baseAddr, UnityOffsets.GameObject.To_TransformInternal, false);
            Position = new UnityTransform(transform_internal, UnityTransform.TransformType.Normal, true, true).GetPosition();

            if (isTransit)
            {
                ulong parameters = Memory.ReadPtr(baseAddr + Offsets.TransitPoint.parameters);
                ulong nameAddr = Memory.ReadPtr(parameters + Offsets.TransitParameters.name);
                ulong descriptionAddr = Memory.ReadPtr(parameters + Offsets.TransitParameters.description);

                Name = LocaleManager.GetItem(Memory.ReadUnityString(nameAddr));
                Description = LocaleManager.GetItem(Memory.ReadUnityString(descriptionAddr));
                Status = ExfilStatus.Transit;
            }
            else
            {
                var namePtr = Memory.ReadPtrChain(baseAddr, new uint[] { Offsets.Exfil.Settings, Offsets.ExfilSettings.Name }, false);

                if (IsPMCExfil)
                {
                    ulong eligibleEntryPointsAddr = Memory.ReadPtr(baseAddr + Offsets.Exfil.EligibleEntryPoints, false);
                    MemArray<ulong> eligibleEntryPoints = new(eligibleEntryPointsAddr, false);
                    foreach (ulong eligibleEntryPoint in eligibleEntryPoints.Items)
                    {
                        string entryPointName = Memory.ReadUnityString(eligibleEntryPoint, false);
                        EligibleEntryPoints.Add(entryPointName);
                    }
                }
                else
                {
                    ulong eligibleIdsPtr = Memory.ReadPtr(baseAddr + Offsets.ScavExfil.EligibleIds);
                    MemList<ulong> idsArr = new(eligibleIdsPtr);
                    foreach (var idPtr in idsArr.Items)
                    {
                        string idName = Memory.ReadUnityString(idPtr);
                        EligibleEntryPoints.Add(idName);
                    }
                }

                string localeID = Memory.ReadUnityString(namePtr);
                Name = LocaleManager.GetItem(localeID);
                if (string.IsNullOrEmpty(Name))
                    Name = "Exfil";
            }
        }

        public void Update(Enums.EExfiltrationStatus status)
        {
            Player LocalPlayer = EFTDMA.LocalPlayer;
            if (LocalPlayer == null) return;

            // Either the infil point or the local profile ID
            string testString;

            if (IsPMCExfil) testString = LocalPlayer.InfilPoint;
            else testString = LocalPlayer.ProfileID;

            if (testString != null && !EligibleEntryPoints.Contains(testString))
            {
                Status = ExfilStatus.Closed;
                return;
            }

            switch (status)
            {
                case Enums.EExfiltrationStatus.NotPresent:
                    Status = ExfilStatus.Closed;
                    break;
                case Enums.EExfiltrationStatus.UncompleteRequirements:
                    Status = ExfilStatus.Pending;
                    break;
                case Enums.EExfiltrationStatus.Countdown:
                    Status = ExfilStatus.Open;
                    break;
                case Enums.EExfiltrationStatus.RegularMode:
                    Status = ExfilStatus.Open;
                    break;
                case Enums.EExfiltrationStatus.Pending:
                    Status = ExfilStatus.Pending;
                    break;
                case Enums.EExfiltrationStatus.AwaitsManualActivation:
                    Status = ExfilStatus.Pending;
                    break;
                default:
                    Status = ExfilStatus.Closed;
                    break;
            }
        }
    }

    public enum ExfilStatus
    {
        Open,
        Pending,
        Closed,
        Transit,
    }
    #endregion
}
