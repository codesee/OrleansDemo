Write-Host "Starting build of Docker container"
Write-Host "Creating staging folder for DemoCluster.Configuration"

$stagingDirectory = ".\ConfigurationStaging"

if (Test-Path $stagingDirectory) {
    Write-Host $stagingDirectory " was found....removing...."
    Remove-Item $stagingDirectory -Recurse
}

New-Item -Name $stagingDirectory -ItemType Directory | Out-Null

Copy-Item -Path .\src\DemoCluster -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.DAL -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.Hosting -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.Configuration -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/", "Dockerfile"
Copy-Item -Path .\src\DemoCluster.Configuration\Dockerfile -Destination $stagingDirectory

Write-Host "Files having been copied to " $stagingDirectory " starting Docker build process"
cd $stagingDirectory
docker build -t testconfiguration .

Write-Host "Docker image successful resetting directory"
cd ..