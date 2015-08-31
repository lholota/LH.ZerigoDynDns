# SetAssemblyVersion.ps1
#
# http://www.luisrocha.net/2009/11/setting-assembly-version-with-windows.html
# http://blogs.msdn.com/b/dotnetinterop/archive/2008/04/21/powershell-script-to-batch-update-assemblyinfo-cs-with-new-version.aspx
# http://jake.murzy.com/post/3099699807/how-to-update-assembly-version-numbers-with-teamcity
# https://github.com/ferventcoder/this.Log/blob/master/build.ps1#L6-L19

Param(
	[string]$Version,
    [string]$Path=$pwd
)

function Help {
    "Sets the AssemblyVersion and AssemblyFileVersion of AssemblyInfo.cs files`n"
    ".\SetAssemblyVersion.ps1 -Version [VersionNumber] -Path [SearchPath]`n"
    "   [VersionNumber]     The version number to set, for example: 1.1.9301.0"
    "   [SearchPath]        The path to search for AssemblyInfo files.`n"
}
function Update-SourceVersion
{
    Param ([string]$Version)
    $NewVersion = 'AssemblyVersion("' + $Version + '")';
    $NewFileVersion = 'AssemblyFileVersion("' + $Version + '")';

    foreach ($o in $input) 
    {
        Write-Host "Updating  '$($o.FullName)' -> $Version"
    
        $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
        $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
        $assemblyVersion = 'AssemblyVersion("' + $version + '")';
        $fileVersion = 'AssemblyFileVersion("' + $version + '")';
        
        (Get-Content $o.FullName) | ForEach-Object  { 
           % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
           % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Out-File $o.FullName -encoding UTF8 -force
    }
}
function Update-AllAssemblyInfoFiles ( $version )
{
    Write-Host "Searching '$Path'"
   foreach ($file in "AssemblyInfo.cs", "AssemblyInfo.vb" ) 
   {
        get-childitem $Path -recurse |? {$_.Name -eq $file} | Update-SourceVersion $version ;
   }
}

Write-Host "Script arguments: $args"

# validate arguments 
Write-Host "Version: $Version"
Write-Host "Path: $Path"

if (($Version -eq '/?') -or ($Version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}")) {
	Write-Host "Version did not match the pattern"
	Help
	exit 1;
}

Update-AllAssemblyInfoFiles $Version