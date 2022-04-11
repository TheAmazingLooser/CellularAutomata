
# Cellular Automata
This repository contains C#-Code for different implementation of cellular automata.
The rendering is powered by the awesome [FNA](https://github.com/FNA-XNA/FNA) framework.

<img src="/Images/CyclicCellularAutoamta_5_5_29.gif" width="250" height="250" /> <img src="/Images/CyclicCellularAutoamta_5_5_29_2.gif" width="250" height="250" />
> GIFs were recorded in a 750x750 game resulution and were sped up by the recording. You mostlikely will not get such smooth animations.

# Prerequisites

 - This project uses [.NET 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0). You can install it [here](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) incase you dont have it already.
 - FNA has the requirement to download their precompiled libraries. You can download them [here](https://fna.flibitijibibo.com/archive/fnalibs.tar.bz2). (You might need a 3rd party program to extract this tar.bz2 archieve)

# How to compile

 - Clone the git repository with `git clone --recursive https://github.com/YoungFlyme/CellularAutomata.git` into a folder of your choice.
 - Open the project solution in any IDE of your choise (JetBrains Rider was used for the developement of this very project). Visual Studio Community-Edition should suffice.
 - Compile the code
 - Go to the output-folder of the compiler (`./bin/x64/Debug/net5.0/` if not changed in the project settings)
 - Copy the needed downloaded FNA-Libraries in the output (depending on your OS. Windows needs the libraries located in the x64-folder of the tar.bz2 archieve for a 64bit compiled executable (x86 for 32bit))
 - Either start a debug-session or launch the exe in the build-output-folder.
# Controls
I added a bunch of controls to change some parameter on how the cyclic cellular automata uses/calculates their data.
 - `CTRL + "+"` or `CTRL + "-"` - Increases or decreases the threshold of the calculations. (This option changes the visualisation the most)
 - `SHIFT + "+"` or `SHIFT + "-"` - Increases or decreases the neighbour distance. Decrease it incase the windows lags. Too low values can impact the visualisation (simulation seems to stop working).
 - `+` or `-` (in combination with every other Key) - Increases or decreases the amount of States. Default is 5. Cannot get lower the 2. While using 5 States a handpicked Color-Palette is used.
 - `Tab` - Reinitializes all cells on the grid with random states and apply a new 5-State Color-Palette randomly out of currently 3.
### Notes
 - The simulation can lag with some settings. Whilst lagging, the input is delayed badly. You might need to press keys longer then usual to let the program know of the input.
 - The `+` and `-` keys from the numpad are not counted as valid input (they dont work)

