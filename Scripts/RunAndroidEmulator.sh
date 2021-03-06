#!/bin/bash

DEVICES="$($ANDROID_HOME/platform-tools/adb devices)";

if [[ $DEVICES == *"emulator-"* ]]; then
{
    echo "Emulator already started"
    echo ""
    echo "$DEVICES"
}
else
{
    echo $ANDROID_HOME
    echo $JAVA_HOME
    
    # Install AVD files
    echo "y" | $ANDROID_HOME/tools/bin/sdkmanager --install 'system-images;android-28;google_apis;x86'

    # Create emulator
    echo "no" | $ANDROID_HOME/tools/bin/avdmanager create avd -n xamarin_android_emulator -k 'system-images;android-28;google_apis;x86' --force
    
    echo "Emulator created successfully $($ANDROID_HOME/emulator/emulator -list-avds), launching it"
    
    nohup $ANDROID_HOME/emulator/emulator -avd xamarin_android_emulator -no-snapshot -no-boot-anim -gpu auto -qemu > /dev/null 2>&1 &
    $ANDROID_HOME/platform-tools/adb wait-for-device shell 'while [[ -z $(getprop sys.boot_completed | tr -d '\r') ]]; do sleep 1; done; input keyevent 82'
    
    $ANDROID_HOME/platform-tools/adb devices
    
    echo "Emulator started"
}
fi
