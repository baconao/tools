function Uninstall
{
	param ($code)
	$args = '/x ' + $code + ' /qn REBOOT=ReallySuppress XMLCONFIG=' + $env:TEMP + '\parameters.xml'
	$process = (Start-Process -FilePath msiexec -ArgumentList $args -Wait -PassThru)
	Write-Host 'Uninstall of ' $code  ' completed with code: ' $process.ExitCode
}
$productList = @('{2B69A9C4-E331-4AD2-90CA-AF6ACFA7836F}', 'DF8E75BF-A5E1-48D8-983D-68A0600F3060')
foreach($prod in $productList) {
	Uninstall -code $prod
}