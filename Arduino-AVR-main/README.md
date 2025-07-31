# Spoofed Arduino cores for the Leonardo

Note: All relevant files are stored in `AppData\Local\Arduino15\packages\arduino\hardware\avr\1.8.6\`

## `boards.txt`

### Changes:

Note: All `.vid`/`.pid` lines should be set to the same `vid`/`pid` as the device you are spoofing to.

- leonardo.name `"Xtrfy M4"`
- leonardo.vid.0
- leonardo.pid.0
- leonardo.vid.1
- leonardo.pid.1
- leonardo.vid.2
- leonardo.pid.2
- leonardo.vid.3
- leonardo.pid.3
- leonardo.upload_port.0.vid
- leonardo.upload_port.0.pid
- leonardo.upload_port.1.vid
- leonardo.upload_port.1.pid
- leonardo.upload_port.2.vid
- leonardo.upload_port.2.pid
- leonardo.upload_port.3.vid
- leonardo.upload_port.3.pid

- leonardo.build.vid
- leonardo.build.pid
- leonardo.build.usb_product `"Xtrfy M4"`

### Additions

- leonardo.build.usb_manufacturer `"Wings Tech"`

## `cores/arduino/USBDesc.h`

### Changes:

Uncomment `#define CDC_DISABLED` - This fully disables the serial port after the initial boot up. To program the board once this is disabled, you must press the upload button in the IDE, wait for it to compile and say uploading, then press the reset button on the arduino.

## `cores/arduino/USBCore.cpp`

### Changes:

Find the conditional block that contains the following:

```c++
#ifdef PLUGGABLE_USB_ENABLED
			char name[ISERIAL_MAX_LEN];
			PluggableUSB().getShortName(name);
			return USB_SendStringDescriptor((uint8_t*)name, strlen(name), 0);
#endif
```

Change it to this replacing the `serialNumber` char* with your desired serial, or leave it empty for no serial:

```c++
#ifdef PLUGGABLE_USB_ENABLED
#if 0
 			char name[ISERIAL_MAX_LEN];
 			PluggableUSB().getShortName(name);
 			return USB_SendStringDescriptor((uint8_t*)name, strlen(name), 0);
#else
			const char* serialNumber = "";
 			return USB_SendStringDescriptor((uint8_t*)serialNumber, (u8)strlen(serialNumber), 0);
#endif
#endif
```
