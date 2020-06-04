
Clear-Host

try
{

	$e = (Start-Process -FilePath "msiexec" -ArgumentList "/i c:\temp\pepe.msi /qn" -Wait -PassThru)
	if($e.ExitCode -ne 0) 	{	Write-Host "broken with " $e.ExitCode  ;	exit 1  	}			
	Write-Output "good !"

}
catch  { 
 	Write-Error "Something went wrong" -ErrorAction Stop 	
}