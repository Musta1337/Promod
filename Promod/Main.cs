using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using InfinityScript;
using System.Reflection;

namespace Promod
{
    public class Promod : BaseScript
    {
        public Promod() : base()
        {
            #region Initializing Dvars
            Call("setDvarifUninitialized", "sv_promodNormalKillcam", "1");
            Call("setDvarifUninitialized", "sv_promodNoSentry", "1");
            Call("setDvarifUninitialized", "sv_promodNoGernadeThrowBack", "1");
            Call("setDvarifUninitialized", "sv_promodCustomScore", "1");
            Call("setDvarifUninitialized", "sv_NEMotd", "^1Welcome to the server.");
            Call("setDvarifUninitialized", "sv_NEObjectiveText", "^1Welcome to the server.");
            Call("setDvarifUninitialized", "sv_NEAlliesName", "^1:thonk:");
            Call("setDvarifUninitialized", "sv_NEAxisName", "^::monkaS:");
            Call("setDvarifUninitialized", "sv_NESVDvar", "1");
            Call("setDvarifUninitialized", "sv_NERemoveAnnouncer", "1");
            Log.Info("Dvars Initialized.");
            #endregion

            #region Settings
            Breath();
            PromodSettings();
            Log.Info("Promod Settings Loaded.");
            #endregion

            #region Assigning things
            PlayerConnected += OnPlayerConnect;
            PlayerConnecting += OnPlayerConnecting;
            #endregion

            Log.Info("Promod script by Musta. Discord: Musta#6382");
        }

        public void PromodSettings()
        {
            string mode = Call<string>("getdvar", "g_gametype");
            if ((Call<int>("getDvarInt", "sv_promodNoGernadeThrowBack") != 0) && !mode.Contains("infect"))
            {
                Call("setdvar", "player_throwBackInnerRadius", "0");
                Call("setdvar", "player_throwBackOuterRadius", "0");
            }
            if (Call<int>("getDvarInt", "sv_promodNormalKillcam") != 0)
            {
                Call("setdvar", "scr_game_allowkillcam", "1");
                Log.Info("Killcam Enabled.");
            }
            else
            {
                Call("setdvar", "scr_game_allowkillcam", "0");
                Log.Info("KillCam Disabled.");
            }
            if (Call<string>("getDvar", "sv_NEMotd") != "null")
            {
                Function.Call("makedvarserverinfo", "motd", Call<string>("getDvar", "sv_NEMotd"));
                Function.Call("makedvarserverinfo", "didyouknow", Call<string>("getDvar", "sv_NEMotd"));
            }
            if ((Call<int>("getDvarInt", "sv_promodNoSentry") != 0) && !mode.Contains("infect"))
            {
                Call("setdvar", "player_MGUseRadius", "0");
            }
            else
            {
                Call("setdvar", "player_MGUseRadius", "1");
            }
            if ((Call<int>("getDvarInt", "sv_promodCustomScore") != 0) && mode != "gun" && mode != "oic" && mode != "dm" && mode != "war" && mode != "infect")
            {
                Utilities.ExecuteCommand("set scr_" + mode + "_score_kill 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_headshot 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_assist 1");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_plant 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_defuse 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_teamkill 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_capture 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_defend 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_defend_assist 1");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_assault 2");
                Utilities.ExecuteCommand("set scr_" + mode + "_score_assault_assist 1");
            }
            if (mode.Contains("sd"))
            {
                Call("setdvar", "g_hardcore", "1");
                Log.Info("Hardcore Enabled.");
            }
            Call("setdvar", "scr_showperksonspawn", "0");
            Utilities.ExecuteCommand("set ui_hud_showdeathicons 0");
            Utilities.ExecuteCommand("set scr_game_matchstarttime 5");
            Utilities.ExecuteCommand("set scr_game_playerwaittime 3");
            Utilities.ExecuteCommand("set scr_sd_planttime 5");
            Utilities.ExecuteCommand("set scr_sd_defusetime 7.5");
            Call("setdvar", "sv_clientSideBullets", "0");
            Call("setdvar", "glass_DamageToDestroy", "50");
            if (Call<int>("getDvarInt", "sv_NESVDvar") != 0)
            {
                Function.Call("setdevDvar", "sv_network_fps", 200);
                Function.Call("setDvar", "sv_hugeSnapshotSize", 10000);
                Function.Call("setDvar", "sv_hugeSnapshotDelay", 100);
                Function.Call("setDvar", "sv_pingDegradation", 0);
                Function.Call("setDvar", "sv_pingDegradationLimit", 9999);
                Function.Call("setDvar", "sv_acceptableRateThrottle", 9999);
                Function.Call("setDvar", "sv_newRateThrottling", 2);
                Function.Call("setDvar", "sv_minPingClamp", 50);
                Function.Call("setDvar", "sv_cumulThinkTime", 1000);
                Function.Call("setDvar", "sys_lockThreads", "all");
                Function.Call("setDvar", "com_maxFrameTime", 1000);
                Function.Call("setDvar", "com_maxFps", 0);
                Function.Call("setDvar", "sv_voiceQuality", 9);
                Function.Call("setDvar", "maxVoicePacketsPerSec", 1000);
                Function.Call("setDvar", "maxVoicePacketsPerSecForServer", 200);
                Function.Call("setDvar", "cg_everyoneHearsEveryone", 1);
            }
            AfterDelay(400, () =>
            {
                try
                {
                    NoSoundADS();
                }
                catch (Exception)
                {
                }
            });
        }

        public void OnPlayerConnect(Entity player)
        {
            player.SetClientDvar("cg_objectiveText", Call<String>("getDvar", "sv_NEObjectiveText"));
            player.Call("clearperks");
            player.SetClientDvar("perk_sprintMultiplier", "1.35");
            player.SetClientDvar("perk_bulletPenetrationMultiplier", "1.8");
            player.SetClientDvar("perk_quickDrawSpeedScale", "1.2");
            player.Call("GiveMaxAmmo", player.CurrentWeapon);
            if (player.HasWeapon("flash_grenade_mp"))
            {
                player.GiveWeapon("flash_grenade_mp");
                player.Call("SetWeaponAmmoClip", "flash_grenade_mp", 1);
            }
            player.OnNotify("weapon_change", delegate (Entity Player, Parameter weap)
            {
                if (weap.ToString() == "briefcase_bomb_mp")
                {
                    Parameter[] parameters = new Parameter[2]
                    {
                        Player.Origin,
                        "mp_bomb_plant"
                    };
                    Call("PlaySoundAtPos", parameters);
                }
                else if (weap.ToString() == "briefcase_bomb_defuse_mp")
                {
                    Parameter[] parameters2 = new Parameter[2]
                    {
                        Player.Origin,
                        "mp_bomb_defuse"
                    };
                    Call("PlaySoundAtPos", parameters2);
                }
            });
            player.SpawnedPlayer += () =>
            {
                player.SetClientDvar("cg_objectiveText", Call<String>("getDvar", "sv_NEObjectiveText"));
                player.Call("clearperks");
                player.Call("GiveMaxAmmo", player.CurrentWeapon);
                if (player.HasWeapon("flash_grenade_mp"))
                {
                    player.GiveWeapon("flash_grenade_mp");
                    player.Call("SetWeaponAmmoClip", "flash_grenade_mp", 1);
                }
            };
        }

        public void OnPlayerConnecting(Entity player)
        {
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("useRelativeTeamColors", "1");
            player.SetClientDvar("cg_crosshairEnemyColor", "0");
            player.SetClientDvar("cg_drawcrosshairnames", "0");
            player.SetClientDvar("cg_brass", "0");
            player.SetClientDvar("r_distortion", "0");
            player.SetClientDvar("r_dlightlimit", "0");
            player.SetClientDvar("r_normalMap", "Flat");
            player.SetClientDvar("r_fog", "0");
            player.SetClientDvar("r_fastskin", "0");
            player.SetClientDvar("snaps", "30");
            player.SetClientDvar("r_drawdecals", "1");
            player.SetClientDvar("clientsideeffects", "0");
            player.SetClientDvar("fx_draw", "1");
            player.SetClientDvar("sys_lockThreads", "all");
            player.SetClientDvar("cl_maxpackets", "100");
            if (Call<int>("getDvarInt", "sv_NERemoveAnnouncer") != 0) { player.SetClientDvar("snd_enableStream", "0"); }
            player.SetClientDvar("ragdoll_enable", "0");
            player.SetClientDvar("waypointIconHeight", "13");
            player.SetClientDvar("waypointIconWidth", "13");
            player.SetClientDvar("g_teamname_allies", Call<string>("getDvar", "sv_NEAlliesName"));
            player.SetClientDvar("g_teamname_axis", Call<string>("getDvar", "sv_NEAxisName"));
            AfterDelay(500, () =>
            {
                player.SetClientDvar("bg_weaponBobMax", "0");
                player.SetClientDvar("bg_viewBobMax", "0");
                player.SetClientDvar("bg_viewBobAmplitudeStandingAds", "0 0");
                player.SetClientDvar("bg_viewBobAmplitudeSprinting", "0 0");
                player.SetClientDvar("bg_viewBobAmplitudeDucked", "0 0");
                player.SetClientDvar("bg_viewBobAmplitudeDuckedAds", "0 0");
                player.SetClientDvar("bg_viewBobAmplitudeProne", "0 0");
                player.SetClientDvar("bg_viewKickRandom", "0.2");
                player.SetClientDvar("bg_viewKickMin", "1");
                player.SetClientDvar("bg_viewKickScale", "0.15");
                player.SetClientDvar("bg_viewKickMax", "75");
            });
        }

        #region Nade Damage Changing.

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (weapon == "frag_grenade_mp" && damage != 15)
            {
                damage = ((damage <= 10) ? ((int)((double)(damage * 12) * 1.15)) : ((int)((double)damage * 1.15)));
            }
            else
            {
                if (weapon == "flash_grenade_mp" && damage < 10)
                {
                    damage = 0;
                }
                if (weapon == "destructible_car" && damage <= 10)
                {
                    damage *= 12;
                }
                damage = (int)((double)damage * 1.15);
            }
        }

        #endregion

        #region Commands

        public override EventEat OnSay2(Entity player, string name, string message)
        {
            try
            {
                message = message.ToLower();
                string[] msg = message.Split(' ');
                if (msg[0].StartsWith("!ft"))
                {
                    string arg = msg[1];
                    if (arg == "0")
                    {
                        player.SetClientDvar("r_filmusetweaks", "0");
                        player.SetClientDvar("r_filmtweakenable", "0");
                        player.SetClientDvar("r_colorMap", "1");
                        player.SetClientDvar("r_specularMap", "1");
                        player.SetClientDvar("r_normalMap", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;0^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "1")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "0.65 0.7 0.8");
                        player.SetClientDvar("r_filmtweakcontrast", "1.3");
                        player.SetClientDvar("r_filmtweakbrightness", "0.15");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1.8 1.8 1.8");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;1^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "2")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "1.15 1.1 1.3");
                        player.SetClientDvar("r_filmtweakcontrast", "1.6");
                        player.SetClientDvar("r_filmtweakbrightness", "0.2");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1.35 1.3 1.25");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;2^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "3")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "0.8 0.8 1.1");
                        player.SetClientDvar("r_filmtweakcontrast", "2.25");
                        player.SetClientDvar("r_filmtweakbrightness", "0.48");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1 1 1.4");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;3^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "4")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "1.8 1.8 2");
                        player.SetClientDvar("r_filmtweakcontrast", "1.25");
                        player.SetClientDvar("r_filmtweakbrightness", "0.02");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "0.8 0.8 1");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;4^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "5")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "1 1 0.8");
                        player.SetClientDvar("r_filmtweakcontrast", "1.1");
                        player.SetClientDvar("r_filmtweakbrightness", "0.05");
                        player.SetClientDvar("r_filmtweakdesaturation", "2");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1.5 1.5 1.5");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;5^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "6")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "1.1 1.1 1.3");
                        player.SetClientDvar("r_filmtweakcontrast", "1.5");
                        player.SetClientDvar("r_filmtweakbrightness", "0.255");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1.3 1.3 1.3");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;6^: has been applied.");
                        return EventEat.EatGame;

                    }
                    if (arg == "7")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "1.7 1.7 2");
                        player.SetClientDvar("r_filmtweakcontrast", "1");
                        player.SetClientDvar("r_filmtweakbrightness", "0.125");
                        player.SetClientDvar("r_filmtweakdesaturation", "0");
                        player.SetClientDvar("r_filmusetweaks", "1");
                        player.SetClientDvar("r_filmtweaklighttint", "1.6 1.6 1.8");
                        player.SetClientDvar("r_filmtweakenable", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;7^: has been applied.");
                        return EventEat.EatGame;
                    }
                    if (arg == "default")
                    {
                        player.SetClientDvar("r_filmtweakdarktint", "0.7 0.85 1");
                        player.SetClientDvar("r_filmtweakcontrast", "1.4");
                        player.SetClientDvar("r_filmtweakdesaturation", "0.2");
                        player.SetClientDvar("r_filmusetweaks", "0");
                        player.SetClientDvar("r_filmtweaklighttint", "1.1 1.05 0.85");
                        player.SetClientDvar("cg_scoreboardpingtext", "1");
                        player.SetClientDvar("waypointIconHeight", "13");
                        player.SetClientDvar("waypointIconWidth", "13");
                        player.SetClientDvar("cl_maxpackets", "100");
                        player.SetClientDvar("r_fog", "0");
                        player.SetClientDvar("fx_drawclouds", "0");
                        player.SetClientDvar("r_distortion", "0");
                        player.SetClientDvar("r_dlightlimit", "0");
                        player.SetClientDvar("cg_brass", "0");
                        player.SetClientDvar("r_filmTweakBrightness", "0.2");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Filmtweak ^;default^: has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!ft <0-7,default>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!fovscale"))
                {
                    string arg = msg[1];
                    if (arg != "")
                    {
                        player.SetClientDvar("cg_fovscale", arg);
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:FovScale ^;" + arg + " ^:has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!fovscale <value>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!fog"))
                {
                    string arg = msg[1];
                    if (arg == "0")
                    {
                        player.SetClientDvar("r_fog", "0");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Fog has been removed.");
                        return EventEat.EatGame;
                    }
                    if (arg == "1")
                    {
                        player.SetClientDvar("r_fog", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Fog has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!fog <0-1>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!detail"))
                {
                    string arg = msg[1];
                    if (arg == "0")
                    {
                        player.SetClientDvar("r_detail", "0");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Detail has been removed.");
                        return EventEat.EatGame;
                    }
                    if (arg == "1")
                    {
                        player.SetClientDvar("r_detail", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Detail has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!detail <0-1>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!distortion"))
                {
                    string arg = msg[1];
                    if (arg == "0")
                    {
                        player.SetClientDvar("r_distortion", "0");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Distortion has been removed.");
                        return EventEat.EatGame;
                    }
                    if (arg == "1")
                    {
                        player.SetClientDvar("r_distortion", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Distortion has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!distortion <0-1>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!dlightlimit"))
                {
                    string arg = msg[1];
                    if (arg == "0")
                    {
                        player.SetClientDvar("r_dlightlimit", "0");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:DlightLimit has been removed.");
                        return EventEat.EatGame;
                    }
                    if (arg == "1")
                    {
                        player.SetClientDvar("r_dlightlimit", "1");
                        Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:DlightLimit has been applied.");
                        return EventEat.EatGame;
                    }
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^1Usage^0: ^:!dlightlimit <0-1>");
                    return EventEat.EatGame;
                }
                if (msg[0].StartsWith("!version"))
                {
                    Utilities.RawSayTo(player, "^0[^1PROMOD^0]: ^:Promod Script ^;v1.0.0.1 ^:by ^1Musta^0.");
                }
            }
            catch (Exception)
            {
            }
            return EventEat.EatNone;
        }
        #endregion

        #region MemoryEdit

        public void Breath()
        {
            byte[] array3 = new byte[2]
            {
            144,
            144
            };
            //byte? nullable = null;
            byte?[] array4 = new byte?[20];
            int int_;
            try
            {
                array4[0] = 131;
                array4[1] = null;
                array4[2] = null;
                array4[3] = null;
                array4[4] = null;
                array4[5] = null;
                array4[6] = null;
                array4[7] = 125;
                array4[8] = 10;
                array4[9] = 199;
                array4[10] = null;
                array4[11] = null;
                array4[12] = null;
                array4[13] = null;
                array4[14] = null;
                array4[15] = 0;
                array4[16] = 0;
                array4[17] = 0;
                array4[18] = 0;
                array4[19] = 133;
                byte?[] nullableArray = array4;
                int value = smethod_62(4194304, 4718592, 1, nullableArray) + 7;
                WriteProcessMemory((IntPtr)(-1), (IntPtr)value, array3, (IntPtr)array3.Length, out int_);
            }
            catch (Exception)
            {
            }
            try
            {
                byte[] array5 = new byte[3]
                {
                144,
                144,
                144
                };
                smethod_62(4194304, 4718592, 1, new byte?[13]
                {
                247,
                null,
                null,
                null,
                null,
                null,
                0,
                128,
                0,
                0,
                116,
                17,
                139
                });
                array4 = new byte?[13]
                {
                116,
                7,
                43,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                };
                array4[3] = null;
                array4[4] = null;
                array4[5] = null;
                array4[6] = 137;
                array4[7] = null;
                array4[8] = null;
                array4[9] = 131;
                array4[10] = null;
                array4[11] = null;
                array4[12] = 0;
                byte?[] nullableArray2 = array4;
                int value2 = smethod_62(4194304, 4718592, 1, nullableArray2) + 6;
                WriteProcessMemory((IntPtr)(-1), (IntPtr)value2, array5, (IntPtr)array5.Length, out int_);
            }
            catch (Exception)
            {
            }
            try
            {
                byte[] array6 = new byte[2]
                {
                144,
                144
                };
                byte[] array7 = new byte[3]
                {
                144,
                144,
                144
                };
                int value3 = FindMem(new byte?[15]
                {
                116,
                29,
                247,
                null,
                null,
                null,
                null,
                null,
                0,
                0,
                0,
                4,
                116,
                17,
                139
                }, 1, 4194304, 4718592) + 12;
                WriteProcessMemory((IntPtr)(-1), (IntPtr)value3, array6, (IntPtr)array6.Length, out int int_2);
                int value4 = FindMem(new byte?[15]
                {
                116,
                53,
                247,
                null,
                null,
                null,
                null,
                null,
                0,
                0,
                0,
                4,
                116,
                41,
                139
                }, 1, 4194304, 4718592) + 12;
                WriteProcessMemory((IntPtr)(-1), (IntPtr)value4, array6, (IntPtr)array6.Length, out int_2);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(IntPtr intptr_0, IntPtr intptr_1, byte[] byte_0, IntPtr intptr_2, out int int_0);

        [DllImport("kernel32.dll")]
        internal static extern bool VirtualProtect(IntPtr intptr_0, IntPtr intptr_1, uint uint_0, out uint uint_1);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, uint flAllocationType, uint flProtect);

        private unsafe void NoSoundADS()
        {
            int num = FindMem(new byte?[14]
            {
            139,
            null,
            139,
            null,
            null,
            null,
            0,
            0,
            131,
            null,
            22,
            117,
            7,
            184
            }, 1, 4194304, 4718592);
            byte[] array = new byte[22]
            {
            80,
            139,
            64,
            12,
            169,
            60,
            0,
            0,
            0,
            88,
            116,
            3,
            49,
            192,
            195,
            139,
            128,
            100,
            1,
            0,
            0,
            233
            };
            IntPtr value = VirtualAlloc(IntPtr.Zero, (UIntPtr)(ulong)(array.Length + 5), 4096u, 64u);
            VirtualProtect((IntPtr)(num + 2), (IntPtr)5, 64u, out uint uint_);
            for (int i = 0; i < array.Length; i++)
            {
                *(sbyte*)((int)value + i) = (sbyte)array[i];
            }
            int num2 = num + 8 - ((int)value + array.Length + 4);
            *(int*)(void*)(IntPtr)((int)value + array.Length) = num2;
            *(sbyte*)(void*)(IntPtr)(num + 2) = -23;
            int num3 = (int)value - (num + 7);
            *(int*)(void*)(IntPtr)(num + 3) = num3;
            *(sbyte*)(void*)(IntPtr)(num + 7) = -112;
            VirtualProtect((IntPtr)(num + 2), (IntPtr)5, uint_, out uint_);
        }

        private unsafe int FindMem(byte?[] search, int num = 1, int start = 16777216, int end = 63963136)
        {
            try
            {
                int num2 = 0;
                for (int i = start; i < end; i++)
                {
                    int num3 = i;
                    bool flag = false;
                    for (int j = 0; j < search.Length; j++)
                    {
                        if (search[j].HasValue)
                        {
                            int num4 = *(byte*)num3;
                            byte? nullable = search[j];
                            if (num4 != nullable.GetValueOrDefault() || !nullable.HasValue || 1 == 0)
                            {
                                break;
                            }
                        }
                        if (j == search.Length - 1)
                        {
                            if (num == 1)
                            {
                                flag = true;
                            }
                            else
                            {
                                num2++;
                                if (num2 == num)
                                {
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            num3++;
                        }
                    }
                    if (flag)
                    {
                        return i;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return 0;
        }
        private unsafe static int smethod_62(int p1, int p2, int p3, byte?[] nullableArray1)
        {
            try
            {
                int num = 0;
                for (int i = p1; i < p2; i++)
                {
                    byte* ptr = (byte*)i;
                    bool flag = false;
                    for (int j = 0; j < nullableArray1.Length; j++)
                    {
                        if (nullableArray1[j].HasValue)
                        {
                            int num2 = *ptr;
                            byte? nullable = nullableArray1[j];
                            if (num2 != nullable.GetValueOrDefault() || ((!nullable.HasValue) ? true : false))
                            {
                                break;
                            }
                        }
                        if (j != nullableArray1.Length - 1)
                        {
                            ptr++;
                        }
                        else if (p3 != 1)
                        {
                            num++;
                            if (num == p3)
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        return i;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return 0;
        }

        #endregion
    }
}
