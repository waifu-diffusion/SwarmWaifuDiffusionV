# SwarmUI Waifu Diffusion V Extension

An extension adding support for wdV models to [SwarmUI](https://github.com/mcmonkeyprojects/SwarmUI)

## Features
- Adds `WaifuEDM` controls to SwarmUI "Generate" tab
- Installs the custom comfy model sampling node for you

## Installation tl;dr
clone the repo to `<swarmui folder>/src/Extensions/SwarmWaifuDiffusionV` and run the Swarm updater script for your OS.

## Installation step-by-step (Swarm self-start ComfyUI)

From within your SwarmUI folder, run these commands:

### Windows users (PowerShell):
```pwsh
New-Item -ItemType Directory -Force -ErrorAction SilentlyContinue .\src\Extensions
cd .\src\Extensions
git clone https://github.com/waifu-diffusion/SwarmWaifuDiffusionV.git
cd ..\..\
.\update-windows.bat
```

### Linux and macOS users:
```sh
mkdir -p src/Extensions
cd src/Extensions
git clone https://github.com/waifu-diffusion/SwarmWaifuDiffusionV.git
cd ../../
./update-linuxmac.sh
```

Launch SwarmUI as you usually would, and you should now have a nice new `WaifuEDM` option set in the main UI.

## Installation step-by-step (Swarm with externally-started ComfyUI)
**Note: You are almost certainly _not_ doing this. If you're not sure, just follow the self-start instructions.**

Follow the self-start instructions above, but you will also need to copy [wdv_test_nodes.py](./WaifuNodes/wdv_test_nodes.py) into your ComfyUI's `custom_nodes` folder (the extension does this automatically for swarm-managed/self-started Comfy instances.)
