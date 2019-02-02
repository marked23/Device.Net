using System;

using AppKit;
using Foundation;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace Usb.Net.MacOSSample
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var VendorId = 0x1209;
            var ProductId = 0x53C1;

            //var usbDeviceFinder = new UsbDeviceFinder(0x534C, 0x0001);
            var usbDeviceFinder = new UsbDeviceFinder(VendorId, ProductId);
            var usbDevice = UsbDevice.OpenUsbDevice(usbDeviceFinder);
            IUsbDevice usbDeviceAsInterface = (IUsbDevice)usbDevice;

            usbDevice.Open();

            var firstInterface = usbDevice.Configs[0].InterfaceInfoList[0];

            usbDeviceAsInterface.ClaimInterface(0);
            //if (!usbDeviceAsInterface.ClaimInterface(0)) throw new Exception("ouch!");

            var readEndpoint = usbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            var writeEndpoint = usbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

            var buffer = new byte[64];
            buffer[0] = 0x3f;
            buffer[1] = 0x23;
            buffer[2] = 0x23;

            writeEndpoint.Write(buffer, 3000, out var poops);
            buffer = new byte[64];

            readEndpoint.Read(buffer, 3000, out poops);

        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
