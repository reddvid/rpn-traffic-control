using Microsoft.Win32;
using OBSWebsocketDotNet;
using RPNTrafficControl.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPNTrafficControl
{
    static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "TrafficControl", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Application.Run(new ControllerContext());
                }
                else
                {
                    MessageBox.Show("RPN Traffic Control is already running and silently lives on the taskbar notification area.", "RPN Traffic Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

//        public class MyCustomApplicationContext : ApplicationContext
//        {
//            static System.Timers.Timer t;
//            protected static OBSWebsocket _obs = new OBSWebsocket();

//            private void InitTimer()
//            {
//                t = new System.Timers.Timer();
//                t.AutoReset = false;
//                t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
//                t.Interval = GetInterval();
//                t.Enabled = true;
//                t.Start();
//            }

//            static double GetInterval()
//            {
//                DateTime now = DateTime.Now;
//                return ((60 - now.Second) * 1000 - now.Millisecond);
//            }


//            static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//            {
//                Debug.WriteLine(DateTime.Now.ToString("hh:mm tt"));
//                Debug.WriteLine(Properties.Settings.Default.StartTime);

//                if (DateTime.Now.ToString("hh:mm tt") == Properties.Settings.Default.StartTime) // Start
//                {
//                    try
//                    {
//                        if (_obs.IsConnected)
//                        {
//                            _obs.StartRecord();
//                        }
//                        else
//                        {
//                            // Do the old method
//                            // TODO: Figure out to reconnect websocket when app is open but is disconnected

//                            Process[] obs = Process.GetProcessesByName("obs64");
//                            if (obs.Length != 0)
//                            {
//                                // Had to Kill on Start
//                                obs[0].Kill();
//                            }

//                            // Disable Startup Check
//                            ProcessStartInfo psi = new ProcessStartInfo();
//                            psi.WorkingDirectory = Properties.Settings.Default.OBSExeLocation.Replace("obs64.exe", string.Empty);
//                            psi.FileName = @"obs64.exe";
//                            psi.UseShellExecute = true;
//                            psi.Arguments = @"--disable-shutdown-check --startrecording";
//                            Process.Start(psi);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK);
//                    }
//                }

//                if (DateTime.Now.ToString("hh:mm tt") == Properties.Settings.Default.StopTime)
//                {
//                    try
//                    {
//                        Process[] obs = Process.GetProcessesByName("obs64");
//                        if (obs.Length != 0)
//                        {
//                            _obs.StopRecord();
//                            // obs[0].Kill();
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK);
//                    }
//                }

//                t.Interval = GetInterval();
//                t.Start();
//            }

//            private NotifyIcon trayIcon;
//            private ContextMenuStrip contextMenuStrip;
//            private ToolStripMenuItem toolStripSettings = new ToolStripMenuItem();
//            private ToolStripMenuItem toolStripQuit = new ToolStripMenuItem();

//            private SettingsPage s = new SettingsPage(_obs);

//            private System.Windows.Forms.Timer doubleClickTimer = new System.Windows.Forms.Timer();
//            private static readonly string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
//            private static readonly string StartupValue = "RPN Traffic Control";

//            public MyCustomApplicationContext()
//            {
//#if DEBUG
//                s.Show();
//#endif
//                InitTimer();

//                doubleClickTimer.Interval = 100;
//                doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

//                toolStripSettings.Text = "Settings";
//                toolStripSettings.Click += ToolStripOpen_Click;
//                toolStripSettings.AutoSize = false;
//                toolStripSettings.Size = new Size(120, 30);
//                toolStripSettings.Margin = new Padding(0, 4, 0, 0);

//                toolStripQuit.Text = "Quit";
//                toolStripQuit.Click += ToolStripExit_Click;
//                toolStripQuit.AutoSize = false;
//                toolStripQuit.Size = new Size(120, 30);
//                toolStripQuit.Margin = new Padding(0, 0, 0, 4);

//                contextMenuStrip = new ContextMenuStrip()
//                {
//                    DropShadowEnabled = true,
//                    ShowCheckMargin = false,
//                    ShowImageMargin = false,
//                    Size = new System.Drawing.Size(310, 170)
//                };

//                contextMenuStrip.Items.AddRange(new ToolStripItem[]
//                {
//                    toolStripSettings,
//                    toolStripQuit
//                });

//                try
//                {
//                    CustomizeContextMenuBackground();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Something went wrong, try again.",
//                                            "RPN Traffic Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }

//                trayIcon = new NotifyIcon()
//                {
//                    ContextMenuStrip = contextMenuStrip,
//                    Icon = Properties.Resources.favicon,
//                    Visible = true,
//                    Text = "RPN Traffic Control"
//                };

//                trayIcon.BalloonTipText = "Access the app's settings by right-clicking on the system tray icon";
//                trayIcon.BalloonTipTitle = "RPN Traffic Control is active";
//                trayIcon.ShowBalloonTip(2000);

//                trayIcon.MouseDown += TrayIcon_MouseDown;
//                trayIcon.MouseClick += TrayIcon_MouseClick;

//                EnableRunAtStartup();

//                try
//                {
//                    RunObs();
//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine("Running OBS failed: " + ex.Message);
//                }
//            }

//            private void EnableRunAtStartup()
//            {
//                RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
//                key.SetValue(StartupValue, Application.ExecutablePath.ToString());
//                key.Close();
//            }

//            private void RunObs()
//            {
//                if (IsItWithinTheTime())
//                {
//                    try
//                    {
//                        if (_obs.IsConnected)
//                        {
//                            _obs.StartRecord();
//                        }
//                        else
//                        {
//                            Process[] obs = Process.GetProcessesByName("obs64");
//                            if (obs.Length != 0)
//                            {
//                                obs[0].Kill();
//                            }

//                            ProcessStartInfo psi = new ProcessStartInfo();
//                            psi.WorkingDirectory = Properties.Settings.Default.OBSExeLocation.Replace("obs64.exe", string.Empty);
//                            psi.FileName = @"obs64.exe";
//                            psi.UseShellExecute = true;
//                            psi.Arguments = @"--disable-shutdown-check --startrecording";
//                            Process.Start(psi);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK);
//                    }
//                }
//            }

//            private bool IsItWithinTheTime()
//            {
//                // Compare properties with saved time properties
//                bool timeIsInRange = false;
//                var startTime = Properties.Settings.Default.StartTime;
//                var endTime = Properties.Settings.Default.StopTime;

//                DateTime start = DateTime.ParseExact(startTime, "hh:mm tt", CultureInfo.InvariantCulture);
//                DateTime end = DateTime.ParseExact(endTime, "hh:mm tt", CultureInfo.InvariantCulture);
//                DateTime now = DateTime.Now;

//                if ((now > start) && (now < end))
//                {
//                    timeIsInRange = true;
//                }
//                return timeIsInRange;
//            }

//            private async void CustomizeContextMenuBackground()
//            {
//                var verticalPadding = 4;
//                contextMenuStrip.Items[0].Font = new Font(this.contextMenuStrip.Items[0].Font, FontStyle.Bold);
//                bool appsUseLight = await Task.Run(() => ReadRegistry());

//                if (appsUseLight)
//                {
//                    contextMenuStrip.Renderer = new MyCustomRenderer { VerticalPadding = verticalPadding, HighlightColor = Color.White, ImageColor = Color.FromArgb(255, 238, 238, 238) };
//                    contextMenuStrip.BackColor = Lighten(Color.White);
//                    contextMenuStrip.ForeColor = Color.Black;
//                }
//                else
//                {
//                    contextMenuStrip.Renderer = new MyCustomRenderer { VerticalPadding = verticalPadding, HighlightColor = Color.Black, ImageColor = Color.FromArgb(255, 43, 43, 43) };
//                    contextMenuStrip.BackColor = Lighten(Color.Black);
//                    contextMenuStrip.ForeColor = Color.White;
//                }

//                contextMenuStrip.MinimumSize = new Size(120, 30);
//                contextMenuStrip.AutoSize = false;
//                contextMenuStrip.ShowImageMargin = false;
//                contextMenuStrip.ShowCheckMargin = false;
//            }

//            private bool ReadRegistry()
//            {
//                bool isUsingLightTheme;
//                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
//                {
//                    if (key != null)
//                    {
//                        var k = key.GetValue("AppsUseLightTheme");
//                        if (k != null)
//                        {
//                            if (k.ToString() == "1")
//                                isUsingLightTheme = true;
//                            else
//                                isUsingLightTheme = false;
//                        }
//                        else
//                        {
//                            isUsingLightTheme = true;
//                        }
//                    }
//                    else
//                        isUsingLightTheme = true;
//                }

//                return isUsingLightTheme;
//            }

//            private Color Lighten(Color color)
//            {
//                int r;
//                int g;
//                int b;

//                if (color.R == 0 && color.G == 0 && color.B == 0)
//                {
//                    r = color.R + 43;
//                    g = color.G + 43;
//                    b = color.B + 43;
//                }
//                else
//                {
//                    r = color.R - 17;
//                    g = color.G - 17;
//                    b = color.B - 17;
//                }

//                return Color.FromArgb(r, g, b);
//            }

//            private bool isFirstClick = true;
//            private bool isDoubleClick = false;
//            private int milliseconds = 0;

//            private void TrayIcon_MouseDown(object sender, MouseEventArgs e)
//            {
//                // This is the first mouse click.
//                if (e.Button == MouseButtons.Left)
//                {
//                    if (isFirstClick)
//                    {
//                        isFirstClick = false;

//                        // Start the double click timer.
//                        doubleClickTimer.Start();
//                    }

//                    // This is the second mouse click.
//                    else
//                    {
//                        // Verify that the mouse click is within the double click
//                        // rectangle and is within the system-defined double 
//                        // click period.
//                        if (milliseconds < SystemInformation.DoubleClickTime)
//                        {
//                            isDoubleClick = true;
//                        }
//                    }
//                }
//            }

//            private void doubleClickTimer_Tick(object sender, EventArgs e)
//            {
//                try
//                {
//                    milliseconds += 50;

//                    // The timer has reached the double click time limit.
//                    if (milliseconds >= SystemInformation.DoubleClickTime)
//                    {
//                        doubleClickTimer.Stop();

//                        if (isDoubleClick)
//                        {
//                            // Perform Double Click
//                            s.Show();
//                        }

//                        // Allow the MouseDown event handler to process clicks again.
//                        isFirstClick = true;
//                        isDoubleClick = false;
//                        milliseconds = 0;
//                    }


//                }
//                catch (Exception) { }
//            }

//            private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
//            {
//                if (e.Button == MouseButtons.Right)
//                {
//                    try
//                    {
//                        CustomizeContextMenuBackground();
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Something went wrong, try again.",
//                                                "RPN Traffic Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    }
//                }
//            }

//            private void ToolStripExit_Click(object sender, EventArgs e)
//            {
//                // Hide tray icon, otherwise it will remain shown until user mouses over it
//                trayIcon.Visible = false;
//                Environment.Exit(0);
//                Application.Exit();
//            }

//            private void ToolStripOpen_Click(object sender, EventArgs e)
//            {
//                try
//                {
//                    s.Show();
//                }
//                catch (Exception)
//                {
//                }
//            }

//            void Exit(object sender, EventArgs e)
//            {
//                // Hide tray icon, otherwise it will remain shown until user mouses over it
//                trayIcon.Visible = false;

//                Application.Exit();
//            }
//        }

      

    }


}
