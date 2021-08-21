#!/usr/bin/env bash

TARGET_DIR=$1
APP_PACKAGE_NAME=$2
TEST_INSTRUMENTATION_NAME=$3
EMULATOR_NAME="emulator-5554"

echo "TARGET_DIR: $TARGET_DIR"
echo "APP_PACKAGE_NAME: $APP_PACKAGE_NAME"
echo "TEST_INSTRUMENTATION_NAME: $TEST_INSTRUMENTATION_NAME"

echo ""
echo "Show installed instrumentation list:"

$ANDROID_HOME/platform-tools/adb -s $EMULATOR_NAME shell pm list instrumentation

echo ""
echo "Start integration tests"

$ANDROID_HOME/platform-tools/adb -s $EMULATOR_NAME shell am instrument -w $APP_PACKAGE_NAME/$TEST_INSTRUMENTATION_NAME

echo ""
echo "Integration tests finished"

echo ""
echo "Copy tests to $TARGET_DIR"

$ANDROID_HOME/platform-tools/adb -s $EMULATOR_NAME pull /storage/emulated/0/Android/data/$APP_PACKAGE_NAME/files/Documents/TestResults.trx $TARGET_DIR
