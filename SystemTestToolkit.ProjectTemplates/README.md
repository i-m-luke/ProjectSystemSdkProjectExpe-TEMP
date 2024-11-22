# Guide
## Adding new file to a template project
1. Include the file in .vstemplate
2. Include the file in ItemGroups in project file of the template project (e.g. *.systestpack) 
3. Include the file in ItemGroups in project file of the project with project templates solution. Include it as Content with IncludeInVSIX set to true. 
4. Ideally set CopyToOutputDirectory to CopyAlways