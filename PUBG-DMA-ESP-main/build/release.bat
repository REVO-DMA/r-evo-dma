@echo off

start /wait "" dotnet publish "..\PUBG DMA ESP.sln" -p:PublishProfile=..\Properties\PublishProfiles\FolderProfile.pubxml

start /wait "" "C:\Program Files\VMProtect Ultimate\VMProtect_Con.exe" "..\Publish\PUBG DMA ESP.exe" -pf "PUBG DMA ESP.vmp"