@echo off

copy "..\Publish\Tarkov DMA Backend.exe" "..\Publish\Tarkov DMA Backend_raw.exe"
copy "..\Publish\Tarkov DMA Backend_raw.map" "..\Publish\Tarkov DMA Backend_protected.map"

timeout /t 1 /nobreak >nul

start /wait "" "C:\Program Files\Code Virtualizer\Virtualizer.exe" /protect "Tarkov DMA Backend.cv"

timeout /t 1 /nobreak >nul

start /wait "" "C:\Program Files\VMProtect Ultimate\VMProtect_Con.exe" "..\Publish\Tarkov DMA Backend_protected.exe" -pf "Tarkov DMA Backend.vmp"

timeout /t 1 /nobreak >nul

if not exist "C:\Users\microPower\Documents\GitHub\Tarkov-DMA-Frontend\src\bin" (
    mkdir "C:\Users\microPower\Documents\GitHub\Tarkov-DMA-Frontend\src\bin"
)

copy "..\Publish\Tarkov DMA Backend_protected.vmp.exe" "C:\Users\microPower\Documents\GitHub\Tarkov-DMA-Frontend\src\bin\backend.exe"
copy "..\UpdateHelper\Publish\UpdateHelper.exe" "C:\Users\microPower\Documents\GitHub\Tarkov-DMA-Frontend\src\bin\UpdateHelper.exe"

cd "C:\Users\microPower\Documents\GitHub\Tarkov-DMA-Frontend"

timeout /t 1 /nobreak >nul

npm run build-windows