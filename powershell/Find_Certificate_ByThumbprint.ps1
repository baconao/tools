$thumbPrint = 'e37fdee44921a9a246dc28d944d05581d96a710f'
$certs = Get-ChildItem -Path cert: -Recurse | Where-Object { $_.Thumbprint -eq $thumbPrint } 
Write-Host $certs