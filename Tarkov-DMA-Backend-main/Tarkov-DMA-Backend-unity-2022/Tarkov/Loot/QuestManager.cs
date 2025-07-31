using System.Collections.Frozen;
using System.Timers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.Unity.Collections;
using Timer = System.Timers.Timer;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public static class QuestManager
    {
        public static List<ActiveQuest> ActiveQuests { get; private set; } = new();
        public static List<QuestRequiredItem> QuestRequiredItems { get; private set; } = new();
        
        private static Timer _timer;

        private const int REFRESH_INTERVAL_MS = 2500;

        #region Static Zones

        private static readonly FrozenDictionary<string, FrozenDictionary<string, Vector3>> QuestZones = new Dictionary<string, FrozenDictionary<string, Vector3>>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "laboratory", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Transits_streets", new Vector3(-169.07996f, 0.91f, -420.87805f) },
                    { "quest_zone_keeper7_test", new Vector3(-131.53511f, 1.0027318f, -357.84436f) },
                    { "Mark_transits_labs", new Vector3(-169.22f, 0.75f, -420.87805f) },
                    { "quest_zone_keeper7_saferoom", new Vector3(-182.86711f, 1.2907319f, -312.87537f) },
                    { "Control_room", new Vector3(-170.7431f, 4.75f, -283.03f) },
                    { "Server_room", new Vector3(-125.55f, 2.54f, -305.09097f) },
                    { "Dome", new Vector3(-172.2812f, 1.01f, -371.2509f) },
                    { "peace_027_area", new Vector3(-131.94911f, 5.4087315f, -336.86536f) },
                    { "meh_56_wifi_area_mark_1", new Vector3(-129.0491f, 1.5987315f, -358.50937f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "Sandbox", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Sandbox_2_AGS_exploration", new Vector3(101.043f, 34.16f, 276.36f) },
                    { "Sandbox_2_Kord_exploration", new Vector3(56.603f, 32.336f, 130.605f) },
                    { "quest_terminal_camera_3", new Vector3(-16.4086f, 30.97f, 71.983f) },
                    { "Mark_transits_sandbox", new Vector3(222.82912f, 15.993f, 65.72f) },
                    { "quest_terminal_camera_1", new Vector3(-35.01f, 26.234f, -17.424f) },
                    { "Sandbox_3_Vino_exploration", new Vector3(117.06f, 24.228f, 26.86f) },
                    { "Sandbox_mine_quest", new Vector3(60.354362f, 23.741716f, 170.88956f) },
                    { "Prospect_mira", new Vector3(172.43f, 17.31f, 64.7f) },
                    { "Sandbox_5_Office_exploration", new Vector3(-13.48f, 31.01f, 51.33f) },
                    { "Sandbox_5_DeadGroup_exploration", new Vector3(-21.01f, 24.228f, -25.1f) },
                    { "Sandbox_5_Laborant_exploration", new Vector3(-60.45f, 25.62f, 27.42f) },
                    { "Sandbox_1_MedicalArea_exploration", new Vector3(156.2f, 25.52f, -83.59f) },
                    { "quest_terminal_camera_4", new Vector3(-4.807f, 30.943f, 57.66f) },
                    { "quest_terminal_kill_zone", new Vector3(-13.48f, 31.01f, 28.27f) },
                    { "Transits_to_streets", new Vector3(222.82912f, 15.993f, 65.72f) },
                    { "quest_terminal_camera_2", new Vector3(-35.426f, 28.599f, 18.857f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "TarkovStreets", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Bank_points_15", new Vector3(8.286f, -2.76f, 5.15f) },
                    { "Check_primorsky", new Vector3(12.4969635f, 2.1215024f, 397.74286f) },
                    { "quest_zone_c11_gmed", new Vector3(92.96399f, 3.587f, 320.38403f) },
                    { "quest_flyer3", new Vector3(-169.84f, 15.41f, 407.59f) },
                    { "quest_zone_find_2st_mech", new Vector3(188.391f, 0.7589998f, 226.506f) },
                    { "Pharma_1", new Vector3(39.054f, 3.655f, 162.4094f) },
                    { "Bank_points_2", new Vector3(83.017f, 1.222f, -30.500004f) },
                    { "quest_zone_kill_cinema", new Vector3(-180.39996f, 16.5f, 400.7f) },
                    { "Bank_points_14", new Vector3(0.446f, -2.7387998f, -18.789f) },
                    { "Bank_points_4", new Vector3(-58.149f, 3.654f, 453.87f) },
                    { "quest_zone_c5_mar", new Vector3(47.625004f, 12.658f, 153.123f) },
                    { "Bank_points_12", new Vector3(-166.477f, 1.625f, -3.377f) },
                    { "quest_zone_c8_dom2", new Vector3(107.935f, 7.569f, 232.193f) },
                    { "Office_quest", new Vector3(164.247f, 1.722f, 167.288f) },
                    { "Bank_points_7", new Vector3(-80.367f, 1.583f, -64.723f) },
                    { "quest_produkt3", new Vector3(-77.17f, 2.33f, 158.91f) },
                    { "quest_zone_kill_stilo", new Vector3(-102.90001f, 8.6f, -25.59999f) },
                    { "quest_zone_keeper8_2", new Vector3(36.983f, 11.075001f, 88.167f) },
                    { "quest_zone_place_c22_harley_3", new Vector3(99.46399f, 3.204f, 356.224f) },
                    { "quest_zone_c7_mel", new Vector3(42.982f, 11.753399f, 151.462f) },
                    { "quest_zone_kill_c17_adm", new Vector3(-69.2f, 12.9f, 115.8f) },
                    { "quest_zone_place_c24_tigr2", new Vector3(172.48f, 4.79f, 416.94f) },
                    { "quest_zone_keeper8_1_hide3", new Vector3(36.688f, 10.583f, 85.519f) },
                    { "quest_zone_keeper00", new Vector3(-34.333004f, 4.26f, 239.87f) },
                    { "Hookah_quest", new Vector3(-211.759f, 4.199f, 297.287f) },
                    { "quest_zone_keeper10_place", new Vector3(10.4939995f, 4.752f, 217.564f) },
                    { "Bank_points_13", new Vector3(-53.575f, 1.579f, -69.353f) },
                    { "Bank_points_10", new Vector3(-35.198f, 1.575f, -69.976f) },
                    { "quest_zone_find_2st_med_invent1", new Vector3(224.87f, 4.818f, 172.806f) },
                    { "quest_zone_c27_sect", new Vector3(-130.801f, 9.489f, 269.741f) },
                    { "quest_zone_c21_look", new Vector3(-67.752f, 5.947f, 65.883f) },
                    { "Mark_Transits_streets(Interchange)", new Vector3(293.39f, 4.47f, 501.02f) },
                    { "quest_zone_place_c14_revx_1", new Vector3(-139.43f, 3.5570002f, 438.81f) },
                    { "quest_city_trotil2", new Vector3(68.257f, 3.044f, 309.979f) },
                    { "quest_produkt2", new Vector3(153.686f, 4.68f, 299.044f) },
                    { "quest_zone_hide_2st_mech", new Vector3(188.591f, 0.3739996f, 226.37898f) },
                    { "quest_zone_find_2st_kpss2", new Vector3(246.254f, -0.32800007f, 52.416f) },
                    { "Bank_points_8", new Vector3(-160.188f, 1.6f, -47.807f) },
                    { "quest_zone_find_2st_med_invent2", new Vector3(182.993f, 2.749f, 101.998f) },
                    { "quest_weap_race", new Vector3(62.4f, 5.2f, 289.9f) },
                    { "quest_zone_hide_barber2", new Vector3(214.295f, 4.254f, 409.255f) },
                    { "quest_zone_c8_dom2_dead", new Vector3(106.772f, 4.653f, 222.482f) },
                    { "Bank_points_9", new Vector3(-148.57f, 6.081f, -62.751f) },
                    { "quest_zone_c6_kpss", new Vector3(131.432f, 7.587f, 232.371f) },
                    { "quest_zone_hide_barber1", new Vector3(206.61302f, 4.33f, 411.782f) },
                    { "quest_zone_c16_koll_2", new Vector3(62.179996f, 7.11f, 272.84f) },
                    { "quest_zone_place_c14_revx_3", new Vector3(2.1899986f, 0.8999996f, 34.04f) },
                    { "Bank_points_6", new Vector3(279.032f, 4.334f, 369.676f) },
                    { "quest_produkt1", new Vector3(-204.31001f, 4.3f, 181.22f) },
                    { "Bank_points_11", new Vector3(-113.874f, 1.609f, -12.51f) },
                    { "quest_zone_c8_dom1", new Vector3(127.09f, 10.636f, 232.57f) },
                    { "Bank_points_5", new Vector3(54.476f, 3.799f, 234.936f) },
                    { "quest_zone_kill_kardinal", new Vector3(134f, 23f, -101.09999f) },
                    { "Bank_points_3", new Vector3(-50.893f, 3.654f, 453.87f) },
                    { "quest_zone_c16_koll_1", new Vector3(15.649998f, 3.3900003f, 226.62f) },
                    { "Office_enter", new Vector3(164.36f, 1.722f, 148.11f) },
                    { "Pharma_3", new Vector3(89.26f, 5.07f, -275.352f) },
                    { "quest_zone_keeper8_1", new Vector3(214.79f, 10.32f, 398.14f) },
                    { "quest_zone_place_c14_revx_2", new Vector3(146.31f, 4.7f, 408.40503f) },
                    { "quest_zone_c29_debt", new Vector3(-80.26001f, 5.98f, 50.5f) },
                    { "quest_zone_keeper8_1_hide2", new Vector3(37.979f, 12.154f, 156.245f) },
                    { "quest_zone_keeper8_1_hide1", new Vector3(215.752f, 9.794f, 395.903f) },
                    { "quest_zone_c25_cinem2", new Vector3(-204.667f, 3.264f, 242.994f) },
                    { "Mark_Transits_streets(Labs)", new Vector3(206.863f, -8.01f, 82.194f) },
                    { "quest_zone_c25_cinem", new Vector3(157.362f, 5.098f, 314.252f) },
                    { "quest_zone_keeper99", new Vector3(-130.397f, 3.2010002f, 396.47302f) },
                    { "quest_zone_find_sillent", new Vector3(184.423f, 6.759f, 55.58f) },
                    { "fond_quest", new Vector3(-58.32f, 3.58f, 457.86f) },
                    { "quest_zone_kill_shool", new Vector3(208f, 10.7f, 135.4f) },
                    { "Pharma_2", new Vector3(-44.62f, 4.189f, 336.07f) },
                    { "quest_zone_hide_sillent2", new Vector3(176.304f, 6.293f, 82.239f) },
                    { "Bank_points_16", new Vector3(236.969f, -1.127f, -93.822f) },
                    { "Bank_points_1", new Vector3(97.422f, 1.232f, -22.249f) },
                    { "quest_city_trotil1", new Vector3(-259.891f, 2.97f, 136.968f) },
                    { "quest_produkt4", new Vector3(275.64697f, 4.303f, 380.83405f) },
                    { "quest_zone_place_c24_tigr1", new Vector3(-69.39f, 3.52f, 358.46002f) },
                    { "Labs_transits", new Vector3(216.70001f, -5.42f, 82.194f) },
                    { "quest_zone_hide_sillent", new Vector3(183.929f, 6.093f, 53.346f) },
                    { "Check_cinema", new Vector3(-142.79999f, 2.1215024f, 397.74286f) },
                    { "quest_zone_keeper10_kill", new Vector3(16.8f, 5.2f, 236f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "Interchange", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "place_SALE_03_KOSTIN", new Vector3(-24.67f, 28.62f, -103.52f) },
                    { "place_WARBLOOD_04_2", new Vector3(274.29f, 22.85f, 14.53f) },
                    { "place_merch_022_5", new Vector3(109.16f, 33.14955f, -54.559998f) },
                    { "place_merch_022_6", new Vector3(-189.29999f, 33.14955f, -54.559998f) },
                    { "place_SALE_03_DINO", new Vector3(94.39f, 28.16f, -120.64f) },
                    { "place_WARBLOOD_04_1", new Vector3(-208.36f, 22.84f, -74.42f) },
                    { "place_merch_022_3", new Vector3(-40.70999f, 33.14955f, -54.559998f) },
                    { "place_skier_11_3", new Vector3(8.095032f, 27.553f, -36.702026f) },
                    { "place_merch_022_1", new Vector3(-19.359985f, 33.14955f, -239.89789f) },
                    { "place_merch_21_1", new Vector3(109.86798f, 22.33f, 288.768f) },
                    { "q_ny_kill_christmas_guys_int", new Vector3(255.23001f, 21.366001f, -58.97f) },
                    { "place_merch_022_4", new Vector3(103.49002f, 33.14955f, -54.559998f) },
                    { "place_merch_21_2", new Vector3(17.557037f, 22.834f, -25.496002f) },
                    { "place_SALE_03_TOPBRAND", new Vector3(94.2f, 28.2f, 10.22f) },
                    { "place_merch_022_2", new Vector3(-19.359985f, 33.14955f, 129.34f) },
                    { "place_SALE_03_AVOKADO", new Vector3(-33.16f, 28.631f, 25.81f) },
                    { "q_ny_hide_christmas_tree_int", new Vector3(255.85f, 21.366001f, -59.01001f) },
                    { "quest_zone_keeper6_safe_hide", new Vector3(-49.320984f, 21.80355f, 45.028f) },
                    { "place_merch_020_1", new Vector3(-50.388f, 27.695f, 25.076004f) },
                    { "place_merch_020_2", new Vector3(179.85562f, 22.314207f, -40.89569f) },
                    { "quest_zone_keeper6_kiba_kill", new Vector3(-9.443756f, 28.241f, -33.70941f) },
                    { "place_SALE_03_TREND", new Vector3(64.17f, 28.07f, -155.4f) },
                    { "place_WARBLOOD_04_3", new Vector3(-172.62f, 22.807f, -359.48f) },
                    { "q_ny_find_christmas_tree_int", new Vector3(255.85f, 21.366001f, -59.01001f) },
                    { "place_skier_12_1", new Vector3(-27.234375f, 28.105f, 2.3956604f) },
                    { "place_merch_21_3", new Vector3(-49.76779f, 22.854f, 59.03575f) },
                    { "place_merch_022_7", new Vector3(92.27002f, 33.14955f, -265.76f) },
                    { "quest_zone_keeper6_kiba_hide", new Vector3(-19.50299f, 27.57155f, -22.401001f) },
                    { "quest_zone_keeper6_cont_hide", new Vector3(138.84702f, 25.06355f, 274.358f) },
                    { "Mark_transits_interchange", new Vector3(261.52f, 23.288347f, 402.45f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "bigmap", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "vaz_feld", new Vector3(579.231f, 0.12918091f, -1.8709993f) },
                    { "gazel", new Vector3(480.77383f, 3.292181f, -76.6871f) },
                    { "quest_terminal_find_zone", new Vector3(500.44f, 15.75f, 107.56f) },
                    { "arena_champ_room", new Vector3(179.93799f, 3.9810002f, 164.798f) },
                    { "fuel4", new Vector3(-334.93317f, 2.220181f, -163.46011f) },
                    { "place_peacemaker_002_N1", new Vector3(505.02997f, 1.24f, -11.290001f) },
                    { "place_skier_12_2", new Vector3(170.795f, 6.885f, 172f) },
                    { "place_flyers1", new Vector3(203.194f, 1.9820001f, -116.576004f) },
                    { "Q019_1", new Vector3(180.981f, 6.3240004f, 162.104f) },
                    { "mech_41_1", new Vector3(170.51999f, 2.3100002f, -107.06f) },
                    { "Q019_2", new Vector3(180.981f, 6.3240004f, 162.104f) },
                    { "room214", new Vector3(176.42181f, 3.330181f, 184.24689f) },
                    { "arena_champ_corpse", new Vector3(182.76999f, 3.4139998f, 163.85f) },
                    { "vremyan_case", new Vector3(368.89383f, 1.6001809f, -49.8851f) },
                    { "Check_mine_zone_custom", new Vector3(171.12338f, 0.77f, 159.91798f) },
                    { "mech_41_2", new Vector3(424.96198f, 2.3100002f, 28.406998f) },
                    { "extraction_zone_zibbo", new Vector3(-193.0472f, 1.9431808f, -206.8981f) },
                    { "fuel1", new Vector3(429.8568f, 2.7361808f, 16.229898f) },
                    { "dead_posylni", new Vector3(27.491812f, 1.493181f, -110.1951f) },
                    { "q_ny_kill_christmas_guys_cust", new Vector3(202.94f, 1.1800001f, 148.62f) },
                    { "fuel2", new Vector3(101.44681f, 3.060181f, -14.1001005f) },
                    { "place_peacemaker_002_N2", new Vector3(565.76f, -0.00999999f, -24.169998f) },
                    { "Q019_3", new Vector3(180.981f, 6.3240004f, 162.104f) },
                    { "room114", new Vector3(235.47781f, 0.42918086f, 160.07489f) },
                    { "prapor_27_1", new Vector3(202.68f, 7.91f, -126.770004f) },
                    { "quest_terminal_final_zone", new Vector3(198.00067f, 8.360001f, 171.59277f) },
                    { "quest_terminal_bitcoin", new Vector3(500.63574f, 17.861f, 91.871445f) },
                    { "q_ny_hide_christmas_tree_cust", new Vector3(198.97f, -0.79255605f, 150.56f) },
                    { "bomj_place", new Vector3(487.88782f, 6.1051807f, -139.84511f) },
                    { "huntsman_020", new Vector3(201.18f, 5.3100004f, 154.53f) },
                    { "place_2A2_unlock_3_customs", new Vector3(11.712997f, -0.719f, 35.593998f) },
                    { "place_peacemaker_007_N1", new Vector3(184.138f, 7.245f, 183.42f) },
                    { "q_ny_find_christmas_tree_cust", new Vector3(198.97f, -0.79255605f, 150.56f) },
                    { "Mark_transits_custom", new Vector3(354.59998f, 2.3100002f, -192.92001f) },
                    { "fuel3", new Vector3(334.95682f, 3.040181f, -189.97011f) },
                    { "place_SADOVOD_03", new Vector3(-212.56999f, 1.1620009f, -109.05f) },
                    { "place_peacemaker_002_N3", new Vector3(17.86f, 0.3499999f, 0.3600006f) },
                    { "room206_water", new Vector3(232.2398f, 3.3901808f, 138.87689f) },
                    { "place_skier_11_2", new Vector3(24.391006f, 0.18600011f, 102.6f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "factory4_day", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "catwalk01", new Vector3(5.9728127f, 8.088181f, 60.9629f) },
                    { "roof02", new Vector3(45.152813f, 5.801181f, 29.320898f) },
                    { "unitaz", new Vector3(22.288813f, 4.777281f, 48.172905f) },
                    { "catwalk03", new Vector3(6.5598106f, 7.756181f, -0.7180996f) },
                    { "locked_office", new Vector3(30.131813f, 8.718182f, 38.8869f) },
                    { "dungeon05", new Vector3(-6.9211884f, -2.109819f, 23.6659f) },
                    { "caseextr", new Vector3(41.741814f, 5.448181f, 40.8179f) },
                    { "showers", new Vector3(14.923813f, 5.213181f, 40.4069f) },
                    { "dungeon03", new Vector3(65.183815f, -2.173819f, -34.0851f) },
                    { "exit0zone", new Vector3(-55.976185f, 1.806181f, 55.983902f) },
                    { "office01", new Vector3(17.193811f, 8.718182f, 38.6419f) },
                    { "exit1zone", new Vector3(-38.528187f, 0.71418095f, 7.2929f) },
                    { "catwalk06", new Vector3(51.686813f, 7.8241806f, 40.143898f) },
                    { "roof01", new Vector3(42.05781f, 8.075181f, 40.6789f) },
                    { "dungeon02", new Vector3(38.935814f, -2.173819f, -34.1231f) },
                    { "catwalk04", new Vector3(37.507812f, 7.804181f, 0.30789948f) },
                    { "catwalk05", new Vector3(51.686813f, 7.804181f, 0.30789948f) },
                    { "secretpassage", new Vector3(66.59781f, -2.1328192f, -28.462101f) },
                    { "dungeon07", new Vector3(-31.471188f, -2.109819f, 27.583899f) },
                    { "pumproom", new Vector3(40.40681f, 0.63318086f, -11.2001f) },
                    { "dungeon06", new Vector3(-17.000187f, -2.109819f, 39.0409f) },
                    { "dungeon04", new Vector3(21.001812f, -2.109819f, -1.1620998f) },
                    { "exit3zone", new Vector3(58.866814f, 0.71418095f, 57.384903f) },
                    { "dungeon01", new Vector3(17.510813f, -2.109819f, -21.3031f) },
                    { "exit2zone", new Vector3(44.93281f, 0.71418095f, -45.0941f) },
                    { "catwalk02", new Vector3(-22.420185f, 8.088181f, 1.6329002f) },
                    { "case_extraction", new Vector3(41.741814f, 5.448181f, 40.8179f) },
                    { "nf2024_10_3", new Vector3(-4.36f, 4.747f, 6.08f) },
                    { "Place_accurate_tools", new Vector3(27.89f, 1.15f, -32.889915f) },
                    { "nf2024_2_1", new Vector3(62.68f, 1.043f, 23.4f) },
                    { "nf2024_3_exit", new Vector3(23.53f, 2.259f, 63.2f) },
                    { "achiv_office", new Vector3(16.509f, 9.106f, 39.299f) },
                    { "nf2024_6_beacon_1", new Vector3(-16.994f, -1.596f, 34.808f) },
                    { "nf2024_4_zone_kill1", new Vector3(19.81f, -2.56f, 46.84f) },
                    { "place_pacemaker_SCOUT_02", new Vector3(-53.107f, 2.292f, 58.188f) },
                    { "huntsman_013", new Vector3(22.34f, 6.1f, 41.73f) },
                    { "nf2024_7_2", new Vector3(-5.474f, 5.776f, -32.477f) },
                    { "nf2024_2_4", new Vector3(-31.57f, 2.33f, 57.84f) },
                    { "nf2024_2_11", new Vector3(18.3f, 1.49f, -15.19f) },
                    { "nf2024_10_1", new Vector3(17.81f, 4.747f, 6.08f) },
                    { "nf2024_6_beacon_2", new Vector3(34.428f, -1.476f, -4.445f) },
                    { "place_pacemaker_SCOUT_03", new Vector3(59.698f, 1.163f, 56.98f) },
                    { "nf2024_2_9", new Vector3(13.63f, 1.163f, -30.07f) },
                    { "place_pacemaker_SCOUT_01", new Vector3(66.665f, -1.676f, -28.878f) },
                    { "place_SADOVOD_01_2", new Vector3(60.076f, 1.093f, -48.389f) },
                    { "nf2024_2_3", new Vector3(64.97f, 1.163f, -47.28f) },
                    { "nf2024_2_2", new Vector3(71.02f, 1.163f, -40.93f) },
                    { "nf2024_2_12", new Vector3(58.323f, 1.73f, 58.976f) },
                    { "kill1", new Vector3(22.34f, 6.1f, 41.73f) },
                    { "Mark_transits_factory", new Vector3(24.202f, 1.91f, 66.63533f) },
                    { "nf2024_2_5", new Vector3(22.58f, 1.163f, -23.83f) },
                    { "Check_mine_zone_factory", new Vector3(25.35f, 8.76f, 39.25f) },
                    { "nf2024_2_7", new Vector3(12.37f, 1.163f, 30.42f) },
                    { "nf2024_10_2", new Vector3(6.756f, 4.747f, 6.08f) },
                    { "place_pacemaker_SCOUT_04", new Vector3(-18.4f, 1.1613333f, -49.022f) },
                    { "place_SADOVOD_01_1", new Vector3(68.193f, 1.059f, -7.383f) },
                    { "nf2024_2_8", new Vector3(-15.77f, 2.259f, 32.58f) },
                    { "nf2024_7_3", new Vector3(-24.441f, 6.197f, 60.426f) },
                    { "nf2024_7_1", new Vector3(63.612f, 6.44f, 35.114f) },
                    { "nf2024_2_6", new Vector3(21.868f, 1.163f, -30.777f) },
                    { "nf2024_2_10", new Vector3(53.37f, 1.48f, -22.46f) },
                    { "nf2024_6_beacon_3", new Vector3(45.615f, -1.596f, -36.126f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "factory4_night", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "catwalk01", new Vector3(5.9728127f, 8.088181f, 60.9629f) },
                    { "roof02", new Vector3(45.152813f, 5.801181f, 29.320898f) },
                    { "unitaz", new Vector3(22.288813f, 4.777281f, 48.172905f) },
                    { "catwalk03", new Vector3(6.5598106f, 7.756181f, -0.7180996f) },
                    { "locked_office", new Vector3(30.131813f, 8.718182f, 38.8869f) },
                    { "dungeon05", new Vector3(-6.9211884f, -2.109819f, 23.6659f) },
                    { "caseextr", new Vector3(41.741814f, 5.448181f, 40.8179f) },
                    { "showers", new Vector3(14.923813f, 5.213181f, 40.4069f) },
                    { "dungeon03", new Vector3(65.183815f, -2.173819f, -34.0851f) },
                    { "exit0zone", new Vector3(-55.976185f, 1.806181f, 55.983902f) },
                    { "office01", new Vector3(17.193811f, 8.718182f, 38.6419f) },
                    { "exit1zone", new Vector3(-38.528187f, 0.71418095f, 7.2929f) },
                    { "catwalk06", new Vector3(51.686813f, 7.8241806f, 40.143898f) },
                    { "roof01", new Vector3(42.05781f, 8.075181f, 40.6789f) },
                    { "dungeon02", new Vector3(38.935814f, -2.173819f, -34.1231f) },
                    { "catwalk04", new Vector3(37.507812f, 7.804181f, 0.30789948f) },
                    { "catwalk05", new Vector3(51.686813f, 7.804181f, 0.30789948f) },
                    { "secretpassage", new Vector3(66.59781f, -2.1328192f, -28.462101f) },
                    { "dungeon07", new Vector3(-31.471188f, -2.109819f, 27.583899f) },
                    { "pumproom", new Vector3(40.40681f, 0.63318086f, -11.2001f) },
                    { "dungeon06", new Vector3(-17.000187f, -2.109819f, 39.0409f) },
                    { "dungeon04", new Vector3(21.001812f, -2.109819f, -1.1620998f) },
                    { "exit3zone", new Vector3(58.866814f, 0.71418095f, 57.384903f) },
                    { "dungeon01", new Vector3(17.510813f, -2.109819f, -21.3031f) },
                    { "catwalk02", new Vector3(-22.420185f, 8.088181f, 1.6329002f) },
                    { "case_extraction", new Vector3(41.741814f, 5.448181f, 40.8179f) },
                    { "nf2024_10_3", new Vector3(-4.36f, 4.747f, 6.08f) },
                    { "kill1", new Vector3(22.34f, 9.58f, 39.22f) },
                    { "Place_accurate_tools", new Vector3(27.89f, 1.15f, -32.889915f) },
                    { "nf2024_2_1", new Vector3(62.68f, 1.043f, 23.4f) },
                    { "nf2024_3_exit", new Vector3(23.53f, 2.259f, 63.2f) },
                    { "achiv_office", new Vector3(16.509f, 9.106f, 39.299f) },
                    { "nf2024_6_beacon_1", new Vector3(-16.994f, -1.596f, 34.808f) },
                    { "nf2024_4_zone_kill1", new Vector3(19.81f, -2.56f, 46.84f) },
                    { "place_pacemaker_SCOUT_02", new Vector3(-53.107f, 2.292f, 58.188f) },
                    { "nf2024_7_2", new Vector3(-5.474f, 5.776f, -32.477f) },
                    { "nf2024_2_4", new Vector3(-31.57f, 2.33f, 57.84f) },
                    { "ter_017_area_1", new Vector3(24.609f, 9.17f, 39.078f) },
                    { "nf2024_2_11", new Vector3(18.3f, 1.49f, -15.19f) },
                    { "nf2024_10_1", new Vector3(17.81f, 4.747f, 6.08f) },
                    { "nf2024_6_beacon_2", new Vector3(34.428f, -1.476f, -4.445f) },
                    { "place_pacemaker_SCOUT_03", new Vector3(59.698f, 1.163f, 56.98f) },
                    { "nf2024_2_9", new Vector3(13.63f, 1.163f, -30.07f) },
                    { "place_pacemaker_SCOUT_01", new Vector3(66.665f, -1.676f, -28.878f) },
                    { "place_SADOVOD_01_2", new Vector3(60.076f, 1.093f, -48.389f) },
                    { "nf2024_2_3", new Vector3(64.97f, 1.163f, -47.28f) },
                    { "huntsman_013", new Vector3(22.34f, 9.58f, 39.22f) },
                    { "nf2024_2_2", new Vector3(71.02f, 1.163f, -40.93f) },
                    { "nf2024_2_12", new Vector3(58.323f, 1.73f, 58.976f) },
                    { "Mark_transits_factory", new Vector3(24.202f, 1.91f, 66.63533f) },
                    { "nf2024_2_5", new Vector3(22.58f, 1.163f, -23.83f) },
                    { "Check_mine_zone_factory", new Vector3(25.35f, 8.76f, 39.25f) },
                    { "nf2024_2_7", new Vector3(12.37f, 1.163f, 30.42f) },
                    { "nf2024_10_2", new Vector3(6.756f, 4.747f, 6.08f) },
                    { "place_pacemaker_SCOUT_04", new Vector3(-18.4f, 1.1613333f, -49.022f) },
                    { "place_SADOVOD_01_1", new Vector3(68.193f, 1.059f, -7.383f) },
                    { "nf2024_2_8", new Vector3(-15.77f, 2.259f, 32.58f) },
                    { "nf2024_7_3", new Vector3(-24.441f, 6.197f, 60.426f) },
                    { "nf2024_7_1", new Vector3(63.612f, 6.44f, 35.114f) },
                    { "nf2024_2_6", new Vector3(21.868f, 1.163f, -30.777f) },
                    { "nf2024_2_10", new Vector3(53.37f, 1.48f, -22.46f) },
                    { "nf2024_6_beacon_3", new Vector3(45.615f, -1.596f, -36.126f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "Woods", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "place_THX_15", new Vector3(23.794006f, -15.413f, 99.84198f) },
                    { "achiv_plane", new Vector3(-252.79999f, 14.1f, -30.200012f) },
                    { "place_keeper5_1", new Vector3(289.96704f, 23.515f, -439.01196f) },
                    { "NosQuests_8_wood_place", new Vector3(-219.22699f, 64.622f, -224.43396f) },
                    { "meh_45_radio_area_mark_1", new Vector3(222.16003f, 21.382f, -706.6f) },
                    { "huntsman_001", new Vector3(-256f, 9.58f, 9.700012f) },
                    { "quest_terminal_ zone_3", new Vector3(-174.23804f, 52.084f, -232.50696f) },
                    { "q14_8_5", new Vector3(-447.077f, 2.061f, 249f) },
                    { "meh_45_radio_area_mark_2", new Vector3(-156.091f, 52.27f, -273.31897f) },
                    { "bar_fuel3_2", new Vector3(-325.042f, 15.319f, 23.984985f) },
                    { "place_peacemaker_007_2_N2_1", new Vector3(-88.70001f, 12.55f, -717.61f) },
                    { "q14_8_3", new Vector3(-97.25f, 13.993f, -542.05005f) },
                    { "quest_terminal_ zone_1", new Vector3(-234.83002f, 68.563f, -228.06897f) },
                    { "place_keeper5_2", new Vector3(292.484f, 23.353f, -507.38306f) },
                    { "NosQuests_5_Flaer_off", new Vector3(-529.6f, 18.2f, -185.09998f) },
                    { "place_skier_11_1", new Vector3(31.086792f, -13.612729f, 83.011536f) },
                    { "meh_45_radio_area_mark_4", new Vector3(446.33704f, -13.139f, 69.18402f) },
                    { "bar_fuel3_1", new Vector3(356.44995f, 0.35f, -85.880005f) },
                    { "q14_8_1", new Vector3(412.20898f, 13.708f, -608.531f) },
                    { "q14_8_2", new Vector3(-209.22198f, 77.521f, -279.781f) },
                    { "q_ny_kill_christmas_guys_wood", new Vector3(-21.890991f, -14.29f, 162.97198f) },
                    { "meh_45_radio_area_mark_3", new Vector3(-157.60101f, 47.173f, -233.20203f) },
                    { "bar_fuel3_3", new Vector3(-200.40698f, 31.302f, -210.92401f) },
                    { "place_skier_12_3", new Vector3(-6.2143555f, -0.49247456f, -75.55499f) },
                    { "place_peacemaker_007_2_N2", new Vector3(195.97705f, 1.032f, -7.2420044f) },
                    { "pr_scout_col", new Vector3(195.56006f, 12.21f, -595.81995f) },
                    { "huntsman_005_1", new Vector3(-386.786f, 4.224f, 22.593018f) },
                    { "pr_scout_base", new Vector3(283.05005f, 24.04f, -439.39f) },
                    { "prapor_27_2", new Vector3(-189.09998f, 0.3f, 232.20001f) },
                    { "ter_015_area_1", new Vector3(-97.150024f, -13.19f, 219.76001f) },
                    { "NosQuests_11_wood_place", new Vector3(-219.74799f, 62.621f, -219.97498f) },
                    { "q_ny_hide_christmas_tree_wood", new Vector3(-22f, -15.74f, 163.04999f) },
                    { "quest_zone_keeper5", new Vector3(-184.29999f, 45.7f, -255.90002f) },
                    { "q_ny_find_christmas_tree_wood", new Vector3(-21.390015f, -15.74f, 163.23999f) },
                    { "quest_terminal_ zone_2", new Vector3(-202.26599f, 30.326f, -210.42102f) },
                    { "q14_8_4", new Vector3(247.27002f, -9.067f, 291.49f) },
                    { "huntsman_005_2", new Vector3(447.979f, -13.19f, 71.301025f) },
                    { "Woods_mine_quest", new Vector3(341.59998f, 22.907f, -492.30005f) },
                    { "place_2A2_unlock_3_woods", new Vector3(227.69897f, 21.056f, -706.56006f) },
                    { "Mark_transits_woods", new Vector3(-132.35999f, 0.62f, 421.5f) },
                    { "Place_item_tools_woods", new Vector3(-119.04498f, -0.93f, 400.652f) },
                    { "nf2024_1", new Vector3(-510.893f, 17.147f, -178.23499f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "RezervBase", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "tadeush_stryker_area_check_4", new Vector3(-107.222275f, -5.6922226f, -67.3433f) },
                    { "baraholshik_arsenal_area_5", new Vector3(-125.51811f, -7.0462265f, 95.67992f) },
                    { "fuel4", new Vector3(-334.93317f, -101.455826f, -163.46011f) },
                    { "baraholshik_arsenal_area_1", new Vector3(-167.37611f, -1.2852249f, 31.355911f) },
                    { "tadeush_tunguska_area_check_9", new Vector3(-264.63712f, -8.649223f, -71.41809f) },
                    { "mechanik_exit_area_1", new Vector3(-119.75011f, -29.311226f, 171.7919f) },
                    { "tadeush_bmp2_area_mark_12", new Vector3(101.727905f, -5.592224f, 64.444916f) },
                    { "tadeush_stryker_area_mark_3", new Vector3(42.12439f, 0.8737793f, -195.23909f) },
                    { "prapor_024_area_2", new Vector3(-115.030106f, -11.859222f, 33.881912f) },
                    { "tadeush_bmp2_area_mark_13", new Vector3(53.619904f, -3.997223f, 122.661896f) },
                    { "prapor_025_area_1", new Vector3(-90.30911f, -13.41922f, -5.1480865f) },
                    { "baraholshik_arsenal_area_4", new Vector3(-125.5401f, -1.2852249f, 95.681915f) },
                    { "baraholshik_fuel_area_2", new Vector3(11.94989f, -9.889221f, 73.80191f) },
                    { "tadeush_bmp2_area_mark_2", new Vector3(-100.58011f, -3.719223f, -155.87808f) },
                    { "place_flaers2", new Vector3(118.98291f, -4.386223f, -144.5591f) },
                    { "tadeush_tunguska_area_check_5", new Vector3(210.99603f, -4.9422226f, -86.309555f) },
                    { "lijnik_storage_area_1", new Vector3(65.38989f, -20.979225f, -104.64809f) },
                    { "baraholshik_dejurniy_area_2", new Vector3(-164.96411f, -4.221222f, 34.52591f) },
                    { "tadeush_tunguska_area_mark_10", new Vector3(-249.5701f, -3.6492233f, -3.3680878f) },
                    { "baraholshik_arsenal_area_3", new Vector3(-167.37611f, -7.0462265f, 31.355911f) },
                    { "q_ny_hide_christmas_tree_rez", new Vector3(166.90002f, -7.040001f, 0.8500061f) },
                    { "prapor_025_area_3", new Vector3(-149.41011f, -9.329224f, 50.001907f) },
                    { "prapor_hq_area_check_1", new Vector3(-109.6501f, -12.829224f, 37.511917f) },
                    { "quest_zone_keeper3_hide", new Vector3(-114.6591f, -13.614227f, 23.711914f) },
                    { "tadeush_tunguska_area_check_8", new Vector3(144.17188f, -8.649223f, 34.870926f) },
                    { "tadeush_tunguska_area_mark_6", new Vector3(228.90991f, -2.929222f, -55.48809f) },
                    { "prapor_024_area_1", new Vector3(-116.75311f, -13.159225f, 31.686905f) },
                    { "prapor_025_area_5", new Vector3(-73.001114f, -9.653221f, 29.993912f) },
                    { "prapor_025_area_2", new Vector3(-144.2501f, -9.983223f, 6.19191f) },
                    { "q_ny_kill_christmas_guys_rez", new Vector3(166.90002f, -5.589226f, 0.8500061f) },
                    { "tadeush_tunguska_area_check_10", new Vector3(-246.9801f, -3.1592255f, -2.9780884f) },
                    { "baraholshik_fuel_area_1", new Vector3(-9.370117f, -9.459221f, 83.25191f) },
                    { "Mark_transits_rezerve", new Vector3(240.23462f, -6.356224f, -124.415436f) },
                    { "tadeush_tunguska_area_mark_5", new Vector3(211.1499f, -4.3492203f, -86.21809f) },
                    { "baraholshik_fuel_area_4", new Vector3(178.6799f, -3.16922f, -123.64809f) },
                    { "tadeush_bmp2_area_check_12", new Vector3(103.3819f, -4.6292267f, 63.869904f) },
                    { "tadeush_bmp2_area_check_11", new Vector3(79.00989f, -4.6292267f, -31.548088f) },
                    { "tadeush_bmp2_area_check_2", new Vector3(-100.4001f, -4.3192215f, -155.6881f) },
                    { "tadeush_tunguska_area_mark_8", new Vector3(144.14825f, -8.6712265f, 34.93051f) },
                    { "tadeush_tunguska_area_mark_7", new Vector3(183.75009f, -2.929222f, 22.038666f) },
                    { "tadeush_tunguska_area_check_7", new Vector3(184.62988f, -3.6622238f, 21.221909f) },
                    { "eger_barracks_area_1", new Vector3(-102.8701f, -4.139221f, 92.47191f) },
                    { "tadeush_bmp2_area_check_13", new Vector3(53.829895f, -4.139221f, 122.03192f) },
                    { "huntsman_029", new Vector3(78.77f, -12.286003f, -117.1f) },
                    { "tadeush_stryker_area_check_3", new Vector3(42.029907f, 0.28077698f, -195.2381f) },
                    { "baraholshik_fuel_area_3", new Vector3(170.0299f, -9.309227f, -97.85809f) },
                    { "q_ny_find_christmas_tree_rez", new Vector3(166.90002f, -7.040001f, 0.8500061f) },
                    { "tadeush_t90_area_check_1", new Vector3(-160.6201f, -4.3192215f, -117.58809f) },
                    { "tadeush_stryker_area_mark_4", new Vector3(-107.129105f, -5.0992203f, -67.35909f) },
                    { "tadeush_bmp2_area_mark_11", new Vector3(79.02789f, -5.4392242f, -31.787086f) },
                    { "prapor_025_area_4", new Vector3(-107.0101f, -9.388222f, 77.49191f) },
                    { "tadeush_tunguska_area_check_6", new Vector3(228.9599f, -3.6622238f, -55.44809f) },
                    { "Event_terminal_zone", new Vector3(-14.1311035f, 33.881775f, 176.7269f) },
                    { "eger_barracks_area_2", new Vector3(-164.0601f, -4.139221f, 54.131912f) },
                    { "Place_item_tools_rezerve", new Vector3(50.98111f, -0.62496185f, -181.91666f) },
                    { "tadeush_tunguska_area_mark_9", new Vector3(-267.7341f, -9.147224f, -69.50109f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "Lighthouse", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "qlight_extension_mechanik1_hide1", new Vector3(32.548615f, 6.414898f, -640.29034f) },
                    { "meh_42_radio_area_mark_2", new Vector3(-116.48639f, 40.3649f, 88.86865f) },
                    { "qlight_extension_prapor1_utes_exploration5", new Vector3(-7.937393f, 8.604898f, -449.96933f) },
                    { "qlight_find_light_merchant", new Vector3(423.33258f, 22.644897f, 476.89062f) },
                    { "qlight_pc1_ucot_kill", new Vector3(-108.86739f, 22.244898f, -2.3093262f) },
                    { "w_race_cam2", new Vector3(-144.7034f, 5.0018997f, -757.6273f) },
                    { "q_ny_find_christmas_tree_light", new Vector3(114.32262f, 2.8148994f, 268.11072f) },
                    { "w_race_cam3", new Vector3(-180.4074f, 5.1948967f, -630.7793f) },
                    { "place_flyers4", new Vector3(-94.50939f, 0.6609001f, -555.3173f) },
                    { "qlight_find_scav_group1", new Vector3(-70.35739f, 26.779995f, 132.23065f) },
                    { "meh_53_wificamera_area_mark_4", new Vector3(-77.32939f, 12.014898f, -790.0833f) },
                    { "meh_42_radio_area_mark_3", new Vector3(71.11261f, 10.164898f, 392.52063f) },
                    { "quest_zone_place_c22_harley_2", new Vector3(-116.55432f, 18.580898f, -19.449951f) },
                    { "meh_42_radio_area_mark_4", new Vector3(127.57262f, 6.994898f, 293.00073f) },
                    { "qlight_fuel_blood_bezovoz3", new Vector3(29.082611f, 5.9948997f, -423.3593f) },
                    { "w_race_cam1", new Vector3(35.256607f, 5.263897f, -558.83234f) },
                    { "meh_53_wificamera_area_mark_3", new Vector3(83.14261f, 6.1548996f, -557.7993f) },
                    { "Marafon_kill_village_lighthouse", new Vector3(-72.885315f, 5.777012f, -286.24924f) },
                    { "meh_50_visit_area_check_1", new Vector3(442.2326f, 24.114899f, 460.69067f) },
                    { "meh_53_wificamera_area_mark_1", new Vector3(-12.217392f, 7.324898f, -306.1393f) },
                    { "qlight_extension_mechanik1_exploration1", new Vector3(32.548615f, 6.414898f, -640.29034f) },
                    { "qlight_extension_prapor1_utes_exploration8", new Vector3(-130.3274f, 15.337898f, -730.50934f) },
                    { "qlight_pr1_heli1_mark", new Vector3(-96.48739f, 2.374897f, -552.58936f) },
                    { "qlight_extension_prapor1_exploration2", new Vector3(-97.73739f, 15.394897f, -753.8093f) },
                    { "qlight_extension_prapor1_utes_exploration6", new Vector3(29.762604f, 15.284899f, -591.3493f) },
                    { "qlight_mark_vech4", new Vector3(-41.467392f, 10.194899f, 16.490662f) },
                    { "quest_zone_place_c22_harley_1", new Vector3(-140.79639f, 40.305897f, 107.966675f) },
                    { "qlight_extension_prapor1_utes_exploration10", new Vector3(-186.74739f, 15.337898f, -643.4723f) },
                    { "qlight_extension_prapor1_utes_exploration9", new Vector3(37.652603f, 15.284899f, -652.7653f) },
                    { "qlight_extension_medic1_hide1", new Vector3(-119.21739f, 13.014898f, -841.12933f) },
                    { "qlight_extension_prapor1_exploration3", new Vector3(-196.6474f, 15.394897f, -666.8093f) },
                    { "qlight_extension_prapor1_ags_exploration7", new Vector3(-64.88739f, 15.074898f, -729.6093f) },
                    { "Mark_transits_lighthouse", new Vector3(-338.6f, 17.5f, -168.90002f) },
                    { "qlight_mark_vech1", new Vector3(13.136612f, 2.7868996f, -503.2343f) },
                    { "meh_53_wificamera_area_mark_2", new Vector3(-195.2824f, 6.1548996f, -431.1913f) },
                    { "qlight_hunt_fr_find", new Vector3(-246.5874f, 0.9148979f, -328.25934f) },
                    { "qlight_pr1_heli1_find", new Vector3(-95.947395f, 2.4048996f, -553.3493f) },
                    { "qlight_fuel_blood_bezovoz2", new Vector3(-158.4574f, 6.044899f, -608.01935f) },
                    { "q_ny_kill_christmas_guys_light", new Vector3(114.759995f, 5.1548996f, 267.61f) },
                    { "qlight_mark_vech2", new Vector3(-65.23739f, 3.3578987f, -691.4793f) },
                    { "qlight_extension_prapor1_exploration1", new Vector3(53.232605f, 15.394897f, -623.5593f) },
                    { "qlight_extension_prapor1_utes_exploration11", new Vector3(-68.977394f, 15.424898f, -752.93933f) },
                    { "meh_44_eastLight_kill", new Vector3(-29.567398f, 23.344898f, 112.690674f) },
                    { "qlight_mark_vech3", new Vector3(66.08261f, 2.7648964f, 516.5807f) },
                    { "qlight_extension_medic1_exploration1", new Vector3(-119.21739f, 13.014898f, -841.12933f) },
                    { "qlight_extension_bariga1_exploration1", new Vector3(353.1126f, 17.0239f, 547.82166f) },
                    { "qlight_fuel_blood_bezovoz1", new Vector3(-73.70739f, 11.614899f, -875.5493f) },
                    { "qlight16_peace_terra", new Vector3(136.8986f, 0.79089737f, 47.88867f) },
                    { "qlight_fuel_blood", new Vector3(-116.5974f, 11.674898f, -921.8493f) },
                    { "meh_48_transponder_area_check_1", new Vector3(119.3026f, 2.1548996f, 520.7606f) },
                    { "qlight_br_secure_road", new Vector3(0.032608032f, 7.844898f, 47.690674f) },
                    { "qlight_extension_prapor1_ags_exploration4", new Vector3(43.69261f, 10.954899f, -448.54932f) },
                    { "q_ny_hide_christmas_tree_light", new Vector3(114.64259f, 2.8148994f, 268.11072f) },
                    { "meh_42_radio_area_mark_1", new Vector3(-94.61739f, 1.9438972f, -550.68933f) },
                    { "qlight_pr1_heli2_kill", new Vector3(-51.16739f, 14.544899f, -573.20935f) },
                    { "qlight_find_crushed_heli", new Vector3(-126.69739f, 22.434898f, 301.52063f) },
                    { "quest_terminal_cottage", new Vector3(-111.47139f, 21.638899f, -13.342346f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
            {
                "Shoreline", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)
                {
                    { "place_peacemaker_003_N3", new Vector3(-349.68005f, -39.864f, 185.87f) },
                    { "place_peacemaker_004_N1", new Vector3(-553.0862f, -29.35f, -127.67819f) },
                    { "marafon_kill_village_shoreline", new Vector3(377.30005f, -54.42f, 136.29999f) },
                    { "skier_022_area_2", new Vector3(98.10303f, -47.19f, 96.979034f) },
                    { "place_peacemaker_009_3_N1", new Vector3(-149.87402f, -6.08f, -77.004f) },
                    { "place_peacemaker_010_3", new Vector3(72.910034f, -59.673073f, 348.72998f) },
                    { "huntsman_024_2", new Vector3(382.93994f, -51.93f, -125.899994f) },
                    { "huntsman_026", new Vector3(-229.06006f, 3.285f, -88.856995f) },
                    { "NosQuests_11_shoreline_place", new Vector3(-459.72095f, -47.769f, 564.833f) },
                    { "q_ny_hide_christmas_tree_shorl", new Vector3(-251.91003f, -3.97f, -112.46001f) },
                    { "prapor_022_area_2", new Vector3(96.72998f, -47.19f, 97.10004f) },
                    { "q14_10_point_area", new Vector3(-645f, -20.3f, -200.7f) },
                    { "skier_022_area_1", new Vector3(-263.594f, -3.9580002f, -85.71597f) },
                    { "q14_11_jeep", new Vector3(227.16309f, -54.911f, -159.882f) },
                    { "place_meh_sanitar_room", new Vector3(-322.833f, -2.7529984f, -77.53696f) },
                    { "place_SIGNAL_03_3", new Vector3(-708.72f, -26.21f, 90.59f) },
                    { "place_peacemaker_003_N1", new Vector3(-153.07996f, -17.05f, -280.18f) },
                    { "place_SIGNAL_03_2", new Vector3(-230.55005f, 8.32f, -92.39f) },
                    { "prapor_022_area_1", new Vector3(-263.98303f, -3.9580002f, -85.25296f) },
                    { "place_peacemaker_008_2_N1", new Vector3(-250.5f, -5.1409655f, -48.28f) },
                    { "2A2_unlock_4_discover", new Vector3(-200.31006f, -6.15f, -76.36099f) },
                    { "place_peacemaker_007_1_N1", new Vector3(287.0691f, -49.353f, -42.326996f) },
                    { "place_peacemaker_005_N2", new Vector3(-596.26f, -58.53f, 475.53003f) },
                    { "prapor_27_3", new Vector3(-253.40002f, -5.7f, -99.600006f) },
                    { "place_peacemaker_004_N2", new Vector3(81.433716f, -31.8154f, -166.845f) },
                    { "huntsman_024_1", new Vector3(405.93994f, -51.93f, -44.5f) },
                    { "ter_023_area_1_1", new Vector3(-253.17004f, -4.0699997f, -56.859955f) },
                    { "q_ny_kill_christmas_guys_shorl", new Vector3(-251.87f, -3.97f, -112.32001f) },
                    { "place_SIGNAL_01_2", new Vector3(-483.31006f, -25.762f, 253.65997f) },
                    { "place_peacemaker_003_N2", new Vector3(378.74f, -54.375f, 166.85999f) },
                    { "ter_013_area_4", new Vector3(97.660034f, -47.25f, 70.79999f) },
                    { "place_peacemaker_001_N2", new Vector3(-182.04004f, -63.725f, 459.84998f) },
                    { "place_peacemaker_001", new Vector3(-182.04004f, -63.725f, 459.84998f) },
                    { "ter_013_area_1", new Vector3(312.69995f, -59.18f, 314.5f) },
                    { "place_peacemaker_007_2_N3", new Vector3(-336.64404f, 3.545f, -88.728f) },
                    { "place_SIGNAL_03_1", new Vector3(-483.81006f, -24.47f, 248.83002f) },
                    { "place_peacemaker_005_N1", new Vector3(-234.48999f, -3.47f, -164.42f) },
                    { "skier_022_area_3", new Vector3(-305.38403f, -60.86f, 492.20105f) },
                    { "ter_013_area_3", new Vector3(-258.07495f, -3.958f, -85.66f) },
                    { "Terminal_quest_final", new Vector3(-312.9746f, 6.920006f, -82.243805f) },
                    { "place_SIGNAL_01_1", new Vector3(-230.40002f, 8.35f, -93.619995f) },
                    { "place_peacemaker_008_4_N2", new Vector3(-171.16296f, 0.14f, -75.51401f) },
                    { "prapor_022_area_3", new Vector3(-306.19995f, -60.86f, 494.09998f) },
                    { "place_peacemaker_008_2_N2", new Vector3(-280.55005f, -4.78f, -41.309998f) },
                    { "prapor_27_4", new Vector3(-418.30005f, -56.87f, 546.5f) },
                    { "place_peacemaker_009_2", new Vector3(-336.97498f, -2.729f, -88.08501f) },
                    { "ter_013_area_2", new Vector3(354.91992f, -58.63f, 306.12f) },
                    { "q14_10_kill", new Vector3(-591.80005f, -20.3f, -117.7f) },
                    { "q_ny_find_christmas_tree_shorl", new Vector3(-251.91003f, -3.97f, -112.46001f) },
                    { "place_peacemaker_008_4_N1", new Vector3(-336.54504f, 0.145f, -76.42799f) },
                    { "huntsman_024_3", new Vector3(287.1399f, -49.2f, -46.320007f) },
                    { "unkown_mark_2", new Vector3(251.98706f, -52.69f, -105.11865f) },
                    { "ter_023_area_3_1", new Vector3(-311.90002f, -60.9f, 489.70007f) },
                    { "ter_023_area_2_1", new Vector3(94.20996f, -47.5f, 61.97003f) },
                    { "place_peacemaker_010_2", new Vector3(235.56006f, -64.464f, 442.02002f) },
                    { "NosQuests_8_shoreline_place", new Vector3(-459.33997f, -47.6f, 566.965f) },
                }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)
            },
        }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        #endregion

        public static void Initialize()
        {
            _timer = new(REFRESH_INTERVAL_MS);
            _timer.Elapsed += Refresh;
            _timer.Start();
        }

        private static void Refresh(object sender, ElapsedEventArgs e)
        {
            try
            {
                Player localPlayer = EFTDMA.LocalPlayer;
                if (localPlayer == null)
                    return;

                RefreshInternal(localPlayer);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[QUEST MANAGER] -> Refresh(): Failed to refresh ~ {ex}");
            }
        }

        private readonly struct QuestCandidate(ulong address, string id, string className)
        {
            public readonly ulong Address = address;
            public readonly string ID = id;
            public readonly string ClassName = className;
        }

        public readonly struct ActiveQuest(string name, string description, Vector3 position)
        {
            public readonly string Name = name;
            public readonly string Description = description;
            public readonly Vector3 Position = position;
        }

        public readonly struct QuestRequiredItem(string id, string questName, int count)
        {
            public readonly string ID = id;
            public readonly string QuestName = questName;
            public readonly int Count = count;
        }

        private static void RefreshInternal(Player localPlayer)
        {
            const StringComparison sc = StringComparison.OrdinalIgnoreCase;

            List<ActiveQuest> activeQuests = new();
            List<QuestRequiredItem> questRequiredItems = new();

            // Reset quest consumed status
            foreach (var playerItem in LocalLootManager.Loot)
                playerItem.ResetConsumed();

            ulong questsDataAddr = Memory.ReadPtr(localPlayer.Profile + Offsets.Profile.QuestsData);
            MemList<ulong> questsData = new(questsDataAddr);

            ulong taskConditionCountersAddr = Memory.ReadPtr(localPlayer.Profile + Offsets.Profile.TaskConditionCounters);
            MemDictionary<ulong, ulong> taskConditionCounters = new(taskConditionCountersAddr);

            for (int i = 0; i < questsData.Items.Count; i++)
            {
                try
                {
                    ulong quest = questsData.Items[i];

                    var qStatus = (Enums.EQuestStatus)Memory.ReadValue<int>(quest + Offsets.QuestData.Status);
                    if (qStatus != Enums.EQuestStatus.Started)
                        continue;

                    ulong completedConditionsAddr = Memory.ReadPtr(quest + Offsets.QuestData.CompletedConditions);
                    MemHashSet<ulong> completedConditionsRaw = new(completedConditionsAddr);
                    HashSet<string> completedConditions = new();
                    foreach (ulong strAddr in completedConditionsRaw.Items)
                    {
                        try
                        {
                            string str = Memory.ReadUnityString(strAddr);
                            completedConditions.Add(str);
                        }
                        catch { }
                    }

                    ulong template = Memory.ReadPtr(quest + Offsets.QuestData.Template);
                    ulong questNameAddr = Memory.ReadPtr(template + Offsets.QuestTemplate.Name);
                    string questName = LocaleManager.GetItem(Memory.ReadUnityString(questNameAddr));
                    ulong conditionsAddr = Memory.ReadPtr(template + Offsets.QuestTemplate.Conditions);
                    MemDictionary<ulong, ulong> conditions = new(conditionsAddr);
                    foreach (KeyValuePair<ulong, ulong> conditionKVP in conditions.Items)
                    {
                        try
                        {
                            bool hasHandoverItem = false;

                            ulong listAddr = Memory.ReadPtr(conditionKVP.Value + Offsets.QuestConditionsContainer.ConditionsList);
                            MemList<ulong> list = new(listAddr);
                            List<QuestCandidate> questCandidates = new();
                            foreach (ulong condition in list.Items)
                            {
                                try
                                {
                                    ulong conditionIdAddr = Memory.ReadPtr(condition + Offsets.QuestCondition.id);
                                    string conditionID = Memory.ReadUnityString(conditionIdAddr);

                                    if (completedConditions.Contains(conditionID))
                                        continue;

                                    ulong conditionClassNameAddr = Memory.ReadPtrChain(condition, UnityOffsets.Component.To_NativeClassName);
                                    string conditionClassName = Memory.ReadUtf8String(conditionClassNameAddr, 128);

                                    if (conditionClassName.Equals("ConditionHandoverItem", sc))
                                        hasHandoverItem = true;

                                    questCandidates.Add(new(condition, conditionID, conditionClassName));
                                }
                                catch { }
                            }

                            foreach (QuestCandidate candidate in questCandidates)
                            {
                                try
                                {
                                    string description = LocaleManager.GetItem(candidate.ID);

                                    if (candidate.ClassName.Equals("ConditionPlaceBeacon", sc) ||
                                        candidate.ClassName.Equals("ConditionLeaveItemAtLocation", sc)) // EFT.Quests.ConditionZone
                                    {
                                        // Instant Plant
                                        if (candidate.ClassName.Equals("ConditionPlaceBeacon", sc) &&
                                            ToolkitManager.FeatureState["instantPlant"])
                                        {
                                            Memory.WriteValue(candidate.Address + Offsets.QuestConditionPlaceBeacon.plantTime, 1f);
                                        }

                                        ulong zoneIdAddr = Memory.ReadPtr(candidate.Address + Offsets.QuestConditionPlaceBeacon.zoneId);
                                        string zoneID = Memory.ReadUnityString(zoneIdAddr);

                                        if (QuestZones.TryGetValue(EFTDMA.InternalMapID, out var mapKVP) &&
                                            mapKVP.TryGetValue(zoneID, out Vector3 zonePosition))
                                        {
                                            ActiveQuest activeQuest = new(questName, description, zonePosition);
                                            activeQuests.Add(activeQuest);
                                        }
                                    }
                                    else if (candidate.ClassName.Equals("ConditionCounterCreator", sc))
                                    {
                                        ulong conditionCountersAddr = Memory.ReadPtrChain(candidate.Address, [
                                            Offsets.QuestConditionCounterCreator.Conditions,
                                            Offsets.QuestConditionsContainer.ConditionsList
                                        ]);
                                        MemList<ulong> conditionCounters = new(conditionCountersAddr);
                                        foreach (ulong conditionCounter in conditionCounters.Items)
                                        {
                                            try
                                            {
                                                ulong conditionClassNameAddr = Memory.ReadPtrChain(conditionCounter, UnityOffsets.Component.To_NativeClassName);
                                                string conditionClassName = Memory.ReadUtf8String(conditionClassNameAddr, 128);

                                                if (conditionClassName.Equals("ConditionVisitPlace", sc))
                                                {
                                                    ulong targetIdAddr = Memory.ReadPtr(conditionCounter + Offsets.QuestConditionVisitPlace.target);
                                                    string targetID = Memory.ReadUnityString(targetIdAddr);

                                                    if (QuestZones.TryGetValue(EFTDMA.InternalMapID, out var mapKVP) &&
                                                        mapKVP.TryGetValue(targetID, out Vector3 zonePosition))
                                                    {
                                                        ActiveQuest activeQuest = new(questName, description, zonePosition);
                                                        activeQuests.Add(activeQuest);
                                                    }
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    else if (candidate.ClassName.Equals("ConditionHandoverItem", sc) ||
                                        candidate.ClassName.Equals("ConditionFindItem", sc)) // EFT.Quests.ConditionItem
                                    {
                                        if (hasHandoverItem &&
                                            !candidate.ClassName.Equals("ConditionHandoverItem", sc))
                                        {
                                            continue;
                                        }

                                        int countRequired = (int)Memory.ReadValue<float>(candidate.Address + Offsets.QuestConditionItem.value);
                                        int countCompleted = 0;

                                        foreach (var conditionCounter in taskConditionCounters.Items)
                                        {
                                            try
                                            {
                                                ulong conditionIdAddr = Memory.ReadPtr(conditionCounter.Value + Offsets.ProfileTaskConditionCounter.Id);
                                                string conditionID = Memory.ReadUnityString(conditionIdAddr);

                                                if (conditionID.Equals(candidate.ID, sc))
                                                {
                                                    countCompleted = Memory.ReadValue<int>(conditionCounter.Value + Offsets.ProfileTaskConditionCounter.Value); ;
                                                    break;
                                                }
                                            }
                                            catch { }
                                        }

                                        ulong possibleItemsAddr = Memory.ReadPtr(candidate.Address + Offsets.QuestConditionFindItem.target);
                                        MemArray<ulong> possibleItems = new(possibleItemsAddr);
                                        
                                        int countInInventory = 0;
                                        Dictionary<ulong, string> requiredItems = new();
                                        foreach (ulong item in possibleItems.Items)
                                        {
                                            try
                                            {
                                                string itemID = Memory.ReadUnityString(item);

                                                foreach (var playerItem in LocalLootManager.Loot)
                                                {
                                                    if (playerItem.FoundInRaid &&
                                                        !playerItem.QuestConsumed &&
                                                        itemID.Equals(playerItem.ID, sc))
                                                    {
                                                        playerItem.SetConsumed();
                                                        countInInventory++;
                                                    }
                                                }

                                                requiredItems.TryAdd(item, itemID);
                                            }
                                            catch { }
                                        }

                                        int remainingCount = countRequired - countCompleted;
                                        int amountNeeded = remainingCount - countInInventory;
                                        if (countInInventory < remainingCount)
                                        {
                                            foreach (ulong item in possibleItems.Items)
                                            {
                                                if (!requiredItems.TryGetValue(item, out string itemID))
                                                    continue;

                                                string fixedQuestName = questName;
                                                if (string.IsNullOrEmpty(questName))
                                                    fixedQuestName = "Operational Task";

                                                if (amountNeeded < 0)
                                                    amountNeeded = 0;

                                                QuestRequiredItem requiredItem = new(itemID, fixedQuestName, amountNeeded);
                                                questRequiredItems.Add(requiredItem);
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
                catch { }
            }

            ActiveQuests = activeQuests;
            QuestRequiredItems = questRequiredItems;
        }
    }
}
