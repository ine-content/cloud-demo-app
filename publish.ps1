rm -Force -Recurse publish
dotnet publish -c RELEASE -o publish --no-self-contained 
Compress-Archive .\publish\* publish.zip
Publish-AzWebApp -ResourceGroupName 01tasks -Name inebackend -ArchivePath .\publish.zip 