using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using System.Linq;

namespace MissionPlanner.Swarm
{
    public partial class FormationControl : Form
    {
        Formation SwarmInterface = null;
        bool threadrun = false;

        public class Vec4f
        {
            public float x { get; set;}
            public float y { get; set;}
            public float z { get; set;}
            public float w { get; set;}

            public Vec4f(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        }

        Dictionary<int, List<Vec4f>> sensorStatus = new Dictionary<int, List<Vec4f>>();
        private const int maxGraphLength = 100;

        public FormationControl()
        {
            InitializeComponent();

            SwarmInterface = new Formation();

            TopMost = true;

            Dictionary<String, MAVState> mavStates = new Dictionary<string, MAVState>();
           

            foreach (var port in MainV2.Comports)
            {
                port.OnPacketReceived += MavOnOnPacketReceivedHandler;
                foreach (var mav in port.MAVlist.Where(index => index.compid == (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_AUTOPILOT1))
                { 
                   mavStates.Add(port.BaseStream.PortName + " " + mav.sysid + " " + mav.compid, mav);
                    //Add the sysId to a dictionary
                   sensorStatus.Add(mav.sysid, new List<Vec4f>() );
                }
            }

            if (mavStates.Count == 0)
                return;

            bindingSource1.DataSource = mavStates;

            CMB_mavs.DataSource = bindingSource1;
            CMB_mavs.ValueMember = "Value";
            CMB_mavs.DisplayMember = "Key";

            updateicons();

            this.MouseWheel += new MouseEventHandler(FollowLeaderControl_MouseWheel);

            MessageBox.Show("this is beta, use at own risk");

            MissionPlanner.Utilities.Tracking.AddPage(this.GetType().ToString(), this.Text);
        }

        private void MavOnOnPacketReceivedHandler(object o, MAVLink.MAVLinkMessage linkMessage)
        {
            if ((MAVLink.MAVLINK_MSG_ID)linkMessage.msgid == MAVLink.MAVLINK_MSG_ID.DEBUG_VECT)
            {
                //Decode the packet as sensor data, store using Mavlink.sysid
                MAVLink.mavlink_debug_vect_t payload = (MAVLink.mavlink_debug_vect_t)linkMessage.data;
                Vec4f status = new Vec4f(payload.x, payload.y, payload.z, payload.time_usec * 1000);
                sensorStatus[linkMessage.sysid].Add(status);
                
                //Check if the list is out of range
                if(sensorStatus[linkMessage.sysid].Count > maxGraphLength)
                {
                    sensorStatus[linkMessage.sysid].RemoveAt(0);
                }
            }
            updateicons();
        }

        void FollowLeaderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                grid1.setScale(grid1.getScale() + 4);
            }
            else
            {
                grid1.setScale(grid1.getScale() - 4);
            }
        }

        void updateicons()
        {
            bindingSource1.ResetBindings(false);

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist.Where( index => index.compid == 1))
                {
                    if (mav == SwarmInterface.getLeader())
                    {
                        ((Formation)SwarmInterface).setOffsets(mav, 0, 0, 0);
                        var vector = SwarmInterface.getOffsets(mav);
                        Vec4f stat = sensorStatus[mav.sysid].LastOrDefault();
                        float z = stat==null ? 0 : stat.z;
                        grid1.UpdateIcon(mav, (float)vector.x, (float)vector.y, (float)vector.z, false, z);
                    }
                    else
                    {
                        var vector = SwarmInterface.getOffsets(mav);
                        Vec4f stat = sensorStatus[mav.sysid].LastOrDefault();
                        float z = stat == null ? 0 : stat.z;
                        grid1.UpdateIcon(mav, (float)vector.x, (float)vector.y, (float)vector.z, true, z);
                    }
                }
            }
            grid1.Invalidate();
        }

        private void CMB_mavs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist.Where(index => index.compid == 1))
                {
                    if (mav == CMB_mavs.SelectedValue)
                    {
                        MainV2.comPort = port;
                        port.sysidcurrent = mav.sysid;
                        port.compidcurrent = mav.compid;
                    }
                }
            }
        }

        private void BUT_Start_Click(object sender, EventArgs e)
        {
            if (threadrun == true)
            {
                threadrun = false;
                BUT_Start.Text = Strings.Start;
                return;
            }

            if (SwarmInterface != null)
            {
                new System.Threading.Thread(mainloop) { IsBackground = true }.Start();
                BUT_Start.Text = Strings.Stop;
            }
        }

        private void BUT_SampleToggle_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null && SwarmInterface.GlobalSampleToggle == true)
            {
                BUT_SampleToggle.Text = "Stop Sample";
                BUT_SampleToggle.BackColor = Color.Red;
                BUT_SampleToggle.ForeColor = Color.Red;
                SwarmInterface.MassCommand_ToggleRelay();
                return;
            }

            if (SwarmInterface != null && SwarmInterface.GlobalSampleToggle == false)
            {
                BUT_SampleToggle.Text = "Start Sample";
                BUT_SampleToggle.BackColor = Color.Green;
                BUT_SampleToggle.ForeColor = Color.Green;
                SwarmInterface.MassCommand_ToggleRelay();
            }
        }

        void mainloop()
        {
            threadrun = true;

            // make sure leader is high freq updates
            SwarmInterface.Leader.parent.requestDatastream(MAVLink.MAV_DATA_STREAM.POSITION, 10, SwarmInterface.Leader.sysid, SwarmInterface.Leader.compid);
            SwarmInterface.Leader.cs.rateposition = 10;
            SwarmInterface.Leader.cs.rateattitude = 10;

            while (threadrun && !this.IsDisposed)
            {
                // update leader pos
                SwarmInterface.Update();

                // update other mavs
                SwarmInterface.SendCommand();

                // 10 hz
                System.Threading.Thread.Sleep(100);
            }
        }

        private void BUT_Arm_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Arm();
            }
        }

        private void BUT_Disarm_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Disarm();
            }
        }

        private void BUT_Takeoff_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Takeoff();
            }
        }

        private void BUT_Land_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Land();
            }
        }

        private void BUT_leader_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                var vectorlead = SwarmInterface.getOffsets(MainV2.comPort.MAV);

                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        var vector = SwarmInterface.getOffsets(mav);

                        SwarmInterface.setOffsets(mav, (float)(vector.x - vectorlead.x),
                            (float)(vector.y - vectorlead.y),
                            (float)(vector.z - vectorlead.z));
                    }
                }

                SwarmInterface.setLeader(MainV2.comPort.MAV);
                updateicons();
                BUT_Start.Enabled = true;
                BUT_Updatepos.Enabled = true;
            }
        }

        private void BUT_connect_Click(object sender, EventArgs e)
        {
            Comms.CommsSerialScan.Scan(true);

            DateTime deadline = DateTime.Now.AddSeconds(50);

            while (Comms.CommsSerialScan.foundport == false)
            {
                System.Threading.Thread.Sleep(100);

                if (DateTime.Now > deadline)
                {
                    CustomMessageBox.Show("Timeout waiting for autoscan/no mavlink device connected");
                    return;
                }
            }

            bindingSource1.ResetBindings(false);
        }

        public Vector3 getOffsetFromLeader(MAVState leader, MAVState mav)
        {
            //convert Wgs84ConversionInfo to utm
            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            IGeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int)((leader.cs.lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone,
                leader.cs.lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double[] masterpll = { leader.cs.lng, leader.cs.lat };

            // get leader utm coords
            double[] masterutm = trans.MathTransform.Transform(masterpll);

            double[] mavpll = { mav.cs.lng, mav.cs.lat };

            //getLeader follower utm coords
            double[] mavutm = trans.MathTransform.Transform(mavpll);

            var heading = -leader.cs.yaw;

            var norotation = new Vector3(masterutm[1] - mavutm[1], masterutm[0] - mavutm[0], 0);

            norotation.x *= -1;
            norotation.y *= -1;
            norotation.z = mav.cs.alt -leader.cs.alt;

            return new Vector3(norotation.x * Math.Cos(heading * MathHelper.deg2rad) - norotation.y * Math.Sin(heading * MathHelper.deg2rad), norotation.x * Math.Sin(heading * MathHelper.deg2rad) + norotation.y * Math.Cos(heading * MathHelper.deg2rad), norotation.z);
        }

        private void grid1_UpdateOffsets(MAVState mav, float x, float y, float z, Grid.icon ico)
        {
            if (mav == SwarmInterface.Leader)
            {
                CustomMessageBox.Show("Can not move Leader");
                ico.z = 0;
            }
            else
            {
                ((Formation)SwarmInterface).setOffsets(mav, x, y, z);
            }
        }

        private void Control_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadrun = false;
        }

        private void BUT_Updatepos_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist.Where(index => index.compid == (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_AUTOPILOT1))
                {
                    mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;

                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    {
                   grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                        ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    }
                }
            }
        }

        private void timer_status_Tick(object sender, EventArgs e)
        {
            // clean up old
            foreach (Control ctl in PNL_status.Controls)
            {
                bool match = false;
                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        if (mav == (MAVState)ctl.Tag)
                        {
                            match = true;

                        }
                    }
                }

                if (match == false)
                    ctl.Dispose();
            }

            // setup new
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist.Where(index => index.compid == (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_AUTOPILOT1))
                {
                    bool exists = false;
                    foreach (Control ctl in PNL_status.Controls)
                    {
                        if (ctl is Status && ctl.Tag == mav)
                        {
                            exists = true;
                            ((Status)ctl).GPS.Text = mav.cs.gpsstatus >= 3 ? "OK" : "Bad";
                            ((Status)ctl).Armed.Text = mav.cs.armed.ToString();
                            ((Status)ctl).Mode.Text = mav.cs.mode;
                            ((Status)ctl).MAV.Text = mav.ToString();
                            ((Status)ctl).Guided.Text = mav.GuidedMode.x / 1e7 + ",\n" + mav.GuidedMode.y / 1e7 + ",\n" +
                                                         mav.GuidedMode.z;
                            ((Status)ctl).Location1.Text = mav.cs.lat + ",\n" + mav.cs.lng + ",\n" +
                                                            mav.cs.alt;
                            if (sensorStatus.ContainsKey(mav.sysid))
                            {
                                if (sensorStatus[mav.sysid].Count > 0)
                                {
                                    float windPitch = sensorStatus[mav.sysid].Last().y;
                                    windPitch += mav.cs.pitch;
                                    ((Status)ctl).Speed.Text = sensorStatus[mav.sysid].Last().x.ToString() + "\n" + windPitch.ToString() + "\n" + sensorStatus[mav.sysid].Last().z.ToString();
                                }
                            }
                               
                            if (mav == SwarmInterface.Leader)
                            {
                                ((Status)ctl).ForeColor = Color.Red;
                            }
                            else
                            {
                                ((Status)ctl).ForeColor = Color.Black;
                            }

                            //Update ZEDGraph
                            ZedGraph.ZedGraphControl zed = ((Status)ctl).Zed;
                            zed.GraphPane.CurveList.Clear();

                            // plot the data as curves
                            float[] speeds = sensorStatus[mav.sysid].Select(l => l.x).ToArray();
                            float[] time = sensorStatus[mav.sysid].Select(l => l.w).ToArray();
                            float[] pitch = sensorStatus[mav.sysid].Select(l => l.y + (mav.cs.pitch)).ToArray();

                            double[] speeds1 = Array.ConvertAll(speeds, x => (double)x);
                            double[] pitchs1 = Array.ConvertAll(pitch, x => (double)x);
                            double[] time1 = Array.ConvertAll(time, x => (double)x);

                            var curve1 = zed.GraphPane.AddCurve("Speed", time1, speeds1, Color.Red);
                            var curve2 = zed.GraphPane.AddCurve("Pitch", time1, pitchs1, Color.Green);
                            curve1.Line.IsAntiAlias = true;
                            curve2.Line.IsAntiAlias = true;

                            // style the plot
                            zed.GraphPane.Title.Text = "Speed series";
                            zed.GraphPane.XAxis.Title.Text = "Samples (4Hz)";
                            zed.GraphPane.YAxis.Title.Text = "Speed (m/s)";

                            // auto-axis and update the display
                            zed.GraphPane.YAxis.Scale.Min = 0;
                            zed.GraphPane.YAxis.Scale.Max = 20;
                            zed.GraphPane.XAxis.ResetAutoScale(zed.GraphPane, CreateGraphics());
                            zed.Refresh();
                        }
                    }

                    if (!exists)
                    {
                        Status newstatus = new Status();
                        newstatus.Tag = mav;
                        PNL_status.Controls.Add(newstatus);
                    }
                }
            }
            updateicons();
        }

        private void but_guided_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.GuidedMode();
            }
        }

        private void but_auto_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.AutoMode();
            }
        }
    }
}