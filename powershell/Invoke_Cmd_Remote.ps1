Clear-Host
$s = new-pssession -computername RDDP-VM21
$command = {$res = invoke-command ping 127.0.0.1 }
$job = invoke-command -session $s -JobName pepe -ScriptBlock $command
do
{
	Write-Output $job.State
}
while ($job.State -ne "Completed")
	
Write-Output $job.State.value__
	
Remove-PSSession $s


