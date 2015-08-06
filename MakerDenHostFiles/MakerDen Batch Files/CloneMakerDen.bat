REM Resetting Maker Den
cd \
cd Source
REM Cleaning up
rd /s /q MakerDen
REM Getting the source code
git clone https://github.com/gloveboxes/MakerDen-MAX.git .\MakerDen
cd MakerDen\MakerDen-MAX
start devenv /resetsettings c:\source\MakerDenSettings.vssettings c:\source\makerden\MakerDen-MAX\MakerDen-MAX.sln

