import delay from "delay";
import { Container, DisplayObject } from "pixi.js";
import { followLocalPlayer, FOLLOW_LOCALPLAYER, PIXI_VIEWPORT, setMapFloor, VIEWPORT_SCALES } from "..";
import { Aimline } from "./aimline";
import { Dot as PlayerDot } from "./dot";
import { PlayerHealth } from "./healthBar";
import { HeightIndicator } from "./heightIndicator";
import { localPlayer, removePlayerFromGroup, setLocalPlayer } from "./playerManager";
import { HostileHumanTypes, PlayerColors } from "./playerType";
import { SetStreamLink, UpdateColor as UpdatePlayersTableColor, UpdateStats, UpdateValue, add as addPlayerToTable, RemovePlayer as removePlayerFromTable } from "./table";
import { PlayerText } from "./text";
import { IPC_StaticPlayer } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticPlayer";
import { PlayerType } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/PlayerType";
import { IPC_PlayerStats } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_PlayerStats";
import { DateTime } from "luxon";
import { CheckURL, StreamerChecker } from "../../streamerChecker";

export const SELF_DESTRUCT_TIMEOUT = 3000;

export class Player {
    StaticData: {
        ID: number;
        AccountID: string;
        GroupID: string;
        Faction: string;
    };
    Stats: IPC_PlayerStats;
    DynamicData: {
        Name: string;
        PlayerType: PlayerType;
        DistanceNumber: number;
        Distance: string;
        Hands: string;
        Value: string;
        Status: number;
        Height: number;
        LastUpdate: DateTime;
        StreamData: CheckURL;
    };
    Container: Container<DisplayObject>;
    aimline: Aimline;
    playerDot: PlayerDot;
    heightIndicator: HeightIndicator;
    playerHealth: PlayerHealth;
    playerText: PlayerText;

    constructor(staticPlayer: IPC_StaticPlayer) {
        console.log(`Allocating Player: UniqueID: ${staticPlayer.iD} | AccountID: ${staticPlayer.accountID} | GroupID: ${staticPlayer.groupID} | Name: ${staticPlayer.name} | Faction: ${staticPlayer.faction} | PlayerType: ${staticPlayer.playerType}`);

        // This player data will never change
        this.StaticData = {
            ID: staticPlayer.iD,
            AccountID: staticPlayer.accountID,
            GroupID: staticPlayer.groupID,
            Faction: staticPlayer.faction,
        };
        
        this.DynamicData = {
            Name: staticPlayer.name,
            PlayerType: staticPlayer.playerType,
            DistanceNumber: null,
            Distance: null,
            Hands: null,
            Value: null,
            Status: 0,
            Height: 999,
            LastUpdate: DateTime.now(),
            StreamData: null,
        };

        this.Container = new Container();
        this.Container.cullable = true;
        this.Container.position.x = 0;
        this.Container.position.y = 0;

        // Order player's z-index based on player type
        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer) {
            // Local Player
            this.Container.zIndex = 120;
        } else if (this.DynamicData.PlayerType === PlayerType.Boss || this.DynamicData.PlayerType === PlayerType.BTR) {
            // Scav Boss // BTR
            this.Container.zIndex = 110;
        } else {
            // Everyone else
            this.Container.zIndex = 100;
        }

        // Handles creating all player rendering components
        this.setAlive();

        // Set scale to current scale
        this.setScale(VIEWPORT_SCALES.Players[0], VIEWPORT_SCALES.Players[1]);

        // Add player to canvas
        PIXI_VIEWPORT.addChild(this.Container);

        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer) {
            setLocalPlayer(this);

            const followLocalPlayerLoop = setInterval(async () => {
                if (localPlayer === null || localPlayer.Container == null) {
                    await delay(100);
                    return;
                }

                if (FOLLOW_LOCALPLAYER) followLocalPlayer(true);
                clearInterval(followLocalPlayerLoop);
            });
        }
    }

    setAlive() {
        this.DynamicData.Status = 1;

        // Aimline
        if (this.DynamicData.PlayerType !== PlayerType.BTR) {
            this.aimline = new Aimline(0);
            this.Container.addChild(this.aimline);
        }

        // Player Dot
        const playerColor = PlayerColors.get(this.DynamicData.PlayerType);
        if (playerColor == null) {
            console.log(`[ERROR] -> Player Type: "${this.DynamicData.PlayerType}" is not registered in PlayerColors!`);
        }
        this.playerDot = new PlayerDot(playerColor.dot, playerColor.border);
        this.Container.addChild(this.playerDot);

        // this.playerDot.on("click", (ev) => {
        //     PIXI_VIEWPORT.follow(this.Container);
        //     // playerDot.getGlobalPosition() // Gets the position of the element relative to `appContainerEl`
        // });

        // Player height indicator
        if (this.DynamicData.PlayerType !== PlayerType.LocalPlayer &&
            this.DynamicData.PlayerType !== PlayerType.BTR) {
            this.heightIndicator = new HeightIndicator(0);
            this.Container.addChild(this.heightIndicator);
        }

        // Player Health
        if (this.DynamicData.PlayerType !== PlayerType.LocalPlayer &&
            this.DynamicData.PlayerType !== PlayerType.BTR) {
            this.playerHealth = new PlayerHealth(0);
            this.Container.addChild(this.playerHealth);
        }

        // Player text
        if (this.DynamicData.PlayerType !== PlayerType.LocalPlayer) {
            this.playerText = new PlayerText(this.DynamicData.Name);
            this.Container.addChild(this.playerText);
        }

        addPlayerToTable(this);

        console.log(`Set state of player: "${this.DynamicData.Name}" to: "alive"`);

        // Check if this is a streamer
        if (HostileHumanTypes.includes(this.DynamicData.PlayerType))
        {
            (async () => {
                const streamData = await StreamerChecker.IsUserLive(this.DynamicData.Name);
                if (streamData !== null)
                {
                    this.DynamicData.PlayerType = PlayerType.Streamer;

                    const playerColor = PlayerColors.get(this.DynamicData.PlayerType);

                    this.playerDot.updateColors(playerColor.dot, playerColor.border);
                    UpdatePlayersTableColor(this.StaticData.ID, this.DynamicData.PlayerType);
                    SetStreamLink(this.StaticData.ID, streamData.URL);
                }
            })();
        }

        // TODO: make Raid Log entry
    }

    setDead() {
        this.DynamicData.Status = 2;

        console.log(`Set state of player: "${this.DynamicData.Name}" to: "dead"`);

        this.destroy();

        // TODO: make Raid Log entry
    }

    setExfil() {
        this.DynamicData.Status = 3;

        console.log(`Set state of player: "${this.DynamicData.Name}" to: "exfil"`);

        this.destroy();

        // TODO: make Raid Log entry
    }

    setScale(x: number, y: number) {
        this.Container.scale.x = x;
        this.Container.scale.y = y;
    }

    setValue(newValue: string) {
        if (this.DynamicData.Value === newValue || newValue == null) return;

        this.DynamicData.Value = newValue;

        UpdateValue(this.StaticData.ID, this.DynamicData.Value);
    }

    setPosition(x: number, y: number) {
        this.Container.position.x = x;
        this.Container.position.y = y;
    }

    setLookDirection(radians: number) {
        if (this.DynamicData.PlayerType === PlayerType.BTR ||
            this.aimline == null
        ) {
            return;
        }

        this.aimline.updateAngle(radians);
    }

    setHeight(height: number) {
        if (this.DynamicData.Height === height) {
            return;
        }

        this.DynamicData.Height = height;

        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer) {
            setMapFloor(height);

            return;
        }

        if (this.heightIndicator != null) {
            this.heightIndicator.updateRelativeHeight(height);
        }
    }

    setHealth(currentHealth: number) {
        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer ||
            this.playerHealth == null
        ) {
            return;
        }

        this.playerHealth.updateHealth(currentHealth);
    }

    setDistance(distance: number) {
        // Only update when Distance actually changes
        if (distance !== this.DynamicData.DistanceNumber) {
            this.DynamicData.Distance = String(distance) + "m";
            this.DynamicData.DistanceNumber = distance;
            this.updateText();
        }
    }

    setHands(hands: string) {
        // Only update when Hands contents actually change
        if (hands !== this.DynamicData.Hands) {
            this.DynamicData.Hands = hands;
            this.updateText();
        }
    }

    setStats(stats: IPC_PlayerStats) {
        this.Stats = stats;

        if (stats.name != null && this.DynamicData.Name !== stats.name) {
            this.DynamicData.Name = stats.name;
            this.updateText();
        }

        UpdateStats(this.StaticData.ID, stats);
    }

    /** Intelligently update this player's text. */
    updateText() {
        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer ||
            this.playerText == null) {
            return;
        }

        let distanceString = "";
        if (this.DynamicData.Distance !== null && this.DynamicData.Hands !== "") {
            distanceString = " " + this.DynamicData.Distance;
        }

        let handsString = "";
        if (this.DynamicData.Hands !== null && this.DynamicData.Hands !== "") {
            handsString = "\n" + this.DynamicData.Hands;
        }

        this.playerText.update(this.DynamicData.Name + distanceString + handsString);
    }

    destroy() {
        console.log(`Set state of player: "${this.DynamicData.Name}" to: "destroyed"`);

        removePlayerFromGroup(this.StaticData.ID, this.StaticData.GroupID);
        removePlayerFromTable(this.StaticData.ID);

        // Unfollow LocalPlayer if they are being followed
        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer && FOLLOW_LOCALPLAYER) {
            PIXI_VIEWPORT.plugins.remove("follow");
            PIXI_VIEWPORT.plugins.get("wheel").options.center = null;
        }

        // Remove player from canvas
        this.Container.destroy(true);

        // Set LocalPlayer to null if this is the LocalPlayer
        if (this.DynamicData.PlayerType === PlayerType.LocalPlayer) setLocalPlayer(null);
    }
}
