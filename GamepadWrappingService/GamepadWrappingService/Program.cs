using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace GamepadWrappingService
{
    class Program
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        static void Main(string[] args)
        {
            DirectInput d = new DirectInput();

            List<JoystickUpdate> statusHistory = new List<JoystickUpdate>(15);

            // Runs forever, where the last thing that happened was the controller was unplugged, then it comes back and waits for a new controller.
            while (true)
            {

                // Waits until a gamepad is plugged in.
                Console.WriteLine("Searching for gamepad...");
                while (d.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices).Count == 0) ;

                Console.WriteLine("Connecting...");
                Guid joystickId = d.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)[0].InstanceGuid;
                Joystick stick = new Joystick(d, joystickId);

                Console.WriteLine("Connected to Joystick {0}!", joystickId);

                Console.WriteLine("Setting up properties...");
                stick.Properties.BufferSize = 128;
                Capabilities c = stick.Capabilities;
                Console.WriteLine("Number of axes: " + c.AxeCount);
                Console.WriteLine("Number of Buttons: " + c.ButtonCount);
                Console.WriteLine("Flags: " + c.Flags);

                Console.WriteLine("Finding Minecraft");
                Process[] processes = Process.GetProcessesByName("Minecraft");
                while(processes.Length == 0)
                {
                    Console.WriteLine("Waiting for Minecraft...");
                    Thread.Sleep(5000);
                    processes = Process.GetProcessesByName("Minecraft");
                }
                Console.WriteLine("Minecraft Located!");
                //SetForegroundWindow(hWnd);

                Console.WriteLine("Press any key to begin!");
                Console.Read();
                stick.Acquire();

                Console.WriteLine("Ready to read data!");
                while (!stick.IsDisposed)
                {
                    stick.Poll();
                    JoystickUpdate[] datas = stick.GetBufferedData();
                    foreach (JoystickUpdate state in datas)
                    {
                        if(didSomethingHappen(state))
                            Console.WriteLine(state);

                        switch(state.Offset)
                        {
                            case JoystickOffset.Y:
                                Console.WriteLine("Updating Movement!");
                                if (state.Value > 40000)
                                    SendKeys.SendWait("s");
                                else if (state.Value < 25000)
                                    SendKeys.SendWait("w");
                                break;

                            case JoystickOffset.X:
                                Console.WriteLine("Updating Movement!");
                                if (state.Value > 40000)
                                    SendKeys.SendWait("d");
                                else if (state.Value < 25000)
                                    SendKeys.SendWait("a");
                                break;

                            case JoystickOffset.Buttons0:
                                Console.WriteLine("Jumping!");
                                SendKeys.SendWait(" ");
                                break;
                        }
                    }
                }
            }

            /// Helps to eliminate noise from the controller
            bool didSomethingHappen(JoystickUpdate status)
            {
                Predicate<JoystickUpdate> findUpdateType = new Predicate<JoystickUpdate>((JoystickUpdate update) => { return update.Offset == status.Offset; });
                List<JoystickUpdate> matchingHistory = statusHistory.FindAll(findUpdateType);
                if (matchingHistory.Count > 0)
                {
                    foreach (JoystickUpdate j in matchingHistory)
                    {
                        if (Math.Abs(j.Value - status.Value) > 100 && (status.Offset == JoystickOffset.X || status.Offset == JoystickOffset.Y ||
                            status.Offset == JoystickOffset.RotationX || status.Offset == JoystickOffset.RotationY))
                        {
                            statusHistory[statusHistory.FindIndex(findUpdateType)] = status;
                            return true;
                        }
                        else if (Math.Abs(j.Value - status.Value) > 1 && (status.Offset != JoystickOffset.X || status.Offset != JoystickOffset.Y ||
                            status.Offset != JoystickOffset.RotationX || status.Offset != JoystickOffset.RotationY))
                        {
                            statusHistory[statusHistory.FindIndex(findUpdateType)] = status;
                            return true;
                        }
                    }
                }
                else
                {
                    statusHistory.Add(status);
                    return true;
                }
                return false;
            }
        }
    }
}
