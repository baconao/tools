Clear-Host

$registryKey = (Get-Item -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" | Get-ChildItem )
$registryKey | Get-ItemProperty  -Name DisplayName 


