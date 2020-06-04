Clear-Host

openfiles  /query /fo csv

$folder = "$env:SrcRootPath" 
Write-Output "Looking for " $folder
openfiles  /query /fo csv | 
        Where-Object   { $_.ToLower().Contains( "$env:SrcRootPath".ToLower() ) } |
    ForEach-Object { $_.Split( ',' )[0] }  |
    ForEach-Object { 
		$id = $_ -replace '\"', ''
		Write-Output "Closing: " $id
		net file  $id /CLOSE
		}
	
	
