<#
	.DESCRIPTION
    Script to change meta-data valuein AndroidManifest.xml file.
    For use as building Android app Azure DevOps step.

    .PARAMETER name
    The name of the metadata to find it in the AndroidManifest.
	
	.PARAMETER value
    Desired value of meta-data (string value).
	
	.EXAMPLE
	.\UpdateMetaDataValue.ps1 -name AppCenterKey
							   -value 1232132341-12312123-123123-123123asd

#>
Param(
    [Parameter(Mandatory = $True)][String]$name,
    [Parameter()][String]$value
)

$manifestFiles = Get-ChildItem .\ -recurse -include "AndroidManifest.xml" 
if ($manifestFiles) {
    Write-Host "Will apply $versionCode ($versionName) to $($manifestFiles.count) AndroidManifest.xml files."
    foreach ($file in $manifestFiles) {
        [xml] $XmlManifest = Get-Content -Path $file
        $nodeToChange = Select-Xml -xml $XmlManifest -Xpath "/manifest/application/meta-data[@android:name=""$name""]/@android:value" -namespace @{android = "http://schemas.android.com/apk/res/android"}

        if ($nodeToChange) {
            $nodeToChange.Node.Value = $value

            $XmlManifest.Save($file)
            Write-Host "AndroidManifest.xml - new value applied"
        }
        else {
            Write-Host "Meta-data with name ""$name"" is not found in $file"
        }
    }
} else {
    Write-Host "No AndroidManifest.xml files found."
}