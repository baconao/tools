
Clear-Host

Get-ChildItem -Recurse -Force "c:\temp" | Where-Object{$_.PsIsContainer} | ForEach-Object {
	$folderName = $_.FullName
	$acl = Get-Acl -Path $folderName
	$acl.SetAccessRuleProtection($false, $true)
	Set-Acl $folderName $acl 	
}