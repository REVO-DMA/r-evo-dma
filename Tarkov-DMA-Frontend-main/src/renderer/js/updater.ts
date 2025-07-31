import { RendererProcessIpc } from "electron-better-ipc";
import { ReleaseChannel, UpdaterButtonIconType, UpdaterButtonPressAction, UpdaterStatus } from "../../main/updater/updaterShared";
import { DEV_MODE } from "../env";

export class RendererUpdater
{
    private ipc: RendererProcessIpc;

    private button: HTMLDivElement;

    private buttonIcon: HTMLDivElement;
    private buttonText: HTMLDivElement;

    private buttonPressAction: UpdaterButtonPressAction = UpdaterButtonPressAction.CheckForUpdate;

    private autoUpdateActive: boolean = false;

    constructor(ipc: RendererProcessIpc)
    {
        this.ipc = ipc;

        this.button = (document.getElementById("updaterButton") as HTMLDivElement);

        this.buttonIcon = (document.getElementById("updaterButtonIcon") as HTMLDivElement);
        this.buttonText = (document.getElementById("updaterButtonText") as HTMLDivElement);

        this.BindUpdaterEvents();
        this.BindReleaseChannel();

        setInterval(() => {
            if (this.buttonPressAction !== UpdaterButtonPressAction.CheckForUpdate)
                return;
    
            this.CheckForUpdate();
        }, 60000);
    }

    private BindUpdaterEvents()
    {
        const updatesModalToggleEl = document.getElementById("updatesModalToggle");
        const updatesModalEl = document.getElementById("updatesModal");

        this.button.addEventListener("click", () => {
            if (this.buttonPressAction === UpdaterButtonPressAction.CheckForUpdate)
            {
                this.CheckForUpdate();
            }
            else if (this.buttonPressAction === UpdaterButtonPressAction.DownloadUpdate)
            {
                this.buttonPressAction = UpdaterButtonPressAction.None;

                this.SetButtonIcon(UpdaterButtonIconType.Sync, true);
                this.SetButtonText("Preparing");

                this.ipc.callMain("download-update");
            }
            else if (this.buttonPressAction === UpdaterButtonPressAction.InstallUpdate)
            {
                this.buttonPressAction = UpdaterButtonPressAction.None;
                
                this.SetButtonIcon(UpdaterButtonIconType.Gear, true);
                this.SetButtonText("Installing Update");
                
                this.ipc.callMain("install-update");
            }
        });

        updatesModalToggleEl.addEventListener("click", () => {
            updatesModalToggleEl.classList.remove("attentionGrabber");
            document.getElementById("updatesModalToggleIcon").classList.remove("fa-spin");
        });

        this.ipc.answerMain("updater-status", (updaterStatus: UpdaterStatus) => {
            if (updaterStatus === UpdaterStatus.CheckingForUpdate)
            {
                this.SetButtonIcon(UpdaterButtonIconType.Sync, true);
                this.SetButtonText("Checking For Update");
            }
            else if (updaterStatus === UpdaterStatus.UpdateAvailable)
            {
                // Only apply these styles if the modal isn't already open
                if (updatesModalEl.style.display !== "")
                {
                    updatesModalToggleEl.classList.add("attentionGrabber");
                    document.getElementById("updatesModalToggleIcon").classList.add("fa-spin");
                }

                this.SetButtonIcon(UpdaterButtonIconType.Download);
                this.SetButtonText("Download Update");

                this.buttonPressAction = UpdaterButtonPressAction.DownloadUpdate;

                if (this.autoUpdateActive) this.button.click();
            }
            else if (updaterStatus === UpdaterStatus.NoUpdateAvailable)
            {
                this.ResetButton();
            }
            else if (updaterStatus === UpdaterStatus.UpdateDownloadComplete)
            {
                this.SetButtonIcon(UpdaterButtonIconType.Gear);
                this.SetButtonText("Install Update");

                this.buttonPressAction = UpdaterButtonPressAction.InstallUpdate;

                if (this.autoUpdateActive) this.button.click();
            }
        });

        this.ipc.answerMain("updater-error", (err: Error) => {
            console.error("[UPDATER] ERROR:", err);

            this.ResetButton();
        });

        this.ipc.answerMain("updater-download-progress", (progress: string) => {
            this.SetButtonIcon(UpdaterButtonIconType.Download);
            this.SetButtonText(`Downloading: ${progress}`);
        });
    }

    private SetButtonIcon(icon: UpdaterButtonIconType, spin: boolean = false)
    {
        this.buttonIcon.innerHTML = RendererUpdater.GetIcon(icon, true, spin);
    }
    
    private SetButtonText(text: string)
    {
        this.buttonText.innerText = text;
    }

    private static GetIcon(icon: UpdaterButtonIconType, margin: boolean = false, spin: boolean = false)
    {
        if (icon === UpdaterButtonIconType.Sync)
        {
            return /*html*/`
                <i class="fa-duotone fa-arrows-rotate ${margin ? "me-2" : ""} ${spin ? "fa-spin" : ""}"></i>
            `;
        }
        else if (icon === UpdaterButtonIconType.Download)
        {
            return /*html*/`
                <i class="fa-duotone fa-solid fa-download ${margin ? "me-2" : ""}"></i>
            `;
        }
        else if (icon === UpdaterButtonIconType.Gear)
        {
            return /*html*/`
                <i class="fa-duotone fa-solid fa-gear ${margin ? "me-2" : ""} ${spin ? "fa-spin" : ""}"></i>
            `;
        }
    }

    private BindReleaseChannel()
    {
        const selectedReleaseChannelEl = (document.getElementById("selectedReleaseChannel") as HTMLInputElement);

        const updateChannelDescription_LatestEl = (document.getElementById("updateChannelDescription_Latest") as HTMLDivElement);
        const updateChannelDescription_BetaEl = (document.getElementById("updateChannelDescription_Beta") as HTMLDivElement);
        const updateChannelDescription_AlphaEl = (document.getElementById("updateChannelDescription_Alpha") as HTMLDivElement);

        const hideAllDescriptions = () => {
            updateChannelDescription_LatestEl.style.display = "none";
            updateChannelDescription_BetaEl.style.display = "none";
            updateChannelDescription_AlphaEl.style.display = "none";
        };

        const activateReleaseChannel = (newChannel: ReleaseChannel) => {
            hideAllDescriptions();

            if (newChannel === "latest")
            {
                updateChannelDescription_LatestEl.style.display = "";
            }
            else if (newChannel === "beta")
            {
                updateChannelDescription_BetaEl.style.display = "";
            }
            else if (newChannel === "alpha")
            {   
                updateChannelDescription_AlphaEl.style.display = "";
            }

            this.ipc.callMain("set-update-channel", newChannel);
        };

        // Load saved value
        {
            if (localStorage.getItem("updater_selectedReleaseChannel") == null)
            {
                localStorage.setItem("updater_selectedReleaseChannel", "latest");
            }
            
            const savedValue = (localStorage.getItem("updater_selectedReleaseChannel") as ReleaseChannel);
            selectedReleaseChannelEl.value = savedValue;
            activateReleaseChannel(savedValue);
        }

        selectedReleaseChannelEl.addEventListener("change", () => {
            const newValue = (selectedReleaseChannelEl.value as ReleaseChannel);
            localStorage.setItem("updater_selectedReleaseChannel", newValue);

            activateReleaseChannel(newValue);
            this.ResetButton();
        });
    }

    private ResetButton()
    {
        this.buttonPressAction = UpdaterButtonPressAction.CheckForUpdate;

        this.SetButtonIcon(UpdaterButtonIconType.Sync);
        this.SetButtonText("Check For Update");

        this.autoUpdateActive = false;
    }

    private CheckForUpdate()
    {
        if (DEV_MODE) return;

        this.buttonPressAction = UpdaterButtonPressAction.None;

        this.ipc.callMain("check-for-update");
    }

    public RunAutoUpdate()
    {
        this.autoUpdateActive = true;

        this.button.click();
    }
}