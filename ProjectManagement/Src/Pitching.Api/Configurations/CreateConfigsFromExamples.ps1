<#
	This script is used to create default config files which are needed for compile
#>

Copy-Item -Path Examples\ConnectionStrings.config -Destination ConnectionStrings.config
Copy-Item -Path Examples\AppSettings.config -Destination AppSettings.config
