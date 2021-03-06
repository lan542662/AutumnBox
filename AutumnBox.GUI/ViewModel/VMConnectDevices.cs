﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/8/15 15:43:27 (UTC +8:00)
** desc： ...
*************************************************/

using AutumnBox.Basic.Device;
using AutumnBox.Basic.MultipleDevices;
using AutumnBox.GUI.MVVM;
using AutumnBox.GUI.Util.Bus;
using System.Collections.Generic;
using System.Linq;

namespace AutumnBox.GUI.ViewModel
{
    class VMConnectDevices : ViewModelBase
    {
        public string DisplayMemeberPath { get; set; } = nameof(IDevice.SerialNumber);

        public IDevice Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                RaisePropertyChanged();
                SwitchCommandState();
                RaiseBusEvent();
            }
        }
        private IDevice _selected;

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return _devices;
            }
            set
            {
                _devices = value?.ToArray();
                Selected = _devices?.Count() > 0 ? _devices.First() : null;
                RaisePropertyChanged();
            }
        }
        private IEnumerable<IDevice> _devices;

        public FlexiableCommand ConnectDevice { get; set; }
        public FlexiableCommand DisconnectDevice { get; set; }
        public FlexiableCommand OpenDeviceNetDebugging { get; set; }

        private void SwitchCommandState()
        {
            DisconnectDevice.CanExecuteProp = Selected is NetDevice;
            OpenDeviceNetDebugging.CanExecuteProp = Selected is UsbDevice;
        }

        public VMConnectDevices()
        {
            ConnectDevice = new FlexiableCommand((p) =>
            {
                AtmbContext.Instance.NewExtensionThread("ENetDeviceConnecter")?.Start();
            });
            DisconnectDevice = new FlexiableCommand((p) =>
            {
                AtmbContext.Instance.NewExtensionThread("ENetDeviceDisconnecter")?.Start();
            });
            OpenDeviceNetDebugging = new FlexiableCommand((p) =>
            {
                AtmbContext.Instance.NewExtensionThread("EOpenUsbDeviceNetDebugging")?.Start();
            });
            ConnectedDevicesListener.Instance.DevicesChanged += ConnectedDevicesChanged;
        }

        private void RaiseBusEvent()
        {
            if (Selected == null)
            {
                DeviceSelectionObserver.Instance.RaiseSelectNoDevice();
            }
            else
            {
                DeviceSelectionObserver.Instance.RaiseSelectDevice(Selected);
            }
        }


        private void ConnectedDevicesChanged(object sender, DevicesChangedEventArgs e)
        {
            if (e.Devices.Count() == 0)
            {
                Devices = null;
            }
            else
            {
                Devices = e.Devices;
            }
        }
    }
}
