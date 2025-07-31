const BSGDetectionString = "It is possible (but unlikely) that some day BSG is able to detect this.";
const BanMeDetectionString = "When you play with fire you get burned. Prepare to burn cheater!";
const RestartRequiredToFullyDisable = "<strong>NOTE:</strong> You have to restart your game to fully disable this feature.";

export const featuresManifest = [
    // ============= PMC Features
    {
        type: "featureSection",
        id: "pmc",
        name: "PMC",
        iconClasses: ["fa-duotone", "fa-pills"],
    },
    {
        type: "featureRoot",
        id: "noFall",
        name: "No Fall",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    When enabled, this allows you to stay at your current height when walking off surfaces that you would normally fall off of.
                    <br>
                    <strong>NOTE:</strong> It is known that you get teleported back to your original position sometimes. Unfortunately, nothing can be done about that.
                    <br>
                    <strong>WARNINGS:</strong>
                    <ol>
                        <li>If you disable this while you are high in the air you will fall to your death. Get to a spot that is reasonably close to the ground before disabling.</li>
                        <li>If you jump off of a surface you will fall to the ground. You must walk off of the surface to stay in the air.</li>
                    </ol>
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    <ul>
                        <li>If someone records you while you are using this feature you could get banned.</li>
                        <li>${BSGDetectionString}</li>
                    </ul>
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "infiniteStamina",
        name: "Infinite Stamina",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    When enabled, your stamina bar will automatically be refilled when it gets low. We are using a method that bypasses fatigue! Please only enable this feature if you aren't planning on looting much. You will rubberband after reaching yellow weight.
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    ${BSGDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "enhancedJump",
        name: "Enhanced Jump",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Cosplay as Michael Jordan and leap over most obstacles with ease!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    If you are playing with non-cheesers and they see you jump with this enabled they will most certainly know something is up (pun intended).
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "enhancedThrow",
        name: "Enhanced Throw",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Nail people with grenades from over 150 meters away (M67s work best)! Tom Brady is jealous...
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "instantGrenades",
        name: "Fast Grenades",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Take your PMC back back to the good ole days when you could grenade spam! Pulling out and throwing grenades doesn't take forever anymore.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "muleMode",
        name: "M.U.L.E. Mode",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to carry up to 120 KG without incurring the movement penalties associated with being overweight.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "alwaysSprint",
        name: "Always Sprint",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    You can sprint through swamps and bushes, while eating, while using a CMS or Surv12, etc...
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    If you are playing with non-cheesers and they see you running while using a CMS they will definitely know you are cheesing.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "360Freelook",
        name: "Owl Mode",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to free-look completely behind yourself. Gotta keep that head on a swivel!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "noInertia",
        name: "No Inertia",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This completely removes inertia allowing you to strafe and jump without incurring the inertia movement penalties!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "wideLean",
        name: "Wide Lean",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature allows you to stay behind cover and engage other players or AI. It's really the ultimate HvH tool, and if used to it's full potential you'll never lose! You can also shoot/throw grenades through walls, floors, and ceilings. Note that other players can't see when you are using this feature. It just looks like a normal lean to them.
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    <ul>
                        <li>This is a very ragey feature. If you abuse it too much you'll likely catch lots of reports and get banned.</li>
                        <li>If someone records you while you are using this feature you could get banned.</li>
                        <li>${BSGDetectionString}</li>
                    </ul>
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "xRayVision",
        name: "X-Ray Vision",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to see through all surfaces up to your configured distance. Now you can peep on your prey!
                `,
            },
        ],
    },
    // ============= Gear Features
    {
        type: "featureSection",
        id: "gear",
        name: "Gear",
        iconClasses: ["fa-duotone", "fa-container-storage"],
    },
    {
        type: "featureRoot",
        id: "noPenalties",
        name: "No Penalties",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This removes all movement penalties from all gear. Now you can be a tank while moving like a cheetah!
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "noVisor",
        name: "No Visor",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This prevents the face shield of your helmet from obstructing your view. Now you can see all!
                `,
            },
        ],
    },
    // ============= Camera
    {
        type: "featureSection",
        id: "camera",
        name: "Camera",
        iconClasses: ["fa-duotone", "fa-camera"],
    },
    {
        type: "featureRoot",
        id: "thirdPerson",
        name: "Third Person",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to play with a third person camera perspective.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "nightVision",
        name: "Night Vision",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Fakes the night vision effect as if you're wearing NVGs.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "thermalVision",
        name: "Thermal Vision",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    As if you are always wearing T-7s!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "instantZoom",
        name: "Instant Zoom",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This enables a DayZ style camera zoom effect!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableVignette",
        name: "Disable Vignette",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Disables the vignette (subtle darkness at the corners of the screen).
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableExposure",
        name: "Disable Exposure",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Disables the eyes adjusting effect that happens when you go from a dark to a light area or vice-versa.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableBloom",
        name: "Disable Bloom",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Disables the bloom effect seen on bright objects, increasing visibility in bright areas.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableColorGrading",
        name: "Disable Color Grading",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Disables the default color grading making Tarkov appear more neutrally colored.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableScreenEffects",
        name: "Disable Effects",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Allows you to disable some or all of the camera effects (blurriness, blood drops, blindness, sharpening, etc...).
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    // ============= Weapon Features
    {
        type: "featureSection",
        id: "weapon",
        name: "Weapon",
        iconClasses: ["fa-duotone", "fa-gun"],
    },
    {
        type: "featureRoot",
        id: "aimbot",
        name: "Aimbot",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Our Aimbot has full prediction allowing you to hit shots from over 1000 meters away! Simply hold down your configured hotkey (Left Control by default) to engage the Aimbot.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "instantADS",
        name: "Instant ADS",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Instantly aim down sights making any gun feel like it has 100 ergo!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "noMalfunctions",
        name: "No Malfunctions",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This prevents your weapon from malfunctioning in any way - even when it has very low durability.
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "customRecoil",
        name: "Custom Recoil",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    You can configure your gun's recoil & sway to be as intense as you like. Whether you want no recoil & sway, 50%, or full recoil & sway, this feature allows you to do it!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "removableAttachments",
        name: "Removable Attachments",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to remove all attachments from any weapon. To prevent the weapon from desyncing with the server, never modify the weapon while it is in your hands. To remove attachments, discard them from the weapon. If you are planning to keep the weapon, do not discard it. Just place it in your bag and leave it there.
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "fastBulletLoadUnload",
        name: "Fast Load/Unload",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to load and unload bullets extremely quickly. Gone are the days of growing a beard while packing that 60 rounder!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    ${BSGDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "fastWeaponOps",
        name: "Fast Weapon Ops",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This makes all weapon operations significantly faster (reloading, swapping weapons, fixing malfunctions, etc...).
                `,
            },
        ],
    },
    // ============= World Features
    {
        type: "featureSection",
        id: "world",
        name: "World",
        iconClasses: ["fa-duotone", "fa-island-tropical"],
    },
    {
        type: "featureRoot",
        id: "speedHack",
        name: "Speed Hack",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Run faster than the other mere mortals in your lobby. Now you can book it across Shoreline faster than Usain Bolt!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    <ul>
                        <li>If someone records you while you are using this feature you could get banned.</li>
                        <li>${BSGDetectionString}</li>
                    </ul>
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "lootThroughWalls",
        name: "Loot Through Walls",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Break Nikita's heart by looting anything and everything. Marked room is ripe for the taking! Oh yeah, you can fish dumped gear out of the water sometimes too... Just sayin!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    ${BSGDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "silentLoot",
        name: "Silent Loot",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature allows you to loot the best container item in your proximity without even looking at it!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>Risky</strong> feature:
                    <hr>
                    ${BSGDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "chams",
        name: "Chams",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    X-ray vision deployed, now you can be the ultimate Peeping Tom!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableCulling",
        name: "Disable Culling",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Culling is the reason players disappear on chams when they are not in line of sight. This feature disables culling allowing you to always see all players.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "fullBright",
        name: "Full Bright",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This makes the interior/exterior of buildings and other game objects fully bright. Not a shadow in sight when you're in a firefight!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableShadows",
        name: "Disable Shadows",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This completely disables all shadows. I noticed a nice FPS boost on Streets!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "alwaysDay",
        name: "Always Day",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Sick of night time? Just turn it off!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "sunnyWeather",
        name: "Sunny Weather",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Cloudy weather got you down? Open the curtains to a Sunnier Tarkov!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "disableGrass",
        name: "Disable Grass",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This removes grass from the map giving you a nice FPS boost!
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "instantPlant",
        name: "Instant Plant",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to instantly plant quest items.
                `,
            },
        ],
    },
    // ============= Miscellaneous Features
    {
        type: "featureSection",
        id: "miscellaneous",
        name: "Miscellaneous",
        iconClasses: ["fa-duotone", "fa-list-ul"],
    },
    {
        type: "featureRoot",
        id: "disableInventoryBlur",
        name: "Disable Inventory Blur",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This disables the background blur on the inventory screen.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "fovChanger",
        name: "FOV Changer",
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to set the FOV above the default maximum of 75!
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "noHeadBobbing",
        name: "No Head Bobbing",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This disables head bobbing.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "hideRaidCode",
        name: "Hide Raid Code",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This hides the text at the bottom left of the screen that contains the game version and raid code.
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "streamerMode",
        name: "Streamer Mode",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This spoofs all of your account information making it safe to stream while cheesing! (client side only)
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "keybindFromAnywhere",
        name: "Keybind From Anywhere",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature allows you to bind a hotkey to items outside of your tactical rig & pockets. For example, you can keep your grizzly inside your secure container and use it on a hotkey!
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "antiAFK",
        name: "Anti AFK",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This disables the AFK detection in the game allowing you to farm account hours without using a risky auto clicker/mouse jiggler.
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "autoGym",
        name: "Auto Gym",
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    Automatically completes your PMC's workout as fast as possible.
                `,
            },
        ],
    },
    // ============= PVE Features
    {
        type: "featureSection",
        privateOnly: true,
        id: "pve",
        name: "PVE",
        iconClasses: ["fa-duotone", "fa-robot"],
    },
    {
        type: "featureRoot",
        id: "harmlessAI",
        name: "Harmless AI",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature prevents all AI from engaging you.
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "noFallDamage",
        name: "No Fall Damage",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature prevents you from taking any fall damage.
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "gatherAI",
        name: "Gather AI",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature allows you to teleport all AI on the map to the location you were standing in when you pressed the hotkey. I strongly recommend enabling <code>Harmless AI</code> first!
                `,
            },
        ],
    },
    // ============= Ban Me Features
    {
        type: "featureSection",
        privateOnly: true,
        id: "banMe",
        name: "Ban Me",
        iconClasses: ["fa-duotone", "fa-gavel"],
    },
    {
        type: "featureRoot",
        id: "unmountMountedWeapon",
        name: "Unmount Weapon",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This allows you to unmount mounted weapons. Switch to your gun to release the mounted weapon.
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>BAN ME</strong> feature:
                    <hr>
                    ${BanMeDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "superSpeed",
        name: "Super Speed",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    It's time to dodge bullets and make Superman blush. Just don't go too fast or Nikita might give you a speeding ticket!
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>BAN ME</strong> feature:
                    <hr>
                    ${BanMeDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "characterManipulation",
        name: "Player Motion",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "settings",
            },
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature provides an alternative super speed method. You can also move up and down which allows you to do some unique things.
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>BAN ME</strong> feature:
                    <hr>
                    ${BanMeDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "showOwnDogTag",
        name: "Show Own Dog Tag",
        privateOnly: true,
        available: true,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    This feature allows you to see your own dog tag slot and remove it before death. This prevents anyone who kills you from being able to report you or view your profile!
                    <br>
                    ${RestartRequiredToFullyDisable}
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>BAN ME</strong> feature:
                    <hr>
                    ${BanMeDetectionString}
                `,
            },
        ],
    },
    {
        type: "featureRoot",
        id: "desync",
        name: "Desync",
        privateOnly: true,
        available: false,
        icons: [
            {
                type: "info",
                template: /*html*/ `
                    Feature Information:
                    <hr>
                    TODO
                `,
            },
            {
                type: "warning",
                template: /*html*/ `
                    This is a <strong>BAN ME</strong> feature:
                    <hr>
                    ${BanMeDetectionString}
                `,
            },
        ],
    },
];

export const featureSectionStateDefaults = [
    {
        id: "pmc",
        expanded: true,
    },
    {
        id: "camera",
        expanded: true,
    },
    {
        id: "weapon",
        expanded: true,
    },
    {
        id: "gear",
        expanded: true,
    },
    {
        id: "world",
        expanded: true,
    },
    {
        id: "miscellaneous",
        expanded: true,
    },
    {
        id: "pve",
        expanded: true,
    },
    {
        id: "banMe",
        expanded: true,
    },
];

export const featureStateDefaults = [
    {
        id: "noFall",
        enabled: false,
    },
    {
        id: "infiniteStamina",
        enabled: false,
    },
    {
        id: "enhancedJump",
        enabled: false,
    },
    {
        id: "enhancedThrow",
        enabled: false,
    },
    {
        id: "instantGrenades",
        enabled: false,
    },
    {
        id: "muleMode",
        enabled: false,
    },
    {
        id: "alwaysSprint",
        enabled: false,
    },
    {
        id: "360Freelook",
        enabled: false,
    },
    {
        id: "noInertia",
        enabled: false,
    },
    {
        id: "wideLean",
        enabled: false,
    },
    {
        id: "thirdPerson",
        enabled: false,
    },
    {
        id: "xRayVision",
        enabled: false,
    },
    {
        id: "noPenalties",
        enabled: false,
    },
    {
        id: "noVisor",
        enabled: false,
    },
    {
        id: "nightVision",
        enabled: false,
    },
    {
        id: "thermalVision",
        enabled: false,
    },
    {
        id: "instantZoom",
        enabled: false,
    },
    {
        id: "unlimitedSearch",
        enabled: false,
    },
    {
        id: "disableVignette",
        enabled: false,
    },
    {
        id: "disableExposure",
        enabled: false,
    },
    {
        id: "disableBloom",
        enabled: false,
    },
    {
        id: "disableColorGrading",
        enabled: false,
    },
    {
        id: "disableScreenEffects",
        enabled: false,
    },
    {
        id: "aimbot",
        enabled: false,
    },
    {
        id: "instantADS",
        enabled: false,
    },
    {
        id: "noMalfunctions",
        enabled: false,
    },
    {
        id: "customRecoil",
        enabled: false,
    },
    {
        id: "removableAttachments",
        enabled: false,
    },
    {
        id: "fastBulletLoadUnload",
        enabled: false,
    },
    {
        id: "fastWeaponOps",
        enabled: false,
    },
    {
        id: "speedHack",
        enabled: false,
    },
    {
        id: "lootThroughWalls",
        enabled: false,
    },
    {
        id: "silentLoot",
        enabled: false,
    },
    {
        id: "chams",
        enabled: false,
    },
    {
        id: "disableCulling",
        enabled: false,
    },
    {
        id: "fullBright",
        enabled: false,
    },
    {
        id: "disableShadows",
        enabled: false,
    },
    {
        id: "alwaysDay",
        enabled: false,
    },
    {
        id: "sunnyWeather",
        enabled: false,
    },
    {
        id: "disableGrass",
        enabled: false,
    },
    {
        id: "instantPlant",
        enabled: false,
    },
    {
        id: "disableFog",
        enabled: false,
    },
    {
        id: "disableInventoryBlur",
        enabled: false,
    },
    {
        id: "fovChanger",
        enabled: false,
    },
    {
        id: "noHeadBobbing",
        enabled: false,
    },
    {
        id: "hideRaidCode",
        enabled: false,
    },
    {
        id: "streamerMode",
        enabled: false,
    },
    {
        id: "keybindFromAnywhere",
        enabled: false,
    },
    {
        id: "antiAFK",
        enabled: false,
    },
    {
        id: "autoGym",
        enabled: false,
    },
    {
        id: "unmountMountedWeapon",
        enabled: false,
    },
    {
        id: "harmlessAI",
        enabled: false,
    },
    {
        id: "noFallDamage",
        enabled: false,
    },
    {
        id: "gatherAI",
        enabled: false,
    },
    {
        id: "superSpeed",
        enabled: false,
    },
    {
        id: "characterManipulation",
        enabled: false,
    },
    {
        id: "showOwnDogTag",
        enabled: false,
    },
    {
        id: "desync",
        enabled: false,
    }
];

export const featureToggleHotkeysDefaults = [
    {
        id: "noFall",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "enhancedJump",
        hotkey: "None",
    },
    {
        id: "enhancedThrow",
        hotkey: "None",
    },
    {
        id: "muleMode",
        hotkey: "None",
    },
    {
        id: "360Freelook",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "noInertia",
        hotkey: "None",
    },
    {
        id: "wideLean",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "thirdPerson",
        hotkey: "None",
    },
    {
        id: "xRayVision",
        hotkey: "None",
    },
    {
        id: "noVisor",
        hotkey: "None",
    },
    {
        id: "nightVision",
        hotkey: "None",
    },
    {
        id: "thermalVision",
        hotkey: "None",
    },
    {
        id: "disableVignette",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "disableExposure",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "disableBloom",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "disableColorGrading",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "aimbot",
        hotkey: "None",
    },
    {
        id: "instantADS",
        hotkey: "None",
    },
    {
        id: "customRecoil",
        hotkey: "None",
    },
    {
        id: "speedHack",
        hotkey: "None",
        privateOnly: true,
    },
    {
        id: "fullBright",
        hotkey: "None",
    },
    {
        id: "disableShadows",
        hotkey: "None",
    },
    {
        id: "alwaysDay",
        hotkey: "None",
    },
    {
        id: "sunnyWeather",
        hotkey: "None",
    },
    {
        id: "disableGrass",
        hotkey: "None",
    },
    {
        id: "disableFog",
        hotkey: "None",
    },
    {
        id: "desync",
        hotkey: "None",
        privateOnly: true,
    },
];

export const veryRiskyFeatures = [
    "unmountMountedWeapon",
    "superSpeed",
    "characterManipulation",
    "showOwnDogTag",
    "desync",
];

export const featureSettingsDefaults = [
    {
        id: "instantZoom_hotkey",
        featureID: "instantZoom",
        name: "Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "Mouse2",
    },
    {
        id: "instantZoom_FOV",
        featureID: "instantZoom",
        name: "Zoom FOV",
        uiType: "slider",
        dataType: "int",
        value: 45,
        valueSuffix: "",
        min: 5,
        max: 75,
        step: 1,
    },
    {
        id: "aimbot_hotkey",
        featureID: "aimbot",
        name: "Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "LeftControl",
    },
    {
        id: "aimbot_playerHitboxes",
        featureID: "aimbot",
        name: "Player Hitboxes",
        uiType: "hitboxConfigurator",
        dataType: "hitboxes",
        playerType: "Player",
        value: JSON.stringify({
            headHitboxChance: {
                chance: 20,
                smartTargeting: true,
                availableBones: ["Head", "Neck"],
                selectedBone: "Head",
                side: "",
            },
            thoraxHitboxChance: {
                chance: 20,
                smartTargeting: true,
                availableBones: ["Thorax"],
                selectedBone: "Thorax",
                side: "",
            },
            stomachHitboxChance: {
                chance: 20,
                smartTargeting: true,
                availableBones: ["Stomach"],
                selectedBone: "Stomach",
                side: "",
            },
            leftArmHitboxChance: {
                chance: 10,
                smartTargeting: true,
                availableBones: ["Shoulder", "Elbow", "Hand"],
                selectedBone: "Elbow",
                side: "left",
            },
            rightArmHitboxChance: {
                chance: 10,
                smartTargeting: true,
                availableBones: ["Shoulder", "Elbow", "Hand"],
                selectedBone: "Elbow",
                side: "right",
            },
            leftLegHitboxChance: {
                chance: 10,
                smartTargeting: true,
                availableBones: ["Hip", "Knee", "Ankle"],
                selectedBone: "Knee",
                side: "left",
            },
            rightLegHitboxChance: {
                chance: 10,
                smartTargeting: true,
                availableBones: ["Hip", "Knee", "Ankle"],
                selectedBone: "Knee",
                side: "right",
            },
        }),
    },
    {
        id: "aimbot_aiHitboxes",
        featureID: "aimbot",
        name: "AI Hitboxes",
        uiType: "hitboxConfigurator",
        dataType: "hitboxes",
        playerType: "AI",
        value: JSON.stringify({
            headHitboxChance: {
                chance: 100,
                smartTargeting: true,
                availableBones: ["Head", "Neck"],
                selectedBone: "Head",
                side: "",
            },
            thoraxHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Thorax"],
                selectedBone: "Thorax",
                side: "",
            },
            stomachHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Stomach"],
                selectedBone: "Stomach",
                side: "",
            },
            leftArmHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Shoulder", "Elbow", "Hand"],
                selectedBone: "Elbow",
                side: "left",
            },
            rightArmHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Shoulder", "Elbow", "Hand"],
                selectedBone: "Elbow",
                side: "right",
            },
            leftLegHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Hip", "Knee", "Ankle"],
                selectedBone: "Knee",
                side: "left",
            },
            rightLegHitboxChance: {
                chance: 0,
                smartTargeting: true,
                availableBones: ["Hip", "Knee", "Ankle"],
                selectedBone: "Knee",
                side: "right",
            },
        }),
    },
    {
        id: "aimbot_targetingMode",
        featureID: "aimbot",
        name: "Targeting Mode",
        uiType: "toggleButtons",
        dataType: "TargetingMode",
        value: "Smart",
        values: ["Smart", "CQB", "Crosshair"],
    },
    {
        id: "aimbot_alwaysOn",
        featureID: "aimbot",
        name: "Always On",
        uiType: "checkbox",
        dataType: "bool",
        info: "When enabled, the aimbot will always be active looking for targets - there is no need to press the hotkey.",
        value: false,
    },
    {
        id: "aimbot_visibilityCheck",
        featureID: "aimbot",
        name: "Visibility Check",
        uiType: "checkbox",
        dataType: "bool",
        info: "When enabled, the aimbot will only lock onto a player if you have line of sight on them.",
        value: false,
    },
    {
        id: "aimbot_TargetTeammates",
        featureID: "aimbot",
        name: "Target Teammates",
        uiType: "checkbox",
        dataType: "bool",
        value: false,
    },
    {
        id: "aimbot_maxDistance",
        featureID: "aimbot",
        name: "Max Distance",
        uiType: "slider",
        dataType: "float",
        value: 450,
        valueSuffix: "m",
        min: 100,
        max: 3000,
        step: 10,
    },
    {
        id: "aimbot_fov",
        featureID: "aimbot",
        name: "FOV",
        uiType: "slider",
        dataType: "float",
        value: 30,
        min: 10,
        max: 360,
        step: 5,
    },
    {
        id: "normalSpeedHack_speed",
        featureID: "speedHack",
        name: "Speed Multiplier",
        uiType: "slider",
        dataType: "float",
        value: 1.0,
        valueSuffix: "x",
        min: 1.0,
        max: 1.2,
        step: 0.1,
    },
    {
        id: "enhancedJump_StrengthBuffJumpHeightInc",
        featureID: "enhancedJump",
        name: "Jump Height Multiplier",
        uiType: "slider",
        dataType: "float",
        value: 0.1,
        valueSuffix: "x",
        min: 0.1,
        max: 0.2,
        step: 0.1,
    },
    {
        id: "enhancedThrow_StrengthBuffThrowDistanceInc",
        featureID: "enhancedThrow",
        name: "Throw Distance Multiplier",
        uiType: "slider",
        dataType: "float",
        value: 0.1,
        valueSuffix: "x",
        min: 0.1,
        max: 0.9,
        step: 0.1,
    },
    {
        id: "wideLean_horizontalDistance",
        featureID: "wideLean",
        name: "Horizontal Distance",
        uiType: "slider",
        dataType: "float",
        value: 0.1,
        valueSuffix: "",
        min: 0.1,
        max: 0.2,
        step: 0.1,
    },
    {
        id: "wideLean_verticalDistance",
        featureID: "wideLean",
        name: "Vertical Distance",
        uiType: "slider",
        dataType: "float",
        value: 0.1,
        valueSuffix: "",
        min: 0.1,
        max: 0.2,
        step: 0.1,
    },
    {
        id: "wideLean_verticalAbove_hotkey",
        featureID: "wideLean",
        name: "Air Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "wideLean_verticalBelow_hotkey",
        featureID: "wideLean",
        name: "Ground Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "tpp_horizontalDistance",
        featureID: "thirdPerson",
        name: "Camera Distance",
        uiType: "slider",
        dataType: "float",
        value: 1.3,
        min: 0.3,
        max: 3.0,
        step: 0.01,
    },
    {
        id: "tpp_verticalDistance",
        featureID: "thirdPerson",
        name: "Camera Height",
        uiType: "slider",
        dataType: "float",
        value: 0.6,
        min: -0.7,
        max: 2.0,
        step: 0.01,
    },
    {
        id: "tpp_horizontalOffset",
        featureID: "thirdPerson",
        name: "Camera Offset",
        uiType: "slider",
        dataType: "float",
        value: -0.25,
        min: -3.0,
        max: 3.0,
        step: 0.01,
    },
    {
        id: "xRayVision_nearClipPlane",
        featureID: "xRayVision",
        name: "Distance",
        uiType: "slider",
        dataType: "float",
        value: 0.03,
        valueSuffix: "m",
        min: 0.03,
        max: 10,
        step: 0.01,
    },
    {
        id: "customRecoil_AlwaysAim",
        featureID: "customRecoil",
        name: "Always Aim",
        uiType: "checkbox",
        dataType: "bool",
        value: false,
    },
    {
        id: "customRecoil_ShotIntensity",
        featureID: "customRecoil",
        name: "Recoil Amount",
        uiType: "slider",
        dataType: "float",
        value: 1.0,
        valueSuffix: "x",
        min: 0.0,
        max: 1.0,
        step: 0.01,
    },
    {
        id: "customRecoil_BreathIntensity",
        featureID: "customRecoil",
        name: "Sway Amount",
        uiType: "slider",
        dataType: "float",
        value: 1.0,
        valueSuffix: "x",
        min: 0.0,
        max: 1.0,
        step: 0.01,
    },
    {
        id: "customRecoil_DisableWeaponInertia",
        featureID: "customRecoil",
        name: "Disable Weapon Inertia",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables the root weapon movement caused by strafing and moving the weapon. Root weapon movement makes it harder to be accurate with legit aim.",
        value: false,
    },
    {
        id: "silentLoot_hotkey",
        featureID: "silentLoot",
        name: "Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "silentLoot_distance",
        featureID: "silentLoot",
        name: "LTW Distance",
        uiType: "slider",
        dataType: "float",
        info: "If you are around 2.5m away from the item and it is failing to be picked up change this setting and try again. If the item is large you should increase the value, if the item is small you should reduce the value. The default value of 0.3 works fine for most items.",
        value: 0.3,
        valueSuffix: "m",
        min: 0.1,
        max: 1.0,
        step: 0.1,
    },
    // {
    //     id: "chams_Mode",
    //     featureID: "chams",
    //     name: "Rendering Mode",
    //     uiType: "toggleButtons",
    //     dataType: "ChamsMode",
    //     info: /*html*/`
    //         <code>Always</code> mode shows the player's whole body regardless of whether it's visible or not. It does not distinguish between visible and invisible areas of the body.
    //         <br>
    //         <code>Vis Check</code> mode distinguishes between visible and invisible areas of the body rendering the colors you configure.
    //         <br>
    //         <code>Visible</code> mode only shows the visible parts of the player's body.
    //     `,
    //     value: "Vis Check",
    //     values: ["Always", "Vis Check", "Visible"],
    // },
    {
        id: "chams_LocalPlayerChams",
        featureID: "chams",
        name: "Local Player Chams",
        uiType: "checkbox",
        dataType: "bool",
        info: "Whether or not your player will have wireframe chams applied.",
        value: false,
    },
    {
        id: "chams_AimbotTarget",
        featureID: "chams",
        name: "Aimbot Target Chams",
        uiType: "checkbox",
        dataType: "bool",
        info: "The player you are currently targeting with the aimbot will have different chams colors applied.",
        value: false,
    },
    {
        id: "chams_LocalPlayer",
        featureID: "chams",
        name: "Local Player",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_LocalPlayer_visible",
        color_visible: "#ff0000",
        id_invisible: "chams_LocalPlayer_invisible",
        color_invisible: "#000000",
    },
    {
        id: "chams_AimbotLocked",
        featureID: "chams",
        name: "Aimbot Target",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_AimbotLocked_visible",
        color_visible: "#ff0000",
        id_invisible: "chams_AimbotLocked_invisible",
        color_invisible: "#000000",
    },
    {
        id: "chams_Teammate",
        featureID: "chams",
        name: "Teammate",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_Teammate_visible",
        color_visible: "#00ff00",
        id_invisible: "chams_Teammate_invisible",
        color_invisible: "#00ff00",
    },
    {
        id: "chams_EnemyPMC",
        featureID: "chams",
        name: "Enemy PMC",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_EnemyPMC_visible",
        color_visible: "#ff6a6a",
        id_invisible: "chams_EnemyPMC_invisible",
        color_invisible: "#ff0000",
    },
    {
        id: "chams_PlayerScav",
        featureID: "chams",
        name: "Player Scav",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_PlayerScav_visible",
        color_visible: "#ffffa7",
        id_invisible: "chams_PlayerScav_invisible",
        color_invisible: "#ffff00",
    },
    {
        id: "chams_ScavBoss",
        featureID: "chams",
        name: "Boss",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_ScavBoss_visible",
        color_visible: "#d272c4",
        id_invisible: "chams_ScavBoss_invisible",
        color_invisible: "#e617c8",
    },
    {
        id: "chams_Scav",
        featureID: "chams",
        name: "AI Scav",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_Scav_visible",
        color_visible: "#ffb785",
        id_invisible: "chams_Scav_invisible",
        color_invisible: "#ff6900",
    },
    {
        id: "chams_Default",
        featureID: "chams",
        name: "Other AI",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_Default_visible",
        color_visible: "#8f8fff",
        id_invisible: "chams_Default_invisible",
        color_invisible: "#0000ff",
    },
    {
        id: "chams_Corpse",
        featureID: "chams",
        name: "Corpses",
        uiType: "visCheckColorPicker",
        dataType: "VisCheckColor",
        id_visible: "chams_Corpse_visible",
        color_visible: "#999999",
        id_invisible: "chams_Corpse_invisible",
        color_invisible: "#999999",
    },
    {
        id: "fullBright_brightness",
        featureID: "fullBright",
        name: "Brightness",
        uiType: "slider",
        dataType: "float",
        value: 1.0,
        min: 0.1,
        max: 1.0,
        step: 0.1,
    },
    {
        id: "alwaysDay_Hour",
        featureID: "alwaysDay",
        name: "Hour",
        uiType: "slider",
        dataType: "float",
        value: 12,
        min: 1,
        max: 24,
        step: 1,
    },
    {
        id: "fovChanger_FOV",
        featureID: "fovChanger",
        name: "FOV",
        uiType: "slider",
        dataType: "int",
        value: 75,
        min: 75,
        max: 135,
        step: 1,
    },
    {
        id: "fovChanger_AimFOV",
        featureID: "fovChanger",
        name: "ADS FOV",
        uiType: "slider",
        dataType: "int",
        value: 35,
        min: 5,
        max: 135,
        step: 1,
    },
    {
        id: "fovChanger_TppFOV",
        featureID: "fovChanger",
        name: "Third Person FOV",
        uiType: "slider",
        dataType: "int",
        value: 75,
        min: 75,
        max: 135,
        step: 1,
    },
    {
        id: "disableScreenEffects_NoFlash",
        featureID: "disableScreenEffects",
        name: "No Flash",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables the blindness effect when you are exposed to an extreme flash of light.",
        value: false,
    },
    {
        id: "disableScreenEffects_NoBlood",
        featureID: "disableScreenEffects",
        name: "No Blood",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables the blood drops effect when you are bleeding/injured.",
        value: false,
    },
    {
        id: "disableScreenEffects_NoSharpen",
        featureID: "disableScreenEffects",
        name: "No Sharpen",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables the painkiller screen sharpening effect.",
        value: false,
    },
    {
        id: "disableScreenEffects_NoBlur",
        featureID: "disableScreenEffects",
        name: "No Blur",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables the screen blurriness effect when you take damage.",
        value: false,
    },
    {
        id: "streamerMode_SpoofName",
        featureID: "streamerMode",
        name: "Spoof Name",
        uiType: "checkbox",
        dataType: "bool",
        value: false,
    },
    {
        id: "streamerMode_SpoofLevel",
        featureID: "streamerMode",
        name: "Spoof Level",
        uiType: "checkbox",
        dataType: "bool",
        value: false,
    },
    {
        id: "streamerMode_SpoofDogtags",
        featureID: "streamerMode",
        name: "Spoof Dogtags",
        uiType: "checkbox",
        dataType: "bool",
        value: false,
    },
    {
        id: "streamerMode_HideOverallInfo",
        featureID: "streamerMode",
        name: "Hide Overall Info",
        uiType: "checkbox",
        dataType: "bool",
        info: "Hides all of the information on the <code>Overall</code> profile tab.",
        value: false,
    },
    {
        id: "streamerMode_DisableNotifications",
        featureID: "streamerMode",
        name: "Disable Notifications",
        uiType: "checkbox",
        dataType: "bool",
        info: "Disables all in-game notifications. Notifications show friend names and possibly other incriminating information.",
        value: false,
    },
    {
        id: "gatherAI_hotkey",
        featureID: "gatherAI",
        name: "Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "superSpeed_hotkey",
        featureID: "superSpeed",
        name: "Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "superSpeed_Speed",
        featureID: "superSpeed",
        name: "Speed Multiplier",
        uiType: "slider",
        dataType: "float",
        value: 8,
        valueSuffix: "x",
        min: 2,
        max: 28,
        step: 0.2,
    },
    {
        id: "superSpeed_OnTime",
        featureID: "superSpeed",
        name: "On Time",
        uiType: "slider",
        dataType: "int",
        value: 90,
        valueSuffix: "ms",
        min: 20,
        max: 100,
        step: 5,
    },
    {
        id: "superSpeed_OffTime",
        featureID: "superSpeed",
        name: "Off Time",
        uiType: "slider",
        dataType: "int",
        value: 220,
        valueSuffix: "ms",
        min: 200,
        max: 600,
        step: 5,
    },
    {
        id: "characterManipulationUp_hotkey",
        featureID: "characterManipulation",
        name: "Up Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "characterManipulationDown_hotkey",
        featureID: "characterManipulation",
        name: "Down Hotkey",
        uiType: "hotkey",
        dataType: "UnityKeyCode", // Will be transformed to an int that can be cast to it's actual enum type on the backend
        value: "None",
    },
    {
        id: "characterManipulation_Distance",
        featureID: "characterManipulation",
        name: "Distance",
        uiType: "slider",
        dataType: "float",
        value: 1,
        valueSuffix: "x",
        min: 1,
        max: 10,
        step: 1,
    },
];