Clear-Host
$serverName = 'localhost'
Get-Service -ComputerName $serverName -DisplayName "Monitoring *"  | ForEach-Object { Stop-Service -InputObject $_ -Verbose ; Start-Sleep 2 ; Start-Service -InputObject $_ -Verbose } 

