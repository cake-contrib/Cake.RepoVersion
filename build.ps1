#!/usr/bin/env pwsh

##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER ShowDescription
Shows description about tasks.
.PARAMETER DryRun
Performs a dry run.
.PARAMETER Experimental
Uses the nightly builds of the Roslyn script engine.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
https://cakebuild.net

#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [ValidateSet("Pack", "Publish", "AppVeyor")]
    [string]$Target,
    [string]$Configuration,
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity,
    [switch]$ShowDescription,
    [Alias("WhatIf", "Noop")]
    [switch]$DryRun,
    [switch]$Experimental,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$CAKE_VERSION = "0.33.0"
$CAKE_EXE = Join-Path $TOOLS_DIR "Cake.CoreCLR.$CAKE_VERSION/Cake.dll"

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

###########################################################################
# INSTALL CAKE
###########################################################################

Add-Type -AssemblyName System.IO.Compression.FileSystem
Function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

# Make sure Cake has been installed.
if (!(Test-Path $CAKE_EXE)) {
    Write-Host "Installing Cake..."
    (New-Object System.Net.WebClient).DownloadFile("https://www.nuget.org/api/v2/package/Cake.CoreCLR/$CAKE_VERSION", "$TOOLS_DIR/Cake.CoreCLR.zip")
    Unzip "$TOOLS_DIR/Cake.CoreCLR.zip" "$TOOLS_DIR/Cake.CoreCLR.$CAKE_VERSION"
    Remove-Item "$TOOLS_DIR/Cake.CoreCLR.zip"
}

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

# Build Cake arguments
$cakeArguments = @("$Script");
if ($Target) { $cakeArguments += "-target=$Target" }
if ($Configuration) { $cakeArguments += "-configuration=$Configuration" }
if ($Verbosity) { $cakeArguments += "-verbosity=$Verbosity" }
if ($ShowDescription) { $cakeArguments += "-showdescription" }
if ($DryRun) { $cakeArguments += "-dryrun" }
$cakeArguments += $ScriptArgs

Write-Host "Running build script..."
& dotnet build
& dotnet "$CAKE_EXE" $cakeArguments
exit $LASTEXITCODE
