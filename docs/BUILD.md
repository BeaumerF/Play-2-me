#Build

## Build GamingAnywhere
[Source](http://gaminganywhere.org/doc/quick_start.html)

### Prerequisites

- Visual Studio (C++) 2010 (we did not test it with other versions)
- DirectX SDK (we use the version released in June, 2010) into `C:\Microsoft DirectX SDK`
- Microsoft SDK (we have version 7.0 installed on Windows 7, it may be installed with VS 2010)
- Microsoft Visual C++ 2010 Redistributable Package

### Steps

- Install dependencies by running `install.cmd` in 'deps.pkgs.win32' directory.
- Open Visual C++ command line prompt.
- Build GA by running `nmake /f NMakefile all` command in the 'ga' directory.
- Install GA by running `nmake /f NMakefile install` command in the 'ga' directory. All the generated files will be installed into 'bin' directory.


## Build Play 2 me Server & Client

### Use Visual Studio 2015
