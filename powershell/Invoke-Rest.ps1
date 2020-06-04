
Clear-Host

$credential = Get-Credential -UserName baconao -Message Username

Invoke-RestMethod -Method Delete -Uri https://api.bintray.com/packages/baconao/tools/ExportFIT/versions/1.0.0.0 -Credential $credential

$file = 'c:\personal\gpx\ExportFit\ExportFit\bin\Release\app.publish\ExportFit.application'
Invoke-RestMethod -Method Put -Uri https://api.bintray.com/content/baconao/tools/ExportFIT/1.0.0.0/ExportFit.application -InFile $file -Credential $credential 

Invoke-RestMethod -Method Post -Uri https://api.bintray.com/content/baconao/tools/ExportFIT/1.0.0.0/publish -Credential $credential 