#!/usr/bin/env bash

APK_LOCATION=$1
APP_PACKAGE_NAME=$2

echo "Install application to emulator"

packages="$($ANDROID_HOME/platform-tools/adb -s emulator-5554 shell pm list packages)";

if [[ $packages == *"$APP_PACKAGE_NAME"* ]]; then
{
    echo "$APP_PACKAGE_NAME was found. Apllication data is clearing..."
    $ANDROID_HOME/platform-tools/adb -s emulator-5554 shell pm clear $APP_PACKAGE_NAME
}
fi

echo "Install $APK_LOCATION"

$ANDROID_HOME/platform-tools/adb -s emulator-5554 push $APK_LOCATION /data/local/tmp/
$ANDROID_HOME/platform-tools/adb -s emulator-5554 shell pm install -t -g /data/local/tmp/$APP_PACKAGE_NAME.apk

echo "Application installed"