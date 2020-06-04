Clear-Host
$computer = $Env:COMPUTERNAME
netdom verify $computer
if(!$?) { netdom reset $computer }

