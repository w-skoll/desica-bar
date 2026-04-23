#!/bin/bash
set -e
cd "$(dirname "$0")"

if [ ! -f assets/AppIcon.icns ]; then
  echo "Generating AppIcon.icns..."
  mkdir -p panettone.iconset
  swift make_icon.swift assets/panettone.svg panettone.iconset 16 32 64 128 256 512 1024
  cp panettone.iconset/icon_16.png   panettone.iconset/icon_16x16.png
  cp panettone.iconset/icon_32.png   panettone.iconset/icon_16x16@2x.png
  cp panettone.iconset/icon_32.png   panettone.iconset/icon_32x32.png
  cp panettone.iconset/icon_64.png   panettone.iconset/icon_32x32@2x.png
  cp panettone.iconset/icon_128.png  panettone.iconset/icon_128x128.png
  cp panettone.iconset/icon_256.png  panettone.iconset/icon_128x128@2x.png
  cp panettone.iconset/icon_256.png  panettone.iconset/icon_256x256.png
  cp panettone.iconset/icon_512.png  panettone.iconset/icon_256x256@2x.png
  cp panettone.iconset/icon_512.png  panettone.iconset/icon_512x512.png
  cp panettone.iconset/icon_1024.png panettone.iconset/icon_512x512@2x.png
  iconutil -c icns panettone.iconset -o assets/AppIcon.icns
  rm -rf panettone.iconset
fi

APP="DeSicaBar.app"
rm -rf "$APP"
mkdir -p "$APP/Contents/MacOS" "$APP/Contents/Resources/clips"

# Universal binary (arm64 + x86_64) pinned to macOS 13.0 deployment target.
# Forces Swift to only emit symbols available on Sequoia (15) and below,
# avoiding "symbol not found" crashes on older OS versions when the host
# SDK is newer.
TARGETS=("arm64-apple-macos13.0" "x86_64-apple-macos13.0")
TMP=$(mktemp -d)
for t in "${TARGETS[@]}"; do
  swiftc desica_bar.swift -O \
    -target "$t" \
    -o "$TMP/DeSicaBar-${t%%-*}" \
    -framework Cocoa \
    -framework AVFoundation \
    -framework ServiceManagement
done
lipo -create "$TMP"/DeSicaBar-* -output "$APP/Contents/MacOS/DeSicaBar"
rm -rf "$TMP"

cp Info.plist         "$APP/Contents/Info.plist"
cp assets/panettone.svg "$APP/Contents/Resources/panettone.svg"
cp assets/AppIcon.icns "$APP/Contents/Resources/AppIcon.icns"
cp DISCLAIMER.txt     "$APP/Contents/Resources/DISCLAIMER.txt"
cp LICENSE            "$APP/Contents/Resources/LICENSE.txt"
for ext in mp3 mp4 m4a wav aiff aif caf; do
  cp assets/clips/*.$ext "$APP/Contents/Resources/clips/" 2>/dev/null || true
done

codesign --force --deep --sign - "$APP"
echo "Built $APP"
