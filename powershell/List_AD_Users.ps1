###################################
# Get-MyGroupMembersRecursive.ps1 #
# Created by Hugo Peeters         #
# http://www.peetersonline.nl     #
###################################



param($ParentGroupNames = "AllJiraUsers")

If ($null -eq (Get-PSSnapin "Quest.ActiveRoles.ADManagement" -ErrorAction SilentlyContinue)) {
	Add-PSSnapin "Quest.ActiveRoles.ADManagement"
}



Clear-Host	

$Global:myCol = @()

function Indent
	{
	param([Int]$Level)
	$Global:Indent = $null
	For ($x = 1 ; $x -le $Level ; $x++)
		{
		$Global:Indent += "`t"
		}
	}

function Get-MySubGroupMembersRecursive
	{
	param($DNs,$connection)
	
	ForEach ($DN in $DNs)
		{
		$Object = Get-QADObject $DN -Connection $connection
		If ($Object.Type -eq "Group")
			{
			$i++
			Indent $i
			Write-Host ("{0}{1}" -f $Indent,$Object.DisplayName) -ForegroundColor "blue"
			$Group = Get-QADGroup $DN -Connection $connection
			If ($Group.Members.Length -ge 1)
				{
				Get-MySubGroupMembersRecursive $Group.Members $connection
				}
			$i--
			Indent $i
			Clear-Variable Group -ErrorAction SilentlyContinue
			}
		Else
			{
			$userfound = Get-QADUser $DN  -Connection $connection | Select Name, Firstname, Lastname, LogonName 
			Write-Host ("{0} {1}" -f $Indent,$userfound.Name)
			$Global:myCol += $userfound
			Clear-Variable userfound -ErrorAction SilentlyContinue
			}
		}
	}

ForEach ($ParentGroupName in $ParentGroupNames)
	{
	$Global:Indent = $null
	
	$connection = Connect-QADService 'dbMotion' -Credential (Get-Credential 'dbMotion\eitanb')
	
	$ParentGroup = Get-QADGroup -Name $ParentGroupName -Connection $connection
	Write-Host "====================="
	Write-Host " TREE VIEW PER GROUP"
	Write-Host "====================="
	Write-Host ("{0}" -f $ParentGroup.DisplayName) -ForegroundColor "blue"
	If ($ParentGroup -eq $null)
		{
		Write-Warning "Group $ParentGroupName not found."
		break
		}
	Else
		{
		$FirstMembers = $ParentGroup.Members
		ForEach ($member in $firstmembers)
			{
			Get-MySubGroupMembersRecursive $member $connection
			}
		}
	}
	
	
Write-Host ""
Write-Host "====================="	
Write-Host " All Unique Members: "
Write-Host "====================="	
$myCol | Sort Name | Select Firstname, Lastname, LogonName  -Unique 