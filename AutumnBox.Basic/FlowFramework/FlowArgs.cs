﻿
/* =============================================================================*\
*
* Filename: FlowArgs
* Description: 
*
* Version: 1.0
* Created: 2017/11/23 15:11:47 (UTC+8:00)
* Compiler: Visual Studio 2017
* 
* Author: zsh2401
* Company: I am free man
*
\* =============================================================================*/
using AutumnBox.Basic.Device;

namespace AutumnBox.Basic.FlowFramework
{
    public class FlowArgs
    {
        public Serial Serial { get { return DevBasicInfo.Serial; } }
        public DeviceBasicInfo DevBasicInfo { get; set; }
    }
}