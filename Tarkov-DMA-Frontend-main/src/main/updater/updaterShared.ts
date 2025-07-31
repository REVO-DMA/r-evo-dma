export enum UpdaterStatus
{
    CheckingForUpdate,
    UpdateAvailable,
    NoUpdateAvailable,
    UpdateDownloadComplete,
}

export enum UpdaterButtonPressAction {
    CheckForUpdate,
    DownloadUpdate,
    InstallUpdate,
    None,
}

export enum UpdaterButtonIconType
{
    Sync,
    Download,
    Gear,
}

export type ReleaseChannel = "latest" | "beta" | "alpha";