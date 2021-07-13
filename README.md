Tama Code Encoder/Decoder Library
=================================

This library allows you to encode and decode Tamagotchi Pix Tama Codes as
binary. You can decode the bytes from a scanned Tama Code, or encode a
Tama Code representation to bytes that you can then generate a QR code from.
The included test app serves as a demo of how to use the library.

## Tama Code decoding
To decode Tama Codes, you will need to supply it to the test app as a hex,
because it cannot access the camera. I suggest using [Binary Eye](https://play.google.com/store/apps/details?id=de.markusfisch.android.binaryeye)
on Android as it actually shows you the hex code rather than try to
display it as text.
