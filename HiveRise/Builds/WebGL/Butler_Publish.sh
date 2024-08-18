#! /bin/bash
echo "Build Folder Path: $1"
echo "buildnumber Path: $2"
butler push "$1" michaelwolf95/hiverise:webgl-dev --userversion-file "$2"
read -p 'Press [Enter] key to continue...'