REM Resetting Maker Den
cd \
cd Source
cd MakerDen\MakerDen-MAX
REM Cleaning up
git reset --hard
start devenv /resetsettings c:\source\MakerDenSettings.vssettings c:\source\makerden\MakerDen-MAX\MakerDen-MAX.sln

