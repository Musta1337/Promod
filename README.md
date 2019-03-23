# Promod
A Open-Source Promod script for TeknoMW3, based on InfinityScript v1.1. Promod script offers a light weight Promod script with all the useless aspects removed that exists in most Promod Script and keeping stability and Performance into perspective.

## Installation
1. Put `promod.dll` in scripts folder.
2. Open server.cfg (Or any server config that you are loading).
3. Scroll to the bottom and Write the following line:
   ```
   loadscript "promod.dll"
   ```
4. Run server.
### Alternative
1. Go to scripts folder and put `promod.dll` in the directory.
2. Rename `promod.dll` to `promod.auto.dll`
3. Run server.

## Configuration
To configure the Promod script, do the following steps:

1. Open server.cfg (Or any server config that you are loading).
2. Scroll to the bottom and Write the following lines:
   ```
   ///////////////////////////////////////////
   // Promod Configuration 
   
   // Toggle Normal Killcam. 0 to disable, 1 to enable.
   seta sv_promodNormalKillcam "1"
   
   // Toggle No Sentry Gun. 0 to disable, 1 to enable.
   seta sv_promodNoSentry "1"
   
   // Toggle No Gernade Throwback. 0 to disable, 1 to enable.
   seta sv_promodNoGernadeThrowBack "1"
   
   // Toggle Custom Score. 0 to disable, 1 to enable.
   seta sv_promodCustomScore "1"
   
   ///////////////////////////////////////////
   // Non Essenstial Dvars
   
   // Loading screen text. null to disable.
   seta sv_NEMotd "^1I am loading screen text. :D"
   
   // Menu text
   seta sv_NEObjectiveText "^1I am menu text. :D"
   
   // Allies team name.
   seta sv_NEAlliesName "^1Team 1"
   
   // Axis team name.
   seta sv_NEAxisName "^1Team 2"
   
   // Toggle Server performance Dvars
   seta sv_NESVDvar "1"
   
   //Removes annoying announcer. 1 to enable, 0 to disable
   seta sv_NERemoveAnnouncer "1"
   
   ////////////////////////////////////////////
   ```
3. Save the config.
4. Run the server.

## Information
There are Total 9 settings in the current build of Promod script, as given:

### Promod Settings
- sv_promodNormalKillcam
- sv_promodNoSentry
- sv_promodNoGernadeThrowBack
- sv_promodCustomScore

### Non Essenstial Settings (Does not effect Promod experience).
- sv_NEAlliesName
- sv_NEAxisName
- sv_NEMotd
- sv_NEObjectiveText
- sv_NESVDvar
- sv_NERemoveAnnouncer

## Contact
You can contact me on my Discord server. https://discord.gg/HFTXzTw

Discord: Musta#6382

## Change Logs

### v1.0.0.0 
Initial Release.

### v1.0.0.1
1. Fixed r_normalMap.
2. Added sv_NERemoveAnnouncer.
3. Removed Ammo and other warning messages.
4. Changes to way the dvars are loaded for client.

#### v1.0.0.2
1. Added several commands:
   - !version
   - !gunx (cg_gun_x)
   - !guny (cg_gun_y)
   - !gunz (cg_gun_z)
2. Fixed perks management.
3. Fixed chat bug.