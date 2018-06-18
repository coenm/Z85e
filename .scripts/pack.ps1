function NugetPack([bool] $pre) 
{
    $Version = $env:GitVersion_MajorMinorPatch
    if ($pre)
    {
        echo "Creating pre-release package..."
    } 
    else 
    {
        echo "Creating stable package..."
    }
    
    dotnet pack -c Release --include-symbols Z85e.sln
}

NugetPack ($env:APPVEYOR_REPO_TAG -eq "false")